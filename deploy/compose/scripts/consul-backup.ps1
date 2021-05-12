Write-Output "[*] Building docker for consul backup..."
docker build -f ./src/consul_backup/Dockerfile ./src/consul_backup/
Write-Output "[*] Executing backup..."
$backup_token = Get-Content ./deploy/compose/_data/backup.txt
docker-compose run --rm consul-backup consul-backup -i consul:8500 -t $backup_token backup_$(Get-date -Format yyyyMMddHHmm)
