import subprocess
import os
import platform
import wmi
import psutil
import socket
import ctypes
import sys
import shutil
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
    """Pergunta se deseja voltar ao menu ou sair (para o menu principal)"""
    while True:
        print("\n" + "="*50)
        print(Fore.CYAN + "1 - Voltar ao Menu Principal" + Style.RESET_ALL)
        print(Fore.CYAN + "0 - Sair" + Style.RESET_ALL)
        opcao = input(Fore.YELLOW + "\nEscolha uma opção: " + Style.RESET_ALL)

        if opcao == "0":
            print(Fore.CYAN + "Encerrando..." + Style.RESET_ALL)
            sys.exit(0)
        elif opcao == "1":
            return  # Volta para o menu principal
        else:
            print(Fore.RED + "Opção inválida! Tente novamente." + Style.RESET_ALL)
    # Se for "1" ou qualquer outra coisa, volta ao menu

def perguntar_continuar_Win():
    """Pergunta se deseja voltar ao menu ou sair"""
    while True:
        print("\n" + "="*50)
        print(Fore.CYAN + "1 - Voltar ao Menu de Otimização" + Style.RESET_ALL)
        print(Fore.CYAN + "2 - Voltar para o Menu Principal" + Style.RESET_ALL)
        print(Fore.CYAN + "0 - Sair" + Style.RESET_ALL)
        opcao = input(Fore.YELLOW + "\nEscolha uma opção: " + Style.RESET_ALL)

        if opcao == "0":
            print(Fore.CYAN + "Encerrando..." + Style.RESET_ALL)
            sys.exit(0)
        elif opcao == "1":
            # Retorna ao menu de otimização do Windows
            return "menu_otimizacao"
        elif opcao == "2":
            # Volta para o menu principal
            return "menu_principal"
        else:
            print(Fore.RED + "Opção inválida! Tente novamente." + Style.RESET_ALL)

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

def speedtest():
    import speedtest

    header("Speed Test")
    debug_step(1, "Executando SpeedTest...")

    st = speedtest.Speedtest()
    st.get_best_server()

    # Calcula as velocidades em Megabits por segundo (Mbs)
    download = st.download() / 1_000_000
    upload = st.upload() / 1_000_000
    ping = st.results.ping

    # Exibe o resultado formatado no console
    print("\n📡 Resultados do Teste de Velocidade:")
    print("----------------------------------------")
    print(f"📥 Download: {download:.2f} Mbs")
    print(f"📤 Upload:   {upload:.2f} Mbs")
    print(f"⚡ Ping:      {ping:.1f} ms")
    print("----------------------------------------\n")
 
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

def otmPing():
    header("Otimizar Ping")
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
    debug_step(2, "Procurando Jumper DNS...")
    dnsJu_path = os.path.join("Scripts", "Apps", "DNS", "DnsJumper.exe")

    if not os.path.exists(dnsJu_path):
        debug_error(f"DNS Jumper não encontrado em: {dnsJu_path}")
        erros.append("DNS Jumper não encontrado")
    else:
        debug_success(f"DNS Jumper encontrado: {dnsJu_path}")

        debug_step(3, "Executando DNS JUMPER...")
        starDNS = subprocess.run(
            [dnsJu_path],
            capture_output=True,
            text= True,
            check=False
        )
        if starDNS.stderr.strip():
            debug_error("Erro ao executar DNS JUMPER")
            erros.append("Executar DNS JUMPER")
        else:
            debug_success("DNS Jumper Executado com sucesso!")

        debug_step(4, "Finalizando DNS Jumper")

    if erros:
        return f"Ocorreu um erro ao executar: {', '.join(erros)}"
    else:
        debug_success("Otimização finalizada!")
        return "Otimização de Ping"

def otmWifi():
    header("Otimizador de Wifi")
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

    print("[1] - OTIMIZAR")
    print("[2] - REVERTER")
    op = input("opcao: ")

    if op == "1":
        header("OTIMIZANDO WI-FI")
        erros = []
        debug_step(2, "Definindo o nível global de ajuste automático de janela TCP para normal")
        autotuninglevel = subprocess.run(
            ["powershell", "-Command", "netsh interface tcp set global autotuninglevel=normal"],
            capture_output=True,
            text=True
        )
        if autotuninglevel.stderr.strip():
            debug_error("Erro ao executar a Definir o nível global de ajuste automático de janela TCP para normal")
            erros.append("autotuninglevel")
        else: 
            debug_success("Definicao bem Sucedida!")

        debug_step(3, "Ative o Receive Side Scaling (RSS) para permitir o balanceamento de carga de pacotes de rede entre múltiplos núcleos.")
        rss = subprocess.run(
            ["powershell", "-Command", "netsh interface tcp set global rss=enabled"],
            capture_output=True,
            text=True
        )
        if rss.stderr.strip():
            debug_error("Erro ao executar a Definir o nível global de ajuste automático de janela TCP para normal")
            erros.append("receiveSideScaling")
        else: 
            debug_success("Aticação bem Sucedida!")

        debug_step(4, "Desative o offload TCP Chimney; processe todas as conexões TCP diretamente na CPU, e não na placa de rede")
        offloadTCPChimney = subprocess.run(
            ["powershell", "-Command", "netsh interface tcp set global chimney=disabled"],
            capture_output=True,
            text=True
        )

        if offloadTCPChimney.stderr.strip():
            debug_error("Erro na Desativação do offload TCP Chimney; processe todas as conexões TCP diretamente na CPU, e não na placa de rede")
            erros.append("offloadTCPChimney")
        else:
            debug_success("Desativação bem Sucedida")

        debug_step(5, "Desativando os ajustes automáticos de heurística do TCP. Não modifique dinamicamente o comportamento do auto-tuning")
        heuristics = subprocess.run(
            ["powershell", "-Command", "netsh int tcp set heuristics disabled"],
            capture_output=True,
            text=True
        )

        if heuristics.stderr.strip():
            debug_error("Erro na Desativação de ajustes automaticos de heuristicas")
            erros.append("Heuristics")
        else:
            debug_success("Desativação bem Sucedida")
        

        if erros:
             return f"Ocorreu um erro ao executar: {', '.join(erros)}"
        else:
            debug_success("Otimização Finalizado")
            return "Sistema Finalizado"
    elif op == "2":
        header("REVERTENDO WI-FI PARA PADRÃO")
        erros = []
        debug_step(2, "Revertendo o nível global de ajuste automático de janela TCP para padrão")
        autotuninglevel = subprocess.run(
            ["powershell", "-Command", "netsh interface tcp set global autotuninglevel=restricted"],
            capture_output=True,
            text=True
        )
        if autotuninglevel.stderr.split():
            debug_error("Erro em reverter nível global de ajuste automático de janela TCP")
            erros.append("autotuninglevel")
        else:
            debug_success("Revresão de nível global de ajuste automático de janela TCP bem sucedida!")
        
        debug_step(3, "Desativando Ative o Receive Side Scaling (RSS) para permitir o balanceamento de carga de pacotes de rede entre múltiplos núcleos.")
        rss = subprocess.run(
            ["powershell", "-Command", "netsh interface tcp set global rss=disabled"],
            capture_output=True,
            text=True
        )

        if rss.stderr.strip():
            debug_error("Erro na desativação do Receive Side Scaling (RSS)")
            erros.append("Receive Side Scaling (RSS)")
        else:
            debug_success("Desativação do Receive Side Scaling")

        debug_step(4, "Ativação do Global Chimney")
        globalChimney = subprocess.run(
            ["powershell", "-Command", "netsh interface tcp set global chimney=enabled"],
            capture_output=True,
            text=True
        )

        if globalChimney.stderr.strip():
            debug_error("Erro na ativação do Global Chimney")
            erros.append("globalChimney")
        else:
            debug_success("Ativação do Global Chimney bem sucedida!")

        debug_step(5, "Ativação da Heuristics")
        heuristics = subprocess.run(
            ["powershell", "-Command", "netsh int tcp set heuristics enabled"],
            capture_output=True,
            text=True
        )
        
        if heuristics.stderr.strip():
            debug_error("Erro na ativação da Heuristics")
            erros.append("heuristics")
        else:
            debug_success("Ativação da heuristics bem sucedida!")


        if erros:
             return f"Ocorreu um erro ao executar: {', '.join(erros)}"
        else:
            debug_success("Otimização Finalizado")
            return "Sistema Finalizado"

def mapNet():
    header("Mapa de conexão")

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

    debug_step(2, "Localizar Servidor")
    net = input("Digite o Servidor que deseja Mapear: ")

    debug_step(3, "Mapeando a rede...")
    print("ATENÇÃO ISSO PODE LEVAR UM TEMPO")
    trackNet = subprocess.run(
        ["powershell", "-Command", f"tracert {net}"],
        capture_output=True,
        text=True
    )

    if trackNet.stderr.strip():
        erros.append("Servidor não encontrado")
        debug_error("Erro ao mapear o Servidor")
    else:
        print(trackNet.stdout)
        debug_success("Servidor Mapeado com sucesso")

def temperatureMonitor():
    header("Monitor de Temperatura")

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

    debug_step(2, "Procurando Monitor de Temperatura")
    hardMonitor_path = os.path.join("Scripts", "Apps", "HardwareMonitor", "OpenHardwareMonitor.exe")

    if not os.path.exists(hardMonitor_path):
        debug_error(f"Sistema de Monitoramento não encontrado em: {hardMonitor_path}")
        erros.append("Hardower Monitor não encontrado")
    else:
        debug_success(f"Sistema de Monitoramento encontrado: {hardMonitor_path}")

        debug_step(3, "Executando Sistema de Monitoramento...")
        startHardMonitor = subprocess.run(
            [hardMonitor_path],
            capture_output=True,
            text=True,
            check=True
        )

        if startHardMonitor.stderr.strip():
            debug_error("Erro ao executar o HardwareMonitor")
            erros.append("HardwareMonitor")
        else:
            debug_success("Execução do HardwareMonitor bem sucedida!")

        
    
    if erros:
        return f"Ocorreu um erro ao executar: {', '.join(erros)}"
    else:
        debug_success("Sistema de monitoramento Finalizado")
        return "Sistema Finalizado"

def restartPoint():
    header("Criando Ponto de Restauração")

    erros = []

    debug_step(1, "Executando ferramente de ponto de Restauração")
    point = subprocess.run(
        ["SystemPropertiesProtection.exe"],
        shell=True, 
        capture_output= True, 
        text= True
    )

    if point.stderr.strip():
        erros.append("Execução de Ponto de Restauração")
        debug_error("Erro ao executar ponto de restauração")
    else:
        debug_success("Ponto de Restauração Criado.")

# ========== Sessão do comando Pos-Instalacao ==========

def list_autInstall(pasta="Install"):
        header("Listando Scrips")
        path_Install = os.path.join("Scripts", pasta)

        if not os.path.exists(path_Install):
            os.makedirs(path_Install)
        
        return [f for f in os.listdir(path_Install) if f.endswith(".ps1")]

def select_autInstall(autInstall):
    header("Selecionano Script")
    print("Selecione script que quer rodar:\n")
    for i, scripts in enumerate(autInstall):
        print(f"[{i}] {scripts.replace('.ps1', '').replace('-', ' ').title()}")
    print()
    while True:
        try:
            choice = int(input("Digite o númeoro do script: "))
            if 0 <= choice < len(autInstall):
                return autInstall[choice]
            else:
                print("Numero Invalide. Tente novamente.")
        except ValueError:
            print("Por favor, digite um número válido")

def execut_autInstall(file):
    header("Executando Script")
    
    # Constrói o caminho completo
    path = os.path.join("Scripts", "Install", file)
    
    # CORREÇÃO: Adicione o número do passo
    debug_step(1, f"Verificando arquivo: {path}")
    
    # Verifica se o arquivo existe
    if not os.path.exists(path):
        debug_error(f"Arquivo não encontrado: {path}")
        return False
    
    debug_success(f"Arquivo encontrado: {file}")
    
    try:
        # CORREÇÃO: Adicione o número do passo
        debug_step(2, f"Executando script PowerShell...")
        resultado = subprocess.run(
            ["powershell", "-ExecutionPolicy", "Bypass", "-File", path],
            check=True,
            capture_output=True,
            text=True
        )
        
        # Se chegou aqui, o script executou com sucesso
        if resultado.stdout:
            print("Saída do script:")
            print(resultado.stdout)
        
        debug_success("Script executado com sucesso!")
        return True
        
    except subprocess.CalledProcessError as e:
        debug_error(f"Erro na execução do script (código {e.returncode})")
        if e.stderr:
            debug_error(f"Erro: {e.stderr}")
        if e.stdout:
            print(f"Saída: {e.stdout}")
        return False
    
def configPosInstall():
    header("Scrips de Pos Instalacao")

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

    debug_step(2, "Procurando Scripts de Pos Instalacao")

    erros = []

    debug_step(2, "Listando Scripts")
    list_install = list_autInstall()  # Retorna a lista de scripts

    if list_install:
        debug_success("Listanao Scrips")
    else:
        debug_error("Erro ao achar a pasta Install ou Script")
        erros.append("Encontrar ou criar a pasta Install")

    debug_step(3, "Selecionar Script")
    # CORREÇÃO: Passa a lista de scripts como argumento
    select_install = select_autInstall(list_install)

    if select_install:
        debug_success("Script Selecionado com Sucesso")
    else:
        debug_error("Erro ao Selecionar Script")
        erros.append("Encontrar Script")

    debug_step(4, "Executando Script")
    # CORREÇÃO: Passa o script selecionado como argumento
    execut_install = execut_autInstall(select_install)

    if execut_install:
        debug_success("Script Executado com Sucesso")
    else:
        debug_error("Erro ao Executar Script")
        erros.append("Executar Script")

    if erros:
        return f"Ocorreu um erro ao executar: {', '.join(erros)}"
    else:
        operacoes = [list_install, select_install, execut_install]
        debug_success("Executando script")
        return operacoes

# ========== Fim da Sessão do comando Pos-Instalacao ==========

def winDefender():
    header("Scaneando com Windows")
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

    debug_step(2, "Update Assinatura")
    updateAssist = subprocess.run(
        ["powershell", "-Command", "Update-MpSignature"],
        capture_output=True,
        text=True
    )

    if updateAssist.stderr.strip():
        debug_error("Erro ao Atualizar os Pacotes")
        erros.append("Atualização de Pacote")
    else:
        debug_success("Atualização de Pacotes Atualizados com Sucesso")

    debug_step(3, "Scaner de Virus")
    scanWin = subprocess.run(
        ["powershell", "-Command", "Start-MpScan -ScanType Quick"],
        capture_output=True,
        text=True
    )

    if scanWin.stderr.strip():
        debug_error("Erro ao Rodar a Verificação de Virus do Scaner")
        erros.append("Scaner de Virus")
    else:
        debug_success("Scaner Bem Sucessedido")
    
    if erros:
             return f"Ocorreu um erro ao executar: {', '.join(erros)}"
    else:
        debug_success("Windows Defender Rodou com sucesso!")
        return "Sistema Finalizado"

# ========== Sessão Otimização Do Windows ==========

def menuOtmWin():
    header("Otimizador do Windows")

    opcoes_esq = [
        "[ 1 ] Otimizar Energia",
        "[ 3 ] Otimizar ALT+TAB",
        "[ 5 ] Desative Serviços Inúteis",
       # "[ 7 ] Desativar Overlays",
        #"[ 9 ] Desativar Hibernação do Windows",
        #"[ 11 ] Desativar Hyper-V",
        #"[ 13 ] Desativar Donwload Maps Manager",
    ]

    opcoes_dir = [
        "[ 2 ] Desat. Efeitos Visuais",
        "[ 4 ] Desat. tarefas e serviços de Telemetria",
        "[ 6 ] Debloater",
        #"[ 8 ] Desat. UAC",
        #"[ 10 ] Desativar Indexação de Arquivos",
        #"[ 12 ] Desat. Aero Peek",
        #"[ 14 ] Desativar SmartScreen",
    ]

    largura_coluna = 45  # espaçamento entre colunas

    print("Selecione a opção que você quer realizar:\n")

    # Exibe o menu em duas colunas
    for i in range(max(len(opcoes_esq), len(opcoes_dir))):
        esq = opcoes_esq[i] if i < len(opcoes_esq) else ""
        dir = opcoes_dir[i] if i < len(opcoes_dir) else ""
        print(f"{esq:<{largura_coluna}}{dir}")

# ========== Funçoes Otimização do Windows==========

def otmEnerg():

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
    header("Otimizando Energia")

    debug_step(2, "Otimizando energia do PC...")
    powercfg = subprocess.run(
    [
        "powershell",
        "-Command",
        (
            "powercfg -duplicatescheme e9a42b02-d5df-448d-aa00-03f14749eb61; "
            "powercfg.exe /setacvalueindex SCHEME_CURRENT SUB_PROCESSOR IdleDisable 0; "
            "powercfg.exe /setactive SCHEME_CURRENT; "
            "powercfg.cpl"
        )
    ],
    capture_output=True,
    text=True

    )

    if powercfg.stderr.strip():
        debug_error("Erro ao aplicar CFG de otimização de energia")
    else:
        debug_success("Sucesso em aplicar CFG de otimização de energia")

    if erros:
        return f"Ocorreu um erro ao executar: {', '.join(erros)}"
    else:
        debug_success("Otimização Completa!")
        return "Otimização de Energia Completa com sucesso"


def otmlAltTab():
    debug_step(1, "Verificando privilégios de administrador...")
    if not is_admin():
        debug_error("Este script precisa ser executado como ADMINISTRADOR!")
        debug_warning("A otimização do ALT+TAB requer privilégios elevados.")
        resposta = input(Fore.YELLOW + "\nDeseja reiniciar como administrador? (s/n): " + Style.RESET_ALL)
        if resposta.lower() == 's':
            run_as_admin()
            return "Reiniciando como administrador..."
        else:
            debug_warning("Continuando sem privilégios elevados...")
    else:
        debug_success("Privilégios de administrador confirmados")

    while True:
        erros = []
        header("ATENÇÃO: ESSA OTIMIZAÇÃO É RECOMENDADA APENAS PARA PCs FRACOS")
        print("[1] - Otimizar")
        print("[2] - Reverter")
        print(" ")
        op = input("Escolha a opção: ").strip()

        if op == "1":
            header("Otimizando ALT + TAB")
            debug_step(2, "Alterando a configuração do Alt+Tab para o modo clássico")

            alteracaoTab = subprocess.run(
                [
                    "powershell",
                    "-Command",
                    (
                        "Set-ItemProperty -Path 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer' "
                        "-Name 'AltTabSettings' -Type DWord -Value 1"
                    )
                ],
                capture_output=True,
                text=True
            )

            if alteracaoTab.returncode != 0:
                debug_error(f"Erro ao alterar o Alt+Tab: {alteracaoTab.stderr.strip()}")
                erros.append("Alteração de AltTab")
            else:
                debug_success("Alteração aplicada com sucesso")

            debug_step(3, "Encerrando Windows Explorer")
            encerraExpl = subprocess.run(
                [
                    "powershell",
                    "-Command",
                    "Get-Process explorer -ErrorAction SilentlyContinue | Stop-Process -Force"
                ],
                capture_output=True,
                text=True
            )

            if encerraExpl.returncode != 0:
                debug_error("Erro ao encerrar o Windows Explorer")
                erros.append("Encerrar o Windows Explorer")
            else:
                debug_success("Windows Explorer encerrado com sucesso")

            subprocess.run(
                ["powershell", "-Command", "Start-Sleep -Seconds 2"],
                capture_output=True,
                text=True
            )

            debug_step(4, "Reiniciando Windows Explorer")
            reincExplo = subprocess.run(
                ["powershell", "-Command", "Start-Process explorer.exe"],
                capture_output=True,
                text=True
            )

            if reincExplo.returncode != 0:
                debug_error("Erro ao reiniciar o Windows Explorer")
                erros.append("Reiniciar Windows Explorer")
            else:
                debug_success("Windows Explorer reiniciado com sucesso")

            break

        elif op == "2":
            header("Revertendo otimização do ALT + TAB")
            debug_step(2, "Removendo configuração e restaurando padrão")

            revertAltTab = subprocess.run(
                [
                    "powershell",
                    "-Command",
                    (
                        "Remove-ItemProperty -Path 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer' "
                        "-Name 'AltTabSettings' -ErrorAction SilentlyContinue"
                    )
                ],
                capture_output=True,
                text=True
            )

            if revertAltTab.returncode != 0:
                debug_error(f"Erro ao reverter o Alt+Tab: {revertAltTab.stderr.strip()}")
                erros.append("Reversão de AltTab")
            else:
                debug_success("Alt+Tab revertido para o modo moderno com sucesso")

            debug_step(3, "Encerrando Windows Explorer")
            encerraExpl = subprocess.run(
                [
                    "powershell",
                    "-Command",
                    "Get-Process explorer -ErrorAction SilentlyContinue | Stop-Process -Force"
                ],
                capture_output=True,
                text=True
            )

            if encerraExpl.returncode != 0:
                debug_error("Erro ao encerrar o Windows Explorer")
                erros.append("Encerrar o Windows Explorer")
            else:
                debug_success("Windows Explorer encerrado com sucesso")

            subprocess.run(
                ["powershell", "-Command", "Start-Sleep -Seconds 2"],
                capture_output=True,
                text=True
            )

            debug_step(4, "Reiniciando Windows Explorer")
            reincExplo = subprocess.run(
                ["powershell", "-Command", "Start-Process explorer.exe"],
                capture_output=True,
                text=True
            )

            if reincExplo.returncode != 0:
                debug_error("Erro ao reiniciar o Windows Explorer")
                erros.append("Reiniciar Windows Explorer")
            else:
                debug_success("Windows Explorer reiniciado com sucesso")

            break

        else:
            debug_error("Comando inválido. Digite 1 para otimizar ou 2 para reverter.")

    if erros:
        return f"Ocorreu um erro ao executar: {', '.join(erros)}"
    else:
        debug_success("Otimização completa!")
        return "Processo concluído com sucesso"


def desatEfeitoVisual():
    debug_step(1, "Verificando privilégios de administrador...")
    if not is_admin():
        debug_error("Este script precisa ser executado como ADMINISTRADOR!")
        debug_warning("A desativação/ajuste de efeitos visuais requer privilégios elevados.")
        resposta = input(Fore.YELLOW + "\nDeseja reiniciar como administrador? (s/n): " + Style.RESET_ALL)
        if resposta.lower() == 's':
            run_as_admin()
            return "Reiniciando como administrador..."
        else:
            debug_warning("Continuando sem privilégios elevados...")
    else:
        debug_success("Privilégios de administrador confirmados")

    erros = []

    # 2) VisualFXSetting no Explorer\VisualEffects (desempenho)
    debug_step(2, "Mudando as configurações de efeitos visuais gerais para priorizar desempenho...")
    efectVisual = subprocess.run(
        [
            "powershell",
            "-Command",
            r'reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects" /v VisualFXSetting /t REG_DWORD /d 2 /f'
        ],
        capture_output=True,
        text=True
    )
    if efectVisual.returncode != 0:
        debug_error(f"Erro ao aplicar VisualFXSetting (Explorer): {efectVisual.stderr.strip()}")
        erros.append("VisualFXSetting (Explorer)")
    else:
        debug_success("Efeitos visuais (Explorer) ajustados para desempenho")

    # 3) Transparência
    debug_step(3, "Desativando transparência (janelas, barra de tarefas) para economizar GPU/CPU...")
    desatTrans = subprocess.run(
        [
            "powershell",
            "-Command",
            r'reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize" /v EnableTransparency /t REG_DWORD /d 0 /f'
        ],
        capture_output=True,
        text=True
    )
    if desatTrans.returncode != 0:
        debug_error(f"Erro ao desativar transparência: {desatTrans.stderr.strip()}")
        erros.append("EnableTransparency")
    else:
        debug_success("Transparência desativada com sucesso")

    # 4) UserPreferencesMask (desativar várias animações/efeitos)
    debug_step(4, "Aplicando máscara de preferências do usuário (desabilita várias animações/efeitos)...")
    userPrefMask = subprocess.run(
        [
            "powershell",
            "-Command",
            r'reg add "HKCU\Control Panel\Desktop" /v UserPreferencesMask /t REG_BINARY /d 9012038010000000 /f'
        ],
        capture_output=True,
        text=True
    )
    if userPrefMask.returncode != 0:
        debug_error(f"Erro ao definir UserPreferencesMask: {userPrefMask.stderr.strip()}")
        erros.append("UserPreferencesMask")
    else:
        debug_success("UserPreferencesMask aplicada com sucesso")

    # 5) VisualFXSetting no Desktop (reforça o ajuste de desempenho)
    debug_step(5, "Forçando ajuste de efeitos visuais para desempenho (nível Desktop)...")
    visualFxDesktop = subprocess.run(
        [
            "powershell",
            "-Command",
            r'reg add "HKCU\Control Panel\Desktop" /v VisualFXSetting /t REG_DWORD /d 2 /f'
        ],
        capture_output=True,
        text=True
    )
    if visualFxDesktop.returncode != 0:
        debug_error(f"Erro ao aplicar VisualFXSetting (Desktop): {visualFxDesktop.stderr.strip()}")
        erros.append("VisualFXSetting (Desktop)")
    else:
        debug_success("Efeitos visuais (Desktop) ajustados para desempenho")

    # (Opcional) Reiniciar Explorer para aplicar imediatamente:
    debug_step(6, "Reiniciando Windows Explorer para aplicar as alterações...")
    subprocess.run(["powershell", "-Command", "Get-Process explorer -ErrorAction SilentlyContinue | Stop-Process -Force"], capture_output=True, text=True)
    subprocess.run(["powershell", "-Command", "Start-Sleep -Seconds 2"], capture_output=True, text=True)
    subprocess.run(["powershell", "-Command", "Start-Process explorer.exe"], capture_output=True, text=True)
    debug_success("Windows Explorer reiniciado")

    if erros:
        return f"Ocorreu um erro ao executar: {', '.join(erros)}"
    else:
        debug_success("Desativação/Ajuste de efeitos visuais concluído!")
        return "Processo concluído com sucesso"

def desatTelemetria():
    header("Desativação da Telemetria e Coleta de Dados do Windows")
    print(Fore.CYAN + "\nEsta função altera políticas do Windows para melhorar a privacidade, "
          "desativando coleta de dados, anúncios e conexões automáticas com servidores da Microsoft.\n" + Style.RESET_ALL)
    print(Fore.YELLOW + "⚠️  Essa otimização é recomendada apenas se você deseja priorizar privacidade e desempenho.\n" + Style.RESET_ALL)

    header("ESCOLHA A OPÇÃO DE EXECUÇÃO")
    print("[1] - Desativar Telemetria e Coleta de Dados")
    print("[2] - Reverter (Restaurar configurações originais)")
    print(" ")
    op = input("Escolha a opção: ").strip()

    debug_step(1, "Verificando privilégios de administrador...")
    if not is_admin():
        debug_error("Este script precisa ser executado como ADMINISTRADOR!")
        debug_warning("A modificação de políticas do sistema requer privilégios elevados.")
        resposta = input(Fore.YELLOW + "\nDeseja reiniciar como administrador? (s/n): " + Style.RESET_ALL)
        if resposta.lower() == 's':
            run_as_admin()
            return "Reiniciando como administrador..."
        else:
            debug_warning("Continuando sem privilégios elevados...")
    else:
        debug_success("Privilégios de administrador confirmados")

    erros = []

    # ==========================================================
    # OPÇÃO 1 - EXECUTAR DESATIVAÇÃO
    # ==========================================================
    if op == "1":
        header("Desativando Telemetria e Coleta de Dados")

        debug_step(2, "Desativando coleta de dados (AllowTelemetry)...")
        cmd1 = r'REG ADD "HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection" /v "AllowTelemetry" /t REG_DWORD /d 0 /f'
        telemetria = subprocess.run(["powershell", "-Command", cmd1], capture_output=True, text=True)
        if telemetria.returncode != 0:
            debug_error(f"Erro ao desativar Telemetria: {telemetria.stderr.strip()}")
            erros.append("AllowTelemetry")
        else:
            debug_success("Telemetria desativada com sucesso")

        debug_step(3, "Desativando coleta de dados de aplicativos (AllowAppDataCollection)...")
        cmd2 = r'REG ADD "HKLM\SOFTWARE\Policies\Microsoft\Windows\System" /v "AllowAppDataCollection" /t REG_DWORD /d 0 /f'
        appData = subprocess.run(["powershell", "-Command", cmd2], capture_output=True, text=True)
        if appData.returncode != 0:
            debug_error(f"Erro ao desativar coleta de dados de aplicativos: {appData.stderr.strip()}")
            erros.append("AllowAppDataCollection")
        else:
            debug_success("Coleta de dados de aplicativos desativada com sucesso")

        debug_step(4, "Bloqueando anúncios e personalização (DisableWindowsAdvertising)...")
        cmd3 = r'REG ADD "HKLM\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo" /v "DisableWindowsAdvertising" /t REG_DWORD /d 1 /f'
        ads = subprocess.run(["powershell", "-Command", cmd3], capture_output=True, text=True)
        if ads.returncode != 0:
            debug_error(f"Erro ao desativar anúncios: {ads.stderr.strip()}")
            erros.append("DisableWindowsAdvertising")
        else:
            debug_success("Publicidade e rastreamento desativados com sucesso")

        debug_step(5, "Desativando experiências do consumidor (DisableMicrosoftConsumerExperience)...")
        cmd4 = r'REG ADD "HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent" /v "DisableMicrosoftConsumerExperience" /t REG_DWORD /d 1 /f'
        consumerExp = subprocess.run(["powershell", "-Command", cmd4], capture_output=True, text=True)
        if consumerExp.returncode != 0:
            debug_error(f"Erro ao desativar experiência do consumidor: {consumerExp.stderr.strip()}")
            erros.append("DisableMicrosoftConsumerExperience")
        else:
            debug_success("Experiências do consumidor desativadas com sucesso")

        debug_step(6, "Impedindo conexão com servidores da Microsoft (DoNotConnectToWindowsUpdateInternetLocations)...")
        cmd5 = r'REG ADD "HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate" /v "DoNotConnectToWindowsUpdateInternetLocations" /t REG_DWORD /d 1 /f'
        winUpdate = subprocess.run(["powershell", "-Command", cmd5], capture_output=True, text=True)
        if winUpdate.returncode != 0:
            debug_error(f"Erro ao desativar conexão com Windows Update: {winUpdate.stderr.strip()}")
            erros.append("DoNotConnectToWindowsUpdateInternetLocations")
        else:
            debug_success("Conexões automáticas com Windows Update desativadas com sucesso")

    # ==========================================================
    # OPÇÃO 2 - REVERTER CONFIGURAÇÕES
    # ==========================================================
    elif op == "2":
        header("Revertendo Configurações de Telemetria")

        debug_step(2, "Reativando coleta de dados (AllowTelemetry)...")
        cmd1 = r'REG ADD "HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection" /v "AllowTelemetry" /t REG_DWORD /d 3 /f'
        telemetria = subprocess.run(["powershell", "-Command", cmd1], capture_output=True, text=True)
        if telemetria.returncode != 0:
            debug_error(f"Erro ao reativar Telemetria: {telemetria.stderr.strip()}")
            erros.append("AllowTelemetry")
        else:
            debug_success("Telemetria reativada com sucesso")

        debug_step(3, "Reativando coleta de dados de aplicativos (AllowAppDataCollection)...")
        cmd2 = r'REG DELETE "HKLM\SOFTWARE\Policies\Microsoft\Windows\System" /v "AllowAppDataCollection" /f'
        appData = subprocess.run(["powershell", "-Command", cmd2], capture_output=True, text=True)
        if appData.returncode != 0:
            debug_error(f"Erro ao reativar coleta de dados de aplicativos: {appData.stderr.strip()}")
            erros.append("AllowAppDataCollection")
        else:
            debug_success("Coleta de dados de aplicativos reativada com sucesso")

        debug_step(4, "Reativando anúncios e personalização (DisableWindowsAdvertising)...")
        cmd3 = r'REG DELETE "HKLM\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo" /v "DisableWindowsAdvertising" /f'
        ads = subprocess.run(["powershell", "-Command", cmd3], capture_output=True, text=True)
        if ads.returncode != 0:
            debug_error(f"Erro ao reativar anúncios: {ads.stderr.strip()}")
            erros.append("DisableWindowsAdvertising")
        else:
            debug_success("Publicidade e personalização reativadas com sucesso")

        debug_step(5, "Reativando experiências do consumidor (DisableMicrosoftConsumerExperience)...")
        cmd4 = r'REG DELETE "HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent" /v "DisableMicrosoftConsumerExperience" /f'
        consumerExp = subprocess.run(["powershell", "-Command", cmd4], capture_output=True, text=True)
        if consumerExp.returncode != 0:
            debug_error(f"Erro ao reativar experiência do consumidor: {consumerExp.stderr.strip()}")
            erros.append("DisableMicrosoftConsumerExperience")
        else:
            debug_success("Experiências do consumidor reativadas com sucesso")

        debug_step(6, "Reativando conexões com Windows Update (DoNotConnectToWindowsUpdateInternetLocations)...")
        cmd5 = r'REG DELETE "HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate" /v "DoNotConnectToWindowsUpdateInternetLocations" /f'
        winUpdate = subprocess.run(["powershell", "-Command", cmd5], capture_output=True, text=True)
        if winUpdate.returncode != 0:
            debug_error(f"Erro ao reativar conexões do Windows Update: {winUpdate.stderr.strip()}")
            erros.append("DoNotConnectToWindowsUpdateInternetLocations")
        else:
            debug_success("Conexões automáticas com Windows Update reativadas com sucesso")

    else:
        debug_error("Comando inválido. Digite 1 para desativar ou 2 para reverter.")
        return "Ação cancelada pelo usuário."

    if erros:
        debug_error(f"Ocorreu um erro ao executar: {', '.join(erros)}")
        return f"Falha parcial — comandos com erro: {', '.join(erros)}"
    else:
        if op == "1":
            debug_success("Telemetria e coleta de dados desativadas com sucesso!")
            return "Processo de desativação concluído com sucesso."
        else:
            debug_success("Configurações de telemetria restauradas para o padrão original!")
            return "Processo de reversão concluído com sucesso."


def servicesInutes():
    header("Otimização de Serviços do Windows")
    print(Fore.CYAN + "\nEsta função desativa ou restaura serviços do Windows "
          "para melhorar o desempenho e reduzir consumo de recursos.\n" + Style.RESET_ALL)
    print(Fore.YELLOW + "⚠️  Use com cautela: alguns serviços desativados podem afetar recursos "
          "como impressão, diagnósticos ou atualizações.\n" + Style.RESET_ALL)

    header("ESCOLHA A OPÇÃO DE EXECUÇÃO")
    print("[1] - Desativar Serviços")
    print("[2] - Reverter Otimização")
    print(" ")
    op = input("Escolha a opção: ").strip()

    # ==========================================================
    # VERIFICAÇÃO DE ADMINISTRADOR
    # ==========================================================
    debug_step(1, "Verificando privilégios de administrador...")
    if not is_admin():
        debug_error("Este script precisa ser executado como ADMINISTRADOR!")
        debug_warning("A modificação de serviços requer privilégios elevados.")
        resposta = input(Fore.YELLOW + "\nDeseja reiniciar como administrador? (s/n): " + Style.RESET_ALL)
        if resposta.lower() == 's':
            run_as_admin()
            return "Reiniciando como administrador..."
        else:
            debug_warning("Continuando sem privilégios elevados...")
    else:
        debug_success("Privilégios de administrador confirmados")

    # ==========================================================
    # VALIDAÇÃO DO UTILITÁRIO SC.EXE
    # ==========================================================
    debug_step(2, "Verificando utilitário do Windows (sc.exe)...")
    if shutil.which("sc.exe") is None:
        debug_error("O utilitário 'sc.exe' não foi encontrado no sistema!")
        debug_warning("Verifique se o Windows está instalado corretamente ou se há restrições de PATH.")
        return "Execução cancelada — dependência ausente."
    else:
        debug_success("Utilitário 'sc.exe' encontrado e disponível.")

    erros = []

    # ==========================================================
    # OPÇÃO 1 - DESATIVAR SERVIÇOS
    # ==========================================================
    if op == "1":
        header("Desativando Serviços Desnecessários")

        servicos = {
            "Spooler": "disabled",
            "wisvc": "disabled",
            "WerSvc": "disabled",
            "WbioSrvc": "disabled",
            "DiagTrack": "disabled",
            "dmwappushservice": "disabled",
            "wuauserv": "disabled",
            "dosvc": "disabled"
        }

        step = 3
        for nome, modo in servicos.items():
            debug_step(step, f"Parando serviço {nome}...")
            stop_cmd = f'sc.exe stop {nome}'
            resultado_stop = subprocess.run(["powershell", "-Command", stop_cmd], capture_output=True, text=True)
            if resultado_stop.returncode != 0:
                debug_warning(f"Serviço {nome} pode já estar parado ou indisponível.")
            else:
                debug_success(f"Serviço {nome} parado com sucesso")

            debug_step(step + 1, f"Configurando serviço {nome} para {modo}...")
            config_cmd = f'sc.exe config {nome} start= {modo}'
            resultado_cfg = subprocess.run(["powershell", "-Command", config_cmd], capture_output=True, text=True)
            if resultado_cfg.returncode != 0:
                debug_error(f"Erro ao configurar {nome}: {resultado_cfg.stderr.strip()}")
                erros.append(nome)
            else:
                debug_success(f"{nome} configurado para {modo} com sucesso")
            step += 2

    # ==========================================================
    # OPÇÃO 2 - REVERTER SERVIÇOS
    # ==========================================================
    elif op == "2":
        header("Revertendo Configurações de Serviços")

        servicos = {
            "Spooler": "auto",
            "wisvc": "demand",
            "WerSvc": "demand",
            "WbioSrvc": "demand",
            "DiagTrack": "demand",
            "dmwappushservice": "demand",
            "wuauserv": "auto",
            "dosvc": "demand"
        }

        step = 3
        for nome, modo in servicos.items():
            debug_step(step, f"Reconfigurando serviço {nome} para {modo}...")
            config_cmd = f'sc.exe config {nome} start= {modo}'
            resultado_cfg = subprocess.run(["powershell", "-Command", config_cmd], capture_output=True, text=True)
            if resultado_cfg.returncode != 0:
                debug_error(f"Erro ao reconfigurar {nome}: {resultado_cfg.stderr.strip()}")
                erros.append(nome)
                step += 2
                continue
            else:
                debug_success(f"Serviço {nome} configurado para {modo} com sucesso")

            debug_step(step + 1, f"Iniciando serviço {nome}...")
            start_cmd = f'sc.exe start {nome}'
            resultado_start = subprocess.run(["powershell", "-Command", start_cmd], capture_output=True, text=True)
            if resultado_start.returncode != 0:
                debug_warning(f"Serviço {nome} não pôde ser iniciado (pode estar desnecessário ou já desativado).")
            else:
                debug_success(f"Serviço {nome} iniciado com sucesso")
            step += 2

    else:
        debug_error("Comando inválido. Digite 1 para desativar ou 2 para reverter.")
        return "Ação cancelada pelo usuário."

    # ==========================================================
    # FINALIZAÇÃO
    # ==========================================================
    if erros:
        debug_error(f"Ocorreu um erro ao processar os seguintes serviços: {', '.join(erros)}")
        return f"Falha parcial — serviços com erro: {', '.join(erros)}"
    else:
        if op == "1":
            debug_success("Todos os serviços desativados com sucesso!")
            return "Otimização concluída com sucesso."
        else:
            debug_success("Serviços restaurados para o padrão original!")
            return "Reversão concluída com sucesso."



# ========== Fim da Sessão Otimização Do Windows ==========

def otmWin():
    while True:
        menuOtmWin()
        op = input(Fore.YELLOW + "\nQual opção você deseja executar: " + Style.RESET_ALL)

        if op == "1":
            resultado = otmEnerg()
            if resultado == "menu_principal":
                break  
            resultado = perguntar_continuar_Win()
            if resultado == "menu_principal":
                break
        elif op == "2":
            resultado_scan = desatEfeitoVisual()
            print(Fore.GREEN + f"\n{resultado_scan}" + Style.RESET_ALL)
            resultado = perguntar_continuar_Win()
            if resultado == "menu_principal":
                break
        elif op == "3":
            resultado_scan = otmlAltTab()
            print(Fore.GREEN + f"\n{resultado_scan}" + Style.RESET_ALL)
            resultado = perguntar_continuar_Win()
            if resultado == "menu_principal":
                break
        elif op == "4":
            resultado_limpeza = desatTelemetria()
            print(Fore.GREEN + f"\n{resultado_limpeza}" + Style.RESET_ALL)
            resultado = perguntar_continuar_Win()
            if resultado == "menu_principal":
                break
        elif op == "5":
            resultado_limpeza = servicesInutes()
            print(Fore.GREEN + f"\n{resultado_limpeza}" + Style.RESET_ALL)
            resultado = perguntar_continuar_Win()
            if resultado == "menu_principal":
                break
    


def mostrar_menu():
    """Exibe o menu principal dividido em duas colunas com AA 'Doctor System'"""

    def ascii_art_2():
        art = r"""
         ___    _  _    ___    ____     _____     ___    ____    _____    ____
        / __)  ( \/ )  / __)  (  _ \   (  _  )   / __)  (_  _)  (  _  )  (  _ \
        \__ \   \  /   \__ \   )(_) )   )(_)(   ( (__     )(     )(_)(    )   /
        (___/   (__)   (___/  (____/   (_____)   \___)   (__)   (_____)  (_)\_)

        """
        print(art)
        print(" " * 30 + "Windows Optimizer and Repair\n")

    # Exibe o ASCII Art
    ascii_art_2()

    # Define as opções
    opcoes_esq = [
        "[ 1 ] Informação da Máquina",
        "[ 3 ] Scanner do Windows",
        "[ 5 ] SpeedTest",
        "[ 7 ] Teste de Ping",
        "[ 9 ] Otimizar Wifi",
        "[ 11 ] Verificar Temperatura",
        "[ 13 ] Criar Ponto de Restauração",
        #"[ 15 ] Adicao de tela de Login",
        #"[ 17 ] Atualizar Windows",
        
    ]

    opcoes_dir = [
        "[ 2 ] Limpar SSD/HD",
        "[ 4 ] Limpar Memória RAM",
        "[ 6 ] Limpar Caches de Wifi/Ethernet",
        "[ 8 ] Otimizar Ping",
        "[ 10 ] Mapa de Conexão",
        "[ 12 ] Otimizar Windows",
        "[ 14 ] Configuração Pós-Instalação",
        "[ 16 ] Rodar Windows Defender",
    ]

    largura_coluna = 45  # espaçamento entre colunas

    print("Selecione a opção que você quer realizar:\n")

    # Exibe o menu em duas colunas
    for i in range(max(len(opcoes_esq), len(opcoes_dir))):
        esq = opcoes_esq[i] if i < len(opcoes_esq) else ""
        dir = opcoes_dir[i] if i < len(opcoes_dir) else ""
        print(f"{esq:<{largura_coluna}}{dir}")

    print("\n[ 0 ] Sair\n")
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
        resultado = speedtest()
        print(Fore.GREEN + f"\n{resultado}" + Style.RESET_ALL)
        perguntar_continuar()
    elif op == "6":
        resultado = clearNet()
        print(Fore.GREEN + f"\n{resultado}" + Style.RESET_ALL)
        perguntar_continuar()
    elif op =="7":
        resultado = testPing()
        print(Fore.GREEN + f"\n{resultado}" + Style.RESET_ALL)
        perguntar_continuar()
    elif op =="8":
        resultado = otmPing()
        print(Fore.GREEN + f"\n{resultado}" + Style.RESET_ALL)
        perguntar_continuar()
    elif op =="9":
        resultado = otmWifi()
        print(Fore.GREEN + f"\n{resultado}" + Style.RESET_ALL)
        perguntar_continuar()
    elif op =="10":
        resultado = mapNet()
        print(Fore.GREEN + f"\n{resultado}" + Style.RESET_ALL)
        perguntar_continuar()
    elif op =="11":
        resultado = temperatureMonitor()
        print(Fore.GREEN + f"\n{resultado}" + Style.RESET_ALL)
        perguntar_continuar()
    elif op =="12":
        resultado = otmWin()
        print(Fore.GREEN + f"\n{resultado}" + Style.RESET_ALL)
        perguntar_continuar()
    elif op =="13":
        resultado = restartPoint()
        print(Fore.GREEN + f"\n{resultado}" + Style.RESET_ALL)
        perguntar_continuar()
    elif op =="14":
        resultado = configPosInstall()
        print(Fore.GREEN + f"\n{resultado}" + Style.RESET_ALL)
        perguntar_continuar()
    elif op =="16":
        resultado = winDefender()
        print(Fore.GREEN + f"\n{resultado}" + Style.RESET_ALL)
        perguntar_continuar()
    elif op == "0":
        print(Fore.CYAN + "Encerrando..." + Style.RESET_ALL)
        sys.exit(0)
    else:
        print(Fore.RED + "Opção inválida!" + Style.RESET_ALL)
        perguntar_continuar()