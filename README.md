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
