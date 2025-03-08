# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:35792ea4ad1db051981f62b313f1be3b46b1f45cadbaa3c288cd0d3056eefb83 AS build

# Define the working directory inside the container
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["webApi/webApi.csproj", "webApi/"]
RUN dotnet restore "webApi/webApi.csproj"

# Copy the rest of the files
COPY . .

# Build the application and publish it to the 'out' folder
RUN dotnet publish "webApi/webApi.csproj" -c Release -o /app/out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:6c4df091e4e531bb93bdbfe7e7f0998e7ced344f54426b7e874116a3dc3233ff

# Define the working directory in the runtime container
WORKDIR /app

# Copy the published app from the build stage
COPY --from=build /app/out .
COPY appsettings.json /app/
# Expose the port your app will run on
EXPOSE 8080

# Set the entry point for the container
ENTRYPOINT ["dotnet", "webApi.dll"]
