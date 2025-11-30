namespace SysDoctor.Scripts
{
    class checkTemperature
    {
       public static void Executar()
        {
            AnsiConsole.MarkupLine("[blue]Verificar Temperatura[/]");
            AnsiConsole.WriteLine();

            var stopwatch = Stopwatch.StartNew();
            var erros = new List<string>();
            string hardwareMonitorPath = null;

            try
            {
                // Busca o arquivo OpenHardwareMonitor.exe em todas as subpastas de HardwareMonitor
                string pastaHardwareMonitor = Path.Combine("Scripts", "Apps", "HardwareMonitor");
                string[] encontrados = Directory.Exists(pastaHardwareMonitor)
                    ? Directory.GetFiles(pastaHardwareMonitor, "OpenHardwareMonitor.exe", SearchOption.AllDirectories)
                    : Array.Empty<string>();

                if (encontrados.Length == 0)
                {
                    DebugWarning("Hardware Monitor não encontrado em nenhuma subpasta de Scripts/Apps/HardwareMonitor.");
                    erros.Add("HARDWARE MONITOR não encontrado");
                }
                else
                {
                    hardwareMonitorPath = Path.GetFullPath(encontrados[0]);
                    var fileInfo = new FileInfo(hardwareMonitorPath);

                    // Se estiver oculto, remove o atributo oculto
                    if (fileInfo.Attributes.HasFlag(FileAttributes.Hidden))
                    {
                        try
                        {
                            fileInfo.Attributes &= ~FileAttributes.Hidden;
                            DebugSuccess("Atributo oculto removido de OpenHardwareMonitor.exe.");
                        }
                        catch (Exception ex)
                        {
                            DebugWarning($"Não foi possível remover atributo oculto: {ex.Message}");
                        }
                    }

                    DebugSuccess($"Hardware Monitor localizado: {hardwareMonitorPath}");
                }

                stopwatch.Stop();
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"[cyan]⏱️ Tempo de preparação: {stopwatch.Elapsed.Seconds} segundos[/]");
                AnsiConsole.WriteLine();

                if (erros.Count > 0)
                {
                    AnsiConsole.MarkupLine($"[red]❌ Ocorreu um erro ao executar: {string.Join(", ", erros)}[/]");
                    AnsiConsole.MarkupLine("[cyan]💡 Dica:[/]");
                    AnsiConsole.MarkupLine("[dim]  1. Baixe o OpenHardwareMonitor de: https://openhardwaremonitor.org/downloads/[/]");
                    AnsiConsole.MarkupLine($"[dim]  2. Coloque o OpenHardwareMonitor.exe em alguma subpasta de: {Path.GetFullPath(pastaHardwareMonitor)}[/]");
                }
                else
                {
                    ExecutarHardwareMonitor(hardwareMonitorPath);
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]💥 Erro durante a verificação de temperatura: {ex.Message}[/]");
            }
        }

        private static void ExecutarHardwareMonitor(string caminhoExecutavel)
        {
            try
            {
                if (string.IsNullOrEmpty(caminhoExecutavel) || !File.Exists(caminhoExecutavel))
                {
                    AnsiConsole.MarkupLine("[red]❌ Não foi possível localizar o OpenHardwareMonitor.[/]");
                    return;
                }

                DebugSuccess($"Iniciando OpenHardwareMonitor: {Path.GetFileName(caminhoExecutavel)}");
                AnsiConsole.MarkupLine("[yellow]⚠️  Aguardando o fechamento do OpenHardwareMonitor...[/]");
                AnsiConsole.WriteLine();

                var processStartInfo = new ProcessStartInfo
                {
                    FileName = caminhoExecutavel,
                    UseShellExecute = true,
                    WorkingDirectory = Path.GetDirectoryName(caminhoExecutavel),
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Normal
                };

                using (var process = Process.Start(processStartInfo))
                {
                    if (process != null)
                    {
                        process.WaitForExit();
                        AnsiConsole.WriteLine();
                        DebugSuccess("Verificação finalizada!");
                        AnsiConsole.MarkupLine("[green]✅ OpenHardwareMonitor foi fechado.[/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]❌ Não foi possível iniciar o OpenHardwareMonitor.[/]");
                    }
                }
            }
            catch (System.ComponentModel.Win32Exception winEx)
            {
                if (winEx.NativeErrorCode == 1223)
                {
                    AnsiConsole.MarkupLine("[yellow]⚠️  Execução cancelada pelo usuário.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]❌ Erro ao executar OpenHardwareMonitor: {winEx.Message}[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Erro ao executar OpenHardwareMonitor: {ex.Message}[/]");
            }
        }

        private static void DebugSuccess(string mensagem)
        {
            AnsiConsole.MarkupLine($"[green]   ✅ {mensagem}[/]");
        }

        private static void DebugWarning(string mensagem)
        {
            AnsiConsole.MarkupLine($"[yellow]   ⚠️  {mensagem}[/]");
        }
    }
}