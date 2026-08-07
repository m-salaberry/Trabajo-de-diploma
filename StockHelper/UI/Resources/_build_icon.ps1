# Genera un .ico multi-resolucion para StockHelper.
# Diseno: caja de carton blanca (cinta al medio) sobre cuadrado redondeado azul.
# Formato: entradas DIB (BMP 32bpp) hasta 128px + PNG para 256px, que es lo que
# entienden tanto el shell de Windows como GDI+/WinForms.
Add-Type -AssemblyName System.Drawing

$OutFile = $args[0]
if (-not $OutFile) { throw "Falta la ruta de salida" }

function New-IconBitmap([int]$s) {
    $bmp = New-Object System.Drawing.Bitmap($s, $s, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $g.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
    $g.Clear([System.Drawing.Color]::Transparent)

    # ---- fondo: cuadrado redondeado con degradado azul ----
    $r = [Math]::Max(2.0, $s * 0.18)
    $d = $r * 2
    $path = New-Object System.Drawing.Drawing2D.GraphicsPath
    $path.AddArc(0, 0, $d, $d, 180, 90)
    $path.AddArc($s - $d, 0, $d, $d, 270, 90)
    $path.AddArc($s - $d, $s - $d, $d, $d, 0, 90)
    $path.AddArc(0, $s - $d, $d, $d, 90, 90)
    $path.CloseFigure()

    $c1 = [System.Drawing.Color]::FromArgb(255, 52, 108, 163)
    $c2 = [System.Drawing.Color]::FromArgb(255, 20, 45, 76)
    $p1 = New-Object System.Drawing.PointF(0, 0)
    $p2 = New-Object System.Drawing.PointF($s, $s)
    $bg = New-Object System.Drawing.Drawing2D.LinearGradientBrush($p1, $p2, $c1, $c2)
    $g.FillPath($bg, $path)

    # ---- caja isometrica: tapa + cara izquierda + cara derecha ----
    $bw   = $s * 0.62               # ancho total de la caja
    $half = $bw / 2.0
    $top  = $bw * 0.26              # alto de media tapa (el rombo)
    $body = $bw * 0.60              # alto de las caras verticales
    $cx   = $s / 2.0
    $y0   = ($s - (2 * $top + $body)) / 2.0    # centrado vertical
    $yMid = $y0 + $top                          # vertices laterales de la tapa
    $yCtr = $y0 + 2 * $top                      # vertice inferior de la tapa
    $yBot = $yCtr + $body

    $cTop   = [System.Drawing.Color]::FromArgb(255, 255, 255, 255)   # tapa
    $cLeft  = [System.Drawing.Color]::FromArgb(255, 223, 231, 241)   # cara iluminada
    $cRight = [System.Drawing.Color]::FromArgb(255, 176, 192, 212)   # cara en sombra

    $faceTop = @(
        (New-Object System.Drawing.PointF($cx, $y0)),
        (New-Object System.Drawing.PointF(($cx + $half), $yMid)),
        (New-Object System.Drawing.PointF($cx, $yCtr)),
        (New-Object System.Drawing.PointF(($cx - $half), $yMid)))
    $faceLeft = @(
        (New-Object System.Drawing.PointF(($cx - $half), $yMid)),
        (New-Object System.Drawing.PointF($cx, $yCtr)),
        (New-Object System.Drawing.PointF($cx, $yBot)),
        (New-Object System.Drawing.PointF(($cx - $half), ($yBot - $top))))
    $faceRight = @(
        (New-Object System.Drawing.PointF(($cx + $half), $yMid)),
        (New-Object System.Drawing.PointF($cx, $yCtr)),
        (New-Object System.Drawing.PointF($cx, $yBot)),
        (New-Object System.Drawing.PointF(($cx + $half), ($yBot - $top))))

    $g.FillPolygon((New-Object System.Drawing.SolidBrush $cLeft),  $faceLeft)
    $g.FillPolygon((New-Object System.Drawing.SolidBrush $cRight), $faceRight)
    $g.FillPolygon((New-Object System.Drawing.SolidBrush $cTop),   $faceTop)

    # Sin cinta ni detalles extra: las tres caras con distinto valor ya leen como caja
    # incluso a 16 px, y cualquier linea adicional se convierte en ruido a ese tamano.

    $g.Dispose()
    return $bmp
}

function Get-PngBytes([System.Drawing.Bitmap]$bmp) {
    $ms = New-Object System.IO.MemoryStream
    $bmp.Save($ms, [System.Drawing.Imaging.ImageFormat]::Png)
    $bytes = $ms.ToArray()
    $ms.Dispose()
    return ,$bytes
}

# Arma una entrada DIB: BITMAPINFOHEADER (con alto doble) + pixeles BGRA bottom-up + mascara AND vacia.
function Get-DibBytes([System.Drawing.Bitmap]$bmp) {
    $w = $bmp.Width; $h = $bmp.Height
    $rect = New-Object System.Drawing.Rectangle(0, 0, $w, $h)
    $data = $bmp.LockBits($rect, [System.Drawing.Imaging.ImageLockMode]::ReadOnly,
                          [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $stride = $data.Stride
    $buf = New-Object byte[] ($stride * $h)
    [System.Runtime.InteropServices.Marshal]::Copy($data.Scan0, $buf, 0, $buf.Length)
    $bmp.UnlockBits($data)

    $maskRow = [int](([Math]::Floor(($w + 31) / 32)) * 4)
    $ms = New-Object System.IO.MemoryStream
    $bw = New-Object System.IO.BinaryWriter($ms)

    $bw.Write([UInt32]40)          # biSize
    $bw.Write([Int32]$w)           # biWidth
    $bw.Write([Int32]($h * 2))     # biHeight = imagen + mascara
    $bw.Write([UInt16]1)           # biPlanes
    $bw.Write([UInt16]32)          # biBitCount
    $bw.Write([UInt32]0)           # biCompression = BI_RGB
    $bw.Write([UInt32]($w * $h * 4 + $maskRow * $h))
    $bw.Write([Int32]0); $bw.Write([Int32]0)
    $bw.Write([UInt32]0); $bw.Write([UInt32]0)

    for ($y = $h - 1; $y -ge 0; $y--) { $bw.Write($buf, $y * $stride, $w * 4) }   # bottom-up
    $bw.Write((New-Object byte[] ($maskRow * $h)))                                # mascara AND en cero

    $bw.Flush()
    $bytes = $ms.ToArray()
    $bw.Dispose(); $ms.Dispose()
    return ,$bytes
}

$sizes = @(256, 128, 64, 48, 32, 16)
$images = @()
foreach ($s in $sizes) {
    $bmp = New-IconBitmap $s
    $isPng = ($s -ge 256)
    $data = if ($isPng) { Get-PngBytes $bmp } else { Get-DibBytes $bmp }
    $images += ,@{ Size = $s; Data = $data; Png = $isPng }
    $bmp.Dispose()
}

$fs = [System.IO.File]::Create($OutFile)
$bw = New-Object System.IO.BinaryWriter($fs)
$bw.Write([UInt16]0)
$bw.Write([UInt16]1)
$bw.Write([UInt16]$images.Count)

$offset = 6 + (16 * $images.Count)
foreach ($img in $images) {
    $dim = if ($img.Size -ge 256) { 0 } else { $img.Size }
    $bw.Write([Byte]$dim); $bw.Write([Byte]$dim)
    $bw.Write([Byte]0); $bw.Write([Byte]0)
    $bw.Write([UInt16]1); $bw.Write([UInt16]32)
    $bw.Write([UInt32]$img.Data.Length)
    $bw.Write([UInt32]$offset)
    $offset += $img.Data.Length
}
foreach ($img in $images) { $bw.Write($img.Data) }
$bw.Flush(); $bw.Dispose(); $fs.Dispose()

$fmt = ($images | ForEach-Object { "{0}{1}" -f $_.Size, $(if ($_.Png) { "(png)" } else { "(dib)" }) }) -join ", "
"OK -> $OutFile ({0} KB) :: {1}" -f [Math]::Round((Get-Item $OutFile).Length / 1KB, 1), $fmt
