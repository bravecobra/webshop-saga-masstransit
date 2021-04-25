# Running a Jaeger instance for distributed tracing

## Using the jaeger operator

Following [the jaeger docs](https://www.jaegertracing.io/docs/1.22/operator/), we install the jaeger operator

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

Forward the jaeger instance

```powershell
kubectl port-forward service/jaeger-query 8888:16686 --namespace infrastructure
```

or add it to [Traefik](traefik.md):

```powershell
kubectl apply -f ./infrastructure/traefik/routes/jaeger.yaml
```
