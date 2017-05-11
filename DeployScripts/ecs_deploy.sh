#!/bin/bash
set -ev

ServiceName=$1
ClusterName=$2

if [ -n "$AWS_ACCESS_KEY_ID" ] && [ -n "$ServiceName" ] && [ -n "$ClusterName" ]; then
    Region="us-west-2"
    aws ecr get-login --region $Region | source /dev/stdin
    ecs deploy $ClusterName $ServiceName --region $Region
fi