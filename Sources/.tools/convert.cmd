setlocal ENABLEDELAYEDEXPANSION
@set dst=%~1
@set tls=%~2

@for /r "%dst%\text\%3" %%f IN (*.mo) do (
@"%tls%\msgunfmt.exe" %%f > "%dst%\text\%3\%%~nf.po"
@"%tls%\resgenEx.exe" "%dst%\text\%3\%%~nf.po" "%dst%\Resources\Text\%%~nf.%3.resx"
)