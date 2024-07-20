FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app 
EXPOSE 80

COPY *.sln .
COPY Device.Microservice/*.csproj ./Device.Microservice/
COPY Device.Services.Contracts/*.csproj ./Device.Services.Contracts/
COPY Device.Services.Business/*.csproj ./Device.Services.Business/
COPY Device.Data.Access/*.csproj ./Device.Data.Access/
COPY Device.Data.Contracts/*.csproj ./Device.Data.Contracts/
COPY Device.Data.Object/*.csproj ./Device.Data.Object/

RUN dotnet restore ./Device.Microservice/Device.Microservice.csproj

COPY Device.Microservice/. ./Device.Microservice/
COPY Device.Services.Contracts/. ./Device.Services.Contracts/
COPY Device.Services.Business/. ./Device.Services.Business/
COPY Device.Data.Access/. ./Device.Data.Access/
COPY Device.Data.Contracts/. ./Device.Data.Contracts/
COPY Device.Data.Object/. ./Device.Data.Object/


WORKDIR /app/Device.Microservice
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app 
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Device.Microservice.dll"]