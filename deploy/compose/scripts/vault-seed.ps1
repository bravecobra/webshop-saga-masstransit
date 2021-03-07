Write-Output "[*] Config local environment..."
$global:VAULT_ADDR="http://127.0.0.1:8200"

Function InitVault (){
    ((Get-Content ./deploy/compose/_data/keys.txt | Select-String -Pattern 'Initial Root Token:') -split ":\s+")[1].Replace("[0m","")
}
Function Auth(){
    Write-Output "[*] Auth..."
    docker-compose exec vault vault login -address="$global:VAULT_ADDR" $global:VAULT_TOKEN
}

function CreateWebUIUser {
    Write-Output "[*] Create user... Remember to change the defaults!!"
    docker-compose exec vault vault auth enable  -address="$global:VAULT_ADDR" userpass
    docker-compose exec vault vault policy write -address="$global:VAULT_ADDR" admin ./config/admin.hcl
    docker-compose exec vault vault write -address="$global:VAULT_ADDR" auth/userpass/users/webui password=webui policies=admin
}

function CreateBackupToken {
    Write-Output "[*] Create backup token..."
    $global:BACKUP_TOKEN = (((docker-compose exec vault vault token create -address="$global:VAULT_ADDR" -display-name="backup_token" | Select-String -Pattern 'token  ') -split "\s+") -split "\s+")[1]
    Write-Output "Backup token : $global:BACKUP_TOKEN"
    Write-Output "$global:BACKUP_TOKEN" > ./deploy/compose/_data/backup.txt
    Write-Output "[*] Creating new mount point..."
}

function CreateDatabaseConnectionStrings {
    docker-compose exec vault vault auth enable -address="$global:VAULT_ADDR" approle
    docker-compose exec vault vault secrets enable -address="$global:VAULT_ADDR" -path='Catalog' -version=2 kv
    docker-compose exec vault vault kv put -address="$global:VAULT_ADDR" Catalog/static 'password=Password_123'

    docker-compose exec vault vault secrets enable -address="$global:VAULT_ADDR" -path='Catalog' database
    docker-compose exec vault vault write -address="$global:VAULT_ADDR" Catalog/config/Catalog-database \
        plugin_name=mssql-database-plugin \
        connection_url='sqlserver://{{username}}:{{password}}@sqlserverdb:1433' \
        allowed_roles="Catalog-role" \
        username="sa" \
        password="Password_123"
    docker-compose exec vault vault write -address="$global:VAULT_ADDR" Catalog/roles/Catalog-role \
        db_name=Catalog \
        creation_statements="CREATE LOGIN [{{name}}] WITH PASSWORD = '{{password}}'; USE Catalog; CREATE USER [{{name}}] FOR LOGIN [{{name}}]; GRANT SELECT,UPDATE,INSERT,DELETE TO [{{name}}];" \
        default_ttl="2m" \
        max_ttl="5m"
    docker-compose exec vault vault policy write -address="$global:VAULT_ADDR" Catalog ./Catalog-role-policy.hcl
    docker-compose exec vault vault write -address="$global:VAULT_ADDR" auth/approle/role/Catalog-role \
        role_id="Catalog-role" \
        token_policies="Catalog" \
        token_ttl=1h \
        token_max_ttl=2h \
        secret_id_num_uses=5
    Write-Output "Catalog-role" > Catalog/vault-agent/role-id
    docker-compose exec vault vault write -f -field=secret_id auth/approle/role/Catalog-role/secret-id > Catalog/vault-agent/secret-id
}

# function CreateMountAssessment {
#     ## MOUNTS
#     docker-compose exec vault vault secrets list -address="$global:VAULT_ADDR"
#     docker-compose exec vault vault secrets enable -address="$global:VAULT_ADDR" -path=assessment -description="Secrets used in the assessment" generic
#     docker-compose exec vault vault write -address="$global:VAULT_ADDR" assessment/server1_ad value1=name value2=pwd
# }

$global:VAULT_TOKEN = InitVault
Auth
CreateWebUIUser
CreateBackupToken
#CreateMountAssessment