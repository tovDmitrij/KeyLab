#!/bin/sh

cd ../
docker compose build;
docker compose up -d;
echo Complete!;