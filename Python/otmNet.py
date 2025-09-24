import subprocess
import os
import platform
import wmi
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
        ["powershell", "-Commnad", "ipconfig /flushdns"],
        capture_output = True,
        text = True
    )

    reRegistDNS = subprocess.run(
        ["powershell", "-Commnad", "ipconfig /registerdns"],
        capture_output = True,
        text = True
    )

    erros = []
    if flushDNS.stderr.strip():
        erros.append("Limpar DNS")
    if reRegistDNS.stderr.strip():
        erros.append("Re-Registro de DNS")
    if erros:
        return f"Ocorreu um erro ao limpar: {', '.join(erros)}"
    else:
        txt_info("Ajuste de DNS ok")

def ipOtm():
    renIP_rel = subprocess.run(
        ["powershell", "-Commnad", "ipconfig /release"],
        capture_output = True,
        text = True
    )

    renIP_ren = subprocess.run(
        ["powershell", "-Commnad", "ipconfig /renew"],
        capture_output = True,
        text = True
    )

    restTcpIP = subprocess.run(
        ["powershell", "-Commnad", "netsh int ip reset"],
        capture_output = True,
        text = True
    )

    resetWiSock = subprocess.run(
        ["powershell", "-Commnad", "netsh winsock reset"],
        capture_output = True,
        text = True
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
        txt_info("Ajuste de IP ok")

print(dnsOtm())
print(ipOtm())