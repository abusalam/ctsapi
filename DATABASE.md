## Create or Update Database

Run the following command to create or update the database

```sh
docker-compose exec dotnet dotnet ef database update
```

## Scaffold the database

Run the following commands to reverse engineer the database to scaffold the PensionDbContext and Entities

```sh
docker-compose exec dotnet dotnet ef dbcontext scaffold "Name=ConnectionStrings:DBConnection" Npgsql.EntityFrameworkCore.PostgreSQL --data-annotations --context PensionDbContext --context-dir DAL --schema cts_pension --output-dir DAL/Entities/Pension --force
```

## Update Migrations

Drop the databaase using Database Management API and run the following commands to update the migrations

```sh
docker-compose exec dotnet dotnet ef migrations remove
docker-compose exec dotnet dotnet ef migrations add CreateInitialSchema
```
