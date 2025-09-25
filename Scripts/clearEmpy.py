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


def clearEmpy():
    clearEmpy = subprocess.run(
        ["powershell", "-Command", "Clear-RecycleBin -Force -ErrorAction SilentlyContinue"],
        capture_output=True, text=True
    )

    if clearEmpy.returncode != 0:
        return "Lixeira limpa com sucesso"
    else:
        return f"Erro ao limpar a lixeira: {clearEmpy.stderr.strip()}"

print(clearEmpy())

     
