#!bin/bash
set -e

##remove publish files
rm -rf /app/publish/

##create directories in publish
mkdir -p /app/publish/web/content
mkdir -p /app/publish/web/configuration

##run dotnet 
dotnet restore
dotnet publish src/SmarterBalanced.SampleItems.Web/project.json -c release -o /app/publish/web
cp AccessibilityAccommodationConfigurations/AccessibilityConfig.xml /app/publish/web/configuration/AccessibilityConfig.xml

#get ec2 content

aws s3 cp s3://cass-sb-itemviewerservice-content/content.zip content.zip
unzip -o content.zip -d /app/publish/web/content/
echo "unzipped"
rm -f content.zip
echo "removed content.zip"

#Docker build app
aws ecr get-login --region us-west-2 | source /dev/stdin
docker build -t sampleitemsapp /app/publish/web/.
docker tag sampleitemsapp:latest 047189625337.dkr.ecr.us-west-2.amazonaws.com/sampleitemsapp:latest
docker push 047189625337.dkr.ecr.us-west-2.amazonaws.com/sampleitemsapp:latest
