@echo off
echo Aplicando migrations no banco de dados...
echo.

dotnet ef database update ^
    --project src\Sales.Infra\Sales.Infra.csproj ^
    --startup-project src\Sales.Api\Sales.Api.csproj

if %ERRORLEVEL% neq 0 (
    echo.
    echo Falha ao aplicar as migrations.
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo Banco de dados atualizado com sucesso.
pause
