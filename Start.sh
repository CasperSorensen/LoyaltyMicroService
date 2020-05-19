#!/bin/bash

sudo docker-compose -f "docker-compose.yml" -f "docker-compose.override.yml" -f "docker-compose.efk.yml" -p loy_serv up
