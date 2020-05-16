# web-dynamic-database
Web/API project created with .NET Core 3.1

Connects to SQL server and allows the ability to add new tables/columns to a database and access them via web interface or API endpoint without regenerating models/views. Extremely powerful tool for managing automation settings.

To provision the application, Modify the database parameters in CreateDatabaseSchema.ps1 and the connection string in appsettings.json - then deploy the project to your favorite web server.

All authorization/roles/security has been removed to make this project dynamic and allow for custom implementations.

Full description to come soon.

TODO:
- Add Requires statement to CreateDatabaseSchema.ps1
- Configure environments properly using appsettings.json
- Update PowerShell DB Creation script to replace default connection strings in appsettings.json
- Enhance field validation & Sanatize inputs to prevent SQL Injection
- Add tutorial to web interface
- build container image
