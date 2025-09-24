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


def clearTemp():
    clearUserTemp = subprocess.run(
        ["powershell", "-Command", "Remove-Item -Path \"$env:TEMP\\*\" -Recurse -Force -ErrorAction SilentlyContinue"],
        capture_output=True, text=True
    )

    clearSysTemp = subprocess.run(
        ["powershell", "-Command", "Remove-Item -Path \"$env:windir\\Temp\\*\" -Recurse -Force -ErrorAction SilentlyContinue"],
        capture_output=True, text=True
    )

    clearEmpy = subprocess.run(
        ["powershell","-Command","Clear-RecycleBin -Force -ErrorAction SilentlyContinue"],
        capture_output=True, text=True
    )

    erros = []
    #stderr e uma fincao da biblioteca subprocess que mostra se teve erro
    if clearUserTemp.stderr.strip():
        erros.append("Temp do Usuário")
    if clearSysTemp.stderr.strip():
        erros.append("Temp do Sistema")
    if clearEmpy.stderr.strip():
        erros.append("Lixeira")

    if erros:
        return f"Ocorreu um erro ao limpar: {', '.join(erros)}"
    else:
        return "Arquivos temporários limpos com sucesso"

print(clearTemp())
