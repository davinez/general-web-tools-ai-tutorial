# general-web-tools-ai-tutorials


# CoreApp

## Database

- Migrations

Run the following commands to create the database schema:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

```ps1
# Create a new migration	
Add-Migration [MigrationName]

# Remove the latest migration	
Remove-Migration

# Update the database to the latest migration	
Update-Database

# Update the database to a specific migration	
Update-Database -Migration [MigrationName]

# List all available migrations	
Get-Migrations

# Generate SQL script for all migrations	
Script-Migration

#Drop the database	
Drop-Database

```



## AI

- For open web ui and other AI services / tools, run:
```bash
docker compose -f docker-compose.ai.yml up
```


