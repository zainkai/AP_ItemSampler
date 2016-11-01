FROM microsoft/aspnetcore:1.0.1
ARG source=.
WORKDIR /app
COPY $source .
EXPOSE 80
ENTRYPOINT ["dotnet", "SmarterBalanced.SampleItems.Web.dll"]

