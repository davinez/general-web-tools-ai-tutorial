#!/bin/bash

SERVICE_NAME="ollama.service"

# Check if the service is enabled
if ! systemctl is-enabled $SERVICE_NAME; then
    # Enable the service if not enabled
    sudo systemctl enable $SERVICE_NAME
fi

# Start the service
sudo systemctl start $SERVICE_NAME
