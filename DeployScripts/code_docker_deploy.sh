#!/bin/bash
set -ev

cd publish/
docker build -t sampleitemscode .
docker tag sampleitemscode:latest osucass/sampleitemscode:$BRANCH
docker push osucass/sampleitemscode:$BRANCH