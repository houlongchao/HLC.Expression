@echo off
rmdir /S /Q Output
dotnet build -c Release
dotnet pack --no-restore -c Release -o Output
Pause