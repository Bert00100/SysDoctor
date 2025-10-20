# 🩺 SysDoctor — Windows Optimizer & Repair Tool

![Windows](https://img.shields.io/badge/Windows-10%2F11-blue?style=flat-square)
![Python](https://img.shields.io/badge/Python-3.11+-yellow?style=flat-square)
![License](https://img.shields.io/badge/license-Free-green?style=flat-square)

---

## 📖 Sumário

- [Sobre o Projeto](#-sobre-o-projeto)
- [Requisitos](#-requisitos)
- [Funcionalidades Principais](#-funcionalidades-principais)
- [Módulo de Otimização do Windows](#-módulo-de-otimização-do-windows)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Permissões e Execução](#-permissões-e-execução)
- [Geração do Executável](#-geração-do-executável)
- [Logs e Depuração](#-logs-e-depuração)
- [Modo Pós-Instalação](#-modo-pós-instalação)
- [Segurança e Reversão](#-segurança-e-reversão)
- [Compatibilidade Testada](#-compatibilidade-testada)
- [Dicas de Uso](#-dicas-de-uso)
- [Licença](#-licença)
- [Autor](#-autor)

---

## 📘 Sobre o Projeto

O **SysDoctor** é uma ferramenta de otimização e reparo para sistemas **Windows 10 e 11**, desenvolvida em **Python** com integração direta ao **PowerShell**.  
Seu objetivo é oferecer um conjunto completo de **manutenção, diagnóstico e melhoria de desempenho** do sistema operacional — tudo via interface interativa e colorida no terminal.

🧩 Ele automatiza tarefas complexas do Windows: limpeza de disco, remoção de apps, ajustes de desempenho, atualização de drivers e muito mais.

---

## ⚙️ Requisitos

### 🧩 Dependências

**Bibliotecas Python necessárias:**
```bash
pip install colorama psutil wmi speedtest-cli
```

**Requisitos do sistema:**
- Windows 10 ou 11 (64 bits)
- PowerShell 5.1 ou superior
- Acesso de **Administrador**

---

## 🚀 Funcionalidades Principais

| Código | Função | Descrição |
|--------|---------|-----------|
| **1** | Informação da Máquina | Exibe nome do PC, usuário, BIOS e interfaces de rede |
| **2** | Limpar SSD/HD | Limpa arquivos temporários e desfragmenta discos |
| **3** | Scanner do Windows | Executa `DISM /RestoreHealth` para reparar o sistema |
| **4** | Limpar Sistema | Remove cache, logs e limpa RAM |
| **5** | Speed Test | Mede velocidade de internet via `speedtest-cli` |
| **6** | Limpar Rede | Reseta IP, DNS e Winsock |
| **7** | Teste de Ping | Verifica latência com servidores DNS |
| **8** | Otimizar Ping | Usa DnsJumper para reduzir latência |
| **9** | Otimizar Wi-Fi | Ajusta parâmetros TCP/IP |
| **10** | Mapa de Conexão | Rastreia rotas de rede (tracert) |
| **11** | Monitorar Temperatura | Usa OpenHardwareMonitor para medir CPU e GPU |
| **12** | Otimizar Windows | Acessa o menu de otimizações avançadas |
| **13** | Ponto de Restauração | Cria restauração de sistema via PowerShell |
| **14** | Pós-instalação | Executa scripts de configuração personalizados |
| **15** | Atualizar Windows | Busca e agenda atualizações |
| **16** | Windows Defender | Atualiza definições e executa verificação rápida |

---

## 🧠 Módulo de Otimização do Windows

O **menu de otimização** reúne ajustes e políticas para reduzir o consumo de recursos, aumentar o desempenho e melhorar a privacidade do usuário.

| Código | Função | Descrição |
|--------|---------|-----------|
| **1** | Otimizar Energia | Ativa plano de alto desempenho |
| **2** | Desativar Efeitos Visuais | Remove animações e transparências |
| **3** | Otimizar ALT+TAB | Alterna entre modo clássico e moderno |
| **4** | Desativar Telemetria | Bloqueia coleta de dados e rastreamento |
| **5** | Desativar Serviços Inúteis | Desliga serviços não essenciais (Spooler, DiagTrack, etc.) |
| **6** | Debloater | Remove apps padrão (Cortana, OfficeHub, OneDrive, etc.) |
| **7** | Desativar Overlays | Desliga Game Bar e Game Mode |
| **8** | Desativar UAC | Remove o prompt de Controle de Conta de Usuário |
| **9** | Desativar Hibernação | Libera espaço e acelera inicialização |
| **10** | Desativar Indexação | Para o serviço de busca e indexação de arquivos |
| **11** | Desativar Hyper-V | Desativa virtualização nativa do Windows |
| **12** | Desativar Aero Peek | Remove transparência da barra de tarefas |
| **13** | Desativar Download Maps Manager | Desativa o serviço de mapas offline |
| **14** | Desativar SmartScreen | Desativa filtro de downloads do Windows |

---

## 🧩 Estrutura do Projeto

```
SysDoctor/
│
├── SysDoctor.exe            # Executável gerado via PyInstaller
├── main.py                  # Código-fonte principal
│
├── Scripts/
│   ├── Apps/
│   │   ├── RamMap/
│   │   ├── DNS/
│   │   └── HardwareMonitor/
│   ├── Install/
│   └── ...
│
├── README.md
└── requirements.txt
```

---

## 🔐 Permissões e Execução

A maioria das funções requer **privilégios administrativos**.

```python
def is_admin():
    try:
        return ctypes.windll.shell32.IsUserAnAdmin()
    except:
        return False
```

Se não estiver executando como administrador, o programa tenta se elevar automaticamente via:

```python
ctypes.windll.shell32.ShellExecuteW(None, "runas", sys.executable, params, None, 1)
```

---

## 🛠️ Geração do Executável

Compile o SysDoctor com privilégios administrativos:

```bash
pyinstaller --onefile --uac-admin main.py -n sysdoctor
```

Isso garante que o `.exe` sempre solicitará elevação UAC ao iniciar.

---

## 🧾 Logs e Depuração

O SysDoctor utiliza funções de **debug estruturadas**:

```python
debug_step(n, msg)      # Exibe a etapa atual
debug_success(msg)      # Mostra sucesso
debug_error(msg)        # Indica falha
debug_warning(msg)      # Exibe aviso
header(title)           # Cabeçalho visual
txt_info(label, value)  # Exibe valores formatados
```

🟢 **Saída visualmente clara e colorida** com `colorama` — ideal para auditoria de processos.

---

## 🧰 Modo Pós-Instalação

Permite execução de scripts `.ps1` personalizados após uma reinstalação do Windows.  
Scripts são carregados automaticamente da pasta:

```
Scripts/Install/
```

---

## 🛡️ Segurança e Reversão

Todas as funções críticas possuem **modo reversão**, permitindo restaurar configurações originais do Windows:

```
[1] - Desativar
[2] - Reverter (Ativar)
```

---

## 💻 Compatibilidade Testada

| Sistema | Compatível | Observações |
|----------|-------------|-------------|
| Windows 10 Pro | ✅ | Totalmente funcional |
| Windows 11 Home | ✅ | Pode exigir confirmação UAC |
| Windows Server | ⚠️ | Alguns módulos indisponíveis |

---

## 💡 Dicas de Uso

- Execute **sempre como administrador**  
- Feche programas pesados antes de usar o **limpador de RAM**  
- Use o menu `[12] Otimizar Windows` para configurar desempenho global  
- Evite usar o **Debloater** em ambientes corporativos  

---

## 📦 Licença

Uso **Exclusivo Para Code Suporte and Hub**.  
Distribuição comercial requer autorização do autor.

---

## 🧑‍💻 Autor

**Felipe B. Franceschini**  
🧠 Desenvolvedor Python & Otimizador de Sistemas  
💬 “Feito para manter o Windows leve, limpo e rápido — do jeito certo.”

---
