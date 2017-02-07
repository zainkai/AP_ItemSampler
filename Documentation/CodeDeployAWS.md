Code Deploy is triggered by Travis CI using the deploy section in the `.travis.yml` file. When the build on the dev, stage, or master branch is successful the branch specific deployment is run.  
The Travis build publishes the application using `dotnet publish`, and zips it into an archive, then pushes it to an Amazon S3 bucket, then triggers a Code Deploy deployment. The Code Deploy deployment deploys the application to a running EC2 instance that was set up using the instructions below. 

### EC2 Instance setup
The EC2 instance running the Sample Items Website needs an IAM role granting it read permissions to S3 and full access to Code Deploy.
It requires [Nginx](https://www.nginx.com/resources/wiki/), [Supervisor], .NET Core, and the Amazon Code Deploy Agent to be installed on the instance.
Nginx is used for reverse proxy. 

Nginx and Supervisor can be installed with apt.
```sh
sudo apt-get update
sudo apt-get install nginx
sudo apt-get install supervisor
```
.NET Core can be installed with apt after adding the the `dotnet-release` repo.
```sh
sudo sh -c 'echo "deb [arch=amd64] https://apt-mo.trafficmanager.net/repos/dotnet-release/ trusty main" > /etc/apt/sources.list.d/dotnetdev.list'
sudo apt-key adv --keyserver apt-mo.trafficmanager.net --recv-keys 417A0893
sudo apt-get update
sudo apt-get install dotnet-dev-1.0.0-preview2-003131
```
The install steps for .NET Core may change so please see [Microsoft's instructions](https://www.microsoft.com/net/core#ubuntu) if the above does not work.  
Please see [Amazon's instructions](https://docs.aws.amazon.com/codedeploy/latest/userguide/how-to-run-agent-install.html) for installing the code deploy agent.

All together in one script that can be run when the instance is created:
```sh
#!/bin/bash
set -ev

sudo apt-get -y update
sudo apt-get -y install nginx
sudo apt-get -y install supervisor
sudo sh -c 'echo "deb [arch=amd64] https://apt-mo.trafficmanager.net/repos/dotnet-release/ trusty main" > /etc/apt/sources.list.d/dotnetdev.list'
sudo apt-key adv --keyserver apt-mo.trafficmanager.net --recv-keys 417A0893
sudo apt-get -y update
sudo apt-get -y install dotnet-dev-1.0.0-preview2-003131
sudo apt-get -y install python-pip
sudo apt-get -y install ruby2.0
sudo apt-get -y install wget
wget https://aws-codedeploy-us-west-2.s3.amazonaws.com/latest/install
chmod +x ./install
sudo ./install auto
```

#### IAM Role
Create an IAM role with the AmazonS3ReadOnlyAccess and AWSCodeDeployFullAccess policies.
Add the following custom policy:
```json
{
    "Statement": [
        {
            "Action": [
                "autoscaling:Describe*",
                "cloudformation:Describe*",
                "cloudformation:GetTemplate",
                "s3:Get*"
            ],
            "Resource": "*",
            "Effect": "Allow"
        }
    ]
}
```
#### What to install
In order to deploy and run the Sample Items website the instance must have the following installed.

- [AWS Code Deploy Agent](https://docs.aws.amazon.com/codedeploy/latest/userguide/how-to-run-agent-install.html)
- [.NET Core](https://www.microsoft.com/net/core#ubuntu)
- Nginx
- Supervisor

### Code Deploy Setup
 - In the AWS web console navigate to Code Deploy
 - Click "Create New Application"
 - Choose an application name. This must match the `application` used in the `.travis.yml` file under the `codedeploy` provider.
 - Choose a deployment group name. This must match the `deployment_group` used in the `.travis.yml` file under the `codedeploy` provider.
 - Under add instances add the EC2 instance you set up above.
 - Leave the default "Deployment Configuration" on "CodeDeployDefault.OneAtATime"
 - Set the Service Role to IAM role you created above.
 - Choose "Create Application"

### S3 setup
Create an S3 bucket to store the zip archive created by the Travis CI build. The name and region must match those specified in the `s3` provider in the `.travis.yml` file.

### appspec.yml
The appspec  is the file that defines how the code deploy agent deploys the application. For the Sample Items Website it copies the configuration files for Nginx and Supervisor, and the application into the correct places, then starts Nginx and Supervisor to run the application.