#!/usr/bin/env bash
set -e

# Change into a temporary directory so we can download + install .NET
pushd /tmp

# Download Microsoft install script
wget https://dot.net/v1/dotnet-install.sh

# Make the script executable
chmod +x ./dotnet-install.sh

# Install .NET 8 (modify channel if needed in the future)
./dotnet-install.sh --channel 8.0

# Return to project directory
cd /opt/build/repo

# Publish the WASM project
dotnet publish CineScope.csproj -c Release

# Copy the actual Blazor output to the folder Netlify expects
mkdir -p release/wwwroot
cp -r bin/Release/net8.0/wwwroot/* release/wwwroot/
cp _redirects release/wwwroot/
echo "DEBUG: Listing release/wwwroot"
ls -al release/wwwroot



