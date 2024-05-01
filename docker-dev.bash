#!/bin/sh

docker compose -f compose-dev.yaml stop;
docker compose -f compose-dev.yaml down;
docker image rm keyboards-dev-api_keyboards;
docker image rm keyboards-dev-api_users;
docker image rm keyboards-dev-api_email;
docker image rm keyboards-dev-api_stats;
docker compose -f compose-dev.yaml build;
docker compose -f compose-dev.yaml -p 'keyboards-dev' up -d;
echo ~~uwu~~;
