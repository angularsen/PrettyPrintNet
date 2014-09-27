@echo off
SET ROOT=%~dp0..
%ROOT%\Tools\7-zip\7za.exe a -tzip %ROOT%\Artifacts\PrettyPrintNet.zip %ROOT%\Artifacts\*