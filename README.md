# Assignment_NashTech

## Add New Migration
dotnet ef migrations add "firstInit" -s .\PES.Presentation\  -p .\PES.Infrastructure\
## Update Database
dotnet ef database update "firstInit" -s .\PES.Presentation\  -p .\PES.Infrastructure\