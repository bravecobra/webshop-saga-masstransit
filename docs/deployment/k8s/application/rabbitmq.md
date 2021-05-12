# RabbitMQ

Installing is done by:

```powershell
helm install -f ./webshop/rabbitmq/rabbitmq-values.yaml rabbitmq bitnami/rabbitmq --namespace webshop

kubectl port-forward --namespace webshop svc/rabbitmq 15672:15672
```

RabbitMQ [values.yaml](https://github.com/bitnami/charts/blob/master/bitnami/rabbitmq/values.yaml)

Credentials (in bash):

```bash
echo "Username      : user"
echo "Password      : $(kubectl get secret --namespace webshop rabbitmq -o jsonpath="{.data.rabbitmq-password}" | base64 --decode)"
echo "ErLang Cookie : $(kubectl get secret --namespace webshop rabbitmq -o jsonpath="{.data.rabbitmq-erlang-cookie}" | base64 --decode)"
```

If you ever ran this before, deleting the release will not remove the Persistent Volume Claim. So starting it again will re-use it, thus using the old credentials as well. So make sure you delete the old PVC, before installing the new release.

To upgrade an existing helm release you need to pass the ERLANG_COOKIE like this:

```powershell
helm upgrade -f ./webshop/rabbitmq-values.yaml rabbitmq bitnami/rabbitmq --namespace webshop --set auth.erlangCookie="VALUE_OF_ERLANG_COOKIE"
```
