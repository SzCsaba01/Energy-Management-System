FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app 
EXPOSE 80

COPY *.sln .
COPY Chat.Microservice/*.csproj ./Chat.Microservice/
COPY Chat.Services.Contracts/*.csproj ./Chat.Services.Contracts/
COPY Chat.Services.Business/*.csproj ./Chat.Services.Business/
COPY Chat.Data.Access/*.csproj ./Chat.Data.Access/
COPY Chat.Data.Contracts/*.csproj ./Chat.Data.Contracts/
COPY Chat.Data.Object/*.csproj ./Chat.Data.Object/

RUN dotnet restore ./Chat.Microservice/Chat.Microservice.csproj

COPY Chat.Microservice/. ./Chat.Microservice/
COPY Chat.Services.Contracts/. ./Chat.Services.Contracts/
COPY Chat.Services.Business/. ./Chat.Services.Business/
COPY Chat.Data.Access/. ./Chat.Data.Access/
COPY Chat.Data.Contracts/. ./Chat.Data.Contracts/
COPY Chat.Data.Object/. ./Chat.Data.Object/


WORKDIR /app/Chat.Microservice
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app 
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Chat.Microservice.dll"]