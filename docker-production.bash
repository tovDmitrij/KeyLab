#!/bin/sh

docker compose -f compose-production.yaml stop;
docker compose -f compose-production.yaml down;
docker image rm keyboards-production-api_keyboards;
docker image rm keyboards-production-api_users;
docker image rm keyboards-production-api_email;
docker image rm keyboards-production-api_stats;
docker compose -f compose-production.yaml build;
docker compose -f compose-production.yaml -p 'keyboards-production' up -d;
echo ~~uwu~~;
