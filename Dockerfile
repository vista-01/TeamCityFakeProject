FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY TeamCityFakeProject/*.csproj TeamCityFakeProject/
COPY TeamCityFakeProject.Tests/*.csproj TeamCityFakeProject.Tests/
RUN dotnet restore TeamCityFakeProject/TeamCityFakeProject.csproj

# copy everything else and build app
COPY . .
WORKDIR /source/TeamCityFakeProject/
RUN dotnet build -c release --no-restore

# test stage -- exposes optional entrypoint
# target entrypoint with: docker build --target testrunner
FROM build AS testrunner
WORKDIR /source/TeamCityFakeProject.Tests
RUN dotnet tool install dotnet-reportgenerator-globaltool --tool-path tools
ENTRYPOINT ["bash", "-c", "dotnet test --configuration Release --logger 'trx;logfilename=/source/TeamCityFakeProject.Tests/TestResults/TeamCityFakeProject.Tests.trx' --collect:'XPlat Code Coverage' --results-directory CoverageResults ; tools/reportgenerator -reports:'/source/TeamCityFakeProject.Tests/CoverageResults/**/coverage.cobertura.xml' -targetdir:'coveragereport' -reporttypes:TeamCitySummary"]


# publish
FROM build AS publish
RUN dotnet publish -c release --no-build -o /app

# build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app .
ENV DOTNET_EnableDiagnostics=0
ENTRYPOINT ["dotnet", "TeamCityFakeProject.dll"]
