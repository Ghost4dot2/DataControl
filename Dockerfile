FROM mcr.microsoft/dotnet/sdk:5.0
WORKDIR /source

#copy csproj and restore as distinct layers
COPY *.sln .
COPY DataControl/*.csproj ./DataControl/
COPY Databases/*.csproj ./Databases/
COPY DatabaseObjects/*.csproj ./DatabaseObjects/
RUN dotnet restore

# copy everything else and build app
COPY DataControl/. ./DataControl/
COPY Databases/. ./Databases/
COPY DatabaseObjects/. ./DatabaseObjects/
WORKDIR /source/DataControl
RUN dotnet publish -c release -o /app -- no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/sdk:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "DataControl.dll"]