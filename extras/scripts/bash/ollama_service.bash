#!/bin/bash

systemctl list-units --type=service --state=running

SERVICE_NAME="ollama.service"

# systemctl is-enabled $SERVICE_NAME; then
# sudo systemctl enable $SERVICE_NAME
systemctl status "ollama.service"
sudo systemctl start "ollama.service"
sudo systemctl stop "ollama.service"
sudo systemctl disable "ollama.service"
