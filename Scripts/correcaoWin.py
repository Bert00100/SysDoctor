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

def scanWin():
    with subprocess.Popen(
        ["powershell", "-Command", "DISM /Online /Cleanup-Image /RestoreHealth"],
        stdout=subprocess.PIPE,
        stderr=subprocess.STDOUT,
        text=True,
        bufsize=1
    ) as proc:
        # Mostra o output em tempo real, sem prefixos
        for line in proc.stdout:
            print(line, end="")
    retcode = proc.wait()
    if retcode != 0:
        print(f"\nError: comando retornou código {retcode}")
        return f"Error: comando retornou código {retcode}"
    else:
        return "OK"

resultado = scanWin()
print("Resultado:", resultado)