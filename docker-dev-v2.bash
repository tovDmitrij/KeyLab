#!/bin/sh

docker compose -p keylab-dev stop;
docker compose -p keylab-dev down;
docker compose -f compose-dev.yaml -p 'keylab-dev' build;
docker compose -f compose-dev.yaml -p 'keylab-dev' up -d;
echo ~~uwu~~;
