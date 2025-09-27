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

header("Informações do Sistema")
txt_info("Nome da Máquina", platform.node())
txt_info("Nome do Usuário", os.getlogin())
txt_info("Versão do Sistema Operacional", platform.platform())

header("Informações da BIOS")
c = wmi.WMI()
for bios in c.Win32_BIOS():
    txt_info("Serial Number", bios.SerialNumber if bios.SerialNumber else "N/A")

header("Informações da Placa de Rede")
ip_interfaces = psutil.net_if_addrs()

for interface_name, addresses in ip_interfaces.items():
    for address in addresses:
        if address.family == socket.AF_INET:  # Se for IPv4
            txt_info(f"Interface: {interface_name}", address.address)