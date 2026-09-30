param([string]$Configuration = "Debug")

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot
Push-Location $root
try {
    dotnet restore Lab2.sln
    if ($LASTEXITCODE -ne 0) { throw "Не удалось восстановить пакеты." }
    dotnet tool restore
    if ($LASTEXITCODE -ne 0) { throw "Не удалось восстановить ReportGenerator." }

    $results = Join-Path $root ("artifacts/coverage/" + [guid]::NewGuid())
    dotnet test Lab2.sln --no-restore --configuration $Configuration `
        --collect:"XPlat Code Coverage" --settings coverage.runsettings `
        --results-directory $results
    if ($LASTEXITCODE -ne 0) { throw "Есть непройденные тесты." }

    $files = @(Get-ChildItem -Path $results -Filter coverage.cobertura.xml -Recurse)
    if ($files.Count -ne 1) { throw "Ожидался один файл покрытия." }
    $coverageFile = $files[0].FullName
    [xml]$coverage = Get-Content -LiteralPath $coverageFile -Raw
    $culture = [System.Globalization.CultureInfo]::InvariantCulture
    $line = [double]::Parse($coverage.coverage.'line-rate', $culture) * 100
    $branch = [double]::Parse($coverage.coverage.'branch-rate', $culture) * 100
    Write-Host "Line Coverage: $line%; Branch Coverage: $branch%"
    if ($line -lt 90 -or $branch -lt 90) { throw "Покрытие ниже 90%." }

    dotnet tool run reportgenerator "-reports:$coverageFile" `
        "-targetdir:coveragereport" "-reporttypes:Html;TextSummary"
    if ($LASTEXITCODE -ne 0) { throw "Не удалось создать HTML-отчёт." }
    Write-Host "Отчёт: coveragereport/index.html"
}
finally {
    Pop-Location
}
