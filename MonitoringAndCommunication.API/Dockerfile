FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app 
EXPOSE 80

COPY *.sln .
COPY MonitoringAndCommunication.Microservice/*.csproj ./MonitoringAndCommunication.Microservice/
COPY MonitoringAndCommunication.Services.Contracts/*.csproj ./MonitoringAndCommunication.Services.Contracts/
COPY MonitoringAndCommunication.Services.Business/*.csproj ./MonitoringAndCommunication.Services.Business/
COPY MonitoringAndCommunication.Data.Access/*.csproj ./MonitoringAndCommunication.Data.Access/
COPY MonitoringAndCommunication.Data.Contracts/*.csproj ./MonitoringAndCommunication.Data.Contracts/
COPY MonitoringAndCommunication.Data.Object/*.csproj ./User.Data.Object/

RUN dotnet restore ./MonitoringAndCommunication.Microservice/MonitoringAndCommunication.Microservice.csproj

COPY MonitoringAndCommunication.Microservice/. ./MonitoringAndCommunication.Microservice/
COPY MonitoringAndCommunication.Services.Contracts/. ./MonitoringAndCommunication.Services.Contracts/
COPY MonitoringAndCommunication.Services.Business/. ./MonitoringAndCommunication.Services.Business/
COPY MonitoringAndCommunication.Data.Access/. ./MonitoringAndCommunication.Data.Access/
COPY MonitoringAndCommunication.Data.Contracts/. ./MonitoringAndCommunication.Data.Contracts/
COPY MonitoringAndCommunication.Data.Object/. ./MonitoringAndCommunication.Data.Object/


WORKDIR /app/MonitoringAndCommunication.Microservice
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app 
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "MonitoringAndCommunication.Microservice.dll"]