#!/bin/bash

SERVICE_NAME="ollama.service"

# Stop the service
sudo systemctl stop $SERVICE_NAME

# Disable the service
sudo systemctl disable $SERVICE_NAME
