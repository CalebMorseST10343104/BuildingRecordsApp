#!/bin/bash
set -eu

SCRIPT_DIR=$(cd "$(dirname "$0")" && pwd)
REPOSITORY_ROOT=$(cd "$SCRIPT_DIR/../.." && pwd)
ARTIFACTS_DIR="$REPOSITORY_ROOT/artifacts/macos-intel"
VERSION_FILE="$REPOSITORY_ROOT/VERSION"

if [ ! -f "$VERSION_FILE" ]; then
    printf 'Missing VERSION file: %s\n' "$VERSION_FILE" >&2
    exit 1
fi

APP_VERSION=$(tr -d '[:space:]' < "$VERSION_FILE")
if ! printf '%s' "$APP_VERSION" | grep -Eq '^[0-9]+\.[0-9]+\.[0-9]+$'; then
    printf 'VERSION must contain MAJOR.MINOR.PATCH, for example 1.0.0.\n' >&2
    exit 1
fi

IFS=. read -r VERSION_MAJOR VERSION_MINOR VERSION_PATCH <<EOF
$APP_VERSION
EOF
BUILD_VERSION=$((VERSION_MAJOR * 1000000 + VERSION_MINOR * 1000 + VERSION_PATCH + 1))
ZIP_PATH="$ARTIFACTS_DIR/Chelsea-Building-Register-v$APP_VERSION-macOS-Intel.zip"
CHECKSUM_PATH="$ZIP_PATH.sha256"
STAGING_DIR=$(mktemp -d)
APP_BUNDLE="$STAGING_DIR/Chelsea Building Register.app"
PUBLISH_DIR="$STAGING_DIR/publish"
GENERATED_PLIST="$STAGING_DIR/Info.plist"

cleanup() {
    rm -rf "$STAGING_DIR"
}
trap cleanup EXIT

mkdir -p "$ARTIFACTS_DIR"

if [ "${SKIP_TESTS:-0}" != "1" ]; then
    dotnet test "$REPOSITORY_ROOT/buildingapp.sln" --configuration Release
fi

dotnet publish "$REPOSITORY_ROOT/BuildingRecordsApp/BuildingRecordsApp.csproj" \
    -p:PublishProfile=MacIntel \
    -p:Version="$APP_VERSION" \
    -p:PublishDir="$PUBLISH_DIR/"

mkdir -p "$APP_BUNDLE/Contents/MacOS" "$APP_BUNDLE/Contents/Resources/app"
sed -e "s/__MARKETING_VERSION__/$APP_VERSION/g" -e "s/__BUILD_VERSION__/$BUILD_VERSION/g" \
    "$SCRIPT_DIR/Info.plist" > "$GENERATED_PLIST"
plutil -lint "$GENERATED_PLIST" >/dev/null
cp "$GENERATED_PLIST" "$APP_BUNDLE/Contents/Info.plist"
cp "$SCRIPT_DIR/ChelseaBuildingRegister" "$APP_BUNDLE/Contents/MacOS/ChelseaBuildingRegister"
cp -R "$PUBLISH_DIR/." "$APP_BUNDLE/Contents/Resources/app/"
chmod 755 "$APP_BUNDLE/Contents/MacOS/ChelseaBuildingRegister"
chmod 755 "$APP_BUNDLE/Contents/Resources/app/BuildingRecordsApp"

xattr -cr "$APP_BUNDLE"
codesign --force --deep --sign - "$APP_BUNDLE"
codesign --verify --deep --strict "$APP_BUNDLE"
rm -f "$ZIP_PATH"
ditto -c -k --sequesterRsrc --keepParent "$APP_BUNDLE" "$ZIP_PATH"
shasum -a 256 "$ZIP_PATH" > "$CHECKSUM_PATH"

printf 'Created Chelsea Building Register %s:\n%s\n%s\n' "$APP_VERSION" "$ZIP_PATH" "$CHECKSUM_PATH"
