param(
    [Parameter(Mandatory=$false)]
    [string]$ProcessName = "",
    
    [Parameter(Mandatory=$false)]
    [switch]$EnableLogging = $false
)

function Write-LogMessage {
    param([string]$Message)
    if ($EnableLogging) {
        $logDir = Join-Path $PSScriptRoot "../logs"
        if (-not (Test-Path $logDir)) {
            New-Item -ItemType Directory -Force -Path $logDir | Out-Null
        }
        $logPath = Join-Path $logDir "debug.log"
        "$(Get-Date) - $Message" | Out-File -FilePath $logPath -Append
    }
}

# Sample data
$sampleArray = @(
    [PSCustomObject]@{
        Name = "Sample Process 1"
        Id = "001"
        Status = "Running"
    },
    [PSCustomObject]@{
        Name = "Sample Process 2"
        Id = "002"
        Status = "Stopped"
    },
    [PSCustomObject]@{
        Name = "Sample Process 3"
        Id = "003"
        Status = "Running"
    }
)

Write-LogMessage "Script started with ProcessName: '$ProcessName'"

if ([string]::IsNullOrWhiteSpace($ProcessName)) {
    Write-LogMessage "Returning all results"
    $sampleArray | ConvertTo-Json
}
else {
    Write-LogMessage "Filtering for ProcessName: '$ProcessName'"
    $filtered = $sampleArray | Where-Object { $_.Name -like "*$ProcessName*" }
    Write-LogMessage "Found $($filtered.Count) matches"
    $filtered | ConvertTo-Json
}
