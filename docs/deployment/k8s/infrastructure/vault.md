# Running a Vault instance

## Installing

```powershell
helm repo add hashicorp https://helm.releases.hashicorp.com
helm repo update
```

```powershell
helm install -f infrastructure/vault/vault-values.yaml vault hashicorp/vault --namespace=infrastructure

kubectl port-forward service/vault-ui 8200:8200 --namespace infrastructure
```

* Vault [values.yml](https://github.com/hashicorp/vault-helm/blob/master/values.yaml)

## Unseal

We need to unseal the vault before we can use it.
Follow the UI to unseal by creating 5 keys, applying 3. Download the keys as json and apply 3 of the base64 keys.
Login (again) with the root token

## Create a user/pass

Create an admin policy with the content of `./infrastructure/vault/vault-admin-policy.json`.

Then enable the userpass under the auth methods and create a new user `webui` with a password and add the `admin` policy under `Generated Token's Policies`.

Now log out and login again with the new `webui` user.
