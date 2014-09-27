@echo off
SET ROOT=%~dp0..

call %ROOT%\Src\.nuget\nuget.exe push %ROOT%\Artifacts\NuGet\*.nupkg