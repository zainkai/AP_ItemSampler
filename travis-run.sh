#!/bin/bash
set -e

PROJECT_DIR="./app/src/app"

dotnet restore
dotnet run -p $PROJECT_DIR &

#get the pid
pid=$!

#Shut it down
kill -s INT $pid
wait