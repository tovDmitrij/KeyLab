#!/bin/sh

docker compose -p keylab-production stop;
docker compose -p keylab-production down;
docker image rm keylab-production-api_keyboards;
docker image rm keylab-production-api_users;
docker image rm keylab-production-api_email;
docker image rm keylab-production-api_stats;
docker compose -f compose-production.yaml -p 'keylab-production' build;
docker compose -f compose-production.yaml -p 'keylab-production' up -d;
echo ~~uwu~~;
