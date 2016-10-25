#!/bin/bash
set -ev

dotnet publish /home/travis/build/osu-cass/SampleItemsWebsite/SmarterBalanced.SampleItems/src/SmarterBalanced.SampleItems.Web/project.json
mv /home/travis/build/osu-cass/SampleItemsWebsite/SmarterBalanced.SampleItems/src/SmarterBalanced.SampleItems.Web/bin/Debug/netcoreapp1.0/publish SampleItemsWebsite
archivename="sampleitemswebsite-${TRAVIS_BRANCH}-latest"
zip -r $archivename appspec.yml CodeDeploy SampleItemsWebsite
mkdir -p dpl_cd_upload
mv "${archivename}.zip" dpl_cd_upload/"${archivename}.zip"