FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY UploadApi/UploadApi.csproj UploadApi/
RUN dotnet restore
COPY . .
WORKDIR /src/UploadApi
RUN dotnet build -c Release -o /app
WORKDIR /src/UploadApi.Test
RUN dotnet test -c Release

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "UploadApi.dll"]
