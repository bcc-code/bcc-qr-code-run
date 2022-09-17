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

}

provider "azurerm" {
  features {}
}

provider "azapi" {
}

locals {
    location        = "EastNorway"
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