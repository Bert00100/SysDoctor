namespace SysDoctor.Scripts
{
    class TestPing
    {
        public static async Task Executar()
        {
            AnsiConsole.MarkupLine("[blue]Teste de Ping[/]");
            AnsiConsole.WriteLine();

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var erros = new List<string>();
                var resultados = new List<(string destino, string resultado, List<string> respostas, string latenciaMedia)>();
                int totalPassos = 2;
                int passoAtual = 0;

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
                        var task = ctx.AddTask("[cyan]Iniciando testes de ping...[/]", maxValue: totalPassos);

                        // Passo 1: teste de ping DNS Google
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Testando ping DNS Google (8.8.8.8)...[/]";
                        task.Value = passoAtual;
                        
                        var pingDNS = await ExecutarPingAsync("8.8.8.8", 20);
                        resultados.Add(("DNS Google (8.8.8.8)", pingDNS.resultado, pingDNS.respostas, pingDNS.latenciaMedia));
                        
                        if (pingDNS.sucesso)
                        {
                            DebugSuccess($"Ping DNS Google: {pingDNS.latenciaMedia}ms");
                        }
                        else
                        {
                            DebugWarning($"Falha ao pingar DNS Google: {pingDNS.erro}");
                            erros.Add("Teste de ping DNS Google");
                        }

                        // Passo 2: teste de ping Cloudflare
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Testando ping Cloudflare (1.1.1.1)...[/]";
                        task.Value = passoAtual;
                        
                        var pingCloudflare = await ExecutarPingAsync("1.1.1.1", 20);
                        resultados.Add(("Cloudflare (1.1.1.1)", pingCloudflare.resultado, pingCloudflare.respostas, pingCloudflare.latenciaMedia));
                        
                        if (pingCloudflare.sucesso)
                        {
                            DebugSuccess($"Ping Cloudflare: {pingCloudflare.latenciaMedia}ms");
                        }
                        else
                        {
                            DebugWarning($"Falha ao pingar Cloudflare: {pingCloudflare.erro}");
                            erros.Add("Teste de ping Cloudflare");
                        }

                        task.StopTask();
                    });

                stopwatch.Stop();

                // Resumo final
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"[cyan]⏱️ Tempo total: {stopwatch.Elapsed:mm\\:ss}[/]");
                AnsiConsole.WriteLine();

                // Exibir resultados detalhados em tabela
                var tabela = new Table();
                tabela.Border(TableBorder.Rounded);
                tabela.AddColumn("[cyan]Destino[/]");
                tabela.AddColumn("[cyan]Resultado[/]");

                foreach (var (destino, resultado, respostas, latenciaMedia) in resultados)
                {
                    tabela.AddRow(destino, resultado);
                }

                AnsiConsole.Write(tabela);
                AnsiConsole.WriteLine();

                // Exibir respostas individuais de cada ping
                AnsiConsole.MarkupLine("[cyan]📊 Detalhes das Respostas:[/]");
                AnsiConsole.WriteLine();

                foreach (var (destino, resultado, respostas, latenciaMedia) in resultados)
                {
                    AnsiConsole.MarkupLine($"[yellow]• {destino}:[/]");
                    if (respostas.Count > 0)
                    {
                        foreach (var resposta in respostas)
                        {
                            AnsiConsole.MarkupLine($"  {resposta}");
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("  [red]Nenhuma resposta recebida[/]");
                    }
                    AnsiConsole.WriteLine();
                }

                if (erros.Count > 0)
                {
                    AnsiConsole.MarkupLine($"[yellow]⚠️ Concluído com avisos: {string.Join(", ", erros)}[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[green]✅ Todos os pings foram bem-sucedidos![/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]💥 Erro durante o teste de ping: {ex.Message}[/]");
                AnsiConsole.MarkupLine($"[grey]Detalhes: {ex}[/]");
            }
        }

        private static async Task<(bool sucesso, string latenciaMedia, string resultado, string erro, List<string> respostas)> ExecutarPingAsync(string destino, int timeoutSeconds)
        {
            var respostas = new List<string>();
            var latencias = new List<int>();
            
            try
            {
                using (var process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = "ping",
                        Arguments = $"{destino} -n 4 -w {timeoutSeconds * 1000}",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    };

                    process.Start();

                    // Usar Task.Run para evitar bloqueios
                    var output = await Task.Run(() => process.StandardOutput.ReadToEndAsync());
                    var error = await Task.Run(() => process.StandardError.ReadToEndAsync());

                    // Aguardar o processo terminar com timeout
                    bool exited = process.WaitForExit((timeoutSeconds + 5) * 1000);

                    if (!exited)
                    {
                        try 
                        { 
                            process.Kill(); 
                            await Task.Delay(1000);
                        } 
                        catch { }
                        return (false, "N/A", "[red]Timeout - Tempo esgotado[/]", "Timeout", respostas);
                    }

                    // Extrair respostas individuais e latências
                    var linhas = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var linha in linhas)
                    {
                        // Formato português: "Resposta de 8.8.8.8: bytes=32 tempo=4ms TTL=119"
                        // Formato inglês: "Reply from 8.8.8.8: bytes=32 time=4ms TTL=119"
                        if (linha.Contains("Resposta de") || linha.Contains("Reply from"))
                        {
                            // Extrair informações - regex melhorado para capturar tempo com < também
                            var respostaMatch = System.Text.RegularExpressions.Regex.Match(
                                linha, 
                                @"(Resposta de|Reply from)\s+([0-9.]+).*bytes[=:](\d+).*(tempo|time)[=<](\d+)ms.*TTL[=:](\d+)",
                                System.Text.RegularExpressions.RegexOptions.IgnoreCase
                            );

                            if (respostaMatch.Success)
                            {
                                string ip = respostaMatch.Groups[2].Value;
                                string bytes = respostaMatch.Groups[3].Value;
                                string tempo = respostaMatch.Groups[5].Value;
                                string ttl = respostaMatch.Groups[6].Value;

                                // Adicionar latência à lista para cálculo da média
                                if (int.TryParse(tempo, out int latenciaInt))
                                {
                                    latencias.Add(latenciaInt);
                                }

                                respostas.Add($"[green]Resposta de {ip}: bytes={bytes} tempo={tempo}ms TTL={ttl}[/]");
                            }
                            else
                            {
                                // Fallback: adicionar linha original formatada
                                respostas.Add($"[green]{linha.Trim()}[/]");
                                
                                // Tentar extrair apenas o tempo para o cálculo da média
                                var tempoMatch = System.Text.RegularExpressions.Regex.Match(
                                    linha,
                                    @"(tempo|time)[=<](\d+)ms",
                                    System.Text.RegularExpressions.RegexOptions.IgnoreCase
                                );
                                
                                if (tempoMatch.Success && int.TryParse(tempoMatch.Groups[2].Value, out int latenciaInt))
                                {
                                    latencias.Add(latenciaInt);
                                }
                            }
                        }
                        else if (linha.Contains("Tempo esgotado") || linha.Contains("Request timed out"))
                        {
                            respostas.Add("[red]Tempo esgotado para esta solicitação.[/]");
                        }
                    }

                    // Calcular média das latências
                    string latenciaMediaCalculada = latencias.Count > 0 
                        ? Math.Round(latencias.Average()).ToString() 
                        : "N/A";

                    // Analisar o output
                    if (process.ExitCode == 0 || output.Contains("Média =") || latencias.Count > 0)
                    {
                        // Tentar extrair latência média do output (formato em português)
                        var match = System.Text.RegularExpressions.Regex.Match(output, @"Média = (\d+)ms");
                        if (!match.Success)
                        {
                            // Tentar formato em inglês
                            match = System.Text.RegularExpressions.Regex.Match(output, @"Average = (\d+)ms");
                        }

                        // Se não encontrou no output, usar a média calculada
                        string latenciaMedia = match.Success ? match.Groups[1].Value : latenciaMediaCalculada;

                        // Extrair estatísticas (formato em português)
                        var statsMatch = System.Text.RegularExpressions.Regex.Match(output, @"Enviados = (\d+), Recebidos = (\d+), Perdidos = (\d+)");
                        if (!statsMatch.Success)
                        {
                            // Tentar formato em inglês
                            statsMatch = System.Text.RegularExpressions.Regex.Match(output, @"Sent = (\d+), Received = (\d+), Lost = (\d+)");
                        }

                        if (statsMatch.Success || latencias.Count > 0)
                        {
                            string enviados = statsMatch.Success ? statsMatch.Groups[1].Value : "4";
                            string recebidos = statsMatch.Success ? statsMatch.Groups[2].Value : latencias.Count.ToString();
                            string perdidos = statsMatch.Success ? statsMatch.Groups[3].Value : (4 - latencias.Count).ToString();
                            
                            int perdidosInt = int.Parse(perdidos);
                            int enviadosInt = int.Parse(enviados);
                            
                            string status = perdidosInt == 0 ? "[green]✓ Conectado[/]" : 
                                          perdidosInt < enviadosInt ? "[yellow]⚠️ Parcialmente Conectado[/]" : 
                                          "[red]✗ Falha de Conexão[/]";
                            

                            string resultado = $"{status}\n" +
                                             $"  Latência média: [yellow]{latenciaMedia}ms[/]\n" +
                                             $"  Pacotes: Enviados={enviados}, Recebidos={recebidos}, Perdidos={perdidos}";
                            
                            return (perdidosInt == 0, latenciaMedia, resultado, "", respostas);
                        }
                    }

                    // Verificar diferentes tipos de erro
                    if (output.Contains("Tempo esgotado") || output.Contains("Esgotado") || 
                        output.Contains("Request timed out") || output.Contains("Timed out"))
                    {
                        return (false, "N/A", "[red]✗ Timeout - Host não respondeu[/]", "Timeout", respostas);
                    }
                    else if (output.Contains("não pôde encontrar") || output.Contains("could not find") ||
                             output.Contains("Não é possível encontrar") || output.Contains("Could not find"))
                    {
                        return (false, "N/A", "[red]✗ Host não encontrado[/]", "Host não encontrado", respostas);
                    }
                    else if (output.Contains("Host de destino inacessível") || output.Contains("Destination host unreachable"))
                    {
                        return (false, "N/A", "[red]✗ Host inacessível[/]", "Host inacessível", respostas);
                    }
                    else if (output.Contains("Falha geral") || output.Contains("General failure"))
                    {
                        return (false, "N/A", "[red]✗ Falha geral na conexão[/]", "Falha geral", respostas);
                    }
                    else if (!string.IsNullOrEmpty(error))
                    {
                        return (false, "N/A", $"[red]✗ Erro: {error.Trim()}[/]", error.Trim(), respostas);
                    }
                    else
                    {
                        return (false, "N/A", $"[red]✗ Falha desconhecida[/]\n  Exit Code: {process.ExitCode}", $"Exit Code: {process.ExitCode}", respostas);
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, "N/A", $"[red]✗ Erro no processo: {ex.Message}[/]", ex.Message, respostas);
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