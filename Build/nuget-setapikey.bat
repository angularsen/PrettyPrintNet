@echo off
SET ROOT=%~dp0..
set /p apikey=Enter API key for nuget.org:
echo New api key: %apikey%
%ROOT%\Src\.nuget\nuget.exe setApiKey %apikey%