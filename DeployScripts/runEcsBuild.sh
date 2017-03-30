#!/bin/bash
set -ev

#Do nothing if this is a pull request.
if [ "$TRAVIS_PULL_REQUEST" == "true" ]; then exit 0 fi

Region="us-west-2"
aws ecr get-login --region $Region | source /dev/stdin

if [ ( "$BRANCH" == "feature_travis-docker" ] || [ "$BRANCH" == "dev" ] || [ "$BRANCH" == "stage" ] || [ "$BRANCH" == "master" ) && "$TRAVIS_PULL_REQUEST" == "false" ]
then
    ServiceName="SampleItemsWebsite-dev"
    if [ "$BRANCH" == "dev" ]
    then
        ServiceName="SampleItemsWebsite-dev"
        ClusterName="SampleItemsWebsite-dev"
    elif [ "$BRANCH" == "stage" ]
    then
        ServiceName="SampleItemsWebsite-stage"
        ClusterName="SampleItemsWebsite-stage"
    elif [ "$BRANCH" == "master" ]
    then
        ServiceName="SampleItemsWebsite-prod"
        ClusterName="SampleItemsWebsite-prod"
    elif [ "$BRANCH" == "feature_travis-docker" ]  # For testing
    then
        ServiceName="SampleItemsWebsite-dev"
        ClusterName="SampleItemsWebsite-dev"
    fi

    cd /home/travis/build/osu-cass/SampleItemsWebsite/publish/
    mkdir content
    wget https://s3-us-west-2.amazonaws.com/cass-sb-itemviewerservice-content/content.zip
    unzip -o content.zip -d content/ &> /dev/null
    rm content.zip 
    docker build -t sampleitemsapp .
    docker tag sampleitemsapp:latest osucass/sampleitemsapp:$BRANCH
    docker push osucass/sampleitemsapp:$BRANCH
    ecs deploy $ClusterName $ServiceName --region $Region
fi