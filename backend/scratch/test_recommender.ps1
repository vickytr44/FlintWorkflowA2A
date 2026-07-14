$dataJson = @{
    data = @{
        values = @(
            @{ Month = "Jan"; Sales = "100"; Region = "North" }
            @{ Month = "Feb"; Sales = "150"; Region = "North" }
            @{ Month = "Jan"; Sales = "80"; Region = "South" }
            @{ Month = "Feb"; Sales = "120"; Region = "South" }
        )
    }
} | ConvertTo-Json -Depth 5 -Compress

$prompt = "Please recommend the best chart types for this data : $dataJson"

$body = @{
    message = @{
        kind = "message"
        role = 1
        parts = @(
            @{
                kind = "text"
                text = $prompt
            }
        )
        messageId = $null
        contextId = "test-recommender"
    }
} | ConvertTo-Json -Depth 5

$messageUri = "http://localhost:5000/a2a/chart-recommender-agent/message:stream"

Write-Host "Sending request to $messageUri" -ForegroundColor Cyan
try {
    $response = Invoke-RestMethod -Uri $messageUri -Method Post -ContentType "application/json" -Body $body
    $response | ConvertTo-Json -Depth 6
} catch {
    Write-Host "Error: $_" -ForegroundColor Red
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $reader.ReadToEnd()
    }
}
