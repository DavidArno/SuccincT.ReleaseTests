@echo off
rmdir %UserProfile%\.nuget\packages\succinct /s /q
rmdir %UserProfile%\.nuget\packages\succinct.json /s /q
"C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\MSBuild.exe" /t:Clean /p:Configuration=Release /v:m
dotnet restore
"C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\MSBuild.exe" /t:Rebuild /p:Configuration=Release /v:m
cd TestRunnerRunner
.\bin\Release\net45\TestRunnerRunner.exe
cd ..