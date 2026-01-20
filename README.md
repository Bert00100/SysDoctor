# 🛠️ SysDoctor - Reparador e Otimizador de Windows

Um aplicativo console em C# que fornece ferramentas práticas para otimizar, limpar e diagnosticar seu sistema Windows.

## 📋 Características

### 🔧 Ferramentas do Sistema

- **[1] Informação da Máquina** - Exibe detalhes do hardware e sistema operacional
- **[3] Scanner do Windows** - Verifica a integridade do sistema Windows
- **[5] SpeedTest** - Testa a velocidade de internet
- **[7] Teste de Ping** - Realiza testes de conexão de rede
- **[9] Otimizar Wifi** - Otimiza configurações de conectividade WiFi
- **[11] Verificar Temperatura** - Monitora temperatura do processador
- **[13] Criar Ponto de Restauração** - Cria ponto de restauração do sistema
- **[15] Atualizar Windows** - Verifica e aplica atualizações do sistema
- **[17] Pack de Programas** - Gerencia instalação de programas úteis

### 🧹 Limpeza e Otimização

- **[2] Limpar SSD/HD** - Remove arquivos temporários e desnecessários
- **[4] Limpar Memória RAM** - Libera memória do sistema
- **[6] Limpar Caches de Wifi/Ethernet** - Limpa cache de rede
- **[8] Otimizar Ping** - Melhora latência de rede
- **[10] Mapa de Conexão** - Visualiza conexões de rede ativas
- **[12] Otimizar Windows** - Aplica várias otimizações do sistema
- **[14] Configuração Pós-Instalação** - Configura sistema após nova instalação
- **[16] Rodar Windows Defender** - Executa varredura de antivírus

## 🚀 Como Usar

### Requisitos

- Windows 10 ou superior
- .NET 10.0 ou superior
- PowerShell 5.1 ou superior

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
- 📊 Barras de progresso
- 🎯 Layout centralizado
- 📋 Tabelas organizadas

## ⚙️ Configuração

### Manifest (app.manifest)

- Configurado como `asInvoker` para permitir execução sem privilégios obrigatórios
- Suporta UTF-8 e caracteres especiais
- Compatível com Windows 10 e superiores

### Dependências Principais

- **Spectre.Console** - Interface de console avançada
- **System.Management** - Acesso a informações do sistema
- **.NET 10.0** - Framework base

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

## 📞 Suporte

Para reportar bugs ou sugerir melhorias, entre em contato com o desenvolvedor.

---

**Versão:** 1.0  
**Última atualização:** Dezembro 2025  
**Desenvolvedor:** Bert00100
