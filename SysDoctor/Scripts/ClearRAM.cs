namespace SysDoctor.Scripts
{
    public class ClearRAM
    {
        public static async Task Executar()
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

                await AnsiConsole.Progress()
                    .AutoClear(false)
                    .Columns(new ProgressColumn[]
                    {
                        new TaskDescriptionColumn(),
                        new ProgressBarColumn(),
                        new PercentageColumn(),
                        new ElapsedTimeColumn(),
                    })
                    .StartAsync(async ctx =>
                    {
                        var task = ctx.AddTask("[cyan]Limpando Memória RAM...[/]", maxValue: totalPassos);

                        // Passo 1: Limpando pasta TEMP do Usuário
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Limpando pasta TEMP do Usuário...[/]";
                        task.Value = passoAtual;
                        var clerarTempUser = await ExecutarPowerShellAsync("Remove-Item -Path \"$env:TEMP\\*\" -Recurse -Force -ErrorAction SilentlyContinue", 60);
                        if (clerarTempUser.exitCode == 0 && string.IsNullOrEmpty(clerarTempUser.error))
                        {
                            DebugSuccess("TEMP do Usuário limpo");
                        }
                        else if (!string.IsNullOrEmpty(clerarTempUser.error))
                        {
                            DebugWarning("Aviso ao limpar pasta TEMP do Usuário");
                            erros.Add("TEMP Usuário");
                        }
                        else
                        {
                            DebugSuccess("TEMP do Usuário limpo");
                        }

                        // Passo 2: Limpando pasta TEMP do Sistema
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Limpando pasta TEMP do Sistema...[/]";
                        task.Value = passoAtual;
                        var clerarTempSystem = await ExecutarPowerShellAsync("Remove-Item -Path \"$env:windir\\Temp\\*\" -Recurse -Force -ErrorAction SilentlyContinue", 60);
                        if (clerarTempSystem.exitCode == 0 && string.IsNullOrEmpty(clerarTempSystem.error))
                        {
                            DebugSuccess("TEMP do Sistema limpo");
                        }
                        else if (!string.IsNullOrEmpty(clerarTempSystem.error))
                        {
                            DebugWarning("Aviso ao limpar pasta TEMP do Sistema");
                            erros.Add("TEMP Sistema");
                        }
                        else
                        {
                            DebugSuccess("TEMP do Sistema limpo");
                        }

                        // Passo 3: Limpando Cache do Sistema
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Limpando Cache do Sistema...[/]";
                        task.Value = passoAtual;
                        var clearSystemCache = await ExecutarPowerShellAsync("[System.GC]::Collect(); [System.GC]::WaitForPendingFinalizers(); [System.GC]::Collect();", 30);
                        if (clearSystemCache.exitCode == 0)
                        {
                            DebugSuccess("Cache do Sistema limpo");
                        }
                        else
                        {
                            DebugWarning("Aviso ao limpar Cache do Sistema");
                            if (!string.IsNullOrEmpty(clearSystemCache.error)) erros.Add("Cache Sistema");
                        }

                        // Passo 4: Esvaziando Lixeira
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Esvaziando Lixeira...[/]";
                        task.Value = passoAtual;
                        var clearEmpty = await ExecutarPowerShellAsync("Clear-RecycleBin -Force -ErrorAction SilentlyContinue", 30);
                        if (clearEmpty.exitCode == 0)
                        {
                            DebugSuccess("Lixeira esvaziada");
                        }
                        else
                        {
                            DebugWarning("Aviso ao esvaziar Lixeira");
                            if (!string.IsNullOrEmpty(clearEmpty.error)) erros.Add("Lixeira");
                        }

                        // Passo 5: Limpando Cache de Memória
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Preparando limpeza de RAM...[/]";
                        task.Value = passoAtual;
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                        System.GC.Collect();
                        DebugSuccess("Cache de memória limpo");

                        // Passo 6: Executar RAMMap para liberar memória
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Verificando RAMMap...[/]";
                        task.Value = passoAtual;

                        string rammapPath = Path.Combine("Scripts", "Apps", "RamMap", "RAMMap64.exe");

                        if (!File.Exists(rammapPath))
                        {
                            DebugError($"RAMMap não encontrado em: {rammapPath}");
                            erros.Add("RAMMap não encontrado");
                        }
                        else
                        {
                            DebugSuccess("RAMMap encontrado");
                            task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Preparando RAMMap...[/]";
                        }

                        // Completa a barra de progresso
                        task.Value = totalPassos;
                    });

                // Após a barra de progresso, executa o RAMMap se existir
                string rammapPath = Path.Combine("Scripts", "Apps", "RamMap", "RAMMap64.exe");
                if (File.Exists(rammapPath))
                {
                    await ExecutarRAMMap(rammapPath, erros);
                }

                stopwatch.Stop();

                // Captura memória depois da limpeza
                var memoriaDepois = ObterMemoriaRAM();

                // Limpa a tela antes do resumo final
                AnsiConsole.Clear();

                // Resumo final
                AnsiConsole.MarkupLine("[blue]═══════════════════════════════════════════[/]");
                AnsiConsole.MarkupLine("[blue]       LIMPEZA DE MEMÓRIA RAM CONCLUÍDA    [/]");
                AnsiConsole.MarkupLine("[blue]═══════════════════════════════════════════[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"[cyan]⏱️  Tempo total: {stopwatch.Elapsed.Minutes} minutos e {stopwatch.Elapsed.Seconds} segundos[/]");

                // Exibir estatísticas de memória
                ExibirEstatisticasMemoria(memoriaAntes, memoriaDepois);

                AnsiConsole.WriteLine();
                if (erros.Count > 0)
                {
                    AnsiConsole.MarkupLine($"[yellow]⚠️  Concluído com avisos: {string.Join(", ", erros)}[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[green]✅ Memória RAM Limpa com Sucesso![/]");
                }

                // REMOVIDO: Console.ReadLine() daqui - será tratado no MenuPrincipal
            }
            catch (Exception ex)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"[red]💥 Erro durante a limpeza: {ex.Message}[/]");
                // REMOVIDO: Console.ReadLine() daqui também
            }
        }

        private static async Task ExecutarRAMMap(string rammapPath, List<string> erros)
        {
            try
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[yellow]═══════════════════════════════════════════[/]");
                AnsiConsole.MarkupLine("[yellow]        ⚠️  ATENÇÃO: RAMMAP ABERTO        [/]");
                AnsiConsole.MarkupLine("[yellow]═══════════════════════════════════════════[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[cyan]Para liberar memória no RAMMap:[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[green]  1. Clique em 'Empty' ➜ 'Empty Working Sets'[/]");
                AnsiConsole.MarkupLine("[green]  2. Clique em 'Empty' ➜ 'Empty Standby List'[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[cyan]Após concluir, feche o RAMMap para continuar...[/]");
                AnsiConsole.WriteLine();

                var rammapProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = rammapPath,
                        Arguments = "-accepteula",
                        UseShellExecute = true,
                        Verb = "runas"
                    }
                };

                rammapProcess.Start();
                DebugSuccess("RAMMap aberto - Aguardando conclusão...");

                // Aguardar o usuário fechar o RAMMap
                await Task.Run(() => rammapProcess.WaitForExit());

                DebugSuccess("RAMMap fechado pelo usuário");
                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                erros.Add("RAMMap");
                DebugWarning($"Falha ao executar o RAMMap: {ex.Message}");
                await Task.Delay(2000);
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
            AnsiConsole.WriteLine();

            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn(new TableColumn("Momento").Centered());
            table.AddColumn(new TableColumn("RAM Total").Centered());
            table.AddColumn(new TableColumn("RAM Livre").Centered());
            table.AddColumn(new TableColumn("RAM Usada").Centered());
            table.AddColumn(new TableColumn("% Usada").Centered());

            // Antes
            double percentAntes = (antes.usada / antes.total) * 100;
            string corAntes = percentAntes < 70 ? "green" : percentAntes < 85 ? "yellow" : "red";

            table.AddRow(
                "[bold]Antes[/]",
                $"{antes.total:F2} GB",
                $"{antes.livre:F2} GB",
                $"[{corAntes}]{antes.usada:F2} GB[/]",
                $"[{corAntes}]{percentAntes:F1}%[/]"
            );

            // Depois
            double percentDepois = (depois.usada / depois.total) * 100;
            string corDepois = percentDepois < 70 ? "green" : percentDepois < 85 ? "yellow" : "red";

            table.AddRow(
                "[bold]Depois[/]",
                $"{depois.total:F2} GB",
                $"{depois.livre:F2} GB",
                $"[{corDepois}]{depois.usada:F2} GB[/]",
                $"[{corDepois}]{percentDepois:F1}%[/]"
            );

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();

            // Memória liberada
            double memoriaLiberada = antes.usada - depois.usada;
            if (memoriaLiberada > 0)
            {
                AnsiConsole.MarkupLine($"[green]✅ Memória liberada: {memoriaLiberada:F2} GB ({(memoriaLiberada / antes.usada * 100):F1}% da memória usada)[/]");
            }
            else if (memoriaLiberada < 0)
            {
                AnsiConsole.MarkupLine($"[yellow]⚠️  Uso de memória aumentou: {Math.Abs(memoriaLiberada):F2} GB[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[yellow]⚠️  Sem alteração significativa na memória[/]");
            }
        }

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
            AnsiConsole.MarkupLine($"[yellow]⚠️  {message}[/]");
        }

        private static void DebugError(string message)
        {
            AnsiConsole.MarkupLine($"[red]❌ {message}[/]");
        }
    }
}