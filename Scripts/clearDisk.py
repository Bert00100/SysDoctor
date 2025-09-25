import subprocess
import os
import platform
import wmi
import psutil
import socket
from colorama import Fore, Style, init

init(autoreset=True)

def header(title):
    print(Fore.CYAN + f"\n=== {title} ===" + Style.RESET_ALL)

def txt_info(label, value):
    print(Fore.YELLOW + f"{label:<30}: " + Style.RESET_ALL + f"{value}")

def clearDisk():
    header("As acoes a seguir podem levar algum tempo")

    improvesSSD = subprocess.run(
        ["powershell", "-Command", "Get-Command Optimize-Volume; Import-Module Storage; Optimize-Volume -DriveLetter C -ReTrim -Verbose"],
        capture_output=True,
        text=True
    )
    print(improvesSSD.stdout)

    clearFiles01 = subprocess.run(
        ["powershell", "-Command", "cleanmgr /sagerun:1"],
        capture_output=True,
        text=True
    )
    print(clearFiles01.stdout)

    clearFiles02 = subprocess.run(
        ["powershell", "-Command", "cleanmgr /sagerun:2"],
        capture_output=True,
        text=True
    )
    print(clearFiles02.stdout)

    clearDefrag = subprocess.run(
        ["powershell", "-Command", "defrag C: /U /V"],
        capture_output=True,
        text=True
    )
    print(clearDefrag.stdout)

    erros = []
    if improvesSSD.stderr.strip():
        erros.append("Melhora de SSD")
    if clearFiles01.stderr.strip():
        erros.append("Limpeza de arquivos 01")
    if clearFiles02.stderr.strip():
        erros.append("Limpeza de arquivos 02")
    if clearDefrag.stderr.strip():
        erros.append("Desfragmentar o disco")

    if erros:
        print(f"Ocorreu um erro(s) ao limpar: {', '.join(erros)}")
    else:
        txt_info("Disco Limpo e Melhorado com Sucesso", "")


clearDisk()