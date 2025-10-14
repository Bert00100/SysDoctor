# Opera Browser Install Script - Versão Funcional
Write-Host "=== Instalador do Opera Browser ===" -ForegroundColor Cyan

# URL oficial direta do instalador do Opera
$downloadUrl = "https://download1.operacdn.com/ftp/pub/opera/desktop/109.0.5097.24/win/Opera_109.0.5097.24_Setup.x64.exe"
$installerPath = "$env:TEMP\Opera_Setup.exe"

Write-Host "`n[1/4] Baixando Opera Browser..." -ForegroundColor Yellow
Write-Host "URL: $downloadUrl" -ForegroundColor Gray

try {
    # Download do Opera
    Write-Host "Iniciando download..." -ForegroundColor Gray
    $progressPreference = 'SilentlyContinue'  # Desativa barra de progresso detalhada
    Invoke-WebRequest -Uri $downloadUrl -OutFile $installerPath -UserAgent "Mozilla/5.0" -UseBasicParsing
    
    if (Test-Path $installerPath) {
        $fileSize = (Get-Item $installerPath).Length / 1MB
        Write-Host "Download concluído! Tamanho: $([math]::Round($fileSize, 2)) MB" -ForegroundColor Green
    } else {
        throw "Arquivo não foi baixado corretamente"
    }
}
catch {
    Write-Host "Erro no download principal: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Tentando URL alternativa..." -ForegroundColor Yellow
    
    # URL alternativa
    try {
        $altUrl = "https://net.geo.opera.com/opera/stable/windows?utm_tryagain=yes"
        Write-Host "Usando instalador online..." -ForegroundColor Gray
        
        # Método alternativo usando o instalador online
        $webClient = New-Object System.Net.WebClient
        $webClient.DownloadFile($altUrl, $installerPath)
        $webClient.Dispose()
        
        if (Test-Path $installerPath) {
            Write-Host "Download alternativo bem-sucedido!" -ForegroundColor Green
        } else {
            throw "Download alternativo falhou"
        }
    }
    catch {
        Write-Host "Falha em todos os métodos de download." -ForegroundColor Red
        Write-Host "Você pode baixar manualmente em: https://www.opera.com/download" -ForegroundColor Yellow
        exit 1
    }
}

Write-Host "`n[2/4] Instalando Opera Browser..." -ForegroundColor Yellow
try {
    # Verificar se o arquivo existe
    if (-NOT (Test-Path $installerPath)) {
        throw "Arquivo de instalação não encontrado: $installerPath"
    }
    
    Write-Host "Executando instalação silenciosa..." -ForegroundColor Gray
    Write-Host "Isso pode levar alguns minutos..." -ForegroundColor Gray
    
    # Instalar Opera com parâmetros silenciosos
    $processArgs = @(
        "/silent",           # Instalação silenciosa
        "/launchopera=0",    # Não abrir o Opera após instalação
        "/setdefault=0",     # Não definir como navegador padrão
        "/desktopshortcut=1", # Criar atalho na área de trabalho
        "/allusers=1"        # Instalar para todos os usuários (se admin)
    )
    
    $process = Start-Process -FilePath $installerPath -ArgumentList $processArgs -Wait -PassThru -NoNewWindow
    
    # Aguardar um pouco mais para garantir a instalação
    Write-Host "Aguardando conclusão da instalação..." -ForegroundColor Gray
    Start-Sleep -Seconds 15
    
    if ($process.ExitCode -eq 0) {
        Write-Host "Instalação concluída com sucesso!" -ForegroundColor Green
    } elseif ($process.ExitCode -eq 3010) {
        Write-Host "Instalação concluída (reinicialização pendente)" -ForegroundColor Yellow
    } else {
        Write-Host "Instalação concluída com código: $($process.ExitCode)" -ForegroundColor Yellow
    }
}
catch {
    Write-Host "Erro na instalação: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Tentando instalação básica..." -ForegroundColor Yellow
    
    # Tentativa alternativa sem argumentos
    try {
        Start-Process -FilePath $installerPath -Wait -NoNewWindow
        Write-Host "Instalação básica concluída." -ForegroundColor Green
    }
    catch {
        Write-Host "Falha na instalação básica também." -ForegroundColor Red
        exit 1
    }
}

Write-Host "`n[3/4] Limpando arquivos temporários..." -ForegroundColor Yellow
try {
    if (Test-Path $installerPath) {
        Remove-Item $installerPath -Force
        Write-Host "Arquivo temporário removido!" -ForegroundColor Green
    }
}
catch {
    Write-Host "Aviso: Não foi possível remover o arquivo temporário" -ForegroundColor Yellow
}

Write-Host "`n[4/4] Verificando instalação..." -ForegroundColor Yellow

# Verificar se o Opera foi instalado
$installed = $false
$operaPaths = @(
    "$env:ProgramFiles\Opera\launcher.exe",
    "$env:LOCALAPPDATA\Programs\Opera\launcher.exe", 
    "$env:ProgramFiles (x86)\Opera\launcher.exe",
    "$env:USERPROFILE\AppData\Local\Programs\Opera\launcher.exe"
)

foreach ($path in $operaPaths) {
    if (Test-Path $path) {
        $versionInfo = (Get-Item $path).VersionInfo
        Write-Host "Opera encontrado em: $path" -ForegroundColor Green
        Write-Host "Versão: $($versionInfo.FileVersion)" -ForegroundColor White
        $installed = $true
        break
    }
}

if (-NOT $installed) {
    # Verificar no registro
    Write-Host "Verificando registro..." -ForegroundColor Gray
    $registryPaths = @(
        "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\*",
        "HKLM:\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\*", 
        "HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\*"
    )
    
    foreach ($regPath in $registryPaths) {
        try {
            $programs = Get-ItemProperty $regPath -ErrorAction SilentlyContinue
            $operaReg = $programs | Where-Object { $_.DisplayName -like "*Opera*" }
            if ($operaReg) {
                Write-Host "Opera instalado: $($operaReg.DisplayName)" -ForegroundColor Green
                Write-Host "Versão: $($operaReg.DisplayVersion)" -ForegroundColor White
                $installed = $true
                break
            }
        }
        catch {
            # Continuar se não conseguir acessar algum caminho do registro
        }
    }
}

if ($installed) {
    Write-Host "`n=== Opera Browser instalado com sucesso! ===" -ForegroundColor Green
} else {
    Write-Host "`n=== Aviso: Não foi possível confirmar a instalação automaticamente ===" -ForegroundColor Yellow
    Write-Host "O Opera pode ter sido instalado. Verifique manualmente." -ForegroundColor Gray
}

Write-Host "`nProcesso concluído. Pressione qualquer tecla para continuar..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")