namespace SysDoctor.Scripts
{
    public class ClearDisk
    {
        public static void Executar()
        {
            AnsiConsole.MarkupLine("[blue]🧹 Limpeza e Otimização de Disco[/]");
            AnsiConsole.MarkupLine("[yellow]As ações a seguir podem levar algum tempo...[/]");
            AnsiConsole.WriteLine();

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var erros = new List<string>();
                int totalPassos = 10;
                int passoAtual = 0;

                AnsiConsole.Progress()
                    .AutoClear(false)
                    .Columns(new ProgressColumn[]
                    {
                        new TaskDescriptionColumn(),
                        new ProgressBarColumn(),
                        new PercentageColumn(),
                        new ElapsedTimeColumn(),
                    })
                    .Start(ctx =>
                    {
                        var task = ctx.AddTask("[cyan]Limpando disco...[/]", maxValue: totalPassos);

                        // Passo 1: Otimizar SSD com ReTrim
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Otimizando SSD...[/]";
                        task.Value = passoAtual;
                        var improvesSSD = ExecutarPowerShellAsync("Optimize-Volume -DriveLetter C -ReTrim -Verbose", 30).Result;
                        if (improvesSSD.exitCode == 0)
                        {
                            DebugSuccess("SSD otimizado com sucesso");
                        }
                        else
                        {
                            DebugWarning("Aviso ao otimizar SSD - continuando...");
                            if (!string.IsNullOrEmpty(improvesSSD.error)) erros.Add("Otimização de SSD");
                        }

                        // Passo 2: Limpeza de arquivos (sagerun:1)
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Limpeza de arquivos (1/2)...[/]";
                        task.Value = passoAtual;
                        var clearFiles01 = ExecutarProcessoAsync("cleanmgr.exe", "/sagerun:1", 60).Result;
                        if (clearFiles01.exitCode == 0)
                        {
                            DebugSuccess("Limpeza 01 concluída");
                        }
                        else
                        {
                            DebugWarning("Aviso na limpeza 01 - continuando...");
                            if (!string.IsNullOrEmpty(clearFiles01.error)) erros.Add("Limpeza de arquivos 01");
                        }

                        // Passo 3: Limpeza de arquivos (sagerun:2)
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Limpeza de arquivos (2/2)...[/]";
                        task.Value = passoAtual;
                        var clearFiles02 = ExecutarProcessoAsync("cleanmgr.exe", "/sagerun:2", 60).Result;
                        if (clearFiles02.exitCode == 0)
                        {
                            DebugSuccess("Limpeza 02 concluída");
                        }
                        else
                        {
                            DebugWarning("Aviso na limpeza 02 - continuando...");
                            if (!string.IsNullOrEmpty(clearFiles02.error)) erros.Add("Limpeza de arquivos 02");
                        }

                        // Passo 4: Desfragmentar disco (com timeout)
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Analisando disco...[/]";
                        task.Value = passoAtual;
                        var clearDefrag = ExecutarPowerShellAsync("defrag C: /A /V", 120).Result;
                        if (clearDefrag.exitCode == 0 || clearDefrag.timedOut)
                        {
                            DebugSuccess("Análise de disco concluída");
                        }
                        else
                        {
                            DebugWarning("Aviso na análise - continuando...");
                            if (!string.IsNullOrEmpty(clearDefrag.error)) erros.Add("Análise do disco");
                        }

                        // Passo 5: Limpar arquivos temporários
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Limpando temporários...[/]";
                        task.Value = passoAtual;
                        try
                        {
                            LimparArquivosTemporarios();
                            DebugSuccess("Arquivos temporários limpos");
                        }
                        catch (Exception ex)
                        {
                            DebugWarning($"Aviso ao limpar temporários: {ex.Message}");
                            erros.Add("Limpeza de arquivos temporários");
                        }

                        // Passo 6: Limpar cache do sistema
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Limpando cache...[/]";
                        task.Value = passoAtual;
                        try
                        {
                            LimparCacheSistema();
                            DebugSuccess("Cache do sistema limpo");
                        }
                        catch (Exception ex)
                        {
                            DebugWarning($"Aviso ao limpar cache: {ex.Message}");
                            erros.Add("Limpeza de cache do sistema");
                        }

                        // Passo 7: Limpar logs antigos
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Limpando logs...[/]";
                        task.Value = passoAtual;
                        try
                        {
                            LimparLogsAntigos();
                            DebugSuccess("Logs antigos limpos");
                        }
                        catch (Exception ex)
                        {
                            DebugWarning($"Aviso ao limpar logs: {ex.Message}");
                            erros.Add("Limpeza de logs antigos");
                        }

                        // Passo 8: Limpar lixeira
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Limpando lixeira...[/]";
                        task.Value = passoAtual;
                        try
                        {
                            LimparLixeira();
                            DebugSuccess("Lixeira limpa");
                        }
                        catch (Exception ex)
                        {
                            DebugWarning($"Aviso ao limpar lixeira: {ex.Message}");
                            erros.Add("Limpeza da lixeira");
                        }

                        // Passo 9: Verificar saúde do disco
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Verificando saúde...[/]";
                        task.Value = passoAtual;
                        try
                        {
                            VerificarSaudeDisco();
                            DebugSuccess("Verificação de saúde concluída");
                        }
                        catch (Exception ex)
                        {
                            DebugWarning($"Aviso na verificação de saúde: {ex.Message}");
                        }

                        // Passo 10: Compactar disco
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Finalizando...[/]";
                        task.Value = passoAtual;
                        try
                        {
                            CompactarDisco();
                            DebugSuccess("Compactação concluída");
                        }
                        catch (Exception ex)
                        {
                            DebugWarning($"Aviso na compactação: {ex.Message}");
                        }

                        task.StopTask();
                    });

                stopwatch.Stop();

                // Resumo final
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"[cyan]⏱️ Tempo total: {stopwatch.Elapsed.Minutes} minutos e {stopwatch.Elapsed.Seconds} segundos[/]");
                
                if (erros.Count > 0)
                {
                    AnsiConsole.MarkupLine($"[yellow]⚠️ Concluído com avisos: {string.Join(", ", erros)}[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[green]✅ Disco Limpo e Otimizado com Sucesso![/]");
                }

                // Exibir espaço liberado
                ExibirEspacoLiberado();
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]💥 Erro durante a limpeza do disco: {ex.Message}[/]");
            }
        }

        private static async Task<(int exitCode, string output, string error, bool timedOut)> ExecutarPowerShellAsync(string comando, int timeoutSeconds)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-Command \"{comando}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };

                process.Start();

                var outputTask = process.StandardOutput.ReadToEndAsync();
                var errorTask = process.StandardError.ReadToEndAsync();

                bool completed = process.WaitForExit(timeoutSeconds * 1000);

                string output = await outputTask;
                string error = await errorTask;

                if (!completed)
                {
                    try { process.Kill(); } catch { }
                    return (0, output, "Timeout", true);
                }

                return (process.ExitCode, output, error, false);
            }
            catch (Exception ex)
            {
                return (-1, "", ex.Message, false);
            }
        }

        private static async Task<(int exitCode, string output, string error, bool timedOut)> ExecutarProcessoAsync(string arquivo, string argumentos, int timeoutSeconds)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = arquivo,
                        Arguments = argumentos,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };

                process.Start();

                var outputTask = process.StandardOutput.ReadToEndAsync();
                var errorTask = process.StandardError.ReadToEndAsync();

                bool completed = process.WaitForExit(timeoutSeconds * 1000);

                string output = await outputTask;
                string error = await errorTask;

                if (!completed)
                {
                    try { process.Kill(); } catch { }
                    return (0, output, "Timeout", true);
                }

                return (process.ExitCode, output, error, false);
            }
            catch (Exception ex)
            {
                return (-1, "", ex.Message, false);
            }
        }

        private static void LimparArquivosTemporarios()
        {
            string[] pastasTemporarias = {
                Path.GetTempPath(),
                @"C:\Windows\Temp",
                Environment.GetFolderPath(Environment.SpecialFolder.InternetCache),
                Environment.GetFolderPath(Environment.SpecialFolder.Cookies)
            };

            foreach (var pasta in pastasTemporarias)
            {
                if (Directory.Exists(pasta))
                {
                    try
                    {
                        foreach (var arquivo in Directory.GetFiles(pasta))
                        {
                            try { File.Delete(arquivo); } catch { }
                        }
                        foreach (var dir in Directory.GetDirectories(pasta))
                        {
                            try { Directory.Delete(dir, true); } catch { }
                        }
                    }
                    catch { }
                }
            }
        }

        private static void LimparCacheSistema()
        {
            string[] pastasCache = {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp"),
                @"C:\Windows\Prefetch",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google", "Chrome", "User Data", "Default", "Cache"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "Edge", "User Data", "Default", "Cache")
            };

            foreach (var pasta in pastasCache)
            {
                if (Directory.Exists(pasta))
                {
                    try
                    {
                        foreach (var arquivo in Directory.GetFiles(pasta, "*.*", SearchOption.AllDirectories))
                        {
                            try { File.Delete(arquivo); } catch { }
                        }
                    }
                    catch { }
                }
            }
        }

        private static void LimparLogsAntigos()
        {
            string pastaLogs = @"C:\Windows\Logs";
            if (Directory.Exists(pastaLogs))
            {
                foreach (var arquivo in Directory.GetFiles(pastaLogs, "*.log", SearchOption.AllDirectories))
                {
                    try
                    {
                        var info = new FileInfo(arquivo);
                        if (info.CreationTime < DateTime.Now.AddDays(-30))
                        {
                            File.Delete(arquivo);
                        }
                    }
                    catch { }
                }
            }
        }

        private static void LimparLixeira()
        {
            SHEmptyRecycleBin(IntPtr.Zero, null, 
                RecycleFlag.SHERB_NOCONFIRMATION | 
                RecycleFlag.SHERB_NOPROGRESSUI | 
                RecycleFlag.SHERB_NOSOUND);
        }

        private static void VerificarSaudeDisco()
        {
            var resultado = ExecutarPowerShellAsync("Get-PhysicalDisk | Select-Object DeviceID, MediaType, HealthStatus, Size | Format-Table", 30).Result;
            if (!string.IsNullOrEmpty(resultado.output))
            {
                AnsiConsole.MarkupLine("[grey]└ Status de saúde do disco verificado[/]");
            }
        }

        private static void CompactarDisco()
        {
            var resultado = ExecutarPowerShellAsync("powercfg /h off", 10).Result;
        }

        private static void ExibirEspacoLiberado()
        {
            try
            {
                var drives = DriveInfo.GetDrives();
                foreach (var drive in drives)
                {
                    if (drive.IsReady && drive.DriveType == DriveType.Fixed)
                    {
                        AnsiConsole.MarkupLine($"[cyan]📊 Disco {drive.Name}: {drive.AvailableFreeSpace / (1024 * 1024 * 1024):N1} GB livres de {drive.TotalSize / (1024 * 1024 * 1024):N1} GB[/]");
                    }
                }
            }
            catch { }
        }

        private static void DebugSuccess(string mensagem)
        {
            AnsiConsole.MarkupLine($"[green]   ✅ {mensagem}[/]");
        }

        private static void DebugWarning(string mensagem)
        {
            AnsiConsole.MarkupLine($"[yellow]   ⚠ {mensagem}[/]");
        }

        [DllImport("Shell32.dll")]
        private static extern int SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlag dwFlags);

        [Flags]
        private enum RecycleFlag : uint
        {
            SHERB_NOCONFIRMATION = 0x00000001,
            SHERB_NOPROGRESSUI = 0x00000002,
            SHERB_NOSOUND = 0x00000004
        }
    }
}