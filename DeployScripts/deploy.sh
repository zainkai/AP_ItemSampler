#!/bin/bash
set -ev

ServiceName=$1
ClusterName=$2

Region="us-west-2"
aws ecr get-login --region $Region | source /dev/stdin

cd /home/travis/build/osu-cass/SampleItemsWebsite/publish/
mkdir content
wget "$CONTENT_PACKAGE_URL" -O content.zip
unzip -o content.zip -d content/ &> /dev/null
rm content.zip 
docker build -t sampleitemsapp .
docker tag sampleitemsapp:latest osucass/sampleitemsapp:$BRANCH
docker push osucass/sampleitemsapp:$BRANCH
ecs deploy $ClusterName $ServiceName --region $Region
