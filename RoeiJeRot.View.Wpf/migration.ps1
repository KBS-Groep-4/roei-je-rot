Write-Host "removing migraion"

dotnet ef migrations remove --project ../RoeiJeRot.Database/RoeiJeRot.Database.csproj --startup-project .\RoeiJeRot.View.Wpf.csproj

Get-ChildItem "../RoeiJeRot.Database/Migrations/*" -Filter *.cs | 
Foreach-Object {
    Remove-Item $_.FullName
}


Write-Host "creating migraion"

dotnet ef migrations add InitialMigrations --project ../RoeiJeRot.Database/RoeiJeRot.Database.csproj --startup-project .\RoeiJeRot.View.Wpf.csproj

Write-Host "updating database"
dotnet ef database update InitialMigrations --project ../RoeiJeRot.Database/RoeiJeRot.Database.csproj --startup-project .\RoeiJeRot.View.Wpf.csproj