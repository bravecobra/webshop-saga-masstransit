# Traefik

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
