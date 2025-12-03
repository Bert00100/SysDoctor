namespace SysDoctor.Scripts
{
    class OtmPing
    {
        private static readonly Dictionary<string, string[]> ServidoresDNS = new()
        {
            { "Google DNS", new[] { "8.8.8.8", "8.8.4.4" } },
            { "Cloudflare", new[] { "1.1.1.1", "1.0.0.1" } },
            { "OpenDNS", new[] { "208.67.222.222", "208.67.220.220" } },
            { "Quad9", new[] { "9.9.9.9", "149.112.112.112" } },
            { "AdGuard", new[] { "94.140.14.14", "94.140.15.15" } },
            { "Level3", new[] { "4.2.2.1", "4.2.2.2" } }
        };

        public static void Executar()
        {
            AnsiConsole.MarkupLine("[blue]🌐 Otimizar DNS para Melhor Ping[/]");
            AnsiConsole.WriteLine();

            try
            {
                // Exibe informações atuais do DNS
                ExibirDNSAtual();

                // Testa velocidade de diferentes servidores DNS
                var melhorDNS = TestarServidoresDNS();

                if (melhorDNS.HasValue)
                {
                    // Pergunta se o usuário quer aplicar o melhor DNS
                    var aplicar = AnsiConsole.Confirm($"Deseja aplicar o DNS mais rápido ({melhorDNS.Value.Nome})?");
                    
                    if (aplicar)
                    {
                        ConfigurarDNS(melhorDNS.Value.Primario, melhorDNS.Value.Secundario, melhorDNS.Value.Nome);
                    }
                }

                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[cyan]💡 Dicas para melhorar o ping:[/]");
                AnsiConsole.MarkupLine("[dim]• Use cabo de rede em vez de Wi-Fi quando possível[/]");
                AnsiConsole.MarkupLine("[dim]• Feche programas que consomem internet[/]");
                AnsiConsole.MarkupLine("[dim]• Reinicie o roteador se necessário[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]💥 Erro durante a otimização de DNS: {ex.Message}[/]");
            }
        }

        private static void ExibirDNSAtual()
        {
            try
            {
                AnsiConsole.MarkupLine("[cyan]📋 DNS Atual:[/]");
                
                // Usa ipconfig para obter DNS atual
                var processInfo = new ProcessStartInfo
                {
                    FileName = "ipconfig",
                    Arguments = "/all",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                using var process = Process.Start(processInfo);
                if (process != null)
                {
                    var output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    // Procura por servidores DNS na saída
                    var lines = output.Split('\n');
                    bool encontrouDNS = false;
                    
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains("DNS Servers") || lines[i].Contains("Servidores DNS"))
                        {
                            encontrouDNS = true;
                            // Extrai o primeiro servidor DNS da mesma linha
                            var parts = lines[i].Split(':');
                            if (parts.Length > 1)
                            {
                                var dns = parts[1].Trim();
                                if (!string.IsNullOrEmpty(dns))
                                {
                                    AnsiConsole.MarkupLine($"[dim]  • {dns}[/]");
                                }
                            }
                            
                            // Verifica as próximas linhas para servidores DNS adicionais
                            for (int j = i + 1; j < lines.Length && j < i + 3; j++)
                            {
                                var line = lines[j].Trim();
                                if (System.Net.IPAddress.TryParse(line, out _))
                                {
                                    AnsiConsole.MarkupLine($"[dim]  • {line}[/]");
                                }
                                else if (line.Contains(':') || line.Length == 0)
                                {
                                    break;
                                }
                            }
                        }
                    }

                    if (!encontrouDNS)
                    {
                        AnsiConsole.MarkupLine("[dim]  • DNS automático (DHCP)[/]");
                    }
                }
                
                AnsiConsole.WriteLine();
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[yellow]⚠️ Não foi possível obter DNS atual: {ex.Message}[/]");
            }
        }

        private static (string Nome, string Primario, string Secundario, int Ping)? TestarServidoresDNS()
        {
            AnsiConsole.MarkupLine("[cyan]⚡ Testando velocidade dos servidores DNS...[/]");
            AnsiConsole.WriteLine();

            var resultados = new List<(string Nome, string Primario, string Secundario, int Ping)>();

            foreach (var dns in ServidoresDNS)
            {
                try
                {
                    var ping = new Ping();
                    var reply = ping.Send(dns.Value[0], 3000);
                    
                    int latencia = reply.Status == IPStatus.Success ? (int)reply.RoundtripTime : 9999;
                    resultados.Add((dns.Key, dns.Value[0], dns.Value[1], latencia));

                    string status = reply.Status == IPStatus.Success ? "✅" : "❌";
                    string latenciaStr = reply.Status == IPStatus.Success ? $"{latencia}ms" : "Falhou";
                    
                    AnsiConsole.MarkupLine($"[dim]{status} {dns.Key.PadRight(12)} ({dns.Value[0].PadRight(15)}) - {latenciaStr}[/]");
                }
                catch
                {
                    resultados.Add((dns.Key, dns.Value[0], dns.Value[1], 9999));
                    AnsiConsole.MarkupLine($"[dim]❌ {dns.Key.PadRight(12)} ({dns.Value[0].PadRight(15)}) - Erro[/]");
                }
            }

            AnsiConsole.WriteLine();

            var melhores = resultados.Where(r => r.Ping < 9999).OrderBy(r => r.Ping).ToList();
            
            if (melhores.Any())
            {
                var melhor = melhores.First();
                AnsiConsole.MarkupLine($"[green]🏆 Melhor DNS encontrado: {melhor.Nome} ({melhor.Ping}ms)[/]");
                return melhor;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]❌ Nenhum servidor DNS respondeu adequadamente[/]");
                return null;
            }
        }

        private static void ConfigurarDNS(string dnsPrimario, string dnsSecundario, string nomeDNS)
        {
            try
            {
                AnsiConsole.Status()
                    .Start($"Configurando DNS para {nomeDNS}...", ctx =>
                    {
                        // Comando netsh para alterar DNS (mais compatível que PowerShell)
                        var adapters = ObterAdaptadoresRede();
                        
                        foreach (var adapter in adapters)
                        {
                            try
                            {
                                // Configura DNS primário
                                var processInfo1 = new ProcessStartInfo
                                {
                                    FileName = "netsh",
                                    Arguments = $"interface ip set dns \"{adapter}\" static {dnsPrimario}",
                                    UseShellExecute = false,
                                    CreateNoWindow = true,
                                    Verb = "runas"
                                };
                                using var process1 = Process.Start(processInfo1);
                                process1?.WaitForExit();

                                // Adiciona DNS secundário
                                var processInfo2 = new ProcessStartInfo
                                {
                                    FileName = "netsh",
                                    Arguments = $"interface ip add dns \"{adapter}\" {dnsSecundario} index=2",
                                    UseShellExecute = false,
                                    CreateNoWindow = true,
                                    Verb = "runas"
                                };
                                using var process2 = Process.Start(processInfo2);
                                process2?.WaitForExit();
                            }
                            catch (Exception ex)
                            {
                                AnsiConsole.MarkupLine($"[yellow]⚠️ Erro ao configurar {adapter}: {ex.Message}[/]");
                            }
                        }

                        // Limpa cache DNS
                        var flushInfo = new ProcessStartInfo
                        {
                            FileName = "ipconfig",
                            Arguments = "/flushdns",
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };
                        using var flushProcess = Process.Start(flushInfo);
                        flushProcess?.WaitForExit();
                    });

                AnsiConsole.MarkupLine($"[green]✅ DNS configurado para {nomeDNS}[/]");
                AnsiConsole.MarkupLine($"[dim]• Primário: {dnsPrimario}[/]");
                AnsiConsole.MarkupLine($"[dim]• Secundário: {dnsSecundario}[/]");
                AnsiConsole.MarkupLine("[cyan]💡 Cache DNS foi limpo automaticamente[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Erro ao configurar DNS: {ex.Message}[/]");
                AnsiConsole.MarkupLine("[cyan]💡 Certifique-se de executar como administrador[/]");
            }
        }

        private static List<string> ObterAdaptadoresRede()
        {
            var adapters = new List<string>();
            
            try
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = "wmic",
                    Arguments = "path win32_networkadapter where \"NetEnabled=true\" get NetConnectionID /format:value",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                using var process = Process.Start(processInfo);
                if (process != null)
                {
                    var output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    foreach (var line in output.Split('\n'))
                    {
                        if (line.StartsWith("NetConnectionID=") && !string.IsNullOrWhiteSpace(line.Split('=')[1]))
                        {
                            var adapterName = line.Split('=')[1].Trim();
                            if (!string.IsNullOrEmpty(adapterName))
                            {
                                adapters.Add(adapterName);
                            }
                        }
                    }
                }
            }
            catch
            {
                // Fallback para adaptadores comuns
                adapters.AddRange(new[] { "Ethernet", "Wi-Fi", "Local Area Connection", "Wireless Network Connection" });
            }

            return adapters;
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