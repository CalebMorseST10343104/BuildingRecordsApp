# Releasing and packaging the application

The application uses semantic versions stored in the repository-root `VERSION` file.

```text
MAJOR.MINOR.PATCH
  1  . 0  . 0
```

- Increase **PATCH** for compatible fixes, for example `1.0.0` → `1.0.1`.
- Increase **MINOR** for compatible features, for example `1.0.1` → `1.1.0`.
- Increase **MAJOR** when an upgrade is intentionally incompatible or requires a special migration process, for example `1.4.0` → `2.0.0`.

The `v` belongs in the Git tag (`v1.0.0`), not in the `VERSION` file.

## What the packaging command does

From the `buildingapp` directory, run:

```bash
./deployment/macos/package-intel.sh
```

The script performs the complete repeatable packaging workflow:

1. Reads and validates `VERSION`.
2. Runs all automated tests in Release configuration.
3. Publishes a self-contained `osx-x64` application; the destination Mac does not need .NET.
4. Builds the `.app` bundle in a temporary local directory.
5. Inserts the release version into `Info.plist` and validates the plist.
6. Removes extended attributes, ad-hoc signs the bundle, and verifies the signature.
7. Creates a versioned ZIP and SHA-256 checksum under `artifacts/macos-intel/`.

`SKIP_TESTS=1 ./deployment/macos/package-intel.sh` is available for packaging experiments only. Do not use it for a real release.

## Prepare a release

1. Ensure the repository is fully downloaded locally. In Finder, use **Download Now** if macOS has offloaded project files.
2. Review the changes:

   ```bash
   git status --short
   git diff
   ```

3. Decide the next semantic version and edit `VERSION`.
4. Add a matching section to `CHANGELOG.md`, describing user-visible changes.
5. Run the test suite:

   ```bash
   dotnet test buildingapp.sln --no-restore
   ```

6. Stage deliberately. The development database is tracked for historical reasons, so do not stage it unless you intentionally changed the sample database:

   ```bash
   git add -A
   git restore --staged BuildingRecordsApp/buildingrecords.db
   git diff --cached --stat
   git diff --cached
   ```

7. Commit and tag the release:

   ```bash
   git commit -m "Prepare release 1.0.1"
   git tag -a v1.0.1 -m "Chelsea Building Register 1.0.1"
   ```

8. Build the package:

   ```bash
   ./deployment/macos/package-intel.sh
   ```

9. Verify the printed checksum whenever the ZIP is copied:

   ```bash
   shasum -a 256 -c artifacts/macos-intel/Chelsea-Building-Register-v1.0.1-macOS-Intel.zip.sha256
   ```

10. Install the ZIP on a test Mac, confirm it opens, check the records, create a backup, and test **Close application**.
11. Push both the commit and tag when ready:

   ```bash
   git push origin main
   git push origin v1.0.1
   ```

If using GitHub Releases, create a release from the matching tag and attach both the ZIP and its `.sha256` file. The generated artifacts are intentionally ignored by Git and should not be committed.

## Upgrade the installed copy

The live database is outside the app bundle, so replacing the application does not replace the register. On the destination Mac:

1. Create a manual backup inside the running app.
2. Choose **Close application** and wait for it to stop.
3. Replace the old app in `/Applications` with the newly extracted app.
4. Open the new app and confirm the register records are present.

Never delete `~/Library/Application Support/Chelsea Building Register/` during an upgrade.
