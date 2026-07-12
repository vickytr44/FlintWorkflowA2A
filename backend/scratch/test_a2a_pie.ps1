$messageUri = "http://localhost:5000/a2a/flint-agent/message:send"
$headers = @{
    "Content-Type" = "application/json"
}
$body = @{
    message = @{
        kind = "message"
        role = 1
        parts = @(
            @{
                kind = "text"
                text = "data`nVacation    20 days`nSick    10 days`nParental    58 days`n`nchart type pie chart"
            }
        )
        messageId = $null
        contextId = "test-session-pie"
    }
} | ConvertTo-Json -Depth 5

Write-Host "`nSending message to A2A endpoint: $messageUri..." -ForegroundColor Cyan
Write-Host "Payload: $body" -ForegroundColor Gray

try {
    $response = Invoke-RestMethod -Uri $messageUri -Method Post -Body $body -Headers $headers
    Write-Host "`nResponse received successfully!" -ForegroundColor Green
    Write-Host "Result (A2A Message):" -ForegroundColor Yellow
    $response | ConvertTo-Json -Depth 6
}
catch {
    Write-Error "Request failed: $_"
}
