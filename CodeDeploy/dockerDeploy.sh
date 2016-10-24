#!/bin/bash
set -ev

if [$TRAVIS_BRANCH == 'dev'] || [$TRAVIS_BRANCH == 'stage'] || [$TRAVIS_BRANCH == 'master']; then
    docker --version
    pip install --user awscli
    export PATH=$PATH:$HOME/.local/bin
    eval $(aws ecr get-login --region us-west-2)
    dotnet restore /home/travis/build/osu-cass/SampleItemsWebsite/SmarterBalanced.SampleItems/
    dotnet publish /home/travis/build/osu-cass/SampleItemsWebsite/SmarterBalanced.SampleItems/src/SmarterBalanced.SampleItems.Web/project.json
    cd /home/travis/build/osu-cass/SampleItemsWebsite/SmarterBalanced.SampleItems/src/SmarterBalanced.SampleItems.Web/bin/Debug/netcoreapp1.0/publish
    docker build -t sampleitemswebsite .
    docker tag sampleitemswebsite:latest 047189625337.dkr.ecr.us-west-2.amazonaws.com/sampleitemswebsite:$TRAVIS_BRANCH
    docker push 047189625337.dkr.ecr.us-west-2.amazonaws.com/sampleitemswebsite:$TRAVIS_BRANCH
fi

if [$TRAVS_BRANCH == 'dev'] then;
    aws ecs run-task --region us-west-2 --cluster SampleItemsBuild --count 1 --task-definition SampleItemsBuild_dev
fi

if [$TRAVS_BRANCH == 'stage'] then;
    aws ecs run-task --region us-west-2 --cluster SampleItemsBuild --count 1 --task-definition SampleItemsBuild_stage
fi

if [$TRAVS_BRANCH == 'master'] then;
    aws ecs run-task --region us-west-2 --cluster SampleItemsBuild --count 1 --task-definition SampleItemsBuild_master
fi