backend "consul" {
  	address = "webshop_consul_1:8500"
  	advertise_addr = "http://webshop_consul_1:8300"
  	scheme = "http"
}
listener "tcp" {
  	address = "0.0.0.0:8200"
    #tls_cert_file = "/config/server.crt"
    #tls_key_file = "/config/server.key"
  	tls_disable = 1
}
ui = true
disable_mlock = true
