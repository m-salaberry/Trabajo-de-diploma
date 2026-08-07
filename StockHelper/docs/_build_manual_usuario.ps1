# Genera el Manual de Usuario de StockHelper en formato Word (.docx) a partir de
# un archivo JSON con el contenido (docs\_manual_usuario.json) y las capturas de
# la carpeta de imagenes indicada en el JSON (imageBaseDir).
#
# Uso:  powershell -ExecutionPolicy Bypass -File .\_build_manual_usuario.ps1
#
# Requiere Microsoft Word instalado (usa automatizacion COM). No necesita pandoc.

$ErrorActionPreference = "Stop"

$here     = $PSScriptRoot
$jsonPath = Join-Path $here "_manual_usuario.json"
$outPath  = Join-Path $here "Manual_de_Usuario_StockHelper.docx"

if (-not (Test-Path $jsonPath)) { throw "No se encontro el JSON de contenido: $jsonPath" }

Write-Host "Leyendo contenido: $jsonPath" -ForegroundColor Cyan
$data = Get-Content -Raw -Encoding UTF8 $jsonPath | ConvertFrom-Json

$imgDir = $data.imageBaseDir
if (-not (Test-Path $imgDir)) { throw "No existe la carpeta de imagenes: $imgDir" }

# Constantes de Word
$wdStyleTitle     = -63   # "Title"
$wdStyleSubtitle  = -75   # "Subtitle"
$wdStyleHeading1  = -2
$wdStyleHeading2  = -3
$wdStyleHeading3  = -4
$wdStyleNormal    = -1
$wdStyleListBullet= -49
$wdAlignCenter    = 1
$wdColorGray      = 8421504
$wdSeekMainDoc    = 0

Write-Host "Abriendo Word..." -ForegroundColor Cyan
$word = New-Object -ComObject Word.Application
$word.Visible = $false
$word.DisplayAlerts = 0

try {
    $doc = $word.Documents.Add()
    $sel = $word.Selection

    function Add-Par([string]$text, [int]$style, [switch]$center, [switch]$italic, [switch]$gray) {
        $sel.Style = $style
        if ($center) { $sel.ParagraphFormat.Alignment = $wdAlignCenter }
        else { $sel.ParagraphFormat.Alignment = 0 }
        $sel.Font.Italic = if ($italic) { 1 } else { 0 }
        $sel.Font.Color  = if ($gray) { $wdColorGray } else { 0 }
        $sel.TypeText($text)
        $sel.TypeParagraph()
        # reset
        $sel.Font.Italic = 0
        $sel.Font.Color  = 0
    }

    function Add-Bullet([string]$label, [string]$desc) {
        $sel.Style = $wdStyleListBullet
        $sel.ParagraphFormat.Alignment = 0
        if ($label) {
            $sel.Font.Bold = 1
            $sel.TypeText($label)
            $sel.Font.Bold = 0
            if ($desc) { $sel.TypeText(": " + $desc) }
        } else {
            $sel.TypeText($desc)
        }
        $sel.TypeParagraph()
    }

    function Add-Image([string]$fileName) {
        $path = Join-Path $imgDir $fileName
        if (-not (Test-Path $path)) { Write-Host "  ADVERTENCIA: falta imagen $fileName" -ForegroundColor Red; return }
        $sel.Style = $wdStyleNormal
        $sel.ParagraphFormat.Alignment = $wdAlignCenter
        $shape = $sel.InlineShapes.AddPicture($path)
        # Limitar el ancho a ~16 cm (453 pt) manteniendo proporcion
        $maxW = 453.0
        if ($shape.Width -gt $maxW) {
            $ratio = $maxW / $shape.Width
            $shape.Width  = $maxW
            $shape.Height = [math]::Round($shape.Height * $ratio)
        }
        $sel.TypeParagraph()
        $sel.ParagraphFormat.Alignment = 0
    }

    # ---- Portada ----
    Add-Par $data.title $wdStyleTitle -center
    if ($data.subtitle) { Add-Par $data.subtitle $wdStyleSubtitle -center }
    if ($data.version)  { Add-Par $data.version  $wdStyleNormal -center -gray }
    $sel.InsertBreak(7)  # wdPageBreak

    # ---- Indice ----
    Add-Par "Contenido" $wdStyleHeading1
    $toc = $doc.TablesOfContents.Add($word.Selection.Range, $true, 1, 3)
    $sel.EndKey(6) | Out-Null  # wdStory
    $sel.InsertBreak(7)

    # ---- Secciones ----
    $n = $data.sections.Count
    $i = 0
    foreach ($s in $data.sections) {
        $i++
        Write-Host ("  [{0}/{1}] {2}" -f $i, $n, $s.heading) -ForegroundColor Green
        $lvl = if ($s.level) { $s.level } else { 1 }
        switch ($lvl) {
            1 { Add-Par $s.heading $wdStyleHeading1 }
            2 { Add-Par $s.heading $wdStyleHeading2 }
            default { Add-Par $s.heading $wdStyleHeading3 }
        }
        if ($s.image) { Add-Image $s.image }
        if ($s.intro) { Add-Par $s.intro $wdStyleNormal }
        if ($s.controlsTitle) { Add-Par $s.controlsTitle $wdStyleHeading3 }
        if ($s.controls) {
            foreach ($c in $s.controls) { Add-Bullet $c.name $c.desc }
        }
        if ($s.notes) {
            foreach ($note in $s.notes) { Add-Par $note $wdStyleNormal -italic -gray }
        }
    }

    # Actualizar el indice ahora que existe todo el contenido
    $doc.TablesOfContents.Item(1).Update() | Out-Null

    Write-Host "Guardando: $outPath" -ForegroundColor Cyan
    if (Test-Path $outPath) { Remove-Item $outPath -Force }
    $doc.SaveAs2([string]$outPath, 16)  # 16 = wdFormatDocumentDefault (.docx)
    $doc.Close()
    Write-Host "OK -> $outPath" -ForegroundColor Green
}
finally {
    $word.Quit()
    [System.Runtime.Interopservices.Marshal]::ReleaseComObject($word) | Out-Null
    [GC]::Collect(); [GC]::WaitForPendingFinalizers()
}
