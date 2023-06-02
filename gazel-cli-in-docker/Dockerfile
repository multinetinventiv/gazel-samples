# Specifies the parent image 
FROM mcr.microsoft.com/dotnet/sdk:6.0 as run

# Installs gazel-cli and sets the environment path for dotnet tools
RUN dotnet tool install --global Gazel.Cli --version 6.2.0
ENV PATH="/root/.dotnet/tools:${PATH}"

