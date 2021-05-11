# Consul

## Installing a Consul instance

```powershell
helm repo add hashicorp https://helm.releases.hashicorp.com
helm repo update
```

```powershell
helm install consul hashicorp/consul -f infrastructure/consul/consul-values.yaml -n infrastructure

# we'll forward later in traefik automatically for testing purposes
kubectl port-forward service/consul-consul-server 8501:8501 --namespace infrastructure
```

or add it to [Traefik](traefik.md):

```powershell
kubectl apply -f ./infrastructure/traefik/routes/consul.yaml
```

Grab the bootstrap ACL token and use it to login into the ACL tab of the UI.

```powershell
kubectl get secrets/consul-consul-bootstrap-acl-token -n infrastructure --template={{.data.token}} | base64 -d
```

Since we run consul with TLS enabled, the certificate that consul generated is self-signed, so you need to pass the browser check.

To upgrade with new values:

```powershell
helm upgrade consul hashicorp/consul -f infrastructure/consul/consul-values.yaml -n infrastructure
```

* Consul [values.yaml](https://github.com/hashicorp/consul-helm/blob/master/values.yaml)

## Forward internal DNS Queries to Consul with CoreDNS

In order for the internal services to be able to use the Consul DNS, we need to update the CoreDNS configmap to add consul as a DNS server

Fetch the ip address of the Consul DNS

```powershell
kubectl get svc consul-consul-dns -o jsonpath='{.spec.clusterIP}' --namespace=infrastructure
```

Update the values of the consul dns service in `.\infrastructure\coredns\coredns.yaml`

> Important: `coredns.yaml` is the syntax for CoreDNS 1.7.0, matching the tested. You can also fetch the current configmap and append the consul part as described in the [Consul Docs](https://www.consul.io/docs/k8s/dns).

```powershell
kubectl apply -f .\infrastructure\coredns\coredns.yaml
```

Test it out by running a job

```powershell
kubectl apply -f .\infrastructure\coredns\test-dns-job.yaml --namespace=infrastructure
```
