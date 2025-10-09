import subprocess
import os
import platform
import wmi
import psutil
import socket
import ctypes
import sys
from colorama import Fore, Style, init

# Inicializa suporte a cores no terminal
init(autoreset=True)

# ========== FUNÇÕES AUXILIARES DE DEBUG ==========
def header(title):
    """Exibe um cabeçalho formatado"""
    print(Fore.CYAN + f"\n=== {title} ===" + Style.RESET_ALL)

def txt_info(label, value):
    """Exibe informação formatada"""
    print(Fore.YELLOW + f"{label:<30}: " + Style.RESET_ALL + f"{value}")

def debug_step(step_number, description):
    """Exibe um passo do debug"""
    print(Fore.MAGENTA + f"\n[PASSO {step_number}] " + Fore.WHITE + description + Style.RESET_ALL)

def debug_success(message):
    """Exibe mensagem de sucesso"""
    print(Fore.GREEN + f"  ✓ {message}" + Style.RESET_ALL)

def debug_error(message):
    """Exibe mensagem de erro"""
    print(Fore.RED + f"  ✗ {message}" + Style.RESET_ALL)

def debug_warning(message):
    """Exibe mensagem de aviso"""
    print(Fore.YELLOW + f"  ⚠ {message}" + Style.RESET_ALL)

def is_admin():
    """Verifica se o script está rodando com privilégios de administrador"""
    try:
        return ctypes.windll.shell32.IsUserAnAdmin()
    except:
        return False

def run_as_admin():
    """Reinicia o script com privilégios de administrador"""
    debug_warning("Solicitando privilégios de administrador...")
    try:
        script = os.path.abspath(sys.argv[0])
        params = ' '.join([script] + sys.argv[1:])
        ctypes.windll.shell32.ShellExecuteW(None, "runas", sys.executable, params, None, 1)
        sys.exit(0)
    except Exception as e:
        debug_error(f"Falha ao solicitar privilégios: {e}")
        return False

def perguntar_continuar():
    """Pergunta se deseja voltar ao menu ou sair"""
    print("\n" + "="*50)
    print(Fore.CYAN + "1 - Voltar ao Menu Principal" + Style.RESET_ALL)
    print(Fore.CYAN + "0 - Sair" + Style.RESET_ALL)
    opcao = input(Fore.YELLOW + "\nEscolha uma opção: " + Style.RESET_ALL)
    
    if opcao == "0":
        print(Fore.CYAN + "Encerrando..." + Style.RESET_ALL)
        sys.exit(0)
    # Se for "1" ou qualquer outra coisa, volta ao menu

# ========== FUNÇÕES PRINCIPAIS ==========

def clearDisk():
    """Limpa e otimiza o disco"""
    header("As acoes a seguir podem levar algum tempo")
    
    debug_step(1, "Otimizando SSD com ReTrim...")
    improvesSSD = subprocess.run(
        ["powershell", "-Command", "Get-Command Optimize-Volume; Import-Module Storage; Optimize-Volume -DriveLetter C -ReTrim -Verbose"],
        capture_output=True,
        text=True
    )
    print(improvesSSD.stdout)
    if improvesSSD.returncode == 0:
        debug_success("SSD otimizado com sucesso")
    else:
        debug_warning("Aviso ao otimizar SSD")

    debug_step(2, "Executando limpeza de arquivos (sagerun:1)...")
    clearFiles01 = subprocess.run(
        ["powershell", "-Command", "cleanmgr /sagerun:1"],
        capture_output=True,
        text=True
    )
    print(clearFiles01.stdout)
    if clearFiles01.returncode == 0:
        debug_success("Limpeza 01 concluída")
    else:
        debug_warning("Aviso na limpeza 01")

    debug_step(3, "Executando limpeza de arquivos (sagerun:2)...")
    clearFiles02 = subprocess.run(
        ["powershell", "-Command", "cleanmgr /sagerun:2"],
        capture_output=True,
        text=True
    )
    print(clearFiles02.stdout)
    if clearFiles02.returncode == 0:
        debug_success("Limpeza 02 concluída")
    else:
        debug_warning("Aviso na limpeza 02")

    debug_step(4, "Desfragmentando disco C:...")
    clearDefrag = subprocess.run(
        ["powershell", "-Command", "defrag C: /U /V"],
        capture_output=True,
        text=True
    )
    print(clearDefrag.stdout)
    if clearDefrag.returncode == 0:
        debug_success("Desfragmentação concluída")
    else:
        debug_warning("Aviso na desfragmentação")

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

def infoMachine():
    """Exibe informações da máquina"""
    debug_step(1, "Coletando informações do sistema...")
    
    header("Informações do Sistema")
    txt_info("Nome da Máquina", platform.node())
    txt_info("Nome do Usuário", os.getlogin())
    txt_info("Versão do Sistema Operacional", platform.platform())
    debug_success("Informações do sistema coletadas")

    debug_step(2, "Coletando informações da BIOS...")
    header("Informações da BIOS")
    try:
        c = wmi.WMI()
        for bios in c.Win32_BIOS():
            txt_info("Serial Number", bios.SerialNumber if bios.SerialNumber else "N/A")
        debug_success("Informações da BIOS coletadas")
    except Exception as e:
        debug_error(f"Erro ao coletar BIOS: {e}")

    debug_step(3, "Coletando informações de rede...")
    header("Informações da Placa de Rede")
    try:
        ip_interfaces = psutil.net_if_addrs()
        for interface_name, addresses in ip_interfaces.items():
            for address in addresses:
                if address.family == socket.AF_INET:  # Se for IPv4
                    txt_info(f"Interface: {interface_name}", address.address)
        debug_success("Informações de rede coletadas")
    except Exception as e:
        debug_error(f"Erro ao coletar rede: {e}")

def scanWin():
    """Executa scan e reparo do Windows com DISM"""
    header("Scan e Reparo do Windows (DISM)")

    debug_step(1, "Verificando privilégios de administrador...")
    if not is_admin():
        debug_error("Este script precisa ser executado como ADMINISTRADOR!")
        debug_warning("A limpeza de RAM requer privilégios elevados.")
        
        resposta = input(Fore.YELLOW + "\nDeseja reiniciar como administrador? (s/n): " + Style.RESET_ALL)
        if resposta.lower() == 's':
            run_as_admin()
            return "Reiniciando como administrador..."
        else:
            debug_warning("Continuando sem limpeza de RAM...")
    else:
        debug_success("Privilégios de administrador confirmados")
    
    debug_step(1, "Iniciando DISM /RestoreHealth...")
    debug_warning("Este processo pode levar vários minutos")
    
    with subprocess.Popen(
        ["powershell", "-Command", "DISM /Online /Cleanup-Image /RestoreHealth"],
        stdout=subprocess.PIPE,
        stderr=subprocess.STDOUT,
        text=True,
        bufsize=1
    ) as proc:
        for line in proc.stdout:
            print(line, end="")
    
    retcode = proc.wait()
    if retcode != 0:
        debug_error(f"Comando retornou código {retcode}")
        return f"Error: comando retornou código {retcode}"
    else:
        debug_success("DISM concluído com sucesso")
        return "OK"

def limparSistema():
    """Limpa sistema (Temp + RAM)"""
    header("LIMPEZA COMPLETA DO SISTEMA")
    
    # Verifica privilégios de administrador
    debug_step(1, "Verificando privilégios de administrador...")
    if not is_admin():
        debug_error("Este script precisa ser executado como ADMINISTRADOR!")
        debug_warning("A limpeza de RAM requer privilégios elevados.")
        
        resposta = input(Fore.YELLOW + "\nDeseja reiniciar como administrador? (s/n): " + Style.RESET_ALL)
        if resposta.lower() == 's':
            run_as_admin()
            return "Reiniciando como administrador..."
        else:
            debug_warning("Continuando sem limpeza de RAM...")
    else:
        debug_success("Privilégios de administrador confirmados")
    
    erros = []

    # 1) Limpar Temp do Usuário
    debug_step(2, "Limpando Temp do usuário...")
    clearUserTemp = subprocess.run(
        ["powershell", "-Command", "Remove-Item -Path \"$env:TEMP\\*\" -Recurse -Force -ErrorAction SilentlyContinue"],
        capture_output=True, text=True
    )
    if clearUserTemp.stderr.strip():
        erros.append("Temp do Usuário")
        debug_warning("Aviso ao limpar Temp do usuário")
    else:
        debug_success("Temp do usuário limpo")

    # 2) Limpar Temp do Sistema
    debug_step(3, "Limpando Temp do sistema...")
    clearSysTemp = subprocess.run(
        ["powershell", "-Command", "Remove-Item -Path \"$env:windir\\Temp\\*\" -Recurse -Force -ErrorAction SilentlyContinue"],
        capture_output=True, text=True
    )
    if clearSysTemp.stderr.strip():
        erros.append("Temp do Sistema")
        debug_warning("Aviso ao limpar Temp do sistema")
    else:
        debug_success("Temp do sistema limpo")

    # 3) Esvaziar Lixeira
    debug_step(4, "Esvaziando lixeira...")
    clearEmpy = subprocess.run(
        ["powershell", "-Command", "Clear-RecycleBin -Force -ErrorAction SilentlyContinue"],
        capture_output=True, text=True
    )
    if clearEmpy.stderr.strip():
        erros.append("Lixeira")
        debug_warning("Aviso ao esvaziar lixeira")
    else:
        debug_success("Lixeira esvaziada")

    # 4) Limpar Memória RAM com RamMap
    if is_admin():
        debug_step(5, "Localizando RAMMap64.exe...")
        rammap_path = os.path.join("Scripts", "Apps", "RamMap", "RAMMap64.exe")
        
        if not os.path.exists(rammap_path):
            debug_error(f"RAMMap não encontrado em: {rammap_path}")
            erros.append("RAMMap não encontrado")
        else:
            debug_success(f"RAMMap encontrado: {rammap_path}")
            
            debug_step(6, "Liberando Working Sets...")
            emptyWorking = subprocess.run(
                [rammap_path, "-Ew"],
                capture_output=True,
                text=True,
                check=False
            )
            if emptyWorking.returncode != 0 or emptyWorking.stderr.strip():
                erros.append("Empty Working Sets")
                debug_warning("Aviso ao liberar Working Sets")
            else:
                debug_success("Working Sets liberados")

            debug_step(7, "Liberando Standby List...")
            emptyStandby = subprocess.run(
                [rammap_path, "-Et"],
                capture_output=True,
                text=True,
                check=False
            )
            if emptyStandby.returncode != 0 or emptyStandby.stderr.strip():
                erros.append("Empty Standby List")
                debug_warning("Aviso ao liberar Standby List")
            else:
                debug_success("Standby List liberada")

    # Retorno final
    if erros:
        return f"Ocorreu um erro ao executar: {', '.join(erros)}"
    else:
        debug_success("Limpeza completa finalizada!")
        return "Limpeza concluída com sucesso (Temp + RAM)"

def clearNet():
    """Otimiza e limpa configurações de rede"""
    header("LIMPEZA DE REDE")
    
    # Verifica privilégios de administrador
    debug_step(1, "Verificando privilégios de administrador...")
    if not is_admin():
        debug_error("Este script precisa ser executado como ADMINISTRADOR!")
        debug_warning("A limpeza de rede requer privilégios elevados.")
        
        resposta = input(Fore.YELLOW + "\nDeseja reiniciar como administrador? (s/n): " + Style.RESET_ALL)
        if resposta.lower() == 's':
            run_as_admin()
            return "Reiniciando como administrador..."
        else:
            debug_warning("Continuando sem privilégios...")
    else:
        debug_success("Privilégios de administrador confirmados")

    erros = []

    debug_step(2, "Limpando DNS da máquina...")
    flushDNS = subprocess.run(
        ["powershell", "-Command", "ipconfig /flushdns"],
        capture_output=True,
        text=True
    )

    if flushDNS.stderr.strip():
        erros.append("Flush DNS")
        debug_error("Erro ao limpar o DNS da máquina")
    else:
        debug_success("Limpeza do DNS realizada com sucesso!")
    
    debug_step(3, "Re-register do DNS...")
    reRegistDNS = subprocess.run(
        ["powershell", "-Command", "ipconfig /registerdns"],
        capture_output=True,
        text=True
    )

    if reRegistDNS.stderr.strip():
        erros.append("Re-Register do DNS")
        debug_error("Erro ao fazer o re-register da máquina")
    else:
        debug_success("Re-register da máquina feito com sucesso!")

    debug_step(4, "Fazendo release do IP...")
    renIP_rel = subprocess.run(
        ["powershell", "-Command", "ipconfig /release"],
        capture_output=True,
        text=True
    )

    if renIP_rel.stderr.strip():
        erros.append("Release IP")
        debug_error("Aviso ao executar release do IP")
    else:
        debug_success("Release do IP executado!")

    debug_step(5, "Renew do IP...")
    renIP_ren = subprocess.run(
        ["powershell", "-Command", "ipconfig /renew"],
        capture_output=True,
        text=True
    )

    if renIP_ren.stderr.strip():
        erros.append("Renew do IP")
        debug_error("Aviso ao executar o renew do IP")
    else:
        debug_success("Renew do IP feito!")
    
    debug_step(6, "Reset de IP...")
    restTcpIP = subprocess.run(
        ["powershell", "-Command", "netsh int ip reset"],
        capture_output=True,
        text=True
    )

    if restTcpIP.stderr.strip():
        erros.append("Reset de IP")
        debug_error("Aviso ao resetar IP")
    else:
        debug_success("Reset do IP feito!")
    
    debug_step(7, "Reset do Winsock...")
    resetWiSock = subprocess.run(
        ["powershell", "-Command", "netsh winsock reset"],
        capture_output=True,
        text=True
    )

    if resetWiSock.stderr.strip():
        erros.append("Reset do WinSock")
        debug_error("Aviso ao resetar o WinSock")
    else:
        debug_success("Reset do Winsock feito com sucesso!")

    if erros:
        return f"Ocorreu um erro ao executar: {', '.join(erros)}"
    else:
        debug_success("Limpeza de rede concluída!")
        return "Limpeza da Rede WiFi/Ethernet concluída"

def testPing():

    header("Teste de Ping")

    erros = []
        
    debug_step(1, "Ping do DNS Google..")
    pingGoogle = subprocess.run(
    ["powershell", "-Command", "ping 8.8.8.8"],
       capture_output=True,
       text=True
    )

    if pingGoogle.stderr.strip():
        erros.append("Erro ao pingar DNS Google")
    else:
        print(pingGoogle.stdout)
        debug_success("Ping bem sucedido")
    

def mostrar_menu():
    """Exibe o menu principal"""
    header("Reparo e Otimização de Windows")
    print("Selecione a opção que você quer realizar\n")
    print("[1] - Informação da Máquina")
    print("[2] - Limpar SSD/HD")
    print("[3] - Scanner do Windows")
    print("[4] - Limpar Memória RAM")
    print("[5] - Limpar Cacches de Wifi/Eternet")
    print("[6] - Teste de Ping")
    #print("[7] - Otimizar Ping")
    #print("[8] - Otimizr Wifi")
    #print("[9] - Mapa de conexão")
    #print("[10] - Verificar Temperatura")
    #print("[11] - Otimizar Windows")
    #print("[12] - Criar Ponsto de Restauração")
    #print("[13] - Configuraçãp pós-instalação")
    
    print(" ")
    print("[0] - Fechar Terminal")

# ========== LOOP PRINCIPAL ==========

while True:
    mostrar_menu()
    op = input(Fore.YELLOW + "\nQual opção você deseja executar: " + Style.RESET_ALL)

    if op == "1":
        infoMachine()
        perguntar_continuar()
    elif op == "2":
        clearDisk()
        perguntar_continuar()
    elif op == "3":
        resultado = scanWin()
        print(Fore.GREEN + f"\n{resultado}" + Style.RESET_ALL)
        perguntar_continuar()
    elif op == "4":
        resultado = limparSistema()
        print(Fore.GREEN + f"\n{resultado}" + Style.RESET_ALL)
        perguntar_continuar()
    elif op == "5":
        resultado = clearNet()
        print(Fore.GREEN + f"\n{resultado}" + Style.RESET_ALL)
        perguntar_continuar()
    elif op =="6":
        resultado = testPing()
        print(Fore.GREEN + f"\n{resultado}" + Style.RESET_ALL)
        perguntar_continuar()
    elif op == "0":
        print(Fore.CYAN + "Encerrando..." + Style.RESET_ALL)
        sys.exit(0)
    else:
        print(Fore.RED + "Opção inválida!" + Style.RESET_ALL)
        perguntar_continuar()