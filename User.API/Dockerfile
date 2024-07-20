FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app 
EXPOSE 80

COPY *.sln .
COPY User.Microservice/*.csproj ./User.Microservice/
COPY User.Services.Contracts/*.csproj ./User.Services.Contracts/
COPY User.Services.Business/*.csproj ./User.Services.Business/
COPY User.Data.Access/*.csproj ./User.Data.Access/
COPY User.Data.Contracts/*.csproj ./User.Data.Contracts/
COPY User.Data.Object/*.csproj ./User.Data.Object/

RUN dotnet restore ./User.Microservice/User.Microservice.csproj

COPY User.Microservice/. ./User.Microservice/
COPY User.Services.Contracts/. ./User.Services.Contracts/
COPY User.Services.Business/. ./User.Services.Business/
COPY User.Data.Access/. ./User.Data.Access/
COPY User.Data.Contracts/. ./User.Data.Contracts/
COPY User.Data.Object/. ./User.Data.Object/


WORKDIR /app/User.Microservice
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app 
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "User.Microservice.dll"]