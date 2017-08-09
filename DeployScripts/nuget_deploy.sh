#!/bin/bash
set -ev

Suffix="$1"

cd $TRAVIS_BUILD_DIR/SmarterBalanced.SampleItems/src/SmarterBalanced.SampleItems.Dal
if [ -z "$SUFFIX" ]; then
    dotnet pack -o .
else
    dotnet pack --version-suffix "$Suffix" -o .
fi

nuget push *.nupkg -Source https://www.nuget.org/api/v2/package