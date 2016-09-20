#!/bin/bash
set -e
sudo nginx -s reload
sudo service supervisor restart