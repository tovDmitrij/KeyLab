#!/bin/sh

docker compose -p keylab-dev stop;
docker compose -p keylab-dev down;
docker image rm keylab-dev-api_keyboards
docker image rm keylab-dev-api_users;
docker image rm keylab-dev-api_email;
docker image rm keylab-dev-api_stats;
docker compose -f compose-dev.yaml -p 'keylab-dev' build;
docker compose -f compose-dev.yaml -p 'keylab-dev' up -d;
echo ~~uwu~~;
