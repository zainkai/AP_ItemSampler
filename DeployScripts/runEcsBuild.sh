#!/bin/bash
set -ev

if [ "$TRAVS_BRANCH" == "dev" ]; then
    aws ecs run-task --region us-west-2 --cluster SampleItemsBuild --count 1 --task-definition SampleItemsBuild_dev
fi

if  [ "$TRAVS_BRANCH" == "stage" ]; then
    aws ecs run-task --region us-west-2 --cluster SampleItemsBuild --count 1 --task-definition SampleItemsBuild_stage
fi

if [ "$TRAVS_BRANCH" == "master" ]; then
    aws ecs run-task --region us-west-2 --cluster SampleItemsBuild --count 1 --task-definition SampleItemsBuild_master
fi

if [ "$TRAVS_BRANCH" == "feature_dockeraws" ]; then
    aws ecs run-task --region us-west-2 --cluster SampleItemsBuild --count 1 --task-definition SampleItemsBuild_dev
fi
