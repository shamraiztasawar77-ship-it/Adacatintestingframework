[xml]$config = Get-Content "$PSScriptRoot\Config.xml"

$workspace = $env:WORKSPACE

if (-not $workspace) {
    $workspace = Get-Location
}

Write-Host "=========================================="
Write-Host " Jenkins XML Build Runner"
Write-Host "=========================================="

Write-Host "Workspace: $workspace"

Set-Location $workspace

foreach ($step in $config.BuildConfiguration.Steps.Step) {

    $name = $step.name
    $command = $step.Command

    Write-Host ""
    Write-Host "=========================================="
    Write-Host "Running Step: $name"
    Write-Host "Command: $command"
    Write-Host "=========================================="

    Invoke-Expression $command

    if ($LASTEXITCODE -ne 0) {
        Write-Host ""
        Write-Host "ERROR: Step '$name' failed."
        exit $LASTEXITCODE
    }

    Write-Host "Step '$name' completed successfully."
}

Write-Host ""
Write-Host "=========================================="
Write-Host "All build steps completed successfully."
Write-Host "=========================================="