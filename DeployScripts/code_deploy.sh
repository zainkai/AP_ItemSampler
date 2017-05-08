#!/bin/bash
set -ev

cd /home/travis/build/osu-cass/SampleItemsWebsite/publish/
docker build -t sampleitemscode .
docker tag sampleitemscode:latest osucass/sampleitemscode:$BRANCH
docker push osucass/sampleitemscode:$BRANCH