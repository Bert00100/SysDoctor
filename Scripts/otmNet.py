import subprocess
import os
import platform
import psutil
import socket
from colorama import Fore, Style, init

# Inicializa suporte a cores no terminal
init(autoreset=True)

def header(title):
    print(Fore.CYAN + f"\n=== {title} ===" + Style.RESET_ALL)

def txt_info(label, value):
    print(Fore.YELLOW + f"{label:<30}: " + Style.RESET_ALL + f"{value}")

def dnsOtm():
    flushDNS = subprocess.run(
        ["powershell", "-Command", "ipconfig /flushdns"],
        capture_output=True,
        text=True
    )

    reRegistDNS = subprocess.run(
        ["powershell", "-Command", "ipconfig /registerdns"],
        capture_output=True,
        text=True
    )

    erros = []
    if flushDNS.stderr.strip():
        erros.append("Limpar DNS")
    if reRegistDNS.stderr.strip():
        erros.append("Re-Registro de DNS")

    if erros:
        return f"Ocorreu um erro ao limpar: {', '.join(erros)}"
    else:
        txt_info("DNS", "Ajuste de DNS ok")

def ipOtm():
    renIP_rel = subprocess.run(
        ["powershell", "-Command", "ipconfig /release"],
        capture_output=True,
        text=True
    )

    renIP_ren = subprocess.run(
        ["powershell", "-Command", "ipconfig /renew"],
        capture_output=True,
        text=True
    )

    restTcpIP = subprocess.run(
        ["powershell", "-Command", "netsh int ip reset"],
        capture_output=True,
        text=True
    )

    resetWiSock = subprocess.run(
        ["powershell", "-Command", "netsh winsock reset"],
        capture_output=True,
        text=True
    )

    erros = []
    if renIP_rel.stderr.strip():
        erros.append("Release do IP")
    if renIP_ren.stderr.strip():
        erros.append("Renew do IP")
    if restTcpIP.stderr.strip():
        erros.append("Resete do TCP IP")
    if resetWiSock.stderr.strip():
        erros.append("Reset do WinSock")

    if erros:
        return f"Ocorreu um erro ao limpar: {', '.join(erros)}"
    else:
        txt_info("IP", "Ajuste de IP ok")



dnsOtm()
ipOtm()

