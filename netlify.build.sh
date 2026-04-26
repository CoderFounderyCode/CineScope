# Publish the WASM project
dotnet publish CineScope.csproj -c Release

# Absolute paths so Netlify cannot miss them
PUBLISH_DIR="/opt/build/repo/release/wwwroot"
SOURCE_DIR="/opt/build/repo/bin/Release/net8.0/wwwroot"
REDIRECTS_FILE="/opt/build/repo/_redirects"

mkdir -p "$PUBLISH_DIR"
cp -r "$SOURCE_DIR"/* "$PUBLISH_DIR"/
cp "$REDIRECTS_FILE" "$PUBLISH_DIR"/

echo "DEBUG: Listing publish directory"
ls -al "$PUBLISH_DIR"
