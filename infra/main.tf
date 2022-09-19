terraform {
  required_version = ">= 1.0"
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.3.0"
    }
    azapi = {
      source  = "Azure/azapi"
      version = "0.4.0"
    }
    postgresql = {
      source  = "cyrilgdn/postgresql"
      version = "1.12.0"
    }
  }
  experiments = [module_variable_optional_attrs]

  backend "azurerm" {
    resource_group_name  = "BCC-Platform"
    storage_account_name = "bccplatformtfstate"
    container_name       = "tfstate"
    key                  = "qr-code-run.terraform.tfstate"
  }

}

provider "azurerm" {
  features {}
}

provider "azapi" {
}

provider "postgresql" {
  superuser = false
  host      = data.azurerm_postgresql_flexible_server.postgresql_server.fqdn
  username  = "psqladmin"
  password  = data.azurerm_key_vault_secret.postgresql_admin_password.value
}

data "azurerm_client_config" "current" {}

locals {
    location        = "norwayeast"
    resource_group  = "qr-code-run-${var.environment}"
    resource_prefix = "qr-code-run-prod"
    platform_resource_prefix = "qr-code-run-${var.environment}"
    platform_resource_group  = "qr-code-run-${var.environment}"
    tags            = {}
}

# Create Resource Group
resource "azurerm_resource_group" "rg" {
  name     = local.resource_group
  location = local.location
  tags     = local.tags
}


# Vault for postgresql
resource "azurerm_key_vault" "postgresql_vault" {
  name                = lower(replace("${local.resource_prefix}-psql","-",""))
  location            = local.location
  resource_group_name = local.resource_group
  tenant_id           = data.azurerm_client_config.current.tenant_id
  sku_name            = "standard"
  purge_protection_enabled = true
  enabled_for_disk_encryption = true
  # access_policy {
  #   tenant_id = data.azurerm_client_config.current.tenant_id
  #   object_id = data.azurerm_client_config.current.object_id
  #   # key_permissions = [
  #   #   "Get", "UnwrapKey", "WrapKey"
  #   # ]
  #   # secret_permissions = [
  #   #   "Get", "Backup", "Delete", "List", "Purge", "Recover", "Restore", "Set",
  #   # ]
  #   # storage_permissions = [
  #   #   "Get"
  #   # ]
  # }
  enable_rbac_authorization = true
}

# Admin Password for Postgresql
resource "random_password" "postgreql_admin_password" {
  length           = 16
  special          = true
  override_special = "!#$%&*()-_=+[]{}<>:?"
}

# Store admin password in Azure Key vault
resource "azurerm_key_vault_secret" "postgreql_admin_password" {
  name         = "postgreql-admin-password"
  value        = random_password.postgreql_admin_password.result
  key_vault_id = azurerm_key_vault.postgresql_vault.id
  depends_on = [ azurerm_key_vault.postgresql_vault ]
}

# Postgresql server
resource "azurerm_postgresql_flexible_server" "postgresql" {
  name                = "${local.resource_prefix}-postgresql"
  location            = local.location
  resource_group_name = local.resource_group

  administrator_login          = "psqladmin"
  administrator_password       = azurerm_key_vault_secret.postgreql_admin_password.value

  sku_name                     = "B_Standard_B1ms"
  version                      = "13"
  storage_mb                   = 32768

  backup_retention_days        = 35
  geo_redundant_backup_enabled = false
  create_mode = "Default"

  tags                          = local.tags

  zone                          = "1"

  # delegated_subnet_id = 
  # private_dns_zone_id =

}

data "http" "myip" {
  url = "http://ipv4.icanhazip.com"
}

# Open firewall for machine deploying server
resource "azurerm_postgresql_flexible_server_firewall_rule" "terraform_deploy_ip" {
  name             = "terraform_deploy_ip"
  server_id        = azurerm_postgresql_flexible_server.postgresql.id
  start_ip_address = "${chomp(data.http.myip.body)}"
  end_ip_address   = "${chomp(data.http.myip.body)}"
}

# Get Container Registry
data "azurerm_container_registry" "acr" {
  name                = "bccplatform"
  resource_group_name = "BCC-Platform"
}

# Get Key Vault for postgresql server
data "azurerm_key_vault" "keyvault" {
  name                = lower(replace("${local.platform_resource_prefix}-psql","-",""))
  resource_group_name = local.platform_resource_group
}

# Get Admin password for postgresql server
data "azurerm_key_vault_secret" "postgresql_admin_password" {
  name         = "postgreql-admin-password"
  key_vault_id = data.azurerm_key_vault.keyvault.id
}

# Get Postgresql Server
data "azurerm_postgresql_flexible_server" "postgresql_server" {
  name                   = "${local.platform_resource_prefix}-postgresql"
  resource_group_name    = local.platform_resource_group
}

# Get platform resource group
data "azurerm_resource_group" "platform_rg" {
  name = local.platform_resource_group
}

# Create Database
# NB! This will only work if server has a public IP and the client execuring terrafrom has been whitelisted in the server's firewall
# Perhaps whitelisting the IP address of the current client can be automated...
module "postgresql_db" {
  source             = "./modules/azure/postgresql_db"
  db_name            = local.resource_prefix
  server_resource_id = data.azurerm_postgresql_flexible_server.postgresql_server.id
  depends_on = [
    azurerm_postgresql_flexible_server_firewall_rule.terraform_deploy_ip
  ]
}

# Store db user password
resource "azurerm_key_vault_secret" "postgreql_user_password" {
  name         = "${local.resource_prefix}-db-user-password"
  value        = module.postgresql_db.db_user_password
  key_vault_id = data.azurerm_key_vault.keyvault.id
}

# Analytics Workspace
module "log_analytics_workspace" {
  source                           = "./modules/azure/log_analytics"
  name                             = "${local.resource_prefix}-logs"
  location                         = local.location
  resource_group_name              = azurerm_resource_group.rg.name
  tags                             = local.tags

}

# Application Insights
module "application_insights" {
  source                           = "./modules/azure/application_insights"
  name                             = "${local.resource_prefix}-env-insights"
  location                         = local.location
  resource_group_name              = azurerm_resource_group.rg.name
  tags                             = local.tags
  application_type                 = "web"
  workspace_id                     = module.log_analytics_workspace.id
}

# VLAN for Container Environment
module "container_apps_vlan" {
  source                           = "./modules/azure/container_apps_vlan"
  name                             = "${local.resource_prefix}-vlan"
  location                         = local.location
  resource_group_name              = azurerm_resource_group.rg.name
  tags                             = local.tags

  depends_on = [
    azurerm_resource_group.rg
  ]
}
