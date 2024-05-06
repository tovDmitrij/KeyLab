#!/bin/sh

docker compose -p keyboards-dev stop;
docker compose -p keyboards-dev down;
docker image rm keyboards-dev-client;
docker image rm keyboards-dev-api_keyboards;
docker image rm keyboards-dev-api_users;
docker image rm keyboards-dev-api_email;
docker image rm keyboards-dev-api_stats;
docker compose -f compose-dev.yaml -p 'keyboards-dev' build;
docker compose -f compose-dev.yaml -p 'keyboards-dev' up -d;
echo ~~uwu~~;
