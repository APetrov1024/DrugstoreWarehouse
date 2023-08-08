# DrugstoreWarehouse
## How to build and run
### Install client libs
1. Install ABP CLI
   ````bash
    > dotnet tool install -g Volo.Abp.Cli
    ````
2. Open /src/DrugstoreWarehouse.Web/ in the terminal and run client libs installation
   ````bash
    > abp install-libs
    ````
### Database
The application is configured to use PostgreSQL. You don't need to create a database manually, just follow these steps:
1. Configure the connection string in appsettings.json for DrugstoreWarehouse.DbMigrator and DrugstoreWarehouse.Web projects
````
"ConnectionStrings": {
  "Default": "Host=localhost;Port=5432;Database=DrugstoreWarehouse;User ID=user;Password=pass;"
}
```` 
2. Run project DrugstoreWarehouse.DbMigrator. It creating DB (if not exists), running migrations and seed initial data

### Now you can run the DrugstoreWarehouse.Web project
Default credentials is:
  * Username: admin
  * Password: 1q2w3E*
