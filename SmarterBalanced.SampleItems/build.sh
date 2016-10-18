#!bin/bash
set -e
dotnet restore
rm -rf /app/publish/
dotnet publish src/SmarterBalanced.SampleItems.Web/project.json -c release -o /app/publish/web
cp AccessibilityAccommodationConfigurations/AccessibilityConfig.xml /app/publish/web/configuration

#get ec2 content
mkdir -p /app/publish/web/content
aws s3 cp s3://cass-sb-itemviewerservice-content/content.zip content.zip
unzip content.zip -d /app/publish/web/content/
rm content.zip

#Docker build app
cd /app/publish/web/
aws ecr get-login --region us-west-2
docker build -t sampleitemsapp .
docker tag sampleitemsapp:latest 047189625337.dkr.ecr.us-west-2.amazonaws.com/sampleitemsapp:latest
docker push 047189625337.dkr.ecr.us-west-2.amazonaws.com/sampleitemsapp:latest
