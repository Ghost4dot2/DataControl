FROM mcr.microsoft.com/dotnet/sdk:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["DataControl/DataControl.csproj", "DataControl/"]
COPY ["Databases/Databases.csproj", "Databases/"]
COPY ["DatabaseObjects/DatabaseObjects.csproj", "DatabaseObjects/"]
RUN dotnet restore "DataControl/DataControl.csproj"
RUN dotnet restore "Databases/Databases.csproj"
RUN dotnet restore "DatabaseObjects/DatabaseObjects.csproj"

COPY . .
WORKDIR "/src/DataControl/"
RUN dotnet build "DataControl.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataControl.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataControl.dll"]

