#!/usr/bin/env bash
set -e

pushd /tmp
wget https://dot.net/v1/dotnet-install.sh
chmod +x ./dotnet-install.sh
./dotnet-install.sh --channel 8.0
popd

# ALWAYS go to the real repo root
cd "$(git rev-parse --show-toplevel)"

dotnet publish CineScope.csproj -c Release

mkdir -p release/wwwroot
cp -r bin/Release/net8.0/wwwroot/* release/wwwroot/
cp _redirects release/wwwroot/

echo "DEBUG: Listing release/wwwroot"
ls -al release/wwwroot
