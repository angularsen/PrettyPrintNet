@echo off
SET ROOT=%~dp0..
SET NugetSpecFile=%ROOT%\Build\PrettyPrintNet.nuspec
SET NuGetExe=%ROOT%\Tools\NuGet.exe
SET NuGetOutDir=%ROOT%\Artifacts\NuGet

mkdir "%NuGetOutDir%"

%NuGetExe% pack %NugetSpecFile% -Verbosity detailed -OutputDirectory "%NuGetOutDir%" -BasePath "%ROOT%" -Symbols