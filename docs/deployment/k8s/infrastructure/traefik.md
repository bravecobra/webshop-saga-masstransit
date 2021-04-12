# Traefik

## Installing

To setup traefik, we deploy the helm chart from [https://github.com/traefik/traefik-helm-chart](https://github.com/traefik/traefik-helm-chart):

We'll install it into the `default` namespace.

```powershell
helm install traefik traefik/traefik -f ./infrastructure/traefik/traefik-values.yaml
```

To expose the dashboard

```powershell
kubectl apply -f ./infrastructure/traefik/routes/dashboard.yaml
```

It should now be accessible through [http://traefik.localhost/dashboard](http://traefik.localhost/dashboard)

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

During the setup of prometheus, we already added the [Traefik dashboard](http://grafana.localhost/d/3ipsWfViz/traefik-2?var-job=traefik-dashboard&var-protocol=http&var-interval=&from=now-3h&to=now) in grafana.

## References

https://traefik.io/blog/capture-traefik-metrics-for-apps-on-kubernetes-with-prometheus/