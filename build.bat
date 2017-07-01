dotnet restore src/SharpSettings/SharpSettings.csproj
dotnet pack src/SharpSettings/SharpSettings.csproj -o ../../nupkgs --include-symbols

dotnet restore src/SharpSettings.MongoDB/SharpSettings.MongoDB.csproj
dotnet pack src/SharpSettings.MongoDB/SharpSettings.MongoDB.csproj -o ../../nupkgs --include-symbols

dotnet restore src/SharpSettings.MongoDB.AspNet/SharpSettings.MongoDB.AspNet.csproj
dotnet pack src/SharpSettings.MongoDB.AspNet/SharpSettings.MongoDB.AspNet.csproj -o ../../nupkgs --include-symbols