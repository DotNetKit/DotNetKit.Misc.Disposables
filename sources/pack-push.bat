pack.bat
.nuget\nuget.exe push *.nupkg -Source https://www.nuget.org/api/v2/package
del /Q *.nupkg
