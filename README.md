# 🛠️ SysDoctor - Reparador e Otimizador de Windows

![Version](https://img.shields.io/badge/version-2.0-blue)
![.NET](https://img.shields.io/badge/.NET-10.0-purple)
![Platform](https://img.shields.io/badge/platform-Windows%2010%2F11-green)
![License](https://img.shields.io/badge/license-Custom-orange)

Um aplicativo console em C# que fornece ferramentas práticas para otimizar, limpar e diagnosticar seu sistema Windows.

**✨ Novidades da Versão 2.0:**
- 🔐 Sistema de autenticação de usuários
- 🎯 14 funcionalidades de otimização (anteriormente 10)
- 🔄 Todas as otimizações são reversíveis
- 📊 Interface aprimorada com Spectre.Console
- 📝 Documentação completa com todos os comandos

## � Índice
- [Características](#-características)
  - [Ferramentas do Sistema](#-ferramentas-do-sistema)
  - [Limpeza e Otimização](#-limpeza-e-otimização)
- [Otimizador Windows - Detalhamento](#-detalhamento-otimizador-windows-opção-12)
- [Como Usar](#-como-usar)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Tratamento de Erros](#-tratamento-de-erros)
- [Interface](#-interface)
- [Troubleshooting](#-troubleshooting)
- [Changelog](#-changelog)

## �📋 Características

### 🔧 Ferramentas do Sistema
- **[1] Informação da Máquina** - Exibe detalhes do hardware e sistema operacional
  - Usa `System.Management` para coletar informações do sistema
- **[3] Scanner do Windows** - Verifica a integridade do sistema Windows
  - **Comando:** `sfc /scannow` - System File Checker
  - **Comando:** `DISM /Online /Cleanup-Image /RestoreHealth`
- **[5] SpeedTest** - Testa a velocidade de internet
  - Conexão com servidores externos para medição de velocidade
- **[7] Teste de Ping** - Realiza testes de conexão de rede
  - **Comando C#:** `System.Net.NetworkInformation.Ping`
- **[9] Otimizar Wifi** - Otimiza configurações de conectividade WiFi
  - **Comando:** `netsh wlan set autoconfig enabled=no interface="Wi-Fi"`
  - **Comando:** `netsh wlan set profileparameter name="<perfil>" connectiontype=ESS`
- **[11] Verificar Temperatura** - Monitora temperatura do processador
  - Usa WMI (Windows Management Instrumentation)
- **[13] Criar Ponto de Restauração** - Cria ponto de restauração do sistema
  - **Comando:** `Checkpoint-Computer -Description "SysDoctor Restore Point" -RestorePointType "MODIFY_SETTINGS"`
- **[15] Atualizar Windows** - Verifica e aplica atualizações do sistema
  - **Comando:** `Get-WindowsUpdate` e `Install-WindowsUpdate`
- **[17] Pack de Programas** - Gerencia instalação de programas úteis
  - Instalação automatizada via Chocolatey ou Winget

### 🧹 Limpeza e Otimização
- **[2] Limpar SSD/HD** - Remove arquivos temporários e desnecessários
  - **Comando:** `cleanmgr.exe /sagerun:1`
  - **Comando:** `Remove-Item "$env:TEMP\*" -Recurse -Force`
- **[4] Limpar Memória RAM** - Libera memória do sistema
  - **Código C#:** `[System.Runtime.GCSettings]::LargeObjectHeapCompactionMode = 1`
  - **Código C#:** `[System.GC]::Collect()`
  - Usa RAMMAP.exe da Sysinternals
- **[6] Limpar Caches de Wifi/Ethernet** - Limpa cache de rede
  - **Comando:** `ipconfig /flushdns`
  - **Comando:** `netsh winsock reset`
  - **Comando:** `netsh int ip reset`
- **[8] Otimizar Ping** - Melhora latência de rede
  - **Comando:** `reg add "HKLM\SOFTWARE\Microsoft\MSMQ\Parameters" /v TCPNoDelay /t REG_DWORD /d 1 /f`
  - **Comando:** `netsh int tcp set global autotuninglevel=normal`
- **[10] Mapa de Conexão** - Visualiza conexões de rede ativas
  - **Comando:** `netstat -ano`
  - **Comando:** `Get-NetTCPConnection`
- **[12] Otimizar Windows** - Aplica várias otimizações do sistema (veja detalhes abaixo)
- **[14] Configuração Pós-Instalação** - Configura sistema após nova instalação
  - Conjunto de otimizações recomendadas pós-formatação
- **[16] Rodar Windows Defender** - Executa varredura de antivírus
  - **Comando:** `Start-MpScan -ScanType FullScan`
- **[18] ISO Windows 11 Pro** - Download de ISO otimizada do Windows 11
  - Link direto para download oficial da Microsoft

## � Detalhamento: Otimizador Windows (Opção 12)

O **Otimizador Windows** oferece 14 funcionalidades específicas para melhorar o desempenho do sistema:

### ⚡ Otimizações de Sistema

#### **[1] Melhorar Desempenho de Energia**
Aplica esquema de energia de alto desempenho
- **Comando:** `powercfg -duplicatescheme e9a42b02-d5df-448d-aa00-03f14749eb61`
- **Comando:** `powercfg.exe /setacvalueindex SCHEME_CURRENT SUB_PROCESSOR IdleDisable 0`
- **Comando:** `powercfg.exe /setactive SCHEME_CURRENT`

#### **[3] Tornar ALT+TAB Mais Rápido**
Ativa o modo clássico de ALT+TAB (recomendado para PCs fracos)
- **Comando:** `Set-ItemProperty -Path 'HKCU:\Software\Microsoft\Windows\CurrentVersion\Explorer' -Name 'AltTabSettings' -Type DWord -Value 1`
- **Ação:** Reinicia o Windows Explorer

#### **[5] Desligar Serviços que Deixam o PC Lento**
Desativa serviços desnecessários do Windows
- **Comando:** `sc.exe stop <serviço>` e `sc.exe config <serviço> start= disabled`
- **Serviços desativados:**
  - Spooler (Impressora)
  - wisvc (Windows Insider Service)
  - WerSvc (Relatório de Erros)
  - WbioSrvc (Biometria)
  - DiagTrack (Telemetria)
  - dmwappushservice (Push Notifications)
  - wuauserv (Windows Update)
  - dosvc (Delivery Optimization)

#### **[7] Desligar Overlays em Jogos**
Desativa Game Bar e Game Mode do Xbox
- **Comandos de Registro:**
  - `reg add "HKCU\Software\Microsoft\GameBar" /v "AllowAutoGameMode" /t REG_DWORD /d 0 /f`
  - `reg add "HKCU\Software\Microsoft\GameBar" /v "AutoGameModeEnabled" /t REG_DWORD /d 0 /f`
  - `reg add "HKCU\System\GameConfigStore" /v "GameDVR_Enabled" /t REG_DWORD /d 0 /f`

#### **[9] Desligar Hibernação**
Remove o arquivo hiberfil.sys e libera espaço em disco
- **Comando:** `powercfg /hibernate off`

#### **[11] Desligar Recursos de Virtualização**
Desativa Hyper-V e recursos de virtualização
- **Comando:** `Disable-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V-All -NoRestart`
- **Comando:** `Disable-WindowsOptionalFeature -Online -FeatureName VirtualMachinePlatform -NoRestart`
- **Comando:** `Disable-WindowsOptionalFeature -Online -FeatureName HypervisorPlatform -NoRestart`

#### **[13] Desligar Downloads em Segundo Plano**
Desativa o serviço MapsBroker (Maps Manager)
- **Comando:** `sc.exe stop MapsBroker`
- **Comando:** `sc.exe config MapsBroker start= disabled`

### 🔧 Otimizações Avançadas

#### **[2] Melhorar Aparência e Desempenho**
Ajusta efeitos visuais para priorizar desempenho
- **Comando:** `reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects" /v VisualFXSetting /t REG_DWORD /d 2 /f`
- **Comando:** `reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize" /v EnableTransparency /t REG_DWORD /d 0 /f`
- **Comando:** `reg add "HKCU\Control Panel\Desktop" /v UserPreferencesMask /t REG_BINARY /d 9012038010000000 /f`

#### **[4] Reduzir Coleta de Dados do Windows**
Desativa telemetria e coleta de dados da Microsoft
- **Comando:** `REG ADD "HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection" /v "AllowTelemetry" /t REG_DWORD /d 0 /f`
- **Comando:** `REG ADD "HKLM\SOFTWARE\Policies\Microsoft\Windows\System" /v "AllowAppDataCollection" /t REG_DWORD /d 0 /f`
- **Comando:** `REG ADD "HKLM\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo" /v "DisableWindowsAdvertising" /t REG_DWORD /d 1 /f`

#### **[6] Remover Apps Desnecessários**
Remove bloatware do Windows (Debloater)
- **Comando PowerShell:** `Get-AppxPackage *<app>* | Remove-AppxPackage`
- **Apps removidos:**
  - Cortana
  - Office Hub
  - Phone Link (Your Phone)
  - Mensagens
  - Mapas
  - Groove Music
  - Get Started
  - Mail e Calendar
  - Alarmes
  - 3D Builder
  - Bing News
  - OneDrive
- **Desativa Copilot:**
  - `reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced" /v ShowCopilotButton /t REG_DWORD /d 0 /f`
  - `reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Copilot" /v TurnOffWindowsCopilot /t REG_DWORD /d 1 /f`
  - `reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search" /v AllowCortana /t REG_DWORD /d 0 /f`

#### **[8] Reduzir Avisos de Segurança**
Desativa o UAC (User Account Control)
- **Verificação:** `sfc /scannow` - Verifica integridade do sistema
- **Comando:** `reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System" /v EnableLUA /t REG_DWORD /d 0 /f`
- ⚠️ **ATENÇÃO:** Reduz a segurança do sistema!

#### **[10] Acelerar Pesquisa de Arquivos**
Desativa o serviço de indexação (Windows Search)
- **Comando:** `sc.exe stop WSearch`
- **Comando:** `sc.exe config WSearch start= disabled`

#### **[12] Desligar Efeitos Visuais Extras**
Desativa Aero Peek (preview de janelas)
- **Comando:** `reg add "HKCU\Software\Microsoft\Windows\DWM" /v EnableAeroPeek /t REG_DWORD /d 0 /f`

#### **[14] Reduzir Alertas do SmartScreen**
Desativa filtro SmartScreen
- **Comando:** `reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer" /v SmartScreenEnabled /t REG_SZ /d Off /f`
- **Comando:** `reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\System" /v EnableSmartScreen /t REG_DWORD /d 0 /f`

### ⚙️ Características do Otimizador

- ✅ **Reversível** - Todas as otimizações podem ser desfeitas
- 🔄 **Interativo** - Menu de escolha para cada otimização
- ⚠️ **Avisos** - Alertas sobre impactos de cada funcionalidade
- 📊 **Progress Bar** - Acompanhamento visual em tempo real
- ✅ **Tratamento de Erros** - Exibe mensagens claras em caso de falha



### Requisitos
- Windows 10 ou superior
- .NET 10.0 ou superior
- PowerShell 5.1 ou superior

### Autenticação de Usuário
Antes de acessar o programa, é necessário validar o email:
- Sistema integrado de autenticação via API
- Email deve estar cadastrado no banco de dados
- Validação ocorre antes de mostrar o menu principal

**Código C# de validação:**
```csharp
bool acessoPermitido = await CheckUsers.Executar();
```

### Instalação e Execução

#### Opção 1: Executar com dotnet
```powershell
cd C:\Users\Usuario\Documents\Python\SysDoctor\SysDoctor
dotnet run
```

#### Opção 2: Executar o arquivo compilado
```powershell
C:\Users\Usuario\Documents\Python\SysDoctor\SysDoctor\bin\Debug\net10.0\win-x64\SysDoctor.exe
```

#### Opção 3: Com privilégios de administrador (recomendado)
Abra o PowerShell como administrador e execute:
```powershell
cd C:\Users\Usuario\Documents\Python\SysDoctor\SysDoctor
dotnet run
```

### Menu de Navegação

1. O programa exibe um menu interativo com duas colunas
2. Digite o número correspondente à funcionalidade desejada
3. Pressione Enter para executar
4. Digite **0** para sair do programa

### Privilégios de Administrador

- ✅ O programa funciona **sem privilégios de administrador**
- ⚠️ Algumas funcionalidades podem funcionar melhor com privilégios elevados
- 🛡️ O status é exibido no topo: **Verde (com admin)** ou **Amarelo (sem admin)**

## 📦 Estrutura do Projeto

```
SysDoctor/
├── Program.cs                 # Arquivo principal com menu
├── GlobalUsings.cs            # Imports globais
├── app.manifest               # Configuração de privilégios
├── SysDoctor.csproj           # Arquivo de projeto
├── Scripts/                   # Funcionalidades individuais
│   ├── InfoMachine.cs         # Informações do sistema
│   ├── ClearDisk.cs           # Limpeza de disco
│   ├── ClearRAM.cs            # Limpeza de memória
│   ├── SpeedTest.cs           # Teste de velocidade
│   ├── TestPing.cs            # Teste de ping
│   ├── OtmWindows.cs          # Otimizações do Windows
│   ├── RunDefender.cs         # Windows Defender
│   ├── UpdateWindows.cs       # Atualizações
│   ├── MapNet.cs              # Mapa de rede
│   ├── checkTemperature.cs    # Temperatura do sistema
│   └── ...outros scripts
└── bin/
    └── Debug/
        └── net10.0/
            └── win-x64/       # Executável compilado
```

## 🔧 Tratamento de Erros

O programa inclui tratamento robusto de exceções:

- **UnauthorizedAccessException** - Quando faltam privilégios para uma operação
- **Mensagens amigáveis** - Informam ao usuário o que aconteceu
- **Continuidade** - O programa continua funcionando mesmo se uma funcionalidade falhar

### Exemplo de Erro Tratado
```
❌ ACESSO NEGADO

⚠️  A funcionalidade 'Limpeza de Disco' requer privilégios de administrador!

💡 Para usar esta funcionalidade:
1. Execute o programa como administrador
2. Ou tente novamente com privilégios elevados
```

## 🎨 Interface

O programa utiliza **Spectre.Console** para:
- ✨ Cores e emojis no console
- 📊 Barras de progresso animadas
- 🎯 Layout centralizado e responsivo
- 📋 Tabelas organizadas em duas colunas
- 🎭 FigletText para títulos grandes e destacados
- ⏱️ Indicadores de tempo de execução

### Menu Principal
```
╔═══════════════════════════════════════════════════════════╗
║           🛡️  EXECUTANDO COMO ADMINISTRADOR ✅           ║
╚═══════════════════════════════════════════════════════════╝

┌─────────────────────────────────────────┬─────────────────────────────────────────┐
│ 🔧 Ferramentas do Sistema              │ 🧹 Limpeza e Otimização                │
├─────────────────────────────────────────┼─────────────────────────────────────────┤
│ [[ 1 ]] Informações do Computador      │ [[ 2 ]] Limpar Disco (SSD / HD)        │
│ [[ 3 ]] Verificar Sistema do Windows   │ [[ 4 ]] Liberar Memória RAM            │
│ ...                                     │ ...                                     │
└─────────────────────────────────────────┴─────────────────────────────────────────┘

📋 [[ 0 ]] Sair
🎯 Digite sua opção:
```

### Exemplo de Output com Progress Bar
```
⚡ Otimizando energia do PC...
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ 100% | 00:05
✓ Sucesso em aplicar CFG de otimização de energia
✓ Otimização de Energia Completa com sucesso!
```

## 🏗️ Arquitetura do Código

### Estrutura de Execução Assíncrona
O programa utiliza async/await para operações que podem demorar:
```csharp
public static async Task Main(string[] args)
{
    // 1. Autenticação
    bool acessoPermitido = await CheckUsers.Executar();
    
    // 2. Menu Principal
    await MenuPrincipalAsync();
}
```

### Pattern de Execução com Tratamento de Erros
```csharp
await ExecutarComTratamentoAsync(
    () => Task.Run(() => OtmWindows.Executar()), 
    "Otimizar Windows"
);
```

Cada funcionalidade é encapsulada em um try-catch que:
1. Captura `UnauthorizedAccessException` para privilégios
2. Captura `AggregateException` para erros assíncronos
3. Exibe mensagens amigáveis ao usuário
4. Permite que o programa continue funcionando

## ⚙️ Configuração

### Manifest (app.manifest)
- Configurado como `asInvoker` para permitir execução sem privilégios obrigatórios
- Suporta UTF-8 e caracteres especiais
- Compatível com Windows 10 e superiores

### Dependências Principais
- **Spectre.Console** - Interface de console avançada
- **System.Management** - Acesso a informações do sistema
- **System.Net.Http** - Requisições HTTP para autenticação
- **.NET 10.0** - Framework base

## 🔐 APIs e Integrações

### Sistema de Autenticação
O SysDoctor implementa autenticação de usuários via API REST:
```csharp
// CheckUsers.cs - Validação de email
HttpResponseMessage response = await client.GetAsync($"api/users/validate?email={email}");
```
- Conecta-se a um servidor externo para validar emails
- Impede acesso não autorizado ao sistema
- Armazena sessão durante a execução

### Windows Management Instrumentation (WMI)
Usado para coletar informações detalhadas do sistema:
```csharp
ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
```

### Process.Start para Comandos do Sistema
Todas as otimizações executam comandos do Windows:
```csharp
ProcessStartInfo psi = new ProcessStartInfo
{
    FileName = "powershell.exe",
    Arguments = "-Command \"<comando>\"",
    UseShellExecute = false,
    RedirectStandardOutput = true,
    CreateNoWindow = true
};
```

## 🐛 Troubleshooting

### Problema: "The requested operation requires elevation"
**Solução:** Abra o PowerShell como administrador e execute novamente

### Problema: Emojis não aparecem
**Solução:** O programa tenta configurar UTF-8 automaticamente. Se não funcionar, habilite UTF-8 no Windows:
```powershell
[System.Environment]::SetEnvironmentVariable('DOTNET_System_Globalization_Invariant', 'false', 'User')
```

### Problema: Script específico não funciona
**Verificação:**
1. Verifique se tem privilégios adequados
2. Leia a mensagem de erro exibida
3. Tente executar como administrador

### Problema: "sc.exe não encontrado"
**Solução:** Verifique a variável de ambiente PATH:
```powershell
$env:Path -split ';' | Select-String System32
```

### Problema: Windows Defender bloqueia o programa
**Solução:** 
1. Adicione exceção no Windows Defender
2. Ou compile o código você mesmo
```powershell
dotnet build -c Release
```

## 💡 Dicas de Uso

### Antes de Otimizar
1. ✅ **Crie um ponto de restauração** (opção 13)
2. ✅ **Execute como administrador** para acesso completo
3. ✅ **Leia os avisos** de cada otimização
4. ✅ **Entenda o impacto** de cada mudança

### Otimizações Recomendadas para Jogos
```
[1] Melhorar Desempenho de Energia
[2] Melhorar Aparência e Desempenho
[5] Desligar Serviços que Deixam o PC Lento
[7] Desligar Overlays em Jogos
[9] Desligar Hibernação
[12] Desligar Efeitos Visuais Extras
```

### Otimizações para Privacidade
```
[4] Reduzir Coleta de Dados do Windows
[6] Remover Apps Desnecessários
[14] Reduzir Alertas do SmartScreen
```

### Para Reverter Todas as Mudanças
Todas as funcionalidades do Otimizador Windows (opção 12) possuem opção de reverter:
1. Entre em cada otimização aplicada
2. Escolha a opção "Reverter" ou "Restaurar padrão"
3. Reinicie o PC quando solicitado

## 📊 Referência Rápida de Comandos

### Comandos PowerShell Utilizados
| Categoria | Comando | Descrição |
|-----------|---------|-----------|
| **Energia** | `powercfg -duplicatescheme <GUID>` | Duplica esquema de energia |
| **Energia** | `powercfg /hibernate off` | Desativa hibernação |
| **Serviços** | `sc.exe stop <nome>` | Para um serviço |
| **Serviços** | `sc.exe config <nome> start= disabled` | Desativa serviço |
| **Registro** | `reg add "<caminho>" /v <nome> /t <tipo> /d <valor> /f` | Adiciona/modifica chave de registro |
| **Apps** | `Get-AppxPackage <nome> \| Remove-AppxPackage` | Remove aplicativo UWP |
| **Rede** | `ipconfig /flushdns` | Limpa cache DNS |
| **Rede** | `netsh winsock reset` | Reseta Winsock |
| **Sistema** | `sfc /scannow` | Verifica integridade do sistema |
| **Sistema** | `DISM /Online /Cleanup-Image /RestoreHealth` | Repara imagem do Windows |
| **Features** | `Disable-WindowsOptionalFeature -Online -FeatureName <nome>` | Desativa recurso do Windows |

### Caminhos de Registro Importantes
| Caminho | Propósito |
|---------|-----------|
| `HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects` | Efeitos visuais |
| `HKCU\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize` | Transparência e temas |
| `HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection` | Telemetria |
| `HKCU\Software\Microsoft\GameBar` | Xbox Game Bar |
| `HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System` | UAC |
| `HKCU\Software\Microsoft\Windows\DWM` | Desktop Window Manager |

### Serviços Modificados
| Nome do Serviço | Nome Exibido | Impacto ao Desativar |
|-----------------|--------------|----------------------|
| `Spooler` | Spooler de Impressão | ⚠️ Impressoras não funcionarão |
| `WSearch` | Windows Search | Pesquisa de arquivos será mais lenta |
| `DiagTrack` | Diagnósticos | Menos telemetria enviada à Microsoft |
| `wuauserv` | Windows Update | ⚠️ Atualizações não serão automáticas |
| `MapsBroker` | Maps Manager | Mapas offline não funcionarão |

## 📝 Notas de Desenvolvimento

- Linguagem: **C# 12**
- Target Framework: **.NET 10.0**
- Runtime: **win-x64**
- Padrão: **async/await** para operações assíncronas
- Logs: Console apenas (sem arquivo de log)

## 🔐 Segurança

- ✅ Verifica permissões antes de operações críticas
- ✅ Mensagens de aviso antes de limpeza
- ✅ Sem modificação de arquivos de sistema críticos
- ✅ Suporta execução limitada sem admin
- ⚠️ **Algumas otimizações reduzem a segurança** (UAC, SmartScreen)
- 🔄 **Todas as mudanças são reversíveis**

## ⚠️ Avisos Importantes

### Uso por Sua Conta e Risco
Este programa modifica configurações do sistema operacional. Embora todas as alterações sejam reversíveis:
- 🛡️ **Sempre crie um ponto de restauração antes** de fazer otimizações
- 📖 **Leia as descrições** de cada funcionalidade
- ⚙️ **Entenda o que cada comando faz** antes de executar
- 🔙 **Saiba como reverter** cada mudança

### Compatibilidade
- ✅ Windows 10 (todas as versões)
- ✅ Windows 11 (todas as versões)
- ❌ Windows 7/8 não são oficialmente suportados
- ⚠️ Algumas funcionalidades podem não funcionar em versões Home

### Requisitos de Administrador
Funcionalidades que **exigem** privilégios de administrador:
- Todas as opções do Otimizador Windows (12)
- Limpar Disco (2)
- Scanner do Windows (3)
- Atualizar Windows (15)
- Windows Defender (16)

Funcionalidades que funcionam **sem** administrador:
- Informação da Máquina (1)
- SpeedTest (5)
- Teste de Ping (7)
- Verificar Temperatura (11)

## 📞 Suporte e Contribuição

### Reportar Bugs
Para reportar bugs ou problemas:
1. Descreva o erro em detalhes
2. Inclua a mensagem de erro completa
3. Informe a versão do Windows
4. Mencione se está executando como administrador

### Sugerir Melhorias
Sugestões são bem-vindas! Entre em contato com:
- **Desenvolvedor:** Bert00100
- **Email:** [Contato disponível via autenticação]

### Compilar do Código Fonte
```powershell
# Clone o repositório
cd C:\Users\Usuario\Documents\Csharp\SysDoctor

# Restaurar dependências
dotnet restore

# Compilar
dotnet build -c Release

# Executar
dotnet run
```

## 📄 Licença e Créditos

### Tecnologias Utilizadas
- **C# 12** - Linguagem de programação
- **.NET 10.0** - Framework
- **Spectre.Console** - Interface de console ([MIT License](https://github.com/spectreconsole/spectre.console))
- **RAMMAP** - Sysinternals (Microsoft)

### Aviso Legal
Este software é fornecido "como está", sem garantias de qualquer tipo. O desenvolvedor não se responsabiliza por quaisquer danos causados pelo uso deste programa. Use por sua conta e risco.

---

**Versão:** 2.0  
**Última atualização:** 20 de Dezembro de 2025  
**Desenvolvedor:** Bert00100  
**Framework:** .NET 10.0  
**Plataforma:** Windows 10/11 (x64)

## 📋 Changelog

### Versão 2.0 (Dezembro 2025)
- ✅ Adicionado sistema de autenticação de usuários (CheckUsers)
- ✅ Expandido Otimizador Windows de 10 para 14 funcionalidades
- ✅ Todas as otimizações agora são reversíveis
- ✅ Adicionada ISO Windows 11 Pro (opção 18)
- ✅ Melhorada interface com Spectre.Console
- ✅ Adicionado tratamento robusto de erros
- ✅ Documentação completa com comandos executados

### Versão 1.0 (2024)
- 🎉 Lançamento inicial
- 🔧 16 funcionalidades básicas
- 🛠️ Otimizações fundamentais do sistema
