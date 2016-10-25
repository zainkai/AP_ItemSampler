#!bin/bash
set -e
mkdir content
#get ec2 content
aws s3 cp $BUCKET/$ContentZip $ContentZip
unzip -o $ContentZip -d content/
rm -f $ContentZip
aws ecr get-login --region us-west-2 | source /dev/stdin

#Docker get code image
docker pull $SampleItemsCodeRepo:$SampleItemsCodeRepoTag
docker tag $SampleItemsCodeRepo:$SampleItemsCodeRepoTag codebaserepo
#Docker build app
docker build -t $IMAGE_NAME:$TAG .
docker tag $IMAGE_NAME:$TAG $REPO$IMAGE_NAME:$TAG
docker push $REPO$IMAGE_NAME:$TAG

#Update running ECS tasks
aws ecs update-service --region us-west-2 --service $SERVICE_NAME --cluster $ECS_CLUSTER
