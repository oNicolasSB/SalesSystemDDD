@echo off
set /p MIGRATION_NAME="Nome da migration: "

if "%MIGRATION_NAME%"=="" (
    echo Erro: o nome da migration nao pode ser vazio.
    pause
    exit /b 1
)

echo.
echo Criando migration "%MIGRATION_NAME%"...
echo.

dotnet ef migrations add %MIGRATION_NAME% ^
    --project src\Sales.Infra\Sales.Infra.csproj ^
    --startup-project src\Sales.Api\Sales.Api.csproj

if %ERRORLEVEL% neq 0 (
    echo.
    echo Falha ao criar a migration.
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo Migration "%MIGRATION_NAME%" criada com sucesso.
pause
