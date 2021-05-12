# Seq

## Installing

To setup seq, we deploy the helm chart:

```powershell
helm repo add datalust https://helm.datalust.co
helm repo add fluent https://fluent.github.io/helm-charts
helm repo update
```

```powershell
helm install -f ./infrastructure/seq/seq-values.yaml seq datalust/seq --namespace=infrastructure
```

The `values.yaml` can be found at [https://github.com/datalust/helm.datalust.co/blob/main/charts/seq/values.yaml](https://github.com/datalust/helm.datalust.co/blob/main/charts/seq/values.yaml)

Next we expose the ui through [Traefik](traefik.md)

```powershell
kubectl apply -f ./infrastructure/traefik/routes/seq.yaml
```

## Collecting Kubernetes logs from all pods

Fluent-bit will monitor all the pods logs and forward them to `seq`. Fluent-bit supports monitoring in [Prometheus](prometheus.md), so it adds a serviceMonitor for it.

```powershell
helm install -f .\infrastructure\seq\fluent-values.yaml fluent-bit fluent/fluent-bit --namespace=infrastructure
```

It also adds a dashboard to [Grafana](prometheus.md) installed with the `prometheus-operator` in the `Infrastructure` folder in `grafana`.

## References

[https://docs.datalust.co/docs/collecting-kubernetes-logs](https://docs.datalust.co/docs/collecting-kubernetes-logs)
