call :InstallXunitRunner
call :RunXunits
exit /b %ERRORLEVEL%


:InstallXunitRunner
if not exist libs\bin\xunit.runner.console.2.3.1\tools\net452\xunit.console.exe (
libs\nuget install xunit.runner.console -version 2.3.1 -outputDirectory libs\bin
)
exit /b %ERRORLEVEL%

:RunXunits
@for /f %%i in ('dir /s /b .\Vukomanovic.Resharper.Macros.Test.dll ^| grep bin\\Release\\ ^') do libs\bin\xunit.runner.console.2.3.1\tools\net452\xunit.console.exe %%i
exit /b %ERRORLEVEL%
