variable "environment" {
  type = string
  default = "prod"
}

variable "github-token" {
  type = string
  sensitive = true
  default = ""
}

variable "resource-prefix" {
  type = string
  default = "qr-code-run-prod"
}

variable "endpoint_domain_name" {
  type= string
  default= "jordenrundt.bcc.no"
}