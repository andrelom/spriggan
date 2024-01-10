#!/bin/bash

source ./tools/unix/util.sh

set_environment_variables

# Menu
echo ""
echo "Usage:"
echo "create   Will create or update default migrations."
echo "remove   It will remove the last migration."
echo "apply    It will apply migrations to the target database."
echo "sql      It will generate the SQL script."
echo ""

# Tasks
function create {
  read -p "Name: " name

  echo ""

  dotnet ef migrations add $name --project "src/Spriggan.Data.Main" -s "src/Spriggan.Web" -c "Spriggan.Data.Main.ApplicationDbContext" -o "Migrations"
}

function remove {
  echo ""

  dotnet ef migrations remove --project "src/Spriggan.Data.Main" -s "src/Spriggan.Web" -c "Spriggan.Data.Main.ApplicationDbContext"
}

function apply {
  read -p "Name (Default Empty): " name

  echo ""

  dotnet ef database update $name --project "src/Spriggan.Data.Main" -s "src/Spriggan.Web" -c "Spriggan.Data.Main.ApplicationDbContext"
}

function sql {
  rm -f "./sql/Main/Migrations.sql"

  dotnet ef migrations script --idempotent --project "src/Spriggan.Data.Main" -s "src/Spriggan.Web" -c "Spriggan.Data.Main.ApplicationDbContext" -o "./sql/Main/Migrations.sql"
}

# Main
read -p "Option: " selection

echo ""

case $selection in
create) create ;;
remove) remove ;;
apply) apply ;;
sql) sql ;;
*) echo "Invalid option." ;;
esac
