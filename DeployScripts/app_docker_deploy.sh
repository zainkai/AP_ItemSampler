#!/bin/bash
set -ev

cd publish/
mkdir content
wget -q "$CONTENT_PACKAGE_URL" -O content.zip
unzip -o content.zip -d content/ &> /dev/null
rm content.zip 
docker build -t sampleitemsapp .
docker tag sampleitemsapp:latest osucass/sampleitemsapp:$BRANCH
docker push osucass/sampleitemsapp:$BRANCH
