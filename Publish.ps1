$ErrorActionPreference = 'Stop'
$status = git status --porcelain
if ($status) { Write-Error "Working tree is not clean.`n$status"; exit 1 }
if ((git rev-parse --abbrev-ref HEAD) -ne 'main') { Write-Error 'Publishing requires main.'; exit 1 }
git fetch origin main --quiet
if ((git rev-parse HEAD) -ne (git rev-parse origin/main)) { Write-Error 'Local main is not synchronized.'; exit 1 }
$project = Join-Path $PSScriptRoot 'ScienceLogic.Em7.Api/ScienceLogic.Em7.Api.csproj'
$output = dotnet build $project -t:GetBuildVersion --getProperty:NuGetPackageVersion -nologo -v:quiet -p:TreatWarningsAsErrors=false
if ($LASTEXITCODE -ne 0) { Write-Error "Could not determine version.`n$output"; exit 1 }
$version = ($output | Select-Object -Last 1).ToString().Trim()
if (git tag -l $version) { Write-Error "Tag '$version' exists."; exit 1 }
Write-Output "Tagging as $version ..."
git tag $version
git push origin $version
Write-Output "Published tag $version."
