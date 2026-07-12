$ErrorActionPreference = "Stop"

$appSettings = Get-Content -Raw -Path "./appsettings.development.json" | ConvertFrom-Json
$apiKey = $appSettings.Llm.GoogleAiStudio.ApiKey

Write-Host "API Key: $apiKey"

$body = @{
    contents = @(
        @{
            parts = @(
                @{
                    text = "Hello! Are you working?"
                }
            )
        }
    )
} | ConvertTo-Json -Depth 5

$uri = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=$apiKey"

try {
    $response = Invoke-RestMethod -Uri $uri -Method Post -Body $body -Headers @{ "Content-Type" = "application/json" }
    Write-Host "Success!" -ForegroundColor Green
    $response | ConvertTo-Json -Depth 5
} catch {
    Write-Host "Failed: $_" -ForegroundColor Red
}
