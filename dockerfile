FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
EXPOSE 5000 5001
WORKDIR /App

RUN apt-get update 
RUN apt-get install -y libgdiplus

COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "wkhtmlToPdfDemo.dll"]