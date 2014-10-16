@echo off
SET ROOT=%~dp0..
SET NuGetExe=%ROOT%\Tools\NuGet.exe
SET SolutionFile=%ROOT%\Src\PrettyPrintNet.sln
%NuGetExe% restore %SolutionFile%