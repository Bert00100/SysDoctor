namespace SysDoctor.Scripts
{
    public class ClearRAM
    {
        public static void Executar()
        {
            AnsiConsole.MarkupLine("[blue]Limpando Memoria RAM[/]");
            AnsiConsole.MarkupLine("[yellow]As ações a seguir podem levar algum tempo...[/]");
            AnsiConsole.WriteLine();

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var erros = new List<string>();
                int totalPassos = 6;
                int passoAtual = 0;

                // Captura memória antes da limpeza
                var memoriaAntes = ObterMemoriaRAM();

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
                        var task = ctx.AddTask("[cyan]Limpando Memória RAM...[/]", maxValue: totalPassos);

                        // Passo 1: Limpando pasta TEMP do Usuário
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Limpando pasta TEMP do Usuário...[/]";
                        task.Value = passoAtual;
                        var clerarTempUser = ExecutarPowerShellAsync("Remove-Item -Path \"$env:TEMP\\*\" -Recurse -Force -ErrorAction SilentlyContinue", 60).Result;
                        if (clerarTempUser.exitCode == 0)
                        {
                            DebugSuccess("TEMP do Usuário limpo");
                        }
                        else
                        {
                            DebugWarning("Aviso ao limpar pasta TEMP do Usuário");
                            if (!string.IsNullOrEmpty(clerarTempUser.error)) erros.Add("TEMP Usuário");
                        }

                        // Passo 2: Limpando pasta TEMP do Sistema
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Limpando pasta TEMP do Sistema...[/]";
                        task.Value = passoAtual;
                        var clerarTempSystem = ExecutarPowerShellAsync("Remove-Item -Path \"$env:windir\\Temp\\*\" -Recurse -Force -ErrorAction SilentlyContinue", 60).Result;
                        if (clerarTempSystem.exitCode == 0)
                        {
                            DebugSuccess("TEMP do Sistema limpo");
                        }
                        else
                        {
                            DebugWarning("Aviso ao limpar pasta TEMP do Sistema");
                            if (!string.IsNullOrEmpty(clerarTempSystem.error)) erros.Add("TEMP Sistema");
                        }

                        // Passo 3: Limpando Cache do Sistema
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Limpando Cache do Sistema...[/]";
                        task.Value = passoAtual;
                        var clearSystemCache = ExecutarPowerShellAsync("[System.GC]::Collect(); [System.GC]::WaitForPendingFinalizers(); [System.GC]::Collect();", 30).Result;
                        if (clearSystemCache.exitCode == 0)
                        {
                            DebugSuccess("Cache do Sistema limpo");
                        }
                        else
                        {
                            DebugWarning("Aviso ao limpar Cache do Sistema");
                            if (!string.IsNullOrEmpty(clearSystemCache.error)) erros.Add("Cache Sistema");
                        }

                        //Passo 4: Esvaziando Lixeira
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Esvaziando Lixeira...[/]";
                        task.Value = passoAtual; 
                        var clearEmpty = ExecutarPowerShellAsync("Clear-RecycleBin -Force -ErrorAction SilentlyContinue", 30).Result;
                        if (clearEmpty.exitCode == 0)
                        {
                            DebugSuccess("Lixeira esvaziada");
                        }
                        else
                        {
                            DebugWarning("Aviso ao esvaziar Lixeira");
                            if (!string.IsNullOrEmpty(clearEmpty.error)) erros.Add("Lixeira");
                        }

                        //Passo 5: Limpando Cache de Memória
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Preparando limpeza de RAM...[/]";
                        task.Value = passoAtual;
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                        System.GC.Collect();
                        DebugSuccess("Cache de memória limpo");

                        //Passo 6: Executar RAMMap para liberar memória
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Executando RAMMap para liberar memória...[/]";
                        task.Value = passoAtual;

                        string rammapPath = Path.Combine("Scripts", "Apps", "RamMap", "RAMMap64.exe");

                        if (!File.Exists(rammapPath))
                        {
                            DebugError($"RAMMap não encontrado em: {rammapPath}");
                            erros.Add("RAMMap não encontrado");
                        }
                        else
                        {
                            DebugSuccess($"RAMMap encontrado");
                            
                            // Liberar Working Sets
                            task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Liberando Working Sets...[/]";
                            var emptyWorking = ExecutarProcesso(rammapPath, "-Ew", 30);

                            // RAMMap retorna exit code diferente de 0, mas ainda funciona
                            // Então verificamos apenas se o programa realmente falhou
                            if (emptyWorking.exitCode == -1) // Timeout ou erro crítico
                            {
                                erros.Add("Empty Working Sets");
                                DebugWarning("Aviso ao liberar Working Sets");
                            }
                            else
                            {
                                DebugSuccess("Working Sets liberados");
                            }
                            
                            // Liberar Standby List
                            task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Liberando Standby List...[/]";
                            var emptyStandby = ExecutarProcesso(rammapPath, "-Et", 30);

                            if (emptyStandby.exitCode == -1) // Timeout ou erro crítico
                            {
                                erros.Add("Empty Standby List");
                                DebugWarning("Aviso ao liberar Standby List");
                            }
                            else
                            {
                                DebugSuccess("Standby List liberada");
                            }
                        }
                        
                        task.StopTask();
                    });

                stopwatch.Stop();

                // Captura memória depois da limpeza
                var memoriaDepois = ObterMemoriaRAM();

                // Resumo final
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"[cyan]⏱️ Tempo total: {stopwatch.Elapsed.Minutes} minutos e {stopwatch.Elapsed.Seconds} segundos[/]");
                
                // Exibir estatísticas de memória
                ExibirEstatisticasMemoria(memoriaAntes, memoriaDepois);

                if (erros.Count > 0)
                {
                    AnsiConsole.MarkupLine($"[yellow]⚠️ Concluído com avisos: {string.Join(", ", erros)}[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[green]✅ Memória RAM Limpa com Sucesso![/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]💥 Erro durante a limpeza: {ex.Message}[/]");
            }
        }

        private static (double totalGB, double livreGB, double usadaGB) ObterMemoriaRAM()
        {
            try
            {
                var performanceInfo = new PerformanceInformation();
                if (GetPerformanceInfo(out performanceInfo, Marshal.SizeOf(performanceInfo)))
                {
                    double totalBytes = performanceInfo.PhysicalTotal.ToInt64() * performanceInfo.PageSize.ToInt64();
                    double livreBytes = performanceInfo.PhysicalAvailable.ToInt64() * performanceInfo.PageSize.ToInt64();
                    
                    double totalGB = totalBytes / (1024.0 * 1024.0 * 1024.0);
                    double livreGB = livreBytes / (1024.0 * 1024.0 * 1024.0);
                    double usadaGB = totalGB - livreGB;

                    return (totalGB, livreGB, usadaGB);
                }
            }
            catch { }

            return (0, 0, 0);
        }

        private static void ExibirEstatisticasMemoria((double total, double livre, double usada) antes, (double total, double livre, double usada) depois)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[cyan]📊 Estatísticas de Memória RAM:[/]");
            
            var table = new Table();
            table.AddColumn("Momento");
            table.AddColumn("RAM Total");
            table.AddColumn("RAM Livre");
            table.AddColumn("RAM Usada");
            table.AddColumn("% Usada");

            // Antes
            double percentAntes = (antes.usada / antes.total) * 100;
            string corAntes = percentAntes < 70 ? "green" : percentAntes < 85 ? "yellow" : "red";
            
            table.AddRow(
                "Antes",
                $"{antes.total:F2} GB",
                $"{antes.livre:F2} GB",
                $"[{corAntes}]{antes.usada:F2} GB[/]",
                $"[{corAntes}]{percentAntes:F1}%[/]"
            );

            // Depois
            double percentDepois = (depois.usada / depois.total) * 100;
            string corDepois = percentDepois < 70 ? "green" : percentDepois < 85 ? "yellow" : "red";
            
            table.AddRow(
                "Depois",
                $"{depois.total:F2} GB",
                $"{depois.livre:F2} GB",
                $"[{corDepois}]{depois.usada:F2} GB[/]",
                $"[{corDepois}]{percentDepois:F1}%[/]"
            );

            AnsiConsole.Write(table);

            // Memória liberada
            double memoriaLiberada = antes.usada - depois.usada;
            if (memoriaLiberada > 0)
            {
                AnsiConsole.MarkupLine($"[green]✅ Memória liberada: {memoriaLiberada:F2} GB[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[yellow]⚠️ Variação de memória: {memoriaLiberada:F2} GB[/]");
            }
        }

        // Estrutura para GetPerformanceInfo
        [StructLayout(LayoutKind.Sequential)]
        private struct PerformanceInformation
        {
            public int Size;
            public IntPtr CommitTotal;
            public IntPtr CommitLimit;
            public IntPtr CommitPeak;
            public IntPtr PhysicalTotal;
            public IntPtr PhysicalAvailable;
            public IntPtr SystemCache;
            public IntPtr KernelTotal;
            public IntPtr KernelPaged;
            public IntPtr KernelNonPaged;
            public IntPtr PageSize;
            public int HandlesCount;
            public int ProcessCount;
            public int ThreadCount;
        }

        [DllImport("psapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetPerformanceInfo(out PerformanceInformation pPerformanceInformation, int size);

        private static (int exitCode, string output, string error) ExecutarProcesso(string fileName, string arguments, int timeoutSeconds)
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    
                    var outputTask = process.StandardOutput.ReadToEndAsync();
                    var errorTask = process.StandardError.ReadToEndAsync();
                    
                    bool finished = process.WaitForExit(timeoutSeconds * 1000);
                    
                    if (!finished)
                    {
                        process.Kill();
                        return (-1, "", "Timeout excedido");
                    }

                    string output = outputTask.Result;
                    string error = errorTask.Result;
            
                    // LOG DETALHADO
                    LogDetalhesProcesso(fileName, arguments, process.ExitCode, output, error);

                    return (process.ExitCode, output, error);
                }
            }
            catch (Exception ex)
            {
                return (-1, "", ex.Message);
            }
        }

        private static void LogDetalhesProcesso(string fileName, string arguments, int exitCode, string output, string error)
        {
            AnsiConsole.MarkupLine($"[dim]📋 Comando: {Path.GetFileName(fileName)} {arguments}[/]");
            AnsiConsole.MarkupLine($"[dim]📊 Exit Code: {exitCode}[/]");
            
            if (!string.IsNullOrWhiteSpace(output))
            {
                AnsiConsole.MarkupLine($"[dim]📤 Output: {output}[/]");
            }
            
            if (!string.IsNullOrWhiteSpace(error))
            {
                AnsiConsole.MarkupLine($"[red]📥 Erro: {error}[/]");
            }
        }

        private static async Task<(int exitCode, string output, string error)> ExecutarPowerShellAsync(string comando, int timeoutSeconds)
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{comando}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    
                    var outputTask = process.StandardOutput.ReadToEndAsync();
                    var errorTask = process.StandardError.ReadToEndAsync();
                    
                    var timeoutTask = Task.Delay(timeoutSeconds * 1000);
                    var processTask = Task.Run(() => process.WaitForExit());

                    var completedTask = await Task.WhenAny(processTask, timeoutTask);

                    if (completedTask == timeoutTask)
                    {
                        process.Kill();
                        return (-1, "", "Timeout excedido");
                    }

                    string output = await outputTask;
                    string error = await errorTask;

                    return (process.ExitCode, output, error);
                }
            }
            catch (Exception ex)
            {
                return (-1, "", ex.Message);
            }
        }

        private static void DebugSuccess(string message)
        {
            AnsiConsole.MarkupLine($"[green]✅ {message}[/]");
        }

        private static void DebugWarning(string message)
        {
            AnsiConsole.MarkupLine($"[yellow]⚠️ {message}[/]");
        }

        private static void DebugError(string message)
        {
            AnsiConsole.MarkupLine($"[red]❌ {message}[/]");
        }
    }
}