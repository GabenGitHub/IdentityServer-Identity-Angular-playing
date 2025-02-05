green=`tput setaf 2`
reset=`tput sgr0`

echo "${green}Creating initial migration PersistedGrantDbContext... ${reset}"
dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
echo "${green}Creating initial migration ConfigurationDbContext... ${reset}"
dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb
echo "${green}Creating initial migration ApplicationDbContext... ${reset}"
dotnet ef migrations add InitApplicationDbContext -c ApplicationDbContext -o Data/Migrations/AppMigrations

echo "${green}Updating database ApplicationDbContext ${reset}"
dotnet ef database update -c ApplicationDbContext
echo "${green}Updating database PersistedGrantDbContext ${reset}"
dotnet ef database update -c PersistedGrantDbContext
echo "${green}Updating database ConfigurationDbContext ${reset}"
dotnet ef database update -c ConfigurationDbContext