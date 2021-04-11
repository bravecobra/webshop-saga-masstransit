# Consul

## Installing a Consul instance

```powershell
helm install consul hashicorp/consul -f infrastructure/consul/consul-values.yaml -n infrastructure

# we'll forward later in traefik automatically for testing purposes
kubectl port-forward service/consul-consul-server 8500:8500 --namespace infrastructure
```

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
