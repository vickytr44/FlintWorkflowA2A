# PowerShell script to test the official Microsoft Agent Framework A2A endpoint and Agent Card discovery

# 1. Test Agent Card Discovery via well-known path
$wellKnownCardUri = "http://localhost:5000/.well-known/agent-card.json"
Write-Host "Fetching Well-Known Agent Card from $wellKnownCardUri..." -ForegroundColor Cyan
try {
    $card = Invoke-RestMethod -Uri $wellKnownCardUri -Method Get
    Write-Host "Success! Found Well-Known Agent Card:" -ForegroundColor Green
    $card | ConvertTo-Json -Depth 5
}
catch {
    Write-Host "Failed to fetch Well-Known Agent Card: $_" -ForegroundColor Yellow
}

Write-Host "----------------------------------"

# 2. Test Agent Card Discovery via A2A path
$a2aCardUri = "http://localhost:5000/a2a/flint-agent/card"
Write-Host "Fetching A2A Agent Card from $a2aCardUri..." -ForegroundColor Cyan
try {
    $card = Invoke-RestMethod -Uri $a2aCardUri -Method Get
    Write-Host "Success! Found A2A Agent Card:" -ForegroundColor Green
    $card | ConvertTo-Json -Depth 5
}
catch {
    Write-Error "Failed to fetch A2A Agent Card: $_"
}

Write-Host "----------------------------------"

# 3. Test A2A message exchange
$messageUri = "http://localhost:5000/a2a/flint-agent/message:stream"
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
                text = "Plot a pie chart showing favorite ice cream flavors: Chocolate 45, Vanilla 30, Strawberry 15, Mint 10"
            }
        )
        messageId = $null
        contextId = "test-session-456"
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
