FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY ["1 - Movies.Domain.Api/Movies.Domain.Api.csproj", "1 - Movies.Domain.Api/"]
COPY ["2 - Movies.Domain/Movies.Domain.csproj", "2 - Movies.Domain/"]
COPY ["3 - Movies.Domain.Infra/Movies.Domain.Infra.csproj", "3 - Movies.Domain.Infra/"]
COPY ["4 - Movies.Domain.CrossCutting/Movies.Domain.CrossCutting.csproj", "4 - Movies.Domain.CrossCutting/"]
COPY ["5 - Movies.Domain.Tests/Movies.Domain.Tests.csproj", "5 - Movies.Domain.Tests/"]

RUN dotnet restore

# copy everything else and build app
COPY ["1 - Movies.Domain.Api/.", "./1 - Movies.Domain.Api/"]
COPY ["2 - Movies.Domain/.", "./2 - Movies.Domain/"]
COPY ["3 - Movies.Domain.Infra/.", "./3 - Movies.Domain.Infra/"]
COPY ["4 - Movies.Domain.CrossCutting/.", "./4 - Movies.Domain.CrossCutting/"] 

WORKDIR "/app/1 - Movies.Domain.Api"
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o out


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build ["/app/1 - Movies.Domain.Api/out", "./"]
EXPOSE 8080
EXPOSE 8081
ENTRYPOINT ["dotnet", "Movies.Domain.Api.dll"]