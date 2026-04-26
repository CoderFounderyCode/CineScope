#!/usr/bin/env bash
set -e

# Install .NET 8
pushd /tmp
wget https://dot.net/v1/dotnet-install.sh
chmod +x ./dotnet-install.sh
./dotnet-install.sh --channel 8.0
popd

# Netlify always checks out the repo into /opt/build/repo
cd /opt/build/repo

echo "DEBUG: Current working directory:"
pwd
echo "DEBUG: Listing repo root:"
ls -al

# Publish Blazor WASM
dotnet publish CineScope.csproj -c Release

# Absolute paths
PUBLISH_DIR="/opt/build/repo/release/wwwroot"
SOURCE_DIR="/opt/build/repo/bin/Release/net8.0/wwwroot"
REDIRECTS_FILE="/opt/build/repo/_redirects"

echo "DEBUG: Source directory:"
ls -al "$SOURCE_DIR"

echo "DEBUG: Checking redirects file:"
ls -al "$REDIRECTS_FILE"

mkdir -p "$PUBLISH_DIR"
cp -r "$SOURCE_DIR"/* "$PUBLISH_DIR"/
cp "$REDIRECTS_FILE" "$PUBLISH_DIR"/

echo "DEBUG: Final publish directory contents:"
ls -al "$PUBLISH_DIR"

