#!/bin/sh

cd ../
docker compose stop;
docker compose down;
docker image rm keyboards-api;
docker compose build;
docker compose up -d;
echo Complete!;