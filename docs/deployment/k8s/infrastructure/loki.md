# Grafana Loki

## Installation

### Loki

```powershell
helm upgrade --install loki --namespace=infrastructure grafana/loki -f ./infrastructure/loki/loki-values.yaml
helm upgrade --install promtail --namespace=infrastructure grafana/promtail -f ./infrastructure/loki/promtail-values.yaml
```

The default values for Loki can be found at [github](https://github.com/grafana/helm-charts/blob/main/charts/loki/values.yaml)
The default values for Promtail can be found at [github](https://github.com/grafana/helm-charts/blob/main/charts/promtail/values.yaml)

Loki should also be available as a service in consul, so we can start sending logs from applications (like Serilog).

We also added `Loki` as an additional dataSource in `Grafana`, when installing [Prometheus](./prometheus.md)

## Add grafana dashboards

### Loki-Promtail

We can add a Loki-Promtail dashboard (ID: 10004)

> We create the dashboard yaml as follows, just for reference in case we need to recreate it

```powershell
kubectl create configmap loki-promtail-dashboard --from-file=loki-promtail-dashboard.json=./infrastructure/loki/loki-promtail-dashboard.json -n infrastructure -o yaml > ./infrastructure/loki/loki-promtail-dashboard.yaml
kubectl label --overwrite -f ./infrastructure/loki/loki-promtail-dashboard.yaml grafana_dashboard=1
kubectl annotate --overwrite -f ./infrastructure/loki/loki-promtail-dashboard.yaml k8s-sidecar-target-directory=/tmp/dashboards/Infrastructure
```

### Loki stack monitoring

We can add a Loki stack monitoring dashboard (ID: 14055)

```powershell
kubectl create configmap loki-monitor-dashboard --from-file=loki-monitor-dashboard.json=./infrastructure/loki/loki-monitor-dashboard.json -n infrastructure -o yaml > ./infrastructure/loki/loki-monitor-dashboard.yaml
kubectl label --overwrite -f ./infrastructure/loki/loki-monitor-dashboard.yaml grafana_dashboard=1
kubectl annotate --overwrite -f ./infrastructure/loki/loki-monitor-dashboard.yaml k8s-sidecar-target-directory=/tmp/dashboards/Infrastructure
```

13865
Import dashboard:
  Loki Logs with quicksearch: 13359, 12019
  Traefik with loki: 13713
