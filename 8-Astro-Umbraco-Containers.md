# Install Astro Umbraco Containers

```bash
# Ensure we have the version specific Umbraco templates
dotnet new install Umbraco.Templates::13.9.1 --force

dotnet new umbraco --force -n "AstroAPI"  --friendly-name "Administrator" --email "admin@example.com" --password "1234567890" --development-database-type SQLite

```
