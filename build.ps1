param(
    [string]$cmd,
    [string]$config = "Debug"
)

$solutionDir = $PSScriptRoot

function Coverage {
    $dir = "$solutionDir/TestResults"
    Remove-Item $dir -Force -Recurse -ErrorAction SilentlyContinue
    dotnet test -c $config --collect "XPlat Code Coverage" -r $dir
    $reports = (Get-Item TestResults/**/*.xml).FullName -join ";"
    Write-Host $reports
    reportgenerator -reports:$reports -targetdir:$dir -reporttypes html
    Start-Process "$dir/index.html"
}

switch ($cmd) {
    "coverage" { Coverage }
    default { throw "'$cmd' is not a valid command." }
}
