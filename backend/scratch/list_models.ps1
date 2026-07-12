$ErrorActionPreference = "Stop"

$appSettings = Get-Content -Raw -Path "./appsettings.development.json" | ConvertFrom-Json
$apiKey = $appSettings.Llm.GoogleAiStudio.ApiKey

Write-Host "API Key: $apiKey"

$uri = "https://generativelanguage.googleapis.com/v1beta/models?key=$apiKey"

try {
    $response = Invoke-RestMethod -Uri $uri -Method Get
    Write-Host "Success!" -ForegroundColor Green
    $response.models | Select-Object name | ConvertTo-Json -Depth 5
} catch {
    Write-Host "Failed: $_" -ForegroundColor Red
}
