# Catalog Service

```powershell
helm install -f ./webshop/catalog/catalog-values.yaml catalog ./webshop/charts/catalog --namespace=webshop

kubectl port-forward --namespace catalog svc/catalog 80:8081
```

## Vault configuration

Configure vault to create db users

```powershell
# Login to Vault
$global:VAULT_ADDR="http://127.0.0.1:8200"
vault login -address="$global:VAULT_ADDR" -method=userpass username=webui password=webui

# get the sql server password secret from K8S
$dbpass64=kubectl get secret --namespace webshop sqlserverdb-mssql-linux-secret -o jsonpath="{.data.sapassword}"
$dbpass=[System.Text.Encoding]::ASCII.GetString([System.Convert]::FromBase64String($dbpass64))

vault auth enable -address="$global:VAULT_ADDR" approle
## Use a static DB Credential -> with the sa:$dbpass
vault secrets enable -address="$global:VAULT_ADDR" -path='Catalog' -version=2 kv
vault kv put -address="$global:VAULT_ADDR" Catalog/static "password=$dbpass"

## Use a dynamic DB Credential -> created when "vault read -address="$global:VAULT_ADDR" CatalogDB/creds/Catalog-role"
vault secrets enable -address="$global:VAULT_ADDR" -path='CatalogDB' database
vault write -address="$global:VAULT_ADDR" CatalogDB/config/Catalog-database plugin_name=mssql-database-plugin connection_url='sqlserver://{{username}}:{{password}}@mssql-linux:1433' allowed_roles="Catalog-role" username="sa" password=$dbpass

vault write -address="$global:VAULT_ADDR" CatalogDB/roles/Catalog-role db_name=Catalog-database creation_statements="CREATE LOGIN [{{name}}] WITH PASSWORD = '{{password}}'; USE Catalog; CREATE USER [{{name}}] FOR LOGIN [{{name}}]; GRANT SELECT,UPDATE,INSERT,DELETE TO [{{name}}];" default_ttl="2m" max_ttl="5m"

vault policy write -address="$global:VAULT_ADDR" Catalog ./webshop/Catalog-role-policy.hcl

vault write -address="$global:VAULT_ADDR" auth/approle/role/Catalog-role role_id="Catalog-role" token_policies="Catalog" token_ttl=1h token_max_ttl=2h secret_id_num_uses=5

Write-Output "Catalog-role" > Catalog-role-id
vault write -f -address="$global:VAULT_ADDR" -field=secret_id auth/approle/role/Catalog-role/secret-id > Catalog-secret-id

# Get a dynamic credential
vault read -address="$global:VAULT_ADDR" CatalogDB/creds/Catalog-role

# Let's inject automatically through the sidecar
#Let vault talk to kubernetes
vault auth enable kubernetes -address="$global:VAULT_ADDR"

kubectl -n infrastructure create serviceaccount vault-auth

kubectl -n infrastructure apply --filename ./infrastructure/vault-auth-serviceaccount.yaml

# Set VAULT_SA_NAME to the service account you created earlier
$VAULT_SA_NAME=kubectl -n infrastructure get sa vault-auth -o jsonpath="{.secrets[*]['name']}"
# Set SA_JWT_TOKEN value to the service account JWT used to access the TokenReview API
$SA_JWT_TOKEN64=kubectl -n infrastructure get secret $VAULT_SA_NAME -o jsonpath="{.data.token}"
$SA_JWT_TOKEN=[System.Text.Encoding]::ASCII.GetString([System.Convert]::FromBase64String($SA_JWT_TOKEN64))
# Set SA_CA_CRT to the PEM encoded CA cert used to talk to Kubernetes API
$SA_CA_CRT64=kubectl -n infrastructure get secret $VAULT_SA_NAME -o jsonpath="{.data['ca\.crt']}"
$SA_CA_CRT=[System.Text.Encoding]::ASCII.GetString([System.Convert]::FromBase64String($SA_CA_CRT64))

# Look in your cloud provider console for this value
vault write auth/kubernetes/config  -address="$global:VAULT_ADDR" token_reviewer_jwt="$SA_JWT_TOKEN" kubernetes_host="https://$KUBERNETES_PORT_443_TCP_ADDR:443" kubernetes_ca_cert="$SA_CA_CRT"
```
