using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Text.Json;

namespace SysDoctor;

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        while (true)
        {
            MostrarMenu();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nQual opção você deseja executar: ");
            Console.ResetColor();
            var option = Console.ReadLine() ?? string.Empty;

            switch (option)
            {
                case "1":
                    InfoMachine();
                    PerguntarContinuar();
                    break;
                case "2":
                    ClearDisk();
                    PerguntarContinuar();
                    break;
                case "3":
                    var scanResult = ScanWin();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n{scanResult}\n");
                    Console.ResetColor();
                    PerguntarContinuar();
                    break;
                case "4":
                    var cleanupResult = LimparSistema();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n{cleanupResult}\n");
                    Console.ResetColor();
                    PerguntarContinuar();
                    break;
                case "5":
                    SpeedTest();
                    PerguntarContinuar();
                    break;
                case "6":
                    var netResult = ClearNet();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n{netResult}\n");
                    Console.ResetColor();
                    PerguntarContinuar();
                    break;
                case "7":
                    TestPing();
                    PerguntarContinuar();
                    break;
                case "8":
                    var pingResult = OtimizarPing();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n{pingResult}\n");
                    Console.ResetColor();
                    PerguntarContinuar();
                    break;
                case "9":
                    var wifiResult = OtimizarWifi();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n{wifiResult}\n");
                    Console.ResetColor();
                    PerguntarContinuar();
                    break;
                case "10":
                    var mapResult = MapNet();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n{mapResult}\n");
                    Console.ResetColor();
                    PerguntarContinuar();
                    break;
                case "11":
                    var tempResult = TemperatureMonitor();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n{tempResult}\n");
                    Console.ResetColor();
                    PerguntarContinuar();
                    break;
                case "12":
                    OtimizacaoWindowsMenu();
                    break;
                case "13":
                    var restoreResult = RestartPoint();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n{restoreResult}\n");
                    Console.ResetColor();
                    PerguntarContinuar();
                    break;
                case "14":
                    var configResult = ConfigPosInstall();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n{configResult}\n");
                    Console.ResetColor();
                    PerguntarContinuar();
                    break;
                case "15":
                    var updateResult = WinUpdate();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n{updateResult}\n");
                    Console.ResetColor();
                    PerguntarContinuar();
                    break;
                case "16":
                    var defenderResult = WinDefender();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n{defenderResult}\n");
                    Console.ResetColor();
                    PerguntarContinuar();
                    break;
                case "0":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Encerrando...");
                    Console.ResetColor();
                    Environment.Exit(0);
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Opção inválida!");
                    Console.ResetColor();
                    PerguntarContinuar();
                    break;
            }
        }
    }

    private static void MostrarMenu()
    {
        AsciiArt();

        var opcoesEsquerda = new List<string>
        {
            "[ 1 ] Informação da Máquina",
            "[ 3 ] Scanner do Windows",
            "[ 5 ] SpeedTest",
            "[ 7 ] Teste de Ping",
            "[ 9 ] Otimizar Wifi",
            "[ 11 ] Verificar Temperatura",
            "[ 13 ] Criar Ponto de Restauração",
            "[ 15 ] Atualizar Windows",
        };

        var opcoesDireita = new List<string>
        {
            "[ 2 ] Limpar SSD/HD",
            "[ 4 ] Limpar Memória RAM",
            "[ 6 ] Limpar Caches de Wifi/Ethernet",
            "[ 8 ] Otimizar Ping",
            "[ 10 ] Mapa de Conexão",
            "[ 12 ] Otimizar Windows",
            "[ 14 ] Configuração Pós-Instalação",
            "[ 16 ] Rodar Windows Defender",
        };

        const int larguraColuna = 45;

        Console.WriteLine("Selecione a opção que você quer realizar:\n");

        for (var i = 0; i < Math.Max(opcoesEsquerda.Count, opcoesDireita.Count); i++)
        {
            var esquerda = i < opcoesEsquerda.Count ? opcoesEsquerda[i] : string.Empty;
            var direita = i < opcoesDireita.Count ? opcoesDireita[i] : string.Empty;
            Console.WriteLine($"{esquerda,-larguraColuna}{direita}");
        }

        Console.WriteLine("\n[ 0 ] Sair\n");
    }

    private static void AsciiArt()
    {
        const string art = @"         ___    _  _    ___         ____     _____     ___    ____    _____    ____
        / __)  ( \\ )  / __)       (  _ \\   (  _  )   / __)  (_  _)  (  _  )  (  _ \
        \\__ \\   \\  /   \\__ \\        )(_) )   )(_)(   ( (__     )(     )(_)(    )   /
        (___/   (__)   (___/  ___  (____/   (_____)   \\___)   (__)   (_____)  (_)\\_)
";
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(art);
        Console.ResetColor();
        Console.WriteLine("                              Windows Optimizer and Repair\n");
    }

    private static void Header(string title)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n=== {title} ===");
        Console.ResetColor();
    }

    private static void TxtInfo(string label, string value)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"{label,-30}: ");
        Console.ResetColor();
        Console.WriteLine(value);
    }

    private static void DebugStep(int stepNumber, string description)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write($"\n[PASSO {stepNumber}] ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(description);
        Console.ResetColor();
    }

    private static void DebugSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"  ✓ {message}");
        Console.ResetColor();
    }

    private static void DebugError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  ✗ {message}");
        Console.ResetColor();
    }

    private static void DebugWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"  ⚠ {message}");
        Console.ResetColor();
    }

    private static bool IsAdministrator()
    {
        using var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    private static void RunAsAdministrator()
    {
        DebugWarning("Solicitando privilégios de administrador...");
        try
        {
            var exePath = Environment.ProcessPath ?? Process.GetCurrentProcess().MainModule?.FileName;
            if (string.IsNullOrWhiteSpace(exePath))
            {
                DebugError("Não foi possível localizar o executável atual.");
                return;
            }

            var startInfo = new ProcessStartInfo(exePath)
            {
                UseShellExecute = true,
                Verb = "runas"
            };

            Process.Start(startInfo);
            Environment.Exit(0);
        }
        catch (Exception ex)
        {
            DebugError($"Falha ao solicitar privilégios: {ex.Message}");
        }
    }

    private static void PerguntarContinuar()
    {
        while (true)
        {
            Console.WriteLine("\n==================================================");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("1 - Voltar ao Menu Principal");
            Console.WriteLine("0 - Sair");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nEscolha uma opção: ");
            Console.ResetColor();
            var option = Console.ReadLine();

            if (option == "0")
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Encerrando...");
                Console.ResetColor();
                Environment.Exit(0);
            }
            else if (option == "1")
            {
                return;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Opção inválida! Tente novamente.");
                Console.ResetColor();
            }
        }
    }

    private static string? PerguntarContinuarWin()
    {
        while (true)
        {
            Console.WriteLine("\n==================================================");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("1 - Voltar ao Menu de Otimização");
            Console.WriteLine("2 - Voltar para o Menu Principal");
            Console.WriteLine("0 - Sair");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nEscolha uma opção: ");
            Console.ResetColor();
            var option = Console.ReadLine();

            switch (option)
            {
                case "0":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Encerrando...");
                    Console.ResetColor();
                    Environment.Exit(0);
                    break;
                case "1":
                    return "menu_otimizacao";
                case "2":
                    return "menu_principal";
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Opção inválida! Tente novamente.");
                    Console.ResetColor();
                    break;
            }
        }
    }

    private static CommandResult RunPowerShell(string command, bool printOutput = true)
    {
        var psi = new ProcessStartInfo("powershell")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        psi.ArgumentList.Add("-NoProfile");
        psi.ArgumentList.Add("-Command");
        psi.ArgumentList.Add(command);

        using var process = Process.Start(psi);
        if (process == null)
        {
            return new CommandResult(-1, string.Empty, "Falha ao iniciar PowerShell");
        }

        var stdout = process.StandardOutput.ReadToEnd();
        var stderr = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (printOutput && !string.IsNullOrEmpty(stdout))
        {
            Console.WriteLine(stdout);
        }

        return new CommandResult(process.ExitCode, stdout, stderr);
    }

    private static CommandResult RunProcess(string fileName, params string[] arguments)
    {
        var psi = new ProcessStartInfo(fileName)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        foreach (var arg in arguments)
        {
            psi.ArgumentList.Add(arg);
        }

        using var process = Process.Start(psi);
        if (process == null)
        {
            return new CommandResult(-1, string.Empty, $"Falha ao iniciar {fileName}");
        }

        var stdout = process.StandardOutput.ReadToEnd();
        var stderr = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (!string.IsNullOrEmpty(stdout))
        {
            Console.WriteLine(stdout);
        }

        return new CommandResult(process.ExitCode, stdout, stderr);
    }

    private static void ClearDisk()
    {
        Header("As ações a seguir podem levar algum tempo");

        DebugStep(1, "Otimizando SSD com ReTrim...");
        var improvesSsd = RunPowerShell("Get-Command Optimize-Volume; Import-Module Storage; Optimize-Volume -DriveLetter C -ReTrim -Verbose");
        if (improvesSsd.ExitCode == 0)
        {
            DebugSuccess("SSD otimizado com sucesso");
        }
        else
        {
            DebugWarning("Aviso ao otimizar SSD");
        }

        DebugStep(2, "Executando limpeza de arquivos (sagerun:1)...");
        var clearFiles01 = RunPowerShell("cleanmgr /sagerun:1");
        if (clearFiles01.ExitCode == 0)
        {
            DebugSuccess("Limpeza 01 concluída");
        }
        else
        {
            DebugWarning("Aviso na limpeza 01");
        }

        DebugStep(3, "Executando limpeza de arquivos (sagerun:2)...");
        var clearFiles02 = RunPowerShell("cleanmgr /sagerun:2");
        if (clearFiles02.ExitCode == 0)
        {
            DebugSuccess("Limpeza 02 concluída");
        }
        else
        {
            DebugWarning("Aviso na limpeza 02");
        }

        DebugStep(4, "Desfragmentando disco C:...");
        var clearDefrag = RunPowerShell("defrag C: /U /V");
        if (clearDefrag.ExitCode == 0)
        {
            DebugSuccess("Desfragmentação concluída");
        }
        else
        {
            DebugWarning("Aviso na desfragmentação");
        }

        var erros = new List<string>();
        if (!string.IsNullOrWhiteSpace(improvesSsd.StandardError))
        {
            erros.Add("Melhora de SSD");
        }
        if (!string.IsNullOrWhiteSpace(clearFiles01.StandardError))
        {
            erros.Add("Limpeza de arquivos 01");
        }
        if (!string.IsNullOrWhiteSpace(clearFiles02.StandardError))
        {
            erros.Add("Limpeza de arquivos 02");
        }
        if (!string.IsNullOrWhiteSpace(clearDefrag.StandardError))
        {
            erros.Add("Desfragmentar o disco");
        }

        if (erros.Count > 0)
        {
            Console.WriteLine($"Ocorreu um erro(s) ao limpar: {string.Join(", ", erros)}");
        }
        else
        {
            TxtInfo("Disco Limpo e Melhorado com Sucesso", string.Empty);
        }
    }

    private static void InfoMachine()
    {
        DebugStep(1, "Coletando informações do sistema...");

        Header("Informações do Sistema");
        TxtInfo("Nome da Máquina", Environment.MachineName);
        TxtInfo("Nome do Usuário", Environment.UserName);
        TxtInfo("Versão do Sistema Operacional", Environment.OSVersion.VersionString);
        DebugSuccess("Informações do sistema coletadas");

        DebugStep(2, "Coletando informações da BIOS...");
        Header("Informações da BIOS");
        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BIOS");
            foreach (var bios in searcher.Get())
            {
                var serial = bios["SerialNumber"]?.ToString() ?? "N/A";
                TxtInfo("Serial Number", serial);
            }
            DebugSuccess("Informações da BIOS coletadas");
        }
        catch (Exception ex)
        {
            DebugError($"Erro ao coletar BIOS: {ex.Message}");
        }

        DebugStep(3, "Coletando informações de rede...");
        Header("Informações da Placa de Rede");
        try
        {
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                var properties = networkInterface.GetIPProperties();
                foreach (var unicast in properties.UnicastAddresses)
                {
                    if (unicast.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        TxtInfo($"Interface: {networkInterface.Name}", unicast.Address.ToString());
                    }
                }
            }
            DebugSuccess("Informações de rede coletadas");
        }
        catch (Exception ex)
        {
            DebugError($"Erro ao coletar rede: {ex.Message}");
        }
    }

    private static string ScanWin()
    {
        Header("Scan e Reparo do Windows (DISM)");

        DebugStep(1, "Verificando privilégios de administrador...");
        if (!IsAdministrator())
        {
            DebugError("Este script precisa ser executado como ADMINISTRADOR!");
            DebugWarning("A limpeza de RAM requer privilégios elevados.");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nDeseja reiniciar como administrador? (s/n): ");
            Console.ResetColor();
            var resposta = Console.ReadLine();
            if (string.Equals(resposta, "s", StringComparison.OrdinalIgnoreCase))
            {
                RunAsAdministrator();
                return "Reiniciando como administrador...";
            }
            DebugWarning("Continuando sem limpeza de RAM...");
        }
        else
        {
            DebugSuccess("Privilégios de administrador confirmados");
        }

        DebugStep(1, "Iniciando DISM /RestoreHealth...");
        DebugWarning("Este processo pode levar vários minutos");

        var psi = new ProcessStartInfo("powershell")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        psi.ArgumentList.Add("-NoProfile");
        psi.ArgumentList.Add("-Command");
        psi.ArgumentList.Add("DISM /Online /Cleanup-Image /RestoreHealth");

        using var process = Process.Start(psi);
        if (process == null)
        {
            DebugError("Não foi possível iniciar o DISM");
            return "Erro ao iniciar o DISM";
        }

        string? line;
        while ((line = process.StandardOutput.ReadLine()) != null)
        {
            Console.WriteLine(line);
        }

        process.WaitForExit();
        if (process.ExitCode != 0)
        {
            DebugError($"Comando retornou código {process.ExitCode}");
            return $"Error: comando retornou código {process.ExitCode}";
        }

        DebugSuccess("DISM concluído com sucesso");
        return "OK";
    }

    private static string LimparSistema()
    {
        Header("LIMPEZA COMPLETA DO SISTEMA");

        DebugStep(1, "Verificando privilégios de administrador...");
        if (!IsAdministrator())
        {
            DebugError("Este script precisa ser executado como ADMINISTRADOR!");
            DebugWarning("A limpeza de RAM requer privilégios elevados.");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nDeseja reiniciar como administrador? (s/n): ");
            Console.ResetColor();
            var resposta = Console.ReadLine();
            if (string.Equals(resposta, "s", StringComparison.OrdinalIgnoreCase))
            {
                RunAsAdministrator();
                return "Reiniciando como administrador...";
            }
            DebugWarning("Continuando sem limpeza de RAM...");
        }
        else
        {
            DebugSuccess("Privilégios de administrador confirmados");
        }

        var erros = new List<string>();

        DebugStep(2, "Limpando Temp do usuário...");
        var clearUserTemp = RunPowerShell("Remove-Item -Path \"$env:TEMP\\*\" -Recurse -Force -ErrorAction SilentlyContinue", printOutput: false);
        if (!string.IsNullOrWhiteSpace(clearUserTemp.StandardError))
        {
            erros.Add("Temp do Usuário");
            DebugWarning("Aviso ao limpar Temp do usuário");
        }
        else
        {
            DebugSuccess("Temp do usuário limpo");
        }

        DebugStep(3, "Limpando Temp do sistema...");
        var clearSysTemp = RunPowerShell("Remove-Item -Path \"$env:windir\\Temp\\*\" -Recurse -Force -ErrorAction SilentlyContinue", printOutput: false);
        if (!string.IsNullOrWhiteSpace(clearSysTemp.StandardError))
        {
            erros.Add("Temp do Sistema");
            DebugWarning("Aviso ao limpar Temp do sistema");
        }
        else
        {
            DebugSuccess("Temp do sistema limpo");
        }

        DebugStep(4, "Esvaziando lixeira...");
        var clearEmpy = RunPowerShell("Clear-RecycleBin -Force -ErrorAction SilentlyContinue", printOutput: false);
        if (!string.IsNullOrWhiteSpace(clearEmpy.StandardError))
        {
            erros.Add("Lixeira");
            DebugWarning("Aviso ao esvaziar lixeira");
        }
        else
        {
            DebugSuccess("Lixeira esvaziada");
        }

        if (IsAdministrator())
        {
            DebugStep(5, "Localizando RAMMap64.exe...");
            var rammapPath = Path.Combine("Scripts", "Apps", "RamMap", "RAMMap64.exe");

            if (!File.Exists(rammapPath))
            {
                DebugError($"RAMMap não encontrado em: {rammapPath}");
                erros.Add("RAMMap não encontrado");
            }
            else
            {
                DebugSuccess($"RAMMap encontrado: {rammapPath}");

                DebugStep(6, "Liberando Working Sets...");
                var emptyWorking = RunProcess(rammapPath, "-Ew");
                if (emptyWorking.ExitCode != 0 || !string.IsNullOrWhiteSpace(emptyWorking.StandardError))
                {
                    erros.Add("Empty Working Sets");
                    DebugWarning("Aviso ao liberar Working Sets");
                }
                else
                {
                    DebugSuccess("Working Sets liberados");
                }

                DebugStep(7, "Liberando Standby List...");
                var emptyStandby = RunProcess(rammapPath, "-Et");
                if (emptyStandby.ExitCode != 0 || !string.IsNullOrWhiteSpace(emptyStandby.StandardError))
                {
                    erros.Add("Empty Standby List");
                    DebugWarning("Aviso ao liberar Standby List");
                }
                else
                {
                    DebugSuccess("Standby List liberada");
                }
            }
        }

        if (erros.Count > 0)
        {
            return $"Ocorreu um erro ao executar: {string.Join(", ", erros)}";
        }

        DebugSuccess("Limpeza completa finalizada!");
        return "Limpeza concluída com sucesso (Temp + RAM)";
    }

    private static void SpeedTest()
    {
        Header("Speed Test");
        DebugStep(1, "Executando SpeedTest...");

        var speedtestPath = Path.Combine("Scripts", "Apps", "Speedtest", "speedtest.exe");
        CommandResult result;

        if (File.Exists(speedtestPath))
        {
            result = RunProcess(speedtestPath, "--format", "json");
        }
        else
        {
            result = RunPowerShell("speedtest --format=json");
        }

        if (result.ExitCode != 0)
        {
            DebugWarning("Falha ao executar speedtest. Certifique-se de que o Speedtest CLI está instalado.");
            return;
        }

        try
        {
            using var data = JsonDocument.Parse(result.StandardOutput);
            var download = data.RootElement.GetProperty("download").GetProperty("bandwidth").GetDouble() * 8 / 1_000_000;
            var upload = data.RootElement.GetProperty("upload").GetProperty("bandwidth").GetDouble() * 8 / 1_000_000;
            var ping = data.RootElement.GetProperty("ping").GetProperty("latency").GetDouble();

            Console.WriteLine("\n📡 Resultados do Teste de Velocidade:");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"📥 Download: {download:F2} Mbs");
            Console.WriteLine($"📤 Upload:   {upload:F2} Mbs");
            Console.WriteLine($"⚡ Ping:      {ping:F1} ms");
            Console.WriteLine("----------------------------------------\n");
        }
        catch (Exception ex)
        {
            DebugWarning($"Não foi possível interpretar o resultado do Speedtest: {ex.Message}");
        }
    }

    private static string ClearNet()
    {
        Header("LIMPEZA DE REDE");

        DebugStep(1, "Verificando privilégios de administrador...");
        if (!IsAdministrator())
        {
            DebugError("Este script precisa ser executado como ADMINISTRADOR!");
            DebugWarning("A limpeza de rede requer privilégios elevados.");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nDeseja reiniciar como administrador? (s/n): ");
            Console.ResetColor();
            var resposta = Console.ReadLine();
            if (string.Equals(resposta, "s", StringComparison.OrdinalIgnoreCase))
            {
                RunAsAdministrator();
                return "Reiniciando como administrador...";
            }
            DebugWarning("Continuando sem privilégios...");
        }
        else
        {
            DebugSuccess("Privilégios de administrador confirmados");
        }

        var erros = new List<string>();

        DebugStep(2, "Limpando DNS da máquina...");
        var flushDns = RunPowerShell("ipconfig /flushdns");
        if (!string.IsNullOrWhiteSpace(flushDns.StandardError))
        {
            erros.Add("Flush DNS");
            DebugError("Erro ao limpar o DNS da máquina");
        }
        else
        {
            DebugSuccess("Limpeza do DNS realizada com sucesso!");
        }

        DebugStep(3, "Re-register do DNS...");
        var reRegisterDns = RunPowerShell("ipconfig /registerdns");
        if (!string.IsNullOrWhiteSpace(reRegisterDns.StandardError))
        {
            erros.Add("Re-Register do DNS");
            DebugError("Erro ao fazer o re-register da máquina");
        }
        else
        {
            DebugSuccess("Re-register da máquina feito com sucesso!");
        }

        DebugStep(4, "Fazendo release do IP...");
        var releaseIp = RunPowerShell("ipconfig /release");
        if (!string.IsNullOrWhiteSpace(releaseIp.StandardError))
        {
            erros.Add("Release IP");
            DebugError("Aviso ao executar release do IP");
        }
        else
        {
            DebugSuccess("Release do IP executado!");
        }

        DebugStep(5, "Renew do IP...");
        var renewIp = RunPowerShell("ipconfig /renew");
        if (!string.IsNullOrWhiteSpace(renewIp.StandardError))
        {
            erros.Add("Renew do IP");
            DebugError("Aviso ao executar o renew do IP");
        }
        else
        {
            DebugSuccess("Renew do IP feito!");
        }

        DebugStep(6, "Reset de IP...");
        var resetIp = RunPowerShell("netsh int ip reset");
        if (!string.IsNullOrWhiteSpace(resetIp.StandardError))
        {
            erros.Add("Reset de IP");
            DebugError("Aviso ao resetar IP");
        }
        else
        {
            DebugSuccess("Reset do IP feito!");
        }

        DebugStep(7, "Reset do Winsock...");
        var resetWinsock = RunPowerShell("netsh winsock reset");
        if (!string.IsNullOrWhiteSpace(resetWinsock.StandardError))
        {
            erros.Add("Reset do WinSock");
            DebugError("Aviso ao resetar o WinSock");
        }
        else
        {
            DebugSuccess("Reset do Winsock feito com sucesso!");
        }

        if (erros.Count > 0)
        {
            return $"Ocorreu um erro ao executar: {string.Join(", ", erros)}";
        }

        DebugSuccess("Limpeza de rede concluída!");
        return "Limpeza da Rede WiFi/Ethernet concluída";
    }

    private static void TestPing()
    {
        Header("Teste de Ping");

        DebugStep(1, "Ping do DNS Google..");
        var pingGoogle = RunPowerShell("ping 8.8.8.8");
        if (!string.IsNullOrWhiteSpace(pingGoogle.StandardError))
        {
            DebugError("Erro ao pingar DNS Google");
        }
        else
        {
            DebugSuccess("Ping bem sucedido");
        }
    }

    private static string OtimizarPing()
    {
        Header("OTIMIZAÇÃO DE PING");
        DebugStep(1, "Verificando privilégios de administrador...");
        if (!IsAdministrator())
        {
            DebugError("Este script precisa ser executado como ADMINISTRADOR!");
            DebugWarning("A otimização requer privilégios elevados.");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nDeseja reiniciar como administrador? (s/n): ");
            Console.ResetColor();
            var resposta = Console.ReadLine();
            if (string.Equals(resposta, "s", StringComparison.OrdinalIgnoreCase))
            {
                RunAsAdministrator();
                return "Reiniciando como administrador...";
            }
            DebugWarning("Continuando sem privilégios...");
        }
        else
        {
            DebugSuccess("Privilégios de administrador confirmados");
        }

        var erros = new List<string>();

        DebugStep(2, "Desativando o autotuninglevel...");
        var autotune = RunPowerShell("netsh interface tcp set global autotuninglevel=disabled");
        if (!string.IsNullOrWhiteSpace(autotune.StandardError))
        {
            DebugError("Erro ao desativar autotuninglevel");
            erros.Add("autotuninglevel");
        }
        else
        {
            DebugSuccess("Autotuninglevel desativado");
        }

        DebugStep(3, "Ativando RSS...");
        var rss = RunPowerShell("netsh interface tcp set global rss=enabled");
        if (!string.IsNullOrWhiteSpace(rss.StandardError))
        {
            DebugError("Erro ao ativar RSS");
            erros.Add("rss");
        }
        else
        {
            DebugSuccess("RSS ativado com sucesso");
        }

        DebugStep(4, "Desativando Global Chimney...");
        var chimney = RunPowerShell("netsh interface tcp set global chimney=disabled");
        if (!string.IsNullOrWhiteSpace(chimney.StandardError))
        {
            DebugError("Erro ao desativar chimney");
            erros.Add("chimney");
        }
        else
        {
            DebugSuccess("Chimney desativado");
        }

        DebugStep(5, "Desativando heuristics...");
        var heuristics = RunPowerShell("netsh int tcp set heuristics disabled");
        if (!string.IsNullOrWhiteSpace(heuristics.StandardError))
        {
            DebugError("Erro ao desativar heuristics");
            erros.Add("heuristics");
        }
        else
        {
            DebugSuccess("Heuristics desativado");
        }

        if (erros.Count > 0)
        {
            return $"Ocorreu um erro ao executar: {string.Join(", ", erros)}";
        }

        DebugSuccess("Otimização Finalizado");
        return "Sistema Finalizado";
    }

    private static string OtimizarWifi()
    {
        Header("OTIMIZAÇÃO DE WI-FI");
        Console.WriteLine("\n[ 1 ] Otimizar Wi-Fi\n[ 2 ] Reverter Wi-Fi\n");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Escolha: ");
        Console.ResetColor();
        var op = Console.ReadLine();

        var erros = new List<string>();

        if (op == "1")
        {
            DebugStep(1, "Ajustando nível global de autotuning");
            var autotuninglevel = RunPowerShell("netsh interface tcp set global autotuninglevel=disabled");
            if (!string.IsNullOrWhiteSpace(autotuninglevel.StandardError))
            {
                DebugError("Erro ao ajustar autotuninglevel");
                erros.Add("autotuninglevel");
            }
            else
            {
                DebugSuccess("Autotuninglevel ajustado");
            }

            DebugStep(2, "Ativando Receive Side Scaling (RSS)");
            var rss = RunPowerShell("netsh interface tcp set global rss=enabled");
            if (!string.IsNullOrWhiteSpace(rss.StandardError))
            {
                DebugError("Erro na ativação do RSS");
                erros.Add("rss");
            }
            else
            {
                DebugSuccess("RSS ativado");
            }

            DebugStep(3, "Desativando Global Chimney");
            var chimney = RunPowerShell("netsh interface tcp set global chimney=disabled");
            if (!string.IsNullOrWhiteSpace(chimney.StandardError))
            {
                DebugError("Erro ao desativar chimney");
                erros.Add("chimney");
            }
            else
            {
                DebugSuccess("Global Chimney desativado");
            }

            DebugStep(4, "Desativando heuristics");
            var heuristics = RunPowerShell("netsh int tcp set heuristics disabled");
            if (!string.IsNullOrWhiteSpace(heuristics.StandardError))
            {
                DebugError("Erro na desativação de heuristics");
                erros.Add("heuristics");
            }
            else
            {
                DebugSuccess("Heuristics desativado");
            }
        }
        else if (op == "2")
        {
            Header("REVERTENDO WI-FI PARA PADRÃO");

            DebugStep(2, "Revertendo autotuninglevel");
            var autotuninglevel = RunPowerShell("netsh interface tcp set global autotuninglevel=restricted");
            if (!string.IsNullOrWhiteSpace(autotuninglevel.StandardError))
            {
                DebugError("Erro em reverter autotuninglevel");
                erros.Add("autotuninglevel");
            }
            else
            {
                DebugSuccess("Reversão do autotuninglevel concluída");
            }

            DebugStep(3, "Desativando RSS");
            var rss = RunPowerShell("netsh interface tcp set global rss=disabled");
            if (!string.IsNullOrWhiteSpace(rss.StandardError))
            {
                DebugError("Erro na desativação do RSS");
                erros.Add("rss");
            }
            else
            {
                DebugSuccess("RSS desativado");
            }

            DebugStep(4, "Ativando Global Chimney");
            var chimney = RunPowerShell("netsh interface tcp set global chimney=enabled");
            if (!string.IsNullOrWhiteSpace(chimney.StandardError))
            {
                DebugError("Erro na ativação do Global Chimney");
                erros.Add("chimney");
            }
            else
            {
                DebugSuccess("Global Chimney ativado");
            }

            DebugStep(5, "Ativando heuristics");
            var heuristics = RunPowerShell("netsh int tcp set heuristics enabled");
            if (!string.IsNullOrWhiteSpace(heuristics.StandardError))
            {
                DebugError("Erro na ativação da heuristics");
                erros.Add("heuristics");
            }
            else
            {
                DebugSuccess("Heuristics ativado");
            }
        }
        else
        {
            DebugWarning("Opção inválida");
            return "Ação cancelada pelo usuário.";
        }

        if (erros.Count > 0)
        {
            return $"Ocorreu um erro ao executar: {string.Join(", ", erros)}";
        }

        DebugSuccess("Otimização Finalizado");
        return "Sistema Finalizado";
    }

    private static string MapNet()
    {
        Header("Mapa de conexão");

        DebugStep(1, "Verificando privilégios de administrador...");
        if (!IsAdministrator())
        {
            DebugError("Este script precisa ser executado como ADMINISTRADOR!");
            DebugWarning("A limpeza de RAM requer privilégios elevados.");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nDeseja reiniciar como administrador? (s/n): ");
            Console.ResetColor();
            var resposta = Console.ReadLine();
            if (string.Equals(resposta, "s", StringComparison.OrdinalIgnoreCase))
            {
                RunAsAdministrator();
                return "Reiniciando como administrador...";
            }
            DebugWarning("Continuando sem limpeza de RAM...");
        }
        else
        {
            DebugSuccess("Privilégios de administrador confirmados");
        }

        DebugStep(2, "Localizar Servidor");
        Console.Write("Digite o Servidor que deseja Mapear: ");
        var net = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(net))
        {
            return "Ação cancelada pelo usuário.";
        }

        DebugStep(3, "Mapeando a rede...");
        Console.WriteLine("ATENÇÃO ISSO PODE LEVAR UM TEMPO");
        var trackNet = RunPowerShell($"tracert {net}");
        if (!string.IsNullOrWhiteSpace(trackNet.StandardError))
        {
            DebugError("Erro ao mapear o Servidor");
            return "Servidor não encontrado";
        }

        DebugSuccess("Servidor Mapeado com sucesso");
        return "Mapeamento concluído";
    }

    private static string TemperatureMonitor()
    {
        Header("Monitor de Temperatura");

        DebugStep(1, "Verificando privilégios de administrador...");
        if (!IsAdministrator())
        {
            DebugError("Este script precisa ser executado como ADMINISTRADOR!");
            DebugWarning("O monitoramento requer privilégios elevados.");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nDeseja reiniciar como administrador? (s/n): ");
            Console.ResetColor();
            var resposta = Console.ReadLine();
            if (string.Equals(resposta, "s", StringComparison.OrdinalIgnoreCase))
            {
                RunAsAdministrator();
                return "Reiniciando como administrador...";
            }
            DebugWarning("Continuando sem privilégios elevados...");
        }
        else
        {
            DebugSuccess("Privilégios de administrador confirmados");
        }

        try
        {
            using var searcher = new ManagementObjectSearcher("root\\\\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
            foreach (var obj in searcher.Get())
            {
                var temp = Convert.ToDouble(obj["CurrentTemperature"]) / 10 - 273.15;
                TxtInfo("Temperatura (°C)", temp.ToString("F1"));
            }
        }
        catch (Exception ex)
        {
            DebugWarning($"Não foi possível obter temperaturas: {ex.Message}");
        }

        return "Monitoramento concluído";
    }

    private static string RestartPoint()
    {
        Header("CRIANDO PONTO DE RESTAURAÇÃO");

        DebugStep(1, "Verificando privilégios de administrador...");
        if (!IsAdministrator())
        {
            DebugError("Este script precisa ser executado como ADMINISTRADOR!");
            DebugWarning("A criação de ponto de restauração requer privilégios elevados.");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nDeseja reiniciar como administrador? (s/n): ");
            Console.ResetColor();
            var resposta = Console.ReadLine();
            if (string.Equals(resposta, "s", StringComparison.OrdinalIgnoreCase))
            {
                RunAsAdministrator();
                return "Reiniciando como administrador...";
            }
            DebugWarning("Continuando sem privilégios...");
        }
        else
        {
            DebugSuccess("Privilégios de administrador confirmados");
        }

        Console.Write("Digite um nome para o ponto de restauração: ");
        var nome = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(nome))
        {
            return "Ação cancelada pelo usuário.";
        }

        var command = $"Checkpoint-Computer -Description \"{nome}\" -RestorePointType APPLICATION_INSTALL";
        var restore = RunPowerShell(command);
        if (!string.IsNullOrWhiteSpace(restore.StandardError))
        {
            DebugError("Erro ao criar ponto de restauração");
            return "Falha ao criar ponto de restauração";
        }

        DebugSuccess("Ponto de restauração criado com sucesso");
        return "Ponto de restauração concluído";
    }

    private static string ConfigPosInstall()
    {
        Header("CONFIGURAÇÃO PÓS-INSTALAÇÃO");
        var pasta = "Install";
        var lista = ListarArquivos(pasta);
        if (lista.Count == 0)
        {
            DebugWarning("Nenhum instalador encontrado");
            return "Nenhuma ação executada";
        }

        var selecionados = SelecionarArquivos(lista);
        if (selecionados.Count == 0)
        {
            DebugWarning("Nenhum aplicativo selecionado");
            return "Nenhuma ação executada";
        }

        foreach (var arquivo in selecionados)
        {
            ExecutarInstalador(arquivo);
        }

        DebugSuccess("Instalação concluída");
        return "Instaladores executados";
    }

    private static List<string> ListarArquivos(string pasta)
    {
        var diretorio = Path.Combine("Scripts", pasta);
        if (!Directory.Exists(diretorio))
        {
            return new List<string>();
        }

        return Directory.GetFiles(diretorio, "*.exe", SearchOption.TopDirectoryOnly).ToList();
    }

    private static List<string> SelecionarArquivos(IReadOnlyList<string> arquivos)
    {
        Console.WriteLine("\nSelecione os aplicativos para instalar (separe por vírgula):\n");
        for (var i = 0; i < arquivos.Count; i++)
        {
            Console.WriteLine($"[{i + 1}] {Path.GetFileName(arquivos[i])}");
        }

        Console.Write("\nOpções: ");
        var input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            return new List<string>();
        }

        var indices = input.Split(',').Select(x => x.Trim());
        var selecionados = new List<string>();
        foreach (var indice in indices)
        {
            if (int.TryParse(indice, out var pos) && pos >= 1 && pos <= arquivos.Count)
            {
                selecionados.Add(arquivos[pos - 1]);
            }
        }

        return selecionados;
    }

    private static void ExecutarInstalador(string arquivo)
    {
        DebugStep(0, $"Executando {Path.GetFileName(arquivo)}...");
        var result = RunProcess(arquivo);
        if (result.ExitCode != 0)
        {
            DebugWarning($"Instalador retornou código {result.ExitCode}");
        }
        else
        {
            DebugSuccess("Instalador finalizado");
        }
    }

    private static string WinDefender()
    {
        Header("WINDOWS DEFENDER");

        DebugStep(1, "Atualizando assinaturas...");
        var update = RunPowerShell("MpCmdRun.exe -SignatureUpdate");
        if (!string.IsNullOrWhiteSpace(update.StandardError))
        {
            DebugWarning("Falha na atualização de assinaturas");
        }
        else
        {
            DebugSuccess("Assinaturas atualizadas");
        }

        DebugStep(2, "Executando verificação completa...");
        var scan = RunPowerShell("MpCmdRun.exe -Scan -ScanType 2");
        if (!string.IsNullOrWhiteSpace(scan.StandardError))
        {
            DebugWarning("Falha ao executar verificação");
        }
        else
        {
            DebugSuccess("Verificação concluída");
        }

        return "Windows Defender executado";
    }

    private static void OtimizacaoWindowsMenu()
    {
        while (true)
        {
            MenuOtmWin();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nEscolha uma opção: ");
            Console.ResetColor();
            var opcao = Console.ReadLine();

            string? resultado = opcao switch
            {
                "1" => OtimizacaoEnergia(),
                "2" => OtimizacaoAltTab(),
                "3" => DesativarEfeitosVisuais(),
                "4" => DesativarTelemetria(),
                "5" => ServiçosInuteis(),
                "6" => Debloater(),
                "7" => Overlays(),
                "8" => DesativarUac(),
                "9" => DesativarHibernacao(),
                "10" => DesativarHyperV(),
                "11" => DesativarDownloadMaps(),
                "12" => DesativarIndexacao(),
                "13" => DesativarAeroPeek(),
                "14" => DesativarSmartScreen(),
                _ => null
            };

            if (opcao == "0")
            {
                return;
            }

            if (resultado != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n{resultado}\n");
                Console.ResetColor();
            }

            var escolha = PerguntarContinuarWin();
            if (escolha == "menu_principal")
            {
                return;
            }
        }
    }

    private static void MenuOtmWin()
    {
        Header("OTIMIZAÇÃO DO WINDOWS");
        Console.WriteLine("[ 1 ] Otimizar Energia");
        Console.WriteLine("[ 2 ] Ajustar Alt+Tab");
        Console.WriteLine("[ 3 ] Desativar Efeitos Visuais");
        Console.WriteLine("[ 4 ] Desativar Telemetria");
        Console.WriteLine("[ 5 ] Desativar Serviços Inúteis");
        Console.WriteLine("[ 6 ] Executar Debloater");
        Console.WriteLine("[ 7 ] Configurar Overlays");
        Console.WriteLine("[ 8 ] Desativar UAC");
        Console.WriteLine("[ 9 ] Desativar Hibernação");
        Console.WriteLine("[ 10 ] Desativar Hyper-V");
        Console.WriteLine("[ 11 ] Desativar Download Maps");
        Console.WriteLine("[ 12 ] Desativar Indexação");
        Console.WriteLine("[ 13 ] Desativar Aero Peek");
        Console.WriteLine("[ 14 ] Desativar SmartScreen");
        Console.WriteLine("[ 0 ] Voltar");
    }

    private static string OtimizacaoEnergia()
    {
        Header("OTIMIZAÇÃO DE ENERGIA");
        var erros = new List<string>();

        DebugStep(1, "Listando planos de energia...");
        var listPlans = RunPowerShell("powercfg /list");
        if (!string.IsNullOrWhiteSpace(listPlans.StandardError))
        {
            erros.Add("Listar planos");
        }

        DebugStep(2, "Ativando modo de alto desempenho...");
        var high = RunPowerShell("powercfg /S SCHEME_MIN");
        if (!string.IsNullOrWhiteSpace(high.StandardError))
        {
            erros.Add("Alto desempenho");
        }

        return erros.Count > 0 ? $"Ocorreu um erro: {string.Join(", ", erros)}" : "Energia otimizada";
    }

    private static string OtimizacaoAltTab()
    {
        Header("OTIMIZAÇÃO ALT+TAB");
        var erros = new List<string>();

        var disable = RunPowerShell("reg add \"HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced\" /v AltTabSettings /t REG_DWORD /d 1 /f", printOutput: false);
        if (!string.IsNullOrWhiteSpace(disable.StandardError))
        {
            erros.Add("AltTabSettings");
        }

        return erros.Count > 0 ? $"Ocorreu um erro ao executar: {string.Join(", ", erros)}" : "Alt+Tab otimizado";
    }

    private static string DesativarEfeitosVisuais()
    {
        Header("DESATIVAR EFEITOS VISUAIS");
        var command = "reg add \"HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VisualEffects\" /v VisualFXSetting /t REG_DWORD /d 2 /f";
        var result = RunPowerShell(command, printOutput: false);
        return string.IsNullOrWhiteSpace(result.StandardError) ? "Efeitos visuais desativados" : "Falha ao desativar efeitos visuais";
    }

    private static string DesativarTelemetria()
    {
        Header("DESATIVAR TELEMETRIA");
        var comandos = new[]
        {
            "sc stop DiagTrack",
            "sc stop dmwappushservice",
            "sc config DiagTrack start=disabled",
            "sc config dmwappushservice start=disabled",
            "reg add \"HKLM\\Software\\Policies\\Microsoft\\Windows\\DataCollection\" /v AllowTelemetry /t REG_DWORD /d 0 /f"
        };

        var erros = new List<string>();
        foreach (var comando in comandos)
        {
            var result = RunPowerShell(comando, printOutput: false);
            if (!string.IsNullOrWhiteSpace(result.StandardError))
            {
                erros.Add(comando);
            }
        }

        return erros.Count > 0 ? $"Ocorreu um erro: {string.Join(", ", erros)}" : "Telemetria desativada";
    }

    private static string ServiçosInuteis()
    {
        Header("DESATIVANDO SERVIÇOS INÚTEIS");
        var comandos = new[]
        {
            "sc stop SysMain",
            "sc config SysMain start=disabled",
            "sc stop WSearch",
            "sc config WSearch start=disabled",
            "sc stop DiagTrack",
            "sc config DiagTrack start=disabled"
        };

        var erros = new List<string>();
        foreach (var comando in comandos)
        {
            var result = RunPowerShell(comando, printOutput: false);
            if (!string.IsNullOrWhiteSpace(result.StandardError))
            {
                erros.Add(comando);
            }
        }

        return erros.Count > 0 ? $"Ocorreu um erro: {string.Join(", ", erros)}" : "Serviços desativados";
    }

    private static string Debloater()
    {
        Header("WINDOWS DEBLOATER");
        var scriptPath = Path.Combine("Scripts", "WinDebloater", "WinDebloater.ps1");
        if (!File.Exists(scriptPath))
        {
            return $"Script não encontrado: {scriptPath}";
        }

        var command = $"& \"{scriptPath}\"";
        var result = RunPowerShell(command);
        return string.IsNullOrWhiteSpace(result.StandardError) ? "Debloat concluído" : "Erro ao executar debloater";
    }

    private static string Overlays()
    {
        Header("OVERLAYS");
        Console.WriteLine("\n[ 1 ] Desativar Overlays\n[ 2 ] Reverter Overlays\n");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Escolha: ");
        Console.ResetColor();
        var op = Console.ReadLine();

        var erros = new List<string>();

        if (op == "1")
        {
            DebugStep(1, "Desativando AllowAutoGameMode...");
            var cmd1 = RunPowerShell("reg add \"HKCU\\Software\\Microsoft\\GameBar\" /v \"AllowAutoGameMode\" /t REG_DWORD /d 0 /f", printOutput: false);
            if (!string.IsNullOrWhiteSpace(cmd1.StandardError)) erros.Add("AllowAutoGameMode");

            DebugStep(2, "Desativando AutoGameModeEnabled...");
            var cmd2 = RunPowerShell("reg add \"HKCU\\Software\\Microsoft\\GameBar\" /v \"AutoGameModeEnabled\" /t REG_DWORD /d 0 /f", printOutput: false);
            if (!string.IsNullOrWhiteSpace(cmd2.StandardError)) erros.Add("AutoGameModeEnabled");

            DebugStep(3, "Desativando ShowStartupPanel...");
            var cmd3 = RunPowerShell("reg add \"HKCU\\Software\\Microsoft\\GameBar\" /v \"ShowStartupPanel\" /t REG_DWORD /d 0 /f", printOutput: false);
            if (!string.IsNullOrWhiteSpace(cmd3.StandardError)) erros.Add("ShowStartupPanel");

            DebugStep(4, "Desativando GameDVR e Xbox Game Bar...");
            var cmd4 = RunPowerShell("reg add \"HKCU\\System\\GameConfigStore\" /v \"GameDVR_Enabled\" /t REG_DWORD /d 0 /f", printOutput: false);
            if (!string.IsNullOrWhiteSpace(cmd4.StandardError)) erros.Add("GameDVR_Enabled");

            var cmd5 = RunPowerShell("reg add \"HKCU\\Software\\Microsoft\\GameBar\" /v \"GamePanelStartupTipIndex\" /t REG_DWORD /d 0 /f", printOutput: false);
            if (!string.IsNullOrWhiteSpace(cmd5.StandardError)) erros.Add("GamePanelStartupTipIndex");
        }
        else if (op == "2")
        {
            DebugStep(2, "Reativando AllowAutoGameMode...");
            var cmd1 = RunPowerShell("reg add \"HKCU\\Software\\Microsoft\\GameBar\" /v \"AllowAutoGameMode\" /t REG_DWORD /d 1 /f", printOutput: false);
            if (!string.IsNullOrWhiteSpace(cmd1.StandardError)) erros.Add("AllowAutoGameMode");

            DebugStep(3, "Reativando AutoGameModeEnabled...");
            var cmd2 = RunPowerShell("reg add \"HKCU\\Software\\Microsoft\\GameBar\" /v \"AutoGameModeEnabled\" /t REG_DWORD /d 1 /f", printOutput: false);
            if (!string.IsNullOrWhiteSpace(cmd2.StandardError)) erros.Add("AutoGameModeEnabled");

            DebugStep(4, "Reativando ShowStartupPanel...");
            var cmd3 = RunPowerShell("reg add \"HKCU\\Software\\Microsoft\\GameBar\" /v \"ShowStartupPanel\" /t REG_DWORD /d 1 /f", printOutput: false);
            if (!string.IsNullOrWhiteSpace(cmd3.StandardError)) erros.Add("ShowStartupPanel");

            DebugStep(5, "Reativando GameDVR e Xbox Game Bar...");
            var cmd4 = RunPowerShell("reg add \"HKCU\\System\\GameConfigStore\" /v \"GameDVR_Enabled\" /t REG_DWORD /d 1 /f", printOutput: false);
            if (!string.IsNullOrWhiteSpace(cmd4.StandardError)) erros.Add("GameDVR_Enabled");

            var cmd5 = RunPowerShell("reg add \"HKCU\\Software\\Microsoft\\GameBar\" /v \"GamePanelStartupTipIndex\" /t REG_DWORD /d 1 /f", printOutput: false);
            if (!string.IsNullOrWhiteSpace(cmd5.StandardError)) erros.Add("GamePanelStartupTipIndex");
        }
        else
        {
            DebugWarning("Opção inválida");
            return "Ação cancelada pelo usuário.";
        }

        if (erros.Count > 0)
        {
            return $"Ocorreu um erro ao executar: {string.Join(", ", erros)}";
        }

        DebugSuccess("Otimização completa!");
        return "Processo concluído com sucesso";
    }

    private static string DesativarUac()
    {
        Header("Desativação do Controle de Conta de Usuário (UAC)");

        DebugStep(1, "Verificando privilégios de administrador...");
        if (!IsAdministrator())
        {
            DebugError("Este script precisa ser executado como ADMINISTRADOR!");
            DebugWarning("A desativação do UAC requer privilégios elevados.");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nDeseja reiniciar como administrador? (s/n): ");
            Console.ResetColor();
            var resposta = Console.ReadLine();
            if (string.Equals(resposta, "s", StringComparison.OrdinalIgnoreCase))
            {
                RunAsAdministrator();
                return "Reiniciando como administrador...";
            }
            DebugWarning("Continuando sem privilégios elevados...");
        }
        else
        {
            DebugSuccess("Privilégios de administrador confirmados");
        }

        var erros = new List<string>();

        DebugStep(2, "Executando verificação de integridade do sistema (sfc /scannow)...");
        var sfcScan = RunPowerShell("sfc /scannow");
        if (sfcScan.ExitCode != 0 || (!string.IsNullOrWhiteSpace(sfcScan.StandardError) && sfcScan.StandardError.Contains("erro", StringComparison.OrdinalIgnoreCase)))
        {
            DebugError("Falha ao executar o verificador de arquivos do sistema (SFC).");
            erros.Add("sfc /scannow");
        }
        else
        {
            DebugSuccess("Verificação de integridade concluída com sucesso.");
        }

        DebugStep(3, "Desativando o UAC (User Account Control)...");
        var disableUac = RunPowerShell("reg add \"HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System\" /v EnableLUA /t REG_DWORD /d 0 /f", printOutput: false);
        if (!string.IsNullOrWhiteSpace(disableUac.StandardError))
        {
            DebugError("Erro ao desativar o UAC.");
            erros.Add("EnableLUA");
        }
        else
        {
            DebugSuccess("UAC desativado com sucesso.");
        }

        DebugStep(4, "Finalizando processo e limpando tela...");
        RunPowerShell("cls");

        if (erros.Count > 0)
        {
            return $"Ocorreu um erro ao executar: {string.Join(", ", erros)}";
        }

        DebugSuccess("Otimização completa!");
        return "Processo concluído com sucesso";
    }

    private static string DesativarHibernacao()
    {
        Header("DESATIVAR HIBERNAÇÃO");
        var result = RunPowerShell("powercfg -h off", printOutput: false);
        return string.IsNullOrWhiteSpace(result.StandardError) ? "Hibernação desativada" : "Erro ao desativar hibernação";
    }

    private static string DesativarHyperV()
    {
        Header("DESATIVAR HYPER-V");
        var comandos = new[]
        {
            "dism.exe /Online /Disable-Feature /FeatureName:Microsoft-Hyper-V-All",
            "bcdedit /set hypervisorlaunchtype off"
        };

        var erros = new List<string>();
        foreach (var comando in comandos)
        {
            var result = RunPowerShell(comando);
            if (!string.IsNullOrWhiteSpace(result.StandardError))
            {
                erros.Add(comando);
            }
        }

        return erros.Count > 0 ? $"Ocorreu um erro: {string.Join(", ", erros)}" : "Hyper-V desativado";
    }

    private static string DesativarDownloadMaps()
    {
        Header("DESATIVAR DOWNLOAD MAPS");
        var result = RunPowerShell("sc config MapsBroker start=disabled", printOutput: false);
        return string.IsNullOrWhiteSpace(result.StandardError) ? "Download Maps desativado" : "Erro ao desativar Download Maps";
    }

    private static string DesativarIndexacao()
    {
        Header("DESATIVAR INDEXAÇÃO");
        var comandos = new[]
        {
            "sc stop WSearch",
            "sc config WSearch start=disabled"
        };

        var erros = new List<string>();
        foreach (var comando in comandos)
        {
            var result = RunPowerShell(comando, printOutput: false);
            if (!string.IsNullOrWhiteSpace(result.StandardError))
            {
                erros.Add(comando);
            }
        }

        return erros.Count > 0 ? $"Ocorreu um erro: {string.Join(", ", erros)}" : "Indexação desativada";
    }

    private static string DesativarAeroPeek()
    {
        Header("DESATIVAR AERO PEEK");
        var command = "reg add \"HKCU\\Software\\Microsoft\\Windows\\DWM\" /v EnableAeroPeek /t REG_DWORD /d 0 /f";
        var result = RunPowerShell(command, printOutput: false);
        return string.IsNullOrWhiteSpace(result.StandardError) ? "Aero Peek desativado" : "Erro ao desativar Aero Peek";
    }

    private static string DesativarSmartScreen()
    {
        Header("DESATIVAR SMARTSCREEN");
        var command = "reg add \"HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\System\" /v EnableSmartScreen /t REG_DWORD /d 0 /f";
        var result = RunPowerShell(command, printOutput: false);
        return string.IsNullOrWhiteSpace(result.StandardError) ? "SmartScreen desativado" : "Erro ao desativar SmartScreen";
    }

    private static string WinUpdate()
    {
        Header("ATUALIZAÇÃO DO WINDOWS");
        var comandos = new[]
        {
            "UsoClient StartScan",
            "UsoClient StartDownload",
            "UsoClient StartInstall"
        };

        var erros = new List<string>();
        foreach (var comando in comandos)
        {
            var result = RunPowerShell(comando);
            if (!string.IsNullOrWhiteSpace(result.StandardError))
            {
                erros.Add(comando);
            }
        }

        return erros.Count > 0 ? $"Ocorreu um erro: {string.Join(", ", erros)}" : "Atualização do Windows iniciada";
    }

    private sealed record CommandResult(int ExitCode, string StandardOutput, string StandardError);
}
