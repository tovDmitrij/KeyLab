#!/bin/sh

docker compose -p keyboards-production stop;
docker compose -p keyboards-production down;
docker image rm keyboards-production-client;
docker image rm keyboards-production-api_keyboards;
docker image rm keyboards-production-api_users;
docker image rm keyboards-production-api_email;
docker image rm keyboards-production-api_stats;
docker compose -f compose-production.yaml -p 'keyboards-production' build;
docker compose -f compose-production.yaml -p 'keyboards-production' up -d;
echo ~~uwu~~;
