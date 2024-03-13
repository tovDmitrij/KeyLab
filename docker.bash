#!/bin/sh

docker compose stop;
docker compose down;
docker image rm keyboards-api_mail;
docker image rm keyboards-api_email;
docker image rm keyboards-api_preview;
docker compose build;
docker compose up -d;
echo Complete!;