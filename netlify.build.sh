#!/usr/bin/env bash
set -e

# Install .NET 8 into the Netlify build environment
pushd /tmp
wget https://dot.net/v1/dotnet-install.sh
chmod +x ./dotnet-install.sh
./dotnet-install.sh --channel 8.0
popd

# ALWAYS go to the actual repo root (no matter where Netlify starts)
cd "$(git rev-parse --show-toplevel)"

echo "DEBUG: Current working directory:"
pwd
echo "DEBUG: Listing repo root:"
ls -al

# Publish the Blazor WASM project
dotnet publish CineScope.csproj -c Release

# Absolute paths so Netlify cannot miss them
PUBLISH_DIR="/opt/build/repo/release/wwwroot"
SOURCE_DIR="/opt/build/repo/bin/Release/net8.0/wwwroot"
REDIRECTS_FILE="/opt/build/repo/_redirects"

echo "DEBUG: Source directory:"
ls -al "$SOURCE_DIR"

echo "DEBUG: Checking redirects file:"
ls -al "$REDIRECTS_FILE"

# Create publish directory and copy files
mkdir -p "$PUBLISH_DIR"
cp -r "$SOURCE_DIR"/* "$PUBLISH_DIR"/
cp "$REDIRECTS_FILE" "$PUBLISH_DIR"/

echo "DEBUG: Final publish directory contents:"
ls -al "$PUBLISH_DIR"
