#!/bin/bash
set -ev

if [ "$BRANCH" == "dev" ]; then
    aws ecs run-task --region us-west-2 --cluster SampleItemsBuild --count 1 --task-definition SampleItemsBuild_dev
fi

if  [ "$BRANCH" == "stage" ]; then
    aws ecs run-task --region us-west-2 --cluster SampleItemsBuild --count 1 --task-definition SampleItemsBuild_stage
fi

if [ "$BRANCH" == "master" ]; then
    aws ecs run-task --region us-west-2 --cluster SampleItemsBuild --count 1 --task-definition SampleItemsBuild_master
fi
