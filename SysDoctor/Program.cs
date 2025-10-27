using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace SysDoctor  // alterado de DoctorSystem
{
    class Program
    {
        // Constantes para cores do console
        private static class ConsoleColors
        {
            public const string Cyan = "\u001b[36m";
            public const string Yellow = "\u001b[33m";
            public const string Green = "\u001b[32m";
            public const string Red = "\u001b[31m";
            public const string Magenta = "\u001b[35m";
            public const string White = "\u001b[37m";
            public const string Reset = "\u001b[0m";
        }

        [DllImport("shell32.dll")]
        private static extern bool IsUserAnAdmin();

        [DllImport("shell32.dll")]
        private static extern IntPtr ShellExecuteW(
            IntPtr hwnd,
            string lpOperation,
            string lpFile,
            string lpParameters,
            string lpDirectory,
            int nShowCmd
        );

        static void Main(string[] args)
        {
            try
            {
                InitializeConsole();
                Console.Title = "SysDoctor - Windows Optimizer";
                Console.WriteLine("Pressione qualquer tecla para iniciar...");
                Console.ReadKey();
                Console.Clear();
                MainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                Console.WriteLine("\nPressione qualquer tecla para sair...");
                Console.ReadKey();
            }
            finally
            {
                // Garante que o console não feche imediatamente
                if (!Console.IsInputRedirected)
                {
                    Console.WriteLine("\nPressione qualquer tecla para sair...");
                    Console.ReadKey();
                }
            }
        }

        static void InitializeConsole()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            
            // Verifica se está sendo executado em um console
            if (!Console.IsOutputRedirected)
            {
                try
                {
                    Console.BufferHeight = Console.WindowHeight;
                    Console.BufferWidth = Math.Max(100, Console.WindowWidth);
                }
                catch
                {
                    // Ignora erros de redimensionamento do buffer
                }
            }
        }

        #region Funções Auxiliares de Debug
        static void Header(string title)
        {
            Console.WriteLine(ConsoleColors.Cyan + $"\n=== {title} ===" + ConsoleColors.Reset);
        }

        static void TxtInfo(string label, string value)
        {
            Console.WriteLine(ConsoleColors.Yellow + $"{label,-30}: " + ConsoleColors.Reset + $"{value}");
        }

        static void DebugStep(int stepNumber, string description)
        {
            Console.WriteLine(ConsoleColors.Magenta + $"\n[PASSO {stepNumber}] " + ConsoleColors.White + description + ConsoleColors.Reset);
        }

        static void DebugSuccess(string message)
        {
            Console.WriteLine(ConsoleColors.Green + $"  ✓ {message}" + ConsoleColors.Reset);
        }

        static void DebugError(string message)
        {
            Console.WriteLine(ConsoleColors.Red + $"  ✗ {message}" + ConsoleColors.Reset);
        }

        static void DebugWarning(string message)
        {
            Console.WriteLine(ConsoleColors.Yellow + $"  ⚠ {message}" + ConsoleColors.Reset);
        }

        static bool IsAdmin()
        {
            try
            {
                return IsUserAnAdmin();
            }
            catch
            {
                return false;
            }
        }

        static bool RunAsAdmin()
        {
            DebugWarning("Solicitando privilégios de administrador...");
            try
            {
                string script = Process.GetCurrentProcess().MainModule.FileName;
                ShellExecuteW(IntPtr.Zero, "runas", script, "", null, 1);
                Environment.Exit(0);
                return true;
            }
            catch (Exception e)
            {
                DebugError($"Falha ao solicitar privilégios: {e.Message}");
                return false;
            }
        }

        static void PerguntarContinuar()
        {
            while (true)
            {
                Console.WriteLine("\n" + new string('=', 50));
                Console.WriteLine(ConsoleColors.Cyan + "1 - Voltar ao Menu Principal" + ConsoleColors.Reset);
                Console.WriteLine(ConsoleColors.Cyan + "0 - Sair" + ConsoleColors.Reset);
                Console.Write(ConsoleColors.Yellow + "\nEscolha uma opção: " + ConsoleColors.Reset);

                string opcao = Console.ReadLine();

                if (opcao == "0")
                {
                    Console.WriteLine(ConsoleColors.Cyan + "Encerrando..." + ConsoleColors.Reset);
                    Environment.Exit(0);
                }
                else if (opcao == "1")
                {
                    return;
                }
                else
                {
                    Console.WriteLine(ConsoleColors.Red + "Opção inválida! Tente novamente." + ConsoleColors.Reset);
                }
            }
        }

        static string PerguntarContinuarWin()
        {
            while (true)
            {
                Console.WriteLine("\n" + new string('=', 50));
                Console.WriteLine(ConsoleColors.Cyan + "1 - Voltar ao Menu de Otimização" + ConsoleColors.Reset);
                Console.WriteLine(ConsoleColors.Cyan + "2 - Voltar para o Menu Principal" + ConsoleColors.Reset);
                Console.WriteLine(ConsoleColors.Cyan + "0 - Sair" + ConsoleColors.Reset);
                Console.Write(ConsoleColors.Yellow + "\nEscolha uma opção: " + ConsoleColors.Reset);

                string opcao = Console.ReadLine();

                if (opcao == "0")
                {
                    Console.WriteLine(ConsoleColors.Cyan + "Encerrando..." + ConsoleColors.Reset);
                    Environment.Exit(0);
                }
                else if (opcao == "1")
                {
                    return "menu_otimizacao";
                }
                else if (opcao == "2")
                {
                    return "menu_principal";
                }
                else
                {
                    Console.WriteLine(ConsoleColors.Red + "Opção inválida! Tente novamente." + ConsoleColors.Reset);
                }
            }
        }
        #endregion

        #region Funções Principais
        static void ClearDisk()
        {
            Header("As acoes a seguir podem levar algum tempo");
            var erros = new List<string>();

            DebugStep(1, "Otimizando SSD com ReTrim...");
            var improvesSSD = RunPowerShellCommand("Get-Command Optimize-Volume; Import-Module Storage; Optimize-Volume -DriveLetter C -ReTrim -Verbose");
            Console.WriteLine(improvesSSD.Output);
            if (improvesSSD.ExitCode == 0)
                DebugSuccess("SSD otimizado com sucesso");
            else
                DebugWarning("Aviso ao otimizar SSD");

            DebugStep(2, "Executando limpeza de arquivos (sagerun:1)...");
            var clearFiles01 = RunPowerShellCommand("cleanmgr /sagerun:1");
            Console.WriteLine(clearFiles01.Output);
            if (clearFiles01.ExitCode == 0)
                DebugSuccess("Limpeza 01 concluída");
            else
                DebugWarning("Aviso na limpeza 01");

            DebugStep(3, "Executando limpeza de arquivos (sagerun:2)...");
            var clearFiles02 = RunPowerShellCommand("cleanmgr /sagerun:2");
            Console.WriteLine(clearFiles02.Output);
            if (clearFiles02.ExitCode == 0)
                DebugSuccess("Limpeza 02 concluída");
            else
                DebugWarning("Aviso na limpeza 02");

            DebugStep(4, "Desfragmentando disco C:...");
            var clearDefrag = RunPowerShellCommand("defrag C: /U /V");
            Console.WriteLine(clearDefrag.Output);
            if (clearDefrag.ExitCode == 0)
                DebugSuccess("Desfragmentação concluída");
            else
                DebugWarning("Aviso na desfragmentação");

            if (!string.IsNullOrEmpty(improvesSSD.Error)) erros.Add("Melhora de SSD");
            if (!string.IsNullOrEmpty(clearFiles01.Error)) erros.Add("Limpeza de arquivos 01");
            if (!string.IsNullOrEmpty(clearFiles02.Error)) erros.Add("Limpeza de arquivos 02");
            if (!string.IsNullOrEmpty(clearDefrag.Error)) erros.Add("Desfragmentar o disco");

            if (erros.Any())
                Console.WriteLine($"Ocorreu um erro(s) ao limpar: {string.Join(", ", erros)}");
            else
                TxtInfo("Disco Limpo e Melhorado com Sucesso", "");
        }

        static void InfoMachine()
        {
            DebugStep(1, "Coletando informações do sistema...");
            
            Header("Informações do Sistema");
            TxtInfo("Nome da Máquina", Environment.MachineName);
            TxtInfo("Nome do Usuário", Environment.UserName);
            TxtInfo("Versão do Sistema Operacional", Environment.OSVersion.ToString());
            DebugSuccess("Informações do sistema coletadas");

            DebugStep(2, "Coletando informações da BIOS...");
            Header("Informações da BIOS");
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
                foreach (ManagementObject obj in searcher.Get())
                {
                    TxtInfo("Serial Number", obj["SerialNumber"]?.ToString() ?? "N/A");
                }
                DebugSuccess("Informações da BIOS coletadas");
            }
            catch (Exception e)
            {
                DebugError($"Erro ao coletar BIOS: {e.Message}");
            }

            DebugStep(3, "Coletando informações de rede...");
            Header("Informações da Placa de Rede");
            try
            {
                foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            TxtInfo($"Interface: {ni.Name}", ip.Address.ToString());
                        }
                    }
                }
                DebugSuccess("Informações de rede coletadas");
            }
            catch (Exception e)
            {
                DebugError($"Erro ao coletar rede: {e.Message}");
            }
        }

        static string ScanWin()
        {
            Header("Scan e Reparo do Windows (DISM)");

            DebugStep(1, "Verificando privilégios de administrador...");
            if (!IsAdmin())
            {
                DebugError("Este script precisa ser executado como ADMINISTRADOR!");
                DebugWarning("A limpeza de RAM requer privilégios elevados.");
                
                Console.Write(ConsoleColors.Yellow + "\nDeseja reiniciar como administrador? (s/n): " + ConsoleColors.Reset);
                string resposta = Console.ReadLine();
                if (resposta?.ToLower() == "s")
                {
                    RunAsAdmin();
                    return "Reiniciando como administrador...";
                }
                else
                {
                    DebugWarning("Continuando sem limpeza de RAM...");
                }
            }
            else
            {
                DebugSuccess("Privilégios de administrador confirmados");
            }
            
            DebugStep(2, "Iniciando DISM /RestoreHealth...");
            DebugWarning("Este processo pode levar vários minutos");
            
            var result = RunPowerShellCommand("DISM /Online /Cleanup-Image /RestoreHealth");
            Console.WriteLine(result.Output);
            
            if (result.ExitCode != 0)
            {
                DebugError($"Comando retornou código {result.ExitCode}");
                return $"Error: comando retornou código {result.ExitCode}";
            }
            else
            {
                DebugSuccess("DISM concluído com sucesso");
                return "OK";
            }
        }

        static string LimparSistema()
        {
            Header("LIMPEZA COMPLETA DO SISTEMA");
            
            DebugStep(1, "Verificando privilégios de administrador...");
            if (!IsAdmin())
            {
                DebugError("Este script precisa ser executado como ADMINISTRADOR!");
                DebugWarning("A limpeza de RAM requer privilégios elevados.");
                
                Console.Write(ConsoleColors.Yellow + "\nDeseja reiniciar como administrador? (s/n): " + ConsoleColors.Reset);
                string resposta = Console.ReadLine();
                if (resposta?.ToLower() == "s")
                {
                    RunAsAdmin();
                    return "Reiniciando como administrador...";
                }
                else
                {
                    DebugWarning("Continuando sem limpeza de RAM...");
                }
            }
            else
            {
                DebugSuccess("Privilégios de administrador confirmados");
            }
            
            var erros = new List<string>();

            // 1) Limpar Temp do Usuário
            DebugStep(2, "Limpando Temp do usuário...");
            var clearUserTemp = RunPowerShellCommand("Remove-Item -Path \"$env:TEMP\\*\" -Recurse -Force -ErrorAction SilentlyContinue");
            if (!string.IsNullOrEmpty(clearUserTemp.Error))
            {
                erros.Add("Temp do Usuário");
                DebugWarning("Aviso ao limpar Temp do usuário");
            }
            else
            {
                DebugSuccess("Temp do usuário limpo");
            }

            // 2) Limpar Temp do Sistema
            DebugStep(3, "Limpando Temp do sistema...");
            var clearSysTemp = RunPowerShellCommand("Remove-Item -Path \"$env:windir\\Temp\\*\" -Recurse -Force -ErrorAction SilentlyContinue");
            if (!string.IsNullOrEmpty(clearSysTemp.Error))
            {
                erros.Add("Temp do Sistema");
                DebugWarning("Aviso ao limpar Temp do sistema");
            }
            else
            {
                DebugSuccess("Temp do sistema limpo");
            }

            // 3) Esvaziar Lixeira
            DebugStep(4, "Esvaziando lixeira...");
            var clearEmpy = RunPowerShellCommand("Clear-RecycleBin -Force -ErrorAction SilentlyContinue");
            if (!string.IsNullOrEmpty(clearEmpy.Error))
            {
                erros.Add("Lixeira");
                DebugWarning("Aviso ao esvaziar lixeira");
            }
            else
            {
                DebugSuccess("Lixeira esvaziada");
            }

            // 4) Limpar Memória RAM com RamMap
            if (IsAdmin())
            {
                DebugStep(5, "Localizando RAMMap64.exe...");
                string rammap_path = Path.Combine("Scripts", "Apps", "RamMap", "RAMMap64.exe");
                
                if (!File.Exists(rammap_path))
                {
                    DebugError($"RAMMap não encontrado em: {rammap_path}");
                    erros.Add("RAMMap não encontrado");
                }
                else
                {
                    DebugSuccess($"RAMMap encontrado: {rammap_path}");
                    
                    DebugStep(6, "Liberando Working Sets...");
                    var emptyWorking = RunProcess(rammap_path, "-Ew");
                    if (emptyWorking.ExitCode != 0 || !string.IsNullOrEmpty(emptyWorking.Error))
                    {
                        erros.Add("Empty Working Sets");
                        DebugWarning("Aviso ao liberar Working Sets");
                    }
                    else
                    {
                        DebugSuccess("Working Sets liberados");
                    }

                    DebugStep(7, "Liberando Standby List...");
                    var emptyStandby = RunProcess(rammap_path, "-Et");
                    if (emptyStandby.ExitCode != 0 || !string.IsNullOrEmpty(emptyStandby.Error))
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

            if (erros.Any())
                return $"Ocorreu um erro ao executar: {string.Join(", ", erros)}";
            else
            {
                DebugSuccess("Limpeza completa finalizada!");
                return "Limpeza concluída com sucesso (Temp + RAM)";
            }
        }

        static void Speedtest()
        {
            // Nota: Esta função requer uma implementação de speedtest em C#
            // Você precisaria usar uma biblioteca como SpeedTest.NET ou implementar manualmente
            Header("Speed Test");
            DebugStep(1, "Executando SpeedTest...");
            DebugWarning("Funcionalidade de speedtest não implementada em C#");
            DebugWarning("Considere usar uma biblioteca como SpeedTest.NET");
        }

        static string ClearNet()
        {
            Header("LIMPEZA DE REDE");
            
            DebugStep(1, "Verificando privilégios de administrador...");
            if (!IsAdmin())
            {
                DebugError("Este script precisa ser executado como ADMINISTRADOR!");
                DebugWarning("A limpeza de rede requer privilégios elevados.");
                
                Console.Write(ConsoleColors.Yellow + "\nDeseja reiniciar como administrador? (s/n): " + ConsoleColors.Reset);
                string resposta = Console.ReadLine();
                if (resposta?.ToLower() == "s")
                {
                    RunAsAdmin();
                    return "Reiniciando como administrador...";
                }
                else
                {
                    DebugWarning("Continuando sem privilégios...");
                }
            }
            else
            {
                DebugSuccess("Privilégios de administrador confirmados");
            }

            var erros = new List<string>();

            DebugStep(2, "Limpando DNS da máquina...");
            var flushDNS = RunPowerShellCommand("ipconfig /flushdns");
            if (!string.IsNullOrEmpty(flushDNS.Error))
            {
                erros.Add("Flush DNS");
                DebugError("Erro ao limpar o DNS da máquina");
            }
            else
            {
                DebugSuccess("Limpeza do DNS realizada com sucesso!");
            }
            
            // ... continuar com os outros comandos de rede

            if (erros.Any())
                return $"Ocorreu um erro ao executar: {string.Join(", ", erros)}";
            else
            {
                DebugSuccess("Limpeza de rede concluída!");
                return "Limpeza da Rede WiFi/Ethernet concluída";
            }
        }
        #endregion

        #region Utilitários
        static (string Output, string Error, int ExitCode) RunPowerShellCommand(string command)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-Command \"{command}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                return (output, error, process.ExitCode);
            }
            catch (Exception ex)
            {
                return ("", ex.Message, -1);
            }
        }

        static (string Output, string Error, int ExitCode) RunProcess(string fileName, string arguments)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = fileName,
                        Arguments = arguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                return (output, error, process.ExitCode);
            }
            catch (Exception ex)
            {
                return ("", ex.Message, -1);
            }
        }
        #endregion

        #region Menu Principal
        static void MostrarMenu()
        {
            Console.Clear();
            
            // ASCII Art
            string asciiArt = @"
             ___    _  _    ___         ____     _____     ___    ____    _____    ____
            / __)  ( \/ )  / __)       (  _ \   (  _  )   / __)  (_  _)  (  _  )  (  _ \
            \__ \   \  /   \__ \        )(_) )   )(_)(   ( (__     )(     )(_)(    )   /
            (___/   (__)   (___/  ___  (____/   (_____)   \___)   (__)   (_____)  (_)\_)
            ";
            
            Console.WriteLine(ConsoleColors.Cyan + asciiArt + ConsoleColors.Reset);
            Console.WriteLine(new string(' ', 30) + "Windows Optimizer and Repair\n");

            string[] opcoesEsq = {
                "[ 1 ] Informação da Máquina",
                "[ 3 ] Scanner do Windows",
                "[ 5 ] SpeedTest",
                "[ 7 ] Teste de Ping",
                "[ 9 ] Otimizar Wifi",
                "[ 11 ] Verificar Temperatura",
                "[ 13 ] Criar Ponto de Restauração",
                "[ 15 ] Atualizar Windows",
            };

            string[] opcoesDir = {
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

            for (int i = 0; i < Math.Max(opcoesEsq.Length, opcoesDir.Length); i++)
            {
                string esq = i < opcoesEsq.Length ? opcoesEsq[i] : "";
                string dir = i < opcoesDir.Length ? opcoesDir[i] : "";
                Console.WriteLine($"{esq,-larguraColuna}{dir}");
            }

            Console.WriteLine("\n[ 0 ] Sair\n");
        }

        static void MainMenu()
        {
            while (true)
            {
                MostrarMenu();
                Console.Write(ConsoleColors.Yellow + "\nQual opção você deseja executar: " + ConsoleColors.Reset);
                string op = Console.ReadLine();

                switch (op)
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
                        string resultadoScan = ScanWin();
                        Console.WriteLine(ConsoleColors.Green + $"\n{resultadoScan}" + ConsoleColors.Reset);
                        PerguntarContinuar();
                        break;
                    case "4":
                        string resultadoLimpeza = LimparSistema();
                        Console.WriteLine(ConsoleColors.Green + $"\n{resultadoLimpeza}" + ConsoleColors.Reset);
                        PerguntarContinuar();
                        break;
                    case "5":
                        Speedtest();
                        PerguntarContinuar();
                        break;
                    case "6":
                        string resultadoClearNet = ClearNet();
                        Console.WriteLine(ConsoleColors.Green + $"\n{resultadoClearNet}" + ConsoleColors.Reset);
                        PerguntarContinuar();
                        break;
                    case "0":
                        Console.WriteLine(ConsoleColors.Cyan + "Encerrando..." + ConsoleColors.Reset);
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine(ConsoleColors.Red + "Opção inválida!" + ConsoleColors.Reset);
                        PerguntarContinuar();
                        break;
                }
            }
        }
        #endregion
    }
}