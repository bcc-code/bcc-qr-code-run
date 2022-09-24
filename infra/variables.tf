variable "environment" {
  type = string
  default = "prod"
}

variable "github-token" {
  type = string
  sensitive = true
  default = ""
}