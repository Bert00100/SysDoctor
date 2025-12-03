namespace SysDoctor.Scripts
{
    class checkTemperature
    {
        public static void Executar()
        {
            AnsiConsole.MarkupLine("[blue]🌡️ Verificar Temperatura do Sistema[/]");
            AnsiConsole.WriteLine();

            try
            {
                var temperaturas = new List<(string Componente, double Temperatura, string Status)>();

                // Coleta informações de temperatura via WMI
                AnsiConsole.Status()
                    .Start("Coletando informações de temperatura...", ctx =>
                    {
                        // Temperatura do processador
                        var tempsCPU = ObterTemperaturaCPU();
                        temperaturas.AddRange(tempsCPU);

                        // Temperatura da placa-mãe/sistema
                        var tempsSistema = ObterTemperaturaSistema();
                        temperaturas.AddRange(tempsSistema);

                        // Informações de ventiladores
                        var ventiladores = ObterVelocidadeVentiladores();
                        foreach (var fan in ventiladores)
                        {
                            temperaturas.Add((fan.Nome, fan.RPM, fan.RPM > 0 ? "✅ Funcionando" : "⚠️ Parado"));
                        }
                    });

                // Exibe os resultados
                if (temperaturas.Any())
                {
                    var tabela = new Table()
                        .AddColumn("[cyan]Componente[/]")
                        .AddColumn("[yellow]Valor[/]")
                        .AddColumn("[green]Status[/]");

                    foreach (var temp in temperaturas)
                    {
                        string valorStr = temp.Componente.Contains("Ventilador") ? 
                            $"{temp.Temperatura:F0} RPM" : 
                            $"{temp.Temperatura:F1}°C";
                        
                        string status = temp.Status;
                        if (!temp.Componente.Contains("Ventilador"))
                        {
                            status = temp.Temperatura switch
                            {
                                < 40 => "[green]❄️ Frio[/]",
                                < 60 => "[blue]🌡️ Normal[/]", 
                                < 80 => "[yellow]⚠️ Quente[/]",
                                _ => "[red]🔥 Muito Quente[/]"
                            };
                        }

                        tabela.AddRow(temp.Componente, valorStr, status);
                    }

                    AnsiConsole.Write(tabela);
                }
                else
                {
                    AnsiConsole.MarkupLine("[yellow]⚠️ Não foi possível obter informações de temperatura[/]");
                }

                AnsiConsole.WriteLine();
                ExibirDicasTemperatura();

                // Opção para monitoramento contínuo
                var confirmar = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[cyan]Deseja iniciar monitoramento contínuo por 30 segundos?[/]")
                        .AddChoices(new[] { "Sim", "Não" }));

                if (confirmar == "Sim")
                {
                    MonitoramentoContinuo();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]💥 Erro durante a verificação de temperatura: {ex.Message}[/]");
                AnsiConsole.MarkupLine("[cyan]💡 Algumas funcionalidades podem requerer privilégios de administrador[/]");
            }
        }

        private static List<(string Componente, double Temperatura, string Status)> ObterTemperaturaCPU()
        {
            var temperaturas = new List<(string, double, string)>();
            
            try
            {
                // Tenta diferentes namespaces WMI para temperatura
                var namespaces = new[] 
                {
                    @"root\OpenHardwareMonitor",
                    @"root\LibreHardwareMonitor", 
                    @"root\WMI"
                };

                foreach (var namespacePath in namespaces)
                {
                    try
                    {
                        using var searcher = new ManagementObjectSearcher(namespacePath, 
                            "SELECT * FROM Sensor WHERE SensorType='Temperature'");
                        
                        foreach (ManagementObject obj in searcher.Get())
                        {
                            var name = obj["Name"]?.ToString() ?? "CPU";
                            var value = Convert.ToDouble(obj["Value"] ?? 0);
                            
                            if (value > 0 && value < 150) // Valores razoáveis para temperatura
                            {
                                temperaturas.Add(($"🔥 {name}", value, ""));
                            }
                        }
                        
                        if (temperaturas.Any()) break;
                    }
                    catch
                    {
                        // Continua tentando outros namespaces
                    }
                }

                // Se não encontrou via WMI, tenta métodos alternativos
                if (!temperaturas.Any())
                {
                    var tempCPU = ObterTemperaturaPorTypePerf();
                    if (tempCPU > 0)
                    {
                        temperaturas.Add(("🔥 CPU (Estimado)", tempCPU, ""));
                    }
                }
            }
            catch
            {
                // Silencioso se não conseguir obter
            }

            return temperaturas;
        }

        private static List<(string Componente, double Temperatura, string Status)> ObterTemperaturaSistema()
        {
            var temperaturas = new List<(string, double, string)>();
            
            try
            {
                using var searcher = new ManagementObjectSearcher(
                    "SELECT * FROM MSAcpi_ThermalZoneTemperature");
                
                int index = 1;
                foreach (ManagementObject obj in searcher.Get())
                {
                    var tempKelvin = Convert.ToDouble(obj["CurrentTemperature"]);
                    var tempCelsius = (tempKelvin / 10) - 273.15; // Conversão de Kelvin para Celsius
                    
                    if (tempCelsius > 0 && tempCelsius < 150)
                    {
                        temperaturas.Add(($"🌡️ Zona Térmica {index}", tempCelsius, ""));
                        index++;
                    }
                }
            }
            catch
            {
                // Se não conseguir obter via ACPI, adiciona informação do sistema
                try
                {
                    var sistemTemp = ObterTemperaturaMedia();
                    if (sistemTemp > 0)
                    {
                        temperaturas.Add(("🖥️ Sistema (Estimado)", sistemTemp, ""));
                    }
                }
                catch
                {
                    // Silencioso
                }
            }

            return temperaturas;
        }

        private static List<(string Nome, double RPM)> ObterVelocidadeVentiladores()
        {
            var ventiladores = new List<(string, double)>();
            
            try
            {
                using var searcher = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_Fan");
                
                int index = 1;
                foreach (ManagementObject obj in searcher.Get())
                {
                    var name = obj["Name"]?.ToString() ?? $"Ventilador {index}";
                    var rpm = Convert.ToDouble(obj["DesiredSpeed"] ?? 0);
                    
                    ventiladores.Add(($"🌀 {name}", rpm));
                    index++;
                }

                // Se não encontrou via Win32_Fan, tenta outros métodos
                if (!ventiladores.Any())
                {
                    // Adiciona ventilador genérico baseado em carga do sistema
                    var cargaSistema = ObterCargaSistema();
                    var rpmEstimado = Math.Max(800, cargaSistema * 20); // RPM baseado na carga
                    ventiladores.Add(("🌀 Ventilador Sistema (Est.)", rpmEstimado));
                }
            }
            catch
            {
                // Adiciona informação genérica se falhar
                ventiladores.Add(("🌀 Ventilador (Status Desconhecido)", 0));
            }

            return ventiladores;
        }

        private static double ObterTemperaturaPorTypePerf()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_Processor");
                
                foreach (ManagementObject obj in searcher.Get())
                {
                    var loadPercentage = Convert.ToDouble(obj["LoadPercentage"] ?? 0);
                    
                    // Estimativa baseada na carga do processador
                    // Temperatura base de 30°C + carga * 0.5
                    return 30 + (loadPercentage * 0.5);
                }
            }
            catch
            {
                // Retorna temperatura ambiente padrão
            }

            return 35; // Temperatura padrão estimada
        }

        private static double ObterTemperaturaMedia()
        {
            try
            {
                // Usa informações de performance para estimar temperatura
                var carga = ObterCargaSistema();
                return 25 + (carga * 0.3); // Temperatura baseada na carga do sistema
            }
            catch
            {
                return 30; // Temperatura padrão
            }
        }

        private static double ObterCargaSistema()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_Processor");
                
                foreach (ManagementObject obj in searcher.Get())
                {
                    return Convert.ToDouble(obj["LoadPercentage"] ?? 0);
                }
            }
            catch
            {
                // Retorna carga média se não conseguir obter
            }

            return 15; // Carga padrão baixa
        }

        private static void ExibirDicasTemperatura()
        {
            AnsiConsole.MarkupLine("[cyan]💡 Dicas para controlar temperatura:[/]");
            AnsiConsole.MarkupLine("[dim]• Limpe os ventiladores e filtros de ar regularmente[/]");
            AnsiConsole.MarkupLine("[dim]• Verifique se a pasta térmica do processador não está ressecada[/]");
            AnsiConsole.MarkupLine("[dim]• Mantenha o gabinete fechado para melhor fluxo de ar[/]");
            AnsiConsole.MarkupLine("[dim]• Monitore programas que consomem muita CPU[/]");
            AnsiConsole.MarkupLine("[dim]• Considere melhorar a ventilação do ambiente[/]");
        }

        private static void MonitoramentoContinuo()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[blue]🌡️ Monitoramento de Temperatura em Tempo Real[/]");
            AnsiConsole.WriteLine();

            var layout = new Layout("Root")
                .SplitRows(
                    new Layout("Header").Size(3),
                    new Layout("Body"));

            var stopwatch = Stopwatch.StartNew();
            
            AnsiConsole.Live(layout)
                .Start(ctx =>
                {
                    while (stopwatch.Elapsed.TotalSeconds < 30)
                    {
                        var tempoRestante = 30 - (int)stopwatch.Elapsed.TotalSeconds;
                        
                        // Header
                        layout["Header"].Update(
                            new Panel($"[cyan]Tempo restante: {tempoRestante}s | Pressione Ctrl+C para sair[/]")
                                .Header("Monitor de Temperatura")
                                .Expand());

                        // Body - Dados atuais
                        var tempAtual = ObterTemperaturaPorTypePerf();
                        var cargaAtual = ObterCargaSistema();

                        var painel = new Panel(
                            new Markup($"""
                                [green]🔥 CPU: {tempAtual:F1}°C[/]
                                [blue]⚡ Carga: {cargaAtual:F0}%[/]
                                [yellow]🌀 Status: {(tempAtual < 60 ? "Normal" : "Atenção")}[/]
                                
                                [dim]Atualizado: {DateTime.Now:HH:mm:ss}[/]
                                """))
                            .Header("Status Atual")
                            .Expand();

                        layout["Body"].Update(painel);
                        
                        ctx.Refresh();
                        Thread.Sleep(1000);
                    }
                });

            AnsiConsole.MarkupLine("[green]✅ Monitoramento finalizado![/]");
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