# Welcome to the Smarter Balanced Sample Items Website
The Sample Items Website .NET Core MVC Web Application. It allows users 
to quickly search for and view sample test question items provided by the 
Smarter Balanced Assessment Consortium.

## Unit Tests
Branch| Build
--- | --- |
Master | [![Build Status](https://travis-ci.org/osu-cass/SampleItemsWebsite.svg?branch=master)](https://travis-ci.org/osu-cass/SampleItemsWebsite)
Stage | [![Build Status](https://travis-ci.org/osu-cass/SampleItemsWebsite.svg?branch=stage)](https://travis-ci.org/osu-cass/SampleItemsWebsite) 
Dev  | [![Build Status](https://travis-ci.org/osu-cass/SampleItemsWebsite.svg?branch=dev)](https://travis-ci.org/osu-cass/SampleItemsWebsite)


## Getting Started
Below is our recommended development environment, although it is not required.
- Windows 10
- Visual Studio Enterprise 2015

## Installation
- Clone the project
    - Don't forget to initialize the project submodules ([instructions](https://git-scm.com/book/en/v2/Git-Tools-Submodules#_cloning_submodules))
- Install Microsoft .NET Core for your operating system ([link](https://www.microsoft.com/net/download/core#/current))
- Install the latest TypeScript compiler ([link](https://www.typescriptlang.org/index.html#download-links))
- Download the latest sample item content package (TODO: Link?)
    - Place this in the location specified in src/Web/appsettings.json (`ContentRootDirectory`)

### Running

#### Using Visual Studio 2015
- Open the project in Visual Studio 2015
- Click the run button

TODO: Add steps for docker, vs code, command line...?

## Contribute
* [Fork It](https://help.github.com/articles/fork-a-repo/)
* [Submit Pull Request](https://help.github.com/articles/about-pull-requests/)
