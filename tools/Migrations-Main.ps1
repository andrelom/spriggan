Import-Module .\tools\Util.psm1 -Force

Set-EnvironmentVariables

# Menu

Write-Host ""
Write-Host "Usage:"
Write-Host "create `t Will create or update default migrations."
Write-Host "remove `t It will remove the last migration."
Write-Host "apply `t It will apply migrations to the target database."
Write-Host "sql `t It will generate the SQL script."
Write-Host ""

# Tasks

function Create {
  $name = Read-Host "Name"

  Write-Host ""

  dotnet ef migrations add $name --project "src\Spriggan.Data.Main" -s "src\Spriggan.Web" -c "Spriggan.Data.Main.ApplicationDbContext" -o "Migrations"
}

function Remove {
  Write-Host ""

  dotnet ef migrations remove --project "src\Spriggan.Data.Main" -s "src\Spriggan.Web" -c "Spriggan.Data.Main.ApplicationDbContext"
}

function Apply {
  $name = Read-Host "Name (Default Empty)"

  Write-Host ""

  dotnet ef database update $name --project "src\Spriggan.Data.Main" -s "src\Spriggan.Web" -c "Spriggan.Data.Main.ApplicationDbContext"
}

function SQL {
  Remove-Item -LiteralPath "./sql/main/Migrations.sql" -Force

  dotnet ef migrations script --idempotent --project "src\Spriggan.Data.Main" -s "src\Spriggan.Web" -c "Spriggan.Data.Main.ApplicationDbContext" -o "./sql/main/Migrations.sql"
}

# Main

$selection = Read-Host "Option"

Write-Host ""

switch ($selection) {
  'create' { Create }
  'remove' { Remove }
  'apply' { Apply }
  'sql' { SQL }
}
