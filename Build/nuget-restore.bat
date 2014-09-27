@echo off
SET ROOT=%~dp0..
SET NuGetExe=%ROOT%\Src\.nuget\NuGet.exe
SET SolutionFile=%ROOT%\Src\PrettyPrintNet.sln
%NuGetExe% restore %SolutionFile%