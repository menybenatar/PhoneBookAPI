# Use the official ASP.NET Core image as the base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official SDK image for build and publish
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the project file and restore as distinct layers
COPY ["PhoneBookAPI/PhoneBookAPI.csproj", "PhoneBookAPI/"]
RUN dotnet restore "PhoneBookAPI/PhoneBookAPI.csproj"

# Copy the rest of the application source code
COPY . .
WORKDIR "/src/PhoneBookAPI"

# Build the application
RUN dotnet build "PhoneBookAPI.csproj" -c Release -o /app/build

# Publish the application to the /app/publish directory
FROM build AS publish
RUN dotnet publish "PhoneBookAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the base image to run the app
FROM base AS final
WORKDIR /app

# Copy the output from the publish step
COPY --from=publish /app/publish .

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "PhoneBookAPI.dll"]
