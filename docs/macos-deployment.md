# Intel macOS deployment

This deployment is intended for a single user on an Intel MacBook. It runs the ASP.NET Core application locally and does not expose it to the local network or internet.

## Build the package

On a Mac with the .NET 9 SDK, from the repository's `buildingapp` directory:

```bash
./deployment/macos/package-intel.sh
```

The script reads the semantic version from `VERSION`, runs the full test suite, and produces two files such as:

```text
artifacts/macos-intel/Building-Manager-v1.1.0-macOS-Intel.zip
artifacts/macos-intel/Building-Manager-v1.1.0-macOS-Intel.zip.sha256
```

The signed `.app` is staged outside synced project folders before it is archived, preventing Finder or file-provider metadata from invalidating the bundle. See [Releasing and packaging](releasing.md) for the complete versioning, tagging, and release workflow.

The application is self-contained for `osx-x64`; the destination Mac does not need .NET installed. The generated bundle is ad-hoc signed, not Apple notarized.

## Install

1. Copy the ZIP file to the Intel MacBook and extract it.
2. Drag **Building Manager.app** into **Applications**.
3. On first launch, Control-click the app, choose **Open**, and confirm the macOS prompt. Depending on the macOS version, approval may instead appear under **System Settings → Privacy & Security**.
4. Optionally drag the app from Applications to the Dock.

Launching the app starts the register and opens `http://127.0.0.1:5180` in the default browser. The server listens only on the Mac itself. A second launch opens the existing instance rather than starting another database process.

The launcher explicitly sets the published `Resources/app` directory as ASP.NET Core's content root so configuration and `wwwroot` styles/scripts resolve correctly regardless of how Finder starts the bundle.

Use **Close application** in the navigation bar before shutting down or installing an update.

## Live data

Program files and live data are deliberately separate. Replacing the `.app` does not replace the register.

```text
~/Library/Application Support/Building Manager/
├── buildingrecords.db
├── Backups/
├── Logs/
│   ├── application.log
│   └── application.log.previous
└── run.lock/                 # present only while running
```

This directory contains personal information and should be protected by the Mac login password and FileVault. Database backups downloaded through the browser also contain the complete register.

Production creates a migrated register containing only a neutral **My Property** container on first launch. It does not insert demonstration people, buildings, units, or operational records. Full sample seeding is enabled only by development configuration.

When upgrading from version 1.0.0, the launcher automatically moves data from the former `Chelsea Building Register` application-support directory to the new `Building Manager` directory. If both directories already exist, it leaves both untouched and uses the new directory.

## Upgrade

1. Open **Backups** and create a manual backup.
2. Use **Close application** and wait a few seconds.
3. Replace the old application in Applications with the new `.app`.
4. Launch it. If the release contains database migrations, another backup is created automatically before they are applied.
5. Confirm the property and unit records are visible.

Do not delete the Application Support directory during an upgrade.

## Recovery

The first recovery step is to preserve the entire Application Support directory before changing anything.

- If the app does not open, inspect `Logs/application.log`.
- If it reports that the port is already in use, restart the Mac or identify the process using port `5180`.
- The launcher removes `run.lock/pid` and the lock directory during normal or browser-requested shutdown. If the Mac loses power during shutdown, the next launch detects the stale process identifier and removes both automatically.
- Restoring a database backup is intentionally not automated yet. Close the application and make an additional copy of all data before manually replacing `buildingrecords.db` with a verified backup.

## Security boundary

The deployment sets `ASPNETCORE_URLS=http://127.0.0.1:5180`. Do not change this to `0.0.0.0`, `*`, the Mac's network address, or a public hostname without first adding authentication and HTTPS.
