$ErrorActionPreference = "Stop"

$appSettings = Get-Content -Raw -Path "./appsettings.development.json" | ConvertFrom-Json
$apiKey = $appSettings.Llm.GoogleAiStudio.ApiKey

Write-Host "API Key: $apiKey"

$body = @{
    model = "gemini-3.1-flash-lite"
    response_format = @{
        type = "json_object"
    }
    messages = @(
        @{
            role = "user"
            content = "Hello!"
        }
    )
} | ConvertTo-Json -Depth 5

$uri = "https://generativelanguage.googleapis.com/v1beta/openai/chat/completions"
$headers = @{
    "Content-Type" = "application/json"
    "Authorization" = "Bearer $apiKey"
}

try {
    $response = Invoke-RestMethod -Uri $uri -Method Post -Body $body -Headers $headers
    Write-Host "Success!" -ForegroundColor Green
    $response | ConvertTo-Json -Depth 5
} catch {
    Write-Host "Failed: $_" -ForegroundColor Red
    if ($_.Exception.Response) {
        $stream = $_.Exception.Response.GetResponseStream()
        $reader = New-Object System.IO.StreamReader($stream)
        $errorResponse = $reader.ReadToEnd()
        Write-Host "Error Body: $errorResponse" -ForegroundColor Red
    }
}
