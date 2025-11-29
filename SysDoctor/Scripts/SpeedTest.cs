namespace SysDoctor.Scripts
{
    class SpeedTest
    {
        public static void Executar()
        {
            ExecutarAsync().GetAwaiter().GetResult();
        }

        private static async Task ExecutarAsync()
        {
            Console.Clear();
            
            AnsiConsole.MarkupLine("[blue]┌────────────────────────────────────────┐[/]");
            AnsiConsole.MarkupLine("[blue]│        SpeedTest de Conexão           │[/]");
            AnsiConsole.MarkupLine("[blue]└────────────────────────────────────────┘[/]");
            AnsiConsole.MarkupLine("[yellow]Este teste pode levar alguns minutos...[/]");
            AnsiConsole.WriteLine();

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var erros = new List<string>();
                double ping = 0;
                double downloadSpeed = 0;
                double uploadSpeed = 0;
                string ipExterno = "Não disponível";

                // Obter IP externo
                AnsiConsole.MarkupLine("[cyan]🌐 Obtendo informações de rede...[/]");
                ipExterno = await ObterIpExterno();
                AnsiConsole.MarkupLine($"[green]✅ IP Externo: {ipExterno}[/]");
                AnsiConsole.WriteLine();

                // Teste de Ping
                AnsiConsole.MarkupLine("[cyan]📡 Testando Latência (Ping)...[/]");
                ping = await TestarPing();
                if (ping > 0)
                {
                    AnsiConsole.MarkupLine($"[green]✅ Ping: {ping:F1} ms[/]");
                }
                else
                {
                    erros.Add("Teste de Ping");
                    AnsiConsole.MarkupLine("[yellow]⚠️ Falha ao testar ping[/]");
                }
                AnsiConsole.WriteLine();

                // Teste de Download com barra de progresso
                AnsiConsole.MarkupLine("[cyan]📥 Testando Velocidade de Download...[/]");
                downloadSpeed = await AnsiConsole.Status()
                    .Spinner(Spinner.Known.Dots)
                    .SpinnerStyle(Style.Parse("green bold"))
                    .StartAsync("[green]Baixando dados de teste...[/]", async ctx =>
                    {
                        return await TestarDownload();
                    });

                if (downloadSpeed > 0)
                {
                    AnsiConsole.MarkupLine($"[green]✅ Download: {downloadSpeed:F2} Mbps[/]");
                }
                else
                {
                    erros.Add("Teste de Download");
                    AnsiConsole.MarkupLine("[yellow]⚠️ Falha ao testar download[/]");
                }
                AnsiConsole.WriteLine();

                // Teste de Upload com barra de progresso
                AnsiConsole.MarkupLine("[cyan]📤 Testando Velocidade de Upload...[/]");
                uploadSpeed = await AnsiConsole.Status()
                    .Spinner(Spinner.Known.Dots)
                    .SpinnerStyle(Style.Parse("blue bold"))
                    .StartAsync("[blue]Enviando dados de teste...[/]", async ctx =>
                    {
                        return await TestarUpload();
                    });

                if (uploadSpeed > 0)
                {
                    AnsiConsole.MarkupLine($"[green]✅ Upload: {uploadSpeed:F2} Mbps[/]");
                }
                else
                {
                    erros.Add("Teste de Upload");
                    AnsiConsole.MarkupLine("[yellow]⚠️ Falha ao testar upload[/]");
                }

                stopwatch.Stop();

                // Limpa a tela antes de exibir os resultados finais
                Console.Clear();
                
                // Exibir cabeçalho novamente
                AnsiConsole.MarkupLine("[blue]┌────────────────────────────────────────┐[/]");
                AnsiConsole.MarkupLine("[blue]│        SpeedTest de Conexão           │[/]");
                AnsiConsole.MarkupLine("[blue]└────────────────────────────────────────┘[/]");
                AnsiConsole.WriteLine();

                // Exibir resultados no estilo da imagem
                ExibirResultados(ping, downloadSpeed, uploadSpeed, ipExterno);

                // Resumo final
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"[cyan]⏱️ Tempo total: {stopwatch.Elapsed:mm\\:ss} minutos[/]");

                if (erros.Count > 0)
                {
                    AnsiConsole.MarkupLine($"[yellow]⚠️ Concluído com avisos: {string.Join(", ", erros)}[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[green]✅ SpeedTest concluído com sucesso![/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Erro ao executar SpeedTest: {ex.Message}[/]");
            }
        }

        private static async Task<string> ObterIpExterno()
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);
                
                // Tenta múltiplos serviços para maior confiabilidade
                string[] servicos = {
                    "https://api.ipify.org",
                    "https://icanhazip.com",
                    "https://ifconfig.me/ip"
                };

                foreach (var servico in servicos)
                {
                    try
                    {
                        var ip = await client.GetStringAsync(servico);
                        return ip.Trim();
                    }
                    catch { }
                }

                return "Não disponível";
            }
            catch
            {
                return "Não disponível";
            }
        }

        private static async Task<double> TestarPing()
        {
            try
            {
                using var ping = new Ping();
                var results = new List<long>();
                // Servidores do Google, Cloudflare e OpenDNS
                string[] hosts = { "8.8.8.8", "1.1.1.1", "208.67.222.222" };

                foreach (var host in hosts)
                {
                    try
                    {
                        var reply = await ping.SendPingAsync(host, 3000);
                        if (reply.Status == IPStatus.Success)
                        {
                            results.Add(reply.RoundtripTime);
                        }
                    }
                    catch { }
                }

                return results.Count > 0 ? results.Average() : 0;
            }
            catch
            {
                return 0;
            }
        }

        private static async Task<double> TestarDownload()
        {
            try
            {
                // Usando Cloudflare Speed Test (50MB)
                string testUrl = "https://speed.cloudflare.com/__down?bytes=52428800";
                
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(60);
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

                var sw = Stopwatch.StartNew();
                var response = await client.GetAsync(testUrl);
                var bytes = await response.Content.ReadAsByteArrayAsync();
                sw.Stop();

                if (response.IsSuccessStatusCode && bytes.Length > 0)
                {
                    // Calcular velocidade em Mbps
                    double seconds = Math.Max(0.1, sw.Elapsed.TotalSeconds);
                    double megabits = (bytes.Length * 8.0) / 1_000_000.0;
                    double mbps = megabits / seconds;

                    return mbps;
                }

                return 0;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Erro no download: {ex.Message}[/]");
                return 0;
            }
        }

        private static async Task<double> TestarUpload()
        {
            try
            {
                // Usando Cloudflare Speed Test para upload (10MB)
                string testUrl = "https://speed.cloudflare.com/__up";

                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(60);
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

                // Criar dados de teste (10 MB)
                var data = new byte[10 * 1024 * 1024];
                new Random().NextBytes(data);
                var content = new ByteArrayContent(data);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                var sw = Stopwatch.StartNew();
                var response = await client.PostAsync(testUrl, content);
                sw.Stop();

                if (response.IsSuccessStatusCode)
                {
                    // Calcular velocidade em Mbps
                    double seconds = Math.Max(0.1, sw.Elapsed.TotalSeconds);
                    double megabits = (data.Length * 8.0) / 1_000_000.0;
                    double mbps = megabits / seconds;

                    return mbps;
                }

                return 0;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Erro no upload: {ex.Message}[/]");
                return 0;
            }
        }

        private static void ExibirResultados(double ping, double download, double upload, string ipExterno)
        {
            AnsiConsole.WriteLine();
            
            // Painel de informações da rede
            var infoPanel = new Panel($"[cyan]🌐 IP Externo:[/] [white]{ipExterno}[/]\n[cyan][/]")
                .Border(BoxBorder.Rounded)
                .BorderColor(Color.Blue)
                .Header("[blue]Informações da Conexão[/]");
            
            AnsiConsole.Write(infoPanel);
            AnsiConsole.WriteLine();

            // Criar painel principal com os resultados estilo speedtest
            var downloadPanel = new Panel(
                Align.Center(
                    new Markup($"[green bold]{download:F2}[/]\n[dim]Mbps[/]"),
                    VerticalAlignment.Middle
                )
            )
            .Header("[green]📥 DOWNLOAD[/]")
            .Border(BoxBorder.Rounded)
            .BorderColor(GetSpeedColor(download, true))
            .Expand();

            var uploadPanel = new Panel(
                Align.Center(
                    new Markup($"[blue bold]{upload:F2}[/]\n[dim]Mbps[/]"),
                    VerticalAlignment.Middle
                )
            )
            .Header("[blue]📤 UPLOAD[/]")
            .Border(BoxBorder.Rounded)
            .BorderColor(GetSpeedColor(upload, false))
            .Expand();

            var pingPanel = new Panel(
                Align.Center(
                    new Markup($"[yellow bold]{ping:F1}[/]\n[dim]ms[/]"),
                    VerticalAlignment.Middle
                )
            )
            .Header("[yellow]⚡ PING[/]")
            .Border(BoxBorder.Rounded)
            .BorderColor(GetPingColor(ping))
            .Expand();

            // Layout em grid
            var grid = new Grid()
                .AddColumn(new GridColumn().PadRight(1))
                .AddColumn(new GridColumn().PadRight(1))
                .AddColumn(new GridColumn());

            grid.AddRow(downloadPanel, uploadPanel, pingPanel);

            AnsiConsole.Write(grid);
            AnsiConsole.WriteLine();

            // Tabela de qualidade
            var table = new Table();
            table.Border = TableBorder.Rounded;
            table.AddColumn(new TableColumn("Métrica").Centered());
            table.AddColumn(new TableColumn("Valor").Centered());
            table.AddColumn(new TableColumn("Qualidade").Centered());

            // Ping
            string pingQuality = ping < 20 ? "⭐ Excelente" : 
                                 ping < 50 ? "✅ Boa" : 
                                 ping < 100 ? "⚠️ Razoável" : 
                                 "❌ Ruim";
            Color pingColor = GetPingColor(ping);
            table.AddRow(
                "Latência", 
                $"[{pingColor}]{ping:F1} ms[/]", 
                $"[{pingColor}]{pingQuality}[/]"
            );

            // Download
            string downloadQuality = download > 100 ? "⭐ Excelente" : 
                                     download > 50 ? "✅ Boa" : 
                                     download > 10 ? "⚠️ Razoável" : 
                                     "❌ Ruim";
            Color downloadColor = GetSpeedColor(download, true);
            table.AddRow(
                "Download", 
                $"[{downloadColor}]{download:F2} Mbps[/]", 
                $"[{downloadColor}]{downloadQuality}[/]"
            );

            // Upload
            string uploadQuality = upload > 50 ? "⭐ Excelente" : 
                                   upload > 20 ? "✅ Boa" : 
                                   upload > 5 ? "⚠️ Razoável" : 
                                   "❌ Ruim";
            Color uploadColor = GetSpeedColor(upload, false);
            table.AddRow(
                "Upload", 
                $"[{uploadColor}]{upload:F2} Mbps[/]", 
                $"[{uploadColor}]{uploadQuality}[/]"
            );

            AnsiConsole.Write(table);
        }

        private static Color GetPingColor(double ping)
        {
            return ping switch
            {
                < 20 => Color.Green,
                < 50 => Color.Blue,
                < 100 => Color.Yellow,
                _ => Color.Red
            };
        }

        private static Color GetSpeedColor(double speed, bool isDownload)
        {
            if (isDownload)
            {
                return speed switch
                {
                    > 100 => Color.Green,
                    > 50 => Color.Blue,
                    > 10 => Color.Yellow,
                    _ => Color.Red
                };
            }
            else
            {
                return speed switch
                {
                    > 50 => Color.Green,
                    > 20 => Color.Blue,
                    > 5 => Color.Yellow,
                    _ => Color.Red
                };
            }
        }
    }
}