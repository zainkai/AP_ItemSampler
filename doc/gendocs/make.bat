@ECHO OFF
SET inputFiles=../ProjectOverview.md ../ProjectSetup.md ../ApplicationSettings.md ../Dependencies.md ../Buildapp.md ../Docker.md
SET output=../allDocumentation.html
SET docheader="Sample Items Website"
@ECHO ON

pandoc -H style.html --toc -V docheader:%docheader% --template=default.html5 -s -o %output% %inputFiles%