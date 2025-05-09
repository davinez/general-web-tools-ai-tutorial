# general-web-tools-ai-tutorials


# CoreApp

## Database

- Migrations

Run the following commands to create the database schema:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```


add-migration [name]	Create a new migration with the specific migration name.

remove-migration	Remove the latest migration.

update-database	Update the database to the latest migration.

update-database [name]	Update the database to a specific migration name point.

get-migrations	Lists all available migrations.

script-migration	Generates a SQL script for all migrations.

drop-database	Drop the database.








