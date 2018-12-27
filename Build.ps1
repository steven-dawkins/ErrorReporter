# if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }

dotnet restore

dotnet build -c Release

$revision = @{ $true = $env:APPVEYOR_BUILD_NUMBER; $false = 1 }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
# $revision = "{0:D4}" -f [convert]::ToInt32($revision, 10)
$revision = "{0}" -f [convert]::ToInt32($revision, 10)

dotnet pack .\ErrorReporter.Core -c Release -o .\artifacts --version-suffix=$revision

# exec { & dotnet test .\test\YOUR_TEST_PROJECT_NAME -c Release }