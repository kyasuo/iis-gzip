@echo off
setlocal
rem iis-gzip.bat
rem This is bat file to build this project.
rem .NET Framework requires 4.X or later.

rem change this path depending on your environment.
rem csc.exe for 32bit ====================
set CSC="C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"
rem csc.exe for 64bit ====================
rem set CSC="C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"

rem build
if not exist "dist/" (
  md dist
)
%CSC% /optimize+ /out:dist\iis-gzip.exe .\iis-gzip\Program.cs .\iis-gzip\IISGzip.cs > nul
pause
