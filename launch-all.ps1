# Launch-all.ps1
# Script to launch all three applications simultaneously

Write-Host "Starting all USP applications..." -ForegroundColor Cyan

# Start USPGradeSystem
Start-Process powershell -ArgumentList "-NoExit", "-Command", "Set-Location '$PSScriptRoot\USPGradeSystem'; dotnet run"
Write-Host "Started USPGradeSystem" -ForegroundColor Green

# Short delay to prevent port conflicts
Start-Sleep -Seconds 2

# Start USPFinance
Start-Process powershell -ArgumentList "-NoExit", "-Command", "Set-Location '$PSScriptRoot\USPFinance'; dotnet run"
Write-Host "Started USPFinance" -ForegroundColor Green

# Short delay to prevent port conflicts
Start-Sleep -Seconds 2

# Start USPSystem
Start-Process powershell -ArgumentList "-NoExit", "-Command", "Set-Location '$PSScriptRoot\USPSystem'; dotnet run"
Write-Host "Started USPSystem" -ForegroundColor Green

Write-Host "All applications started successfully!" -ForegroundColor Cyan
Write-Host "USPGradeSystem: http://localhost:5240 (https://localhost:7258)" 
Write-Host "USPFinance: http://localhost:5291 (https://localhost:7059)"
Write-Host "USPSystem: http://localhost:5136 (https://localhost:7272)" 