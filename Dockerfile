# Use the official .NET 10 SDK image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /app

# Copy the remaining application code and build the app
COPY ./ ./
RUN cd ./Generify/
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Use the official .NET 10 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port your Blazor app will run on
EXPOSE 8080

# Start the Blazor app
ENTRYPOINT ["dotnet", "Generify.dll"]
