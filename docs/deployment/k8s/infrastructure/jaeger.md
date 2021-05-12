# Running a Jaeger instance for distributed tracing

## Using the jaeger operator

Following [the jaeger docs](https://www.jaegertracing.io/docs/1.22/operator/), we install the jaeger operator

```powershell
helm repo add jaegertracing https://jaegertracing.github.io/helm-charts
helm repo update
```

```powershell
helm install jaeger-operator jaegertracing/jaeger-operator -n infrastructure
```

To verify

```powershell
kubectl get deployment jaeger-operator -n infrastructure
```

## Install a jaeger instance with in-memory

Next we install the simplest all-in-one instance of `jaeger` in the `infrastructure` namespace.

```powershell
kubectl apply -f ./infrastructure/jaeger/all-in-one.yaml -n infrastructure
```

We can let the operator install `jaeger` instance across multiple namespaces, with the WATCH_NAMESPACE env variable, but that won't be neccesary here.

## Install a jaeger instance with cassandra

```powershell
helm install -f ./infrastructure/jaeger/jaeger-values.yaml jaeger jaegertracing/jaeger --namespace=infrastructure
```

* Jaeger [values.yml](https://github.com/jaegertracing/helm-charts/blob/master/charts/jaeger/values.yaml)

> Jaeger alternative: [all-in-one-memory](https://github.com/jaegertracing/jaeger-kubernetes/blob/master/all-in-one/jaeger-all-in-one-template.yml)

## Expose the jaeger instance UI

```powershell
kubectl port-forward service/jaeger-query 8888:16686 --namespace infrastructure
```

or add it to [Traefik](traefik.md):

```powershell
kubectl apply -f ./infrastructure/traefik/routes/jaeger.yaml
```

## Hooking up with prometheus

First expose the admin port as a `k8s` service

```powershell
kubectl apply -f ./infrastructure/jaeger/jaeger-admin-service.yaml
```

Next let `Prometheus` monitor that service

```powershell
kubectl apply -f ./infrastructure/prometheus/jaeger-monitor.yaml
```

We should see `Prometheus` getting the metrics now.

Next we add the jaeger dashboard to `Grafana` to display those metrics.

```powershell
kubectl apply -f ./infrastructure/jaeger/jaeger-grafana-dashboard.yaml
```

> This crd was created by generating it from the json file that was fetched from grafama after import it as dashboard `10001`, adding  and annotating the CRD.

```powershell
kubectl apply configmap jaeger-dashboard --from-file=jaeger-dashboard.json=./infrastructure/jaeger/jaeger-grafana-dashboard.json -n infrastructure -o yaml > ./infrastructure/jaeger/jaeger-grafana-dashboard.yaml
kubectl label --overwrite -f ./infrastructure/jaeger/jaeger-grafana-dashboard.yaml grafana_dashboard=1
kubectl annotate --overwrite -f ./infrastructure/jaeger/jaeger-grafana-dashboard.yaml k8s-sidecar-target-directory=/tmp/dashboards/Infrastructure
```
