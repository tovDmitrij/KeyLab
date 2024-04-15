#!/bin/sh

docker compose stop;
docker compose down;
docker image rm keyboards-api_main;
docker image rm keyboards-api_email;
docker image rm keyboards-api_stats;
docker image rm keyboards-client;
docker compose build;
docker compose up -d;
echo ~~uwu~~;
