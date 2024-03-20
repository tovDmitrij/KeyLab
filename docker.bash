#!/bin/sh

docker compose stop;
docker compose down;
docker image rm keyboards-api_main;
docker image rm keyboards-api_email;
docker compose build;
docker compose up -d;
#docker image prune -a -f;
echo ~~uwu~~;
