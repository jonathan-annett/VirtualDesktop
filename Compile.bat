@echo off
:: Markus Scholtes, 2023
:: Compile VirtualDesktop in .Net 4.x environment
setlocal

 node .\src\build.js
 dotnet tool install csharpier -g  
 dotnet csharpier . 

C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe "%~dp0VirtualDesktop11.cs" /win32icon:"%~dp0MScholtes.ico"
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe "%~dp0VirtualDesktop11-23H2.cs" /win32icon:"%~dp0MScholtes.ico"
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe "%~dp0VirtualDesktop11InsiderCanary.cs" /win32icon:"%~dp0MScholtes.ico"
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe "%~dp0VirtualDesktop11-21H2.cs" /win32icon:"%~dp0MScholtes.ico"
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe "%~dp0VirtualDesktop.cs" /win32icon:"%~dp0MScholtes.ico"
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe "%~dp0VirtualDesktopServer2022.cs" /win32icon:"%~dp0MScholtes.ico"
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe "%~dp0VirtualDesktopServer2016.cs" /win32icon:"%~dp0MScholtes.ico"

