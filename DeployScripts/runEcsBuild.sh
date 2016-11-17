#!/bin/bash
set -ev

if [ "$BRANCH" == "dev" ]; then
    aws ecs run-task --region us-west-2 --cluster SampleBuilder --count 1 --task-definition SampleItemsAppBuilder-dev
fi

if  [ "$BRANCH" == "stage" ]; then
    aws ecs run-task --region us-west-2 --cluster SampleBuilder --count 1 --task-definition SampleItemsAppBuilder-stage
fi

if [ "$BRANCH" == "master" ]; then
    aws ecs run-task --region us-west-2 --cluster SampleBuilder --count 1 --task-definition SampleItemsAppBuilder-prod
fi
