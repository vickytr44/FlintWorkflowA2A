$ErrorActionPreference = "Stop"

$appSettings = Get-Content -Raw -Path "./appsettings.development.json" | ConvertFrom-Json
$apiKey = $appSettings.Llm.GoogleAiStudio.ApiKey

$modelsToTest = @("gemini-2.5-flash-lite","Gemma-4-26B","gemini-2.5-flash", "Gemini-3-Flash", "gemini-pro-latest", "gemini-3.1-pro-preview", "gemini-omni-flash-preview")

foreach ($modelName in $modelsToTest) {
    Write-Host "Testing model: $modelName"
    
    $body = @{
        model = $modelName
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
        Write-Host "Success for $modelName!" -ForegroundColor Green
        break
    } catch {
        Write-Host "Failed for $($modelName): $_" -ForegroundColor Red
        if ($_.Exception.Response) {
            $stream = $_.Exception.Response.GetResponseStream()
            $reader = New-Object System.IO.StreamReader($stream)
            $errorResponse = $reader.ReadToEnd()
            Write-Host "Error Body: $errorResponse" -ForegroundColor DarkRed
        }
    }
}
