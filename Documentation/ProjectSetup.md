## Project Setup

### General Steps
- Clone the project
    - Don't forget to initialize the project submodules ([instructions](https://git-scm.com/book/en/v2/Git-Tools-Submodules#_cloning_submodules))
- Install the latest version of Microsoft .NET Core for your operating system ([link](https://www.microsoft.com/net/download/core#/current))
- Install the latest TypeScript compiler ([link](https://www.typescriptlang.org/index.html#download-links))
- Download the latest sample item content package
    - Place this in the location specified in src/Web/appsettings.json (`ContentRootDirectory`)


## Running
#### Using Visual Studio 2015
- Open the project in Visual Studio 2015
- Click the run button


#### Using Command Line
- Change directories into `SampleItemsWebsite\SmarterBalanced.SampleItems`
- Run `dotnet restore`
- `cd src\SmarterBalanced.SampleItems.Web`
- Set environment variable `ASPNETCORE_ENVIRONMENT` to `Development`
    - in Windows: `setx ASPNETCORE_ENVIRONMENT "Development"`, then close and reopen the command prompt
- Run `dotnet run` in `src\SmarterBalanced.SampleItems.Web` to run the project
- Navigate to `http://localhost:<port>` in your browser to view the running site
