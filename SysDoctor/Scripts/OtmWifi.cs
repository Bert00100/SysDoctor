namespace SysDoctor.Scripts
{
    class OtmWifi
    {
        public static void Executar()
        {
            AnsiConsole.MarkupLine("[blue]Otimizador de Wifi[/]");
            AnsiConsole.MarkupLine("[yellow]As ações a seguir podem levar algum tempo...[/]");
            AnsiConsole.WriteLine();

            Console.WriteLine("[1] - OTIMIZAR");
            Console.WriteLine("[2] - REVERTER");
            Console.Write("opcao: ");
            int.TryParse(Console.ReadLine(), out int op);

            if (op == 1)
            {
                OtimizarWifi();
            }
            else if (op == 2)
            {
                ReverterWifi();
            }
            else
            {
                Console.WriteLine("Opção inválida.");
            }
        }

        private static void OtimizarWifi()
        {
            var stopwatch = Stopwatch.StartNew();
            var erros = new List<string>();
            int totalPassos = 4;
            int passoAtual = 0;

            AnsiConsole.MarkupLine("[blue]OTIMIZANDO WI-FI[/]");

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
                    var task = ctx.AddTask("[cyan]Otimizando configurações de rede...[/]", maxValue: totalPassos);

                    // Passo 1: autotuninglevel=normal
                    passoAtual++;
                    task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Ajuste automático TCP para normal[/]";
                    task.Value = passoAtual;
                    DebugStep(passoAtual, "Definindo o nível global de ajuste automático de janela TCP para normal");
                    var autotuninglevel = ExecutarPowerShellAsync("netsh interface tcp set global autotuninglevel=normal", 30).Result;
                    if (!string.IsNullOrWhiteSpace(autotuninglevel.error))
                    {
                        DebugError("Erro ao definir autotuninglevel=normal");
                        erros.Add("autotuninglevel");
                    }
                    else
                    {
                        DebugSuccess("Definição bem sucedida!");
                    }

                    // Passo 2: rss=enabled
                    passoAtual++;
                    task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Ativando RSS[/]";
                    task.Value = passoAtual;
                    DebugStep(passoAtual, "Ativando Receive Side Scaling (RSS)");
                    var rss = ExecutarPowerShellAsync("netsh interface tcp set global rss=enabled", 30).Result;
                    if (!string.IsNullOrWhiteSpace(rss.error))
                    {
                        DebugError("Erro ao ativar RSS");
                        erros.Add("receiveSideScaling");
                    }
                    else
                    {
                        DebugSuccess("Ativação bem sucedida!");
                    }

                    // Passo 3: chimney=disabled
                    passoAtual++;
                    task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Desativando TCP Chimney[/]";
                    task.Value = passoAtual;
                    DebugStep(passoAtual, "Desativando TCP Chimney Offload");
                    var chimney = ExecutarPowerShellAsync("netsh interface tcp set global chimney=disabled", 30).Result;
                    if (!string.IsNullOrWhiteSpace(chimney.error))
                    {
                        DebugError("Erro ao desativar TCP Chimney Offload");
                        erros.Add("offloadTCPChimney");
                    }
                    else
                    {
                        DebugSuccess("Desativação bem sucedida!");
                    }

                    // Passo 4: heuristics=disabled
                    passoAtual++;
                    task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Desativando heurísticas TCP[/]";
                    task.Value = passoAtual;
                    DebugStep(passoAtual, "Desativando heurísticas TCP");
                    var heuristics = ExecutarPowerShellAsync("netsh int tcp set heuristics disabled", 30).Result;
                    if (!string.IsNullOrWhiteSpace(heuristics.error))
                    {
                        DebugError("Erro ao desativar heurísticas TCP");
                        erros.Add("heuristics");
                    }
                    else
                    {
                        DebugSuccess("Desativação bem sucedida!");
                    }

                    task.StopTask();
                });

            stopwatch.Stop();
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[cyan]⏱️ Tempo total: {stopwatch.Elapsed:mm\\:ss}[/]");

            if (erros.Count > 0)
            {
                AnsiConsole.MarkupLine($"[yellow]⚠️ Ocorreu um erro ao executar: {string.Join(", ", erros)}[/]");
            }
            else
            {
                DebugSuccess("Otimização Finalizada!");
                AnsiConsole.MarkupLine("[green]As configurações de rede foram alteradas para otimização do Wi-Fi.[/]");
            }
        }

        private static void ReverterWifi()
        {
            var stopwatch = Stopwatch.StartNew();
            var erros = new List<string>();
            int totalPassos = 4;
            int passoAtual = 0;

            AnsiConsole.MarkupLine("[blue]REVERTENDO WI-FI PARA PADRÃO[/]");

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
                    var task = ctx.AddTask("[cyan]Revertendo configurações de rede...[/]", maxValue: totalPassos);

                    // Passo 1: autotuninglevel=restricted
                    passoAtual++;
                    task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Ajuste automático TCP para restricted[/]";
                    task.Value = passoAtual;
                    DebugStep(passoAtual, "Revertendo autotuninglevel para restricted");
                    var autotuninglevel = ExecutarPowerShellAsync("netsh interface tcp set global autotuninglevel=restricted", 30).Result;
                    if (!string.IsNullOrWhiteSpace(autotuninglevel.error))
                    {
                        DebugError("Erro ao reverter autotuninglevel");
                        erros.Add("autotuninglevel");
                    }
                    else
                    {
                        DebugSuccess("Reversão bem sucedida!");
                    }

                    // Passo 2: rss=disabled
                    passoAtual++;
                    task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Desativando RSS[/]";
                    task.Value = passoAtual;
                    DebugStep(passoAtual, "Desativando Receive Side Scaling (RSS)");
                    var rss = ExecutarPowerShellAsync("netsh interface tcp set global rss=disabled", 30).Result;
                    if (!string.IsNullOrWhiteSpace(rss.error))
                    {
                        DebugError("Erro ao desativar RSS");
                        erros.Add("receiveSideScaling");
                    }
                    else
                    {
                        DebugSuccess("Desativação bem sucedida!");
                    }

                    // Passo 3: chimney=enabled
                    passoAtual++;
                    task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Ativando TCP Chimney[/]";
                    task.Value = passoAtual;
                    DebugStep(passoAtual, "Ativando TCP Chimney Offload");
                    var chimney = ExecutarPowerShellAsync("netsh interface tcp set global chimney=enabled", 30).Result;
                    if (!string.IsNullOrWhiteSpace(chimney.error))
                    {
                        DebugError("Erro ao ativar TCP Chimney Offload");
                        erros.Add("globalChimney");
                    }
                    else
                    {
                        DebugSuccess("Ativação bem sucedida!");
                    }

                    // Passo 4: heuristics=enabled
                    passoAtual++;
                    task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Ativando heurísticas TCP[/]";
                    task.Value = passoAtual;
                    DebugStep(passoAtual, "Ativando heurísticas TCP");
                    var heuristics = ExecutarPowerShellAsync("netsh int tcp set heuristics enabled", 30).Result;
                    if (!string.IsNullOrWhiteSpace(heuristics.error))
                    {
                        DebugError("Erro ao ativar heurísticas TCP");
                        erros.Add("heuristics");
                    }
                    else
                    {
                        DebugSuccess("Ativação bem sucedida!");
                    }

                    task.StopTask();
                });

            stopwatch.Stop();
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[cyan]⏱️ Tempo total: {stopwatch.Elapsed:mm\\:ss}[/]");

            if (erros.Count > 0)
            {
                AnsiConsole.MarkupLine($"[yellow]⚠️ Ocorreu um erro ao executar: {string.Join(", ", erros)}[/]");
            }
            else
            {
                DebugSuccess("Reversão Finalizada!");
                AnsiConsole.MarkupLine("[green]As configurações de rede foram revertidas para o padrão do Windows.[/]");
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
                        Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{comando}\"",
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
                    return (0, output, "Timeout - Processo demorou muito", true);
                }

                return (process.ExitCode, output, error, false);
            }
            catch (Exception ex)
            {
                return (-1, "", ex.Message, false);
            }
        }

        private static void DebugStep(int passo, string mensagem)
        {
            AnsiConsole.MarkupLine($"[cyan]Passo {passo}: {mensagem}[/]");
        }

        private static void DebugSuccess(string mensagem)
        {
            AnsiConsole.MarkupLine($"[green]   ✅ {mensagem}[/]");
        }

        private static void DebugError(string mensagem)
        {
            AnsiConsole.MarkupLine($"[red]   ❌ {mensagem}[/]");
        }
    }
}