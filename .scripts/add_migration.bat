@echo off
IF "%~1"=="" (
    echo Usage: %~nx0 ^<MigrationName^>
    exit /b 1
)
dotnet ef migrations add %~1 --project "Digital.Pages.Data" --context "SafariDigitalContext"
