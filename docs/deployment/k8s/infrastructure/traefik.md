# Traefik

## Installing

To setup traefik, we deploy the helm chart from [https://github.com/traefik/traefik-helm-chart](https://github.com/traefik/traefik-helm-chart):

```powershell
helm repo add traefik https://helm.traefik.io/traefik
helm repo update
```

We'll install it into the `default` namespace.

Generate the default certificate secret from the self-signed certificate generated in the [Intro](./index.md):

```powershell
kubectl create secret tls traefik-cert --cert=./infrastructure/certs/k8s.local.crt --key=./infrastructure/certs/k8s.local.key
```

> TODO: can we deploy it to it's own namespace and still serve any ingress of the cluster

```powershell
helm install traefik traefik/traefik -f ./infrastructure/traefik/traefik-values.yaml
```

To expose the dashboard

```powershell
kubectl apply -f ./infrastructure/traefik/routes/dashboard.yaml
```

It should now be accessible through [https://traefik.infrastructure.k8s.local/dashboard](https://traefik.infrastructure.k8s.local/dashboard)

## Hooking up prometheus

We added the additional arguments in `./infrastructure/traefik/traefik-values.yaml` to add Prometheus for the metrics. This adds an extra rules to expose the `/metrics` path.

Now expose a service for prometheus to fetch metrics from

```powershell
kubectl apply -f ./infrastructure/traefik/traefik-dashboard-service.yaml
```

Then instruct prometheus to monitor that service

```powershell
kubectl apply -f ./infrastructure/prometheus/traefik-monitor.yaml
```

Now setup the alert manager to alert when there are too many requests through a rule

```powershell
kubectl apply -f ./infrastructure/prometheus/traefik-monitor.yaml
```

Next we add the Traefik dashboard to `Grafana` to display those metrics.

```powershell
kubectl apply -f ./infrastructure/traefik/traefik-grafana-dashboard.yaml
```

> This crd was created by generating it from the json file that was fetched from grafana after import it as dashboard `11462`, adding and annotating the CRD.
>
> ```powershell
>kubectl apply configmap traefik-dashboard --from-file=traefik-dashboard.json=./infrastructure/traefik/traefik-grafana-dashboard.json -n infrastructure -o yaml > ./infrastructure/traefik/traefik-grafana-dashboard.yaml
>kubectl label --overwrite -f ./infrastructure/traefik/traefik-grafana-dashboard.yaml grafana_dashboard=1
>kubectl annotate --overwrite -f ./infrastructure/traefik/traefik-grafana-dashboard.yaml k8s-sidecar-target-directory=/tmp/dashboards/Infrastructure
> ```

## Hooking up jaeger for tracing

Once [Jaeger](jaeger.md) is installed, we can add tracing to the `traefik` instance.

we do that by passing 3 extra startup arguments:

```powershell
- "--tracing.jaeger=true"
- "--tracing.jaeger.samplingServerURL=http://jaeger-agent.infrastructure.svc:5778/sampling"
- "--tracing.jaeger.localAgentHostPort=jaeger-agent.infrastructure.svc:6831"
```

and then applying those

```powershell
helm upgrade traefik traefik/traefik -f ./infrastructure/traefik/traefik-values.yaml
```

## References

<https://traefik.io/blog/capture-traefik-metrics-for-apps-on-kubernetes-with-prometheus/>
<https://traefik.io/blog/application-request-tracing-with-traefik-and-jaeger-on-kubernetes/>
