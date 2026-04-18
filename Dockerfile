# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining source code and build the app
COPY . ./
RUN dotnet publish -c Release -o out

# Use the official .NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Expose the port the app runs on (Render uses PORT env var)
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# Start the app
ENTRYPOINT ["dotnet", "backend.dll"]
