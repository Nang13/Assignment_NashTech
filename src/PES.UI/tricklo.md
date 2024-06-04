dotnet ef migrations add Name -s .\FireDetection.Backend.API\  -p .\FireDetection.Backend.Domain\ 
dotnet ef database update -s .\FireDetection.Backend.API\  -p .\FireDetection.Backend.Domain\ 