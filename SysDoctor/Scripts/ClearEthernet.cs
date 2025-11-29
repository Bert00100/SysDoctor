namespace SysDoctor.Scripts
{
    class ClearEthernet
    {
        public static void Executar()
        {
            AnsiConsole.MarkupLine("[blue]Limpando Cache Ethernet/Wifi[/]");
            AnsiConsole.MarkupLine("[yellow]As ações a seguir podem levar algum tempo...[/]");
            AnsiConsole.WriteLine();

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var erros = new List<string>();
                int totalPassos = 8; // Total de comandos
                int passoAtual = 0; // Comando atual

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
                        // Início do progresso
                        var task = ctx.AddTask("[cyan]Limpando Cache Ethernet/Wifi...[/]", maxValue: totalPassos);

                        // Passo 1: Limpando DNS com ipconfig /flushdns
                        passoAtual++;
                        ExecutarComando("ipconfig", "/flushdns", "Limpando Cache DNS", erros, task, passoAtual, totalPassos);

                        // Passo 2: Limpando DNS com Clear-DnsClientCache
                        passoAtual++;
                        ExecutarComandoPowerShell("Clear-DnsClientCache", "Limpando Cache DNS (PowerShell)", erros, task, passoAtual, totalPassos);

                        // Passo 3: Re-registrando DNS
                        passoAtual++;
                        ExecutarComando("ipconfig", "/registerdns", "Re-Registrando DNS", erros, task, passoAtual, totalPassos);

                        // Passo 4: Liberando IP
                        passoAtual++;
                        ExecutarComando("ipconfig", "/release", "Liberando IP", erros, task, passoAtual, totalPassos);

                        // Passo 5: Renovando IP
                        passoAtual++;
                        ExecutarComando("ipconfig", "/renew", "Renovando IP", erros, task, passoAtual, totalPassos);

                        // Passo 6: Resetando WinSock
                        passoAtual++;
                        ExecutarComando("netsh", "winsock reset", "Resetando WinSock", erros, task, passoAtual, totalPassos);

                        // Passo 7: Resetando TCP/IP
                        passoAtual++;
                        ExecutarComando("netsh", "int ip reset", "Resetando TCP/IP", erros, task, passoAtual, totalPassos);

                        // Passo 8: Limpando Cache ARP
                        passoAtual++;
                        ExecutarComando("arp", "-d *", "Limpando Cache ARP", erros, task, passoAtual, totalPassos);

                        task.StopTask();
                    });

                stopwatch.Stop();

                // Resumo final
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"[cyan]⏱️ Tempo total: {stopwatch.Elapsed.Minutes} minutos e {stopwatch.Elapsed.Seconds} segundos[/]");

                if (erros.Count > 0)
                {
                    AnsiConsole.MarkupLine($"[yellow]⚠️ Concluído com {erros.Count} aviso(s). Algumas operações podem não ter sido concluídas.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[green]✅ Cache Limpo e Otimizado com Sucesso![/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]💥 Erro durante a limpeza do Ethernet: {ex.Message}[/]");
            }
        }

        private static void ExecutarComando(string comando, string argumentos, string descricao, List<string> erros, ProgressTask task, int passoAtual, int totalPassos)
        {
            task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: {descricao}...[/]";
            task.Value = passoAtual;

            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = comando,
                        Arguments = argumentos,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };

                process.Start();
                
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                
                process.WaitForExit(30000); // 30 segundos timeout

                if (process.ExitCode == 0 || string.IsNullOrWhiteSpace(error))
                {
                    DebugSuccess($"{descricao} concluído com sucesso");
                }
                else
                {
                    DebugWarning($"Aviso ao executar {descricao}: {error}");
                    erros.Add(descricao);
                }
            }
            catch (Exception ex)
            {
                DebugWarning($"Erro ao executar {descricao}: {ex.Message}");
                erros.Add(descricao);
            }
        }

        private static void ExecutarComandoPowerShell(string comando, string descricao, List<string> erros, ProgressTask task, int passoAtual, int totalPassos)
        {
            task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: {descricao}...[/]";
            task.Value = passoAtual;

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
                
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                
                process.WaitForExit(30000); // 30 segundos timeout

                if (process.ExitCode == 0 || string.IsNullOrWhiteSpace(error))
                {
                    DebugSuccess($"{descricao} concluído com sucesso");
                }
                else
                {
                    DebugWarning($"Aviso ao executar {descricao}: {error}");
                    erros.Add(descricao);
                }
            }
            catch (Exception ex)
            {
                DebugWarning($"Erro ao executar {descricao}: {ex.Message}");
                erros.Add(descricao);
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
