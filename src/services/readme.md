How to run easily

## DB
docker run \
  --name postgres-mmgmt \
  -e POSTGRES_DB=mmgmt \
  -e POSTGRES_USER=admin \
  -e POSTGRES_PASSWORD=admin \
  -p 5432:5432 \
  postgres:latest

docker run -d \
  --name pgadmin-mmgmt \
  -e PGADMIN_DEFAULT_EMAIL=admin@admin.com \
  -e PGADMIN_DEFAULT_PASSWORD=admin \
  -p 8080:80 \
  --link postgres-mmgmt:postgres \
  dpage/pgadmin4

## Rabbit MQ
docker run \
  --name rabbitmq-mmgmt \
  -e RABBITMQ_DEFAULT_USER=guest \
  -e RABBITMQ_DEFAULT_PASS=guest \
  -p 5672:5672 \
  -p 15672:15672 \
  rabbitmq:management


## Redis
docker run -d \
  --name redis-mmgmt \
  -p 6379:6379 \
  redis:latest