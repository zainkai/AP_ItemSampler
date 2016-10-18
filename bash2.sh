#!bin/bash
set -e
dotnet restore
cp /content_mnt -r /content
cp /config_mnt -r /content
rm -rf $(pwd)/publish/web
dotnet publish src/SmarterBalanced.SampleItems.Web/project.json -c release -o $(pwd)/publish/web