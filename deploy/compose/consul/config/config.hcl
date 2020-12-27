log_level = "DEBUG"
datacenter=  "webshop-dc"
server = true

bootstrap_expect = 1
ui = true

connect {
  enabled = true
}

ports {
  grpc = 8502
}

ui_config {
  enabled = true
  // metrics_provider = "prometheus"
  // metrics_proxy {
  //   base_url = "http://prometheus:9090"
  // }
}

#advertise_addr = "consul"
enable_central_service_config = true

telemetry {
  prometheus_retention_time = "10s"
}