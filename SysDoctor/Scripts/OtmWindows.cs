namespace SysDoctor.Scripts
{
    class OtmWindows
    {
        public static void Executar()
        {
            bool continuar = true;

            while (continuar)
            {
                MenuOtmWin();

                Console.Write("\nDigite sua escolha (0 para voltar): ");
                string escolha = Console.ReadLine();

                if (escolha == "0")
                {
                    continuar = false;
                    break;
                }

                // Validação: aceitar apenas números de 1 a 14
                if (!int.TryParse(escolha, out int numero) || numero < 0 || numero > 14)
                {
                    Console.Clear();
                    AnsiConsole.MarkupLine("[red]❌ Opção inválida! Por favor, escolha um número entre 1 e 14 (ou 0 para voltar).[/]");
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    continue;
                }

                ProcessarEscolha(escolha);

                if (continuar && escolha != "0")
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        private static void MenuOtmWin()
        {
            Console.Clear();
            AnsiConsole.Write(
                new FigletText("Otimizador Windows")
                    .LeftJustified()
                    .Color(Color.Blue));

            string[] opcoesEsq = new string[]
            {
                "[ 1 ] Melhorar Desempenho de Energia",
                "[ 3 ] Tornar ALT+TAB Mais Rápido",
                "[ 5 ] Desligar Serviços que Deixam o PC Lento",
                "[ 7 ] Desligar Overlays em Jogos",
                "[ 9 ] Desligar Hibernação",
                "[ 11 ] Desligar Recursos de Virtualização",
                "[ 13 ] Desligar Downloads em Segundo Plano"
            };

            string[] opcoesDir = new string[]
            {
                "[ 2 ] Melhorar Aparência e Desempenho",
                "[ 4 ] Reduzir Coleta de Dados do Windows",
                "[ 6 ] Remover Apps Desnecessários",
                "[ 8 ] Reduzir Avisos de Segurança",
                "[ 10 ] Acelerar Pesquisa de Arquivos",
                "[ 12 ] Desligar Efeitos Visuais Extras",
                "[ 14 ] Reduzir Alertas do SmartScreen"
            };

            var table = new Table()
                .HideHeaders()
                .Border(TableBorder.None)
                .AddColumn(new TableColumn("").Width(45).PadRight(2))
                .AddColumn(new TableColumn("").Width(45))
                .AddRow(
                    new Panel(string.Join("\n", opcoesEsq))
                        .Header("[blue]⚡ Otimizações de Sistema[/]")
                        .Border(BoxBorder.Rounded)
                        .BorderColor(Color.Blue)
                        .Padding(1, 1),
                    
                    new Panel(string.Join("\n", opcoesDir))
                        .Header("[green]🔧 Otimizações Avançadas[/]")
                        .Border(BoxBorder.Rounded)
                        .BorderColor(Color.Green)
                        .Padding(1, 1)
                );

            AnsiConsole.Write(Align.Center(table));
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[dim]📋 [[ 0 ]] Voltar ao menu principal[/]\n");
        }

        private static void ProcessarEscolha(string escolha)
        {
            Console.Clear();

            switch (escolha)
            {
                case "1":
                    OtimizarEnergia();
                    break;
                case "2":
                    DesativarEfeitosVisuais();
                    break;
                case "3":
                    OtimizarAltTab();
                    break;
                case "4":
                    DesativarTelemetria();
                    break;
                case "5":
                    DesativarServicosInuteis();
                    break;
                case "6":
                    Debloater();
                    break;
                case "7":
                    DesativarOverlays();
                    break;
                case "8":
                    DesativarUAC();
                    break;
                case "9":
                    DesativarHibernacao();
                    break;
                case "10":
                    DesativarIndexacao();
                    break;
                case "11":
                    DesativarHyperV();
                    break;
                case "12":
                    DesativarAeroPeek();
                    break;
                case "13":
                    DesativarMapsManager();
                    break;
                case "14":
                    DesativarSmartScreen();
                    break;
                case "0":
                    // Volta ao menu principal
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]❌ Opção inválida![/]");
                    break;
            }
        }

        // Opção 01: Otimizar Energia
        private static void OtimizarEnergia()
        {
            List<string> erros = new List<string>();
            
            AnsiConsole.Write(
                new FigletText("Otimizando Energia")
                    .LeftJustified()
                    .Color(Color.Yellow));

            AnsiConsole.Status()
                .Start("[yellow]⚡ Otimizando energia do PC...[/]", ctx => 
                {
                    try
                    {
                        ProcessStartInfo psi = new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            Arguments = "-Command \"powercfg -duplicatescheme e9a42b02-d5df-448d-aa00-03f14749eb61; powercfg.exe /setacvalueindex SCHEME_CURRENT SUB_PROCESSOR IdleDisable 0; powercfg.exe /setactive SCHEME_CURRENT; powercfg.cpl\"",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        };

                        using (Process powercfg = Process.Start(psi))
                        {
                            string stderr = powercfg.StandardError.ReadToEnd();
                            powercfg.WaitForExit();

                            if (!string.IsNullOrWhiteSpace(stderr))
                            {
                                AnsiConsole.MarkupLine("[red]❌ Erro ao aplicar CFG de otimização de energia[/]");
                                erros.Add(stderr);
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("[green]✓ Sucesso em aplicar CFG de otimização de energia[/]");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red]❌ Erro ao executar comando powercfg: {ex.Message}[/]");
                        erros.Add(ex.Message);
                    }
                });

            if (erros.Count > 0)
            {
                AnsiConsole.MarkupLine($"[red]❌ Ocorreu um erro: {string.Join(", ", erros)}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[green]✓ Otimização de Energia Completa com sucesso![/]");
            }
        }

        // Opção 02: Desativar Efeitos Visuais
        private static void DesativarEfeitosVisuais()
        {
            List<string> erros = new List<string>();
            
            AnsiConsole.Write(
                new FigletText("Efeitos Visuais")
                    .LeftJustified()
                    .Color(Color.Magenta1));

            AnsiConsole.MarkupLine("[yellow]⚡ Ajustando efeitos visuais para priorizar desempenho...[/]\n");

            var opcao = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[magenta1]Escolha a opção:[/]")
                    .AddChoices(new[] { 
                        "Desativar (Melhor Desempenho)", 
                        "Reverter (Padrão Windows)",
                        "Cancelar"
                    }));

            if (opcao == "Cancelar")
            {
                return;
            }

            bool desativar = opcao.StartsWith("Desativar");

            Console.Clear();
            AnsiConsole.Write(
                new FigletText(desativar ? "Desativando" : "Revertendo")
                    .LeftJustified()
                    .Color(Color.Yellow));

            AnsiConsole.Status()
                .Start($"[yellow]🔄 {(desativar ? "Desativando" : "Revertendo")} efeitos visuais...[/]", ctx => 
                {
                    try
                    {
                        // Passo 1: VisualFXSetting no Explorer\VisualEffects
                        ctx.Status($"[yellow]{(desativar ? "Mudando as configurações de efeitos visuais gerais para priorizar desempenho" : "Restaurando configurações padrão de efeitos visuais")}...[/]");
                        
                        string valorVisualFX = desativar ? "2" : "0"; // 2 = Melhor desempenho, 0 = Padrão
                        
                        ProcessStartInfo psiVisualFX = new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            Arguments = $@"-Command ""reg add 'HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects' /v VisualFXSetting /t REG_DWORD /d {valorVisualFX} /f""",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        };

                        using (Process efectVisual = Process.Start(psiVisualFX))
                        {
                            string stderr = efectVisual.StandardError.ReadToEnd();
                            efectVisual.WaitForExit();

                            if (efectVisual.ExitCode != 0)
                            {
                                AnsiConsole.MarkupLine($"[red]❌ Erro ao aplicar VisualFXSetting (Explorer): {stderr.Trim()}[/]");
                                erros.Add("VisualFXSetting (Explorer)");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine($"[green]✓ Efeitos visuais (Explorer) {(desativar ? "ajustados para desempenho" : "restaurados para padrão")}[/]");
                            }
                        }

                        // Passo 2: Transparência
                        ctx.Status($"[yellow]{(desativar ? "Desativando transparência (janelas, barra de tarefas)" : "Reativando transparência")}...[/]");
                        
                        string valorTransparencia = desativar ? "0" : "1"; // 0 = Desativado, 1 = Ativado
                        
                        ProcessStartInfo psiTransparency = new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            Arguments = $@"-Command ""reg add 'HKCU\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize' /v EnableTransparency /t REG_DWORD /d {valorTransparencia} /f""",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        };

                        using (Process desatTrans = Process.Start(psiTransparency))
                        {
                            string stderr = desatTrans.StandardError.ReadToEnd();
                            desatTrans.WaitForExit();

                            if (desatTrans.ExitCode != 0)
                            {
                                AnsiConsole.MarkupLine($"[red]❌ Erro ao {(desativar ? "desativar" : "ativar")} transparência: {stderr.Trim()}[/]");
                                erros.Add("EnableTransparency");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine($"[green]✓ Transparência {(desativar ? "desativada" : "ativada")} com sucesso[/]");
                            }
                        }

                        // Passo 3: UserPreferencesMask
                        if (desativar)
                        {
                            ctx.Status("[yellow]Aplicando máscara de preferências do usuário (desabilita várias animações/efeitos)...[/]");
                            
                            ProcessStartInfo psiUserPref = new ProcessStartInfo
                            {
                                FileName = "powershell.exe",
                                Arguments = @"-Command ""reg add 'HKCU\Control Panel\Desktop' /v UserPreferencesMask /t REG_BINARY /d 9012038010000000 /f""",
                                UseShellExecute = false,
                                RedirectStandardOutput = true,
                                RedirectStandardError = true,
                                CreateNoWindow = true
                            };

                            using (Process userPrefMask = Process.Start(psiUserPref))
                            {
                                string stderr = userPrefMask.StandardError.ReadToEnd();
                                userPrefMask.WaitForExit();

                                if (userPrefMask.ExitCode != 0)
                                {
                                    AnsiConsole.MarkupLine($"[red]❌ Erro ao definir UserPreferencesMask: {stderr.Trim()}[/]");
                                    erros.Add("UserPreferencesMask");
                                }
                                else
                                {
                                    AnsiConsole.MarkupLine("[green]✓ UserPreferencesMask aplicada com sucesso[/]");
                                }
                            }
                        }
                        else
                        {
                            ctx.Status("[yellow]Restaurando configurações padrão de animações...[/]");
                            
                            ProcessStartInfo psiUserPref = new ProcessStartInfo
                            {
                                FileName = "powershell.exe",
                                Arguments = @"-Command ""reg add 'HKCU\Control Panel\Desktop' /v UserPreferencesMask /t REG_BINARY /d 9E3E078012000000 /f""",
                                UseShellExecute = false,
                                RedirectStandardOutput = true,
                                RedirectStandardError = true,
                                CreateNoWindow = true
                            };

                            using (Process userPrefMask = Process.Start(psiUserPref))
                            {
                                string stderr = userPrefMask.StandardError.ReadToEnd();
                                userPrefMask.WaitForExit();

                                if (userPrefMask.ExitCode != 0)
                                {
                                    AnsiConsole.MarkupLine($"[red]❌ Erro ao restaurar UserPreferencesMask: {stderr.Trim()}[/]");
                                    erros.Add("UserPreferencesMask");
                                }
                                else
                                {
                                    AnsiConsole.MarkupLine("[green]✓ UserPreferencesMask restaurada com sucesso[/]");
                                }
                            }
                        }

                        // Passo 4: VisualFXSetting no Desktop
                        ctx.Status($"[yellow]{(desativar ? "Forçando ajuste de efeitos visuais para desempenho (nível Desktop)" : "Restaurando configurações padrão (nível Desktop)")}...[/]");
                        
                        ProcessStartInfo psiVisualFXDesktop = new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            Arguments = $@"-Command ""reg add 'HKCU\Control Panel\Desktop' /v VisualFXSetting /t REG_DWORD /d {valorVisualFX} /f""",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        };

                        using (Process visualFxDesktop = Process.Start(psiVisualFXDesktop))
                        {
                            string stderr = visualFxDesktop.StandardError.ReadToEnd();
                            visualFxDesktop.WaitForExit();

                            if (visualFxDesktop.ExitCode != 0)
                            {
                                AnsiConsole.MarkupLine($"[red]❌ Erro ao aplicar VisualFXSetting (Desktop): {stderr.Trim()}[/]");
                                erros.Add("VisualFXSetting (Desktop)");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine($"[green]✓ Efeitos visuais (Desktop) {(desativar ? "ajustados para desempenho" : "restaurados para padrão")}[/]");
                            }
                        }

                        // Passo 5: Reiniciar Explorer para aplicar imediatamente
                        ctx.Status("[yellow]Reiniciando Windows Explorer para aplicar as alterações...[/]");
                        
                        // Encerrar Explorer
                        ProcessStartInfo psiKillExplorer = new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            Arguments = "-Command \"Get-Process explorer -ErrorAction SilentlyContinue | Stop-Process -Force\"",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        };

                        using (Process killExplorer = Process.Start(psiKillExplorer))
                        {
                            killExplorer.WaitForExit();
                        }

                        // Aguardar 2 segundos
                        System.Threading.Thread.Sleep(2000);

                        // Reiniciar Explorer
                        ProcessStartInfo psiStartExplorer = new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            Arguments = "-Command \"Start-Process explorer.exe\"",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        };

                        using (Process startExplorer = Process.Start(psiStartExplorer))
                        {
                            startExplorer.WaitForExit();
                        }

                        AnsiConsole.MarkupLine("[green]✓ Windows Explorer reiniciado[/]");
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red]❌ Erro ao executar: {ex.Message}[/]");
                        erros.Add(ex.Message);
                    }
                });

            if (erros.Count > 0)
            {
                AnsiConsole.MarkupLine($"[red]❌ Ocorreu um erro ao executar: {string.Join(", ", erros)}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]✓ {(desativar ? "Desativação/Ajuste de efeitos visuais concluído" : "Efeitos visuais restaurados para o padrão")}![/]");
            }
        }

        // Opção 03: Otimizar ALT+TAB
        private static void OtimizarAltTab()
        {
            List<string> erros = new List<string>();
            
            AnsiConsole.Write(
                new FigletText("ALT + TAB")
                    .LeftJustified()
                    .Color(Color.Aqua));

            AnsiConsole.MarkupLine("[yellow]⚠️ ATENÇÃO: ESSA OTIMIZAÇÃO É RECOMENDADA APENAS PARA PCs FRACOS[/]\n");

            var opcao = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[aqua]Escolha a opção:[/]")
                    .AddChoices(new[] { 
                        "Otimizar (Modo Clássico)", 
                        "Reverter (Modo Moderno)",
                        "Cancelar"
                    }));

            if (opcao == "Cancelar")
            {
                return;
            }

            bool otimizar = opcao.StartsWith("Otimizar");

            Console.Clear();
            AnsiConsole.Write(
                new FigletText(otimizar ? "Otimizando" : "Revertendo")
                    .LeftJustified()
                    .Color(Color.Yellow));

            AnsiConsole.Status()
                .Start($"[yellow]🔄 {(otimizar ? "Otimizando" : "Revertendo")} ALT+TAB...[/]", ctx => 
                {
                    try
                    {
                        // Passo 1: Alterar configuração do Alt+Tab
                        ctx.Status($"[yellow]Alterando configuração do Alt+Tab para o modo {(otimizar ? "clássico" : "moderno")}...[/]");
                        
                        string comando = otimizar 
                            ? "Set-ItemProperty -Path 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer' -Name 'AltTabSettings' -Type DWord -Value 1"
                            : "Remove-ItemProperty -Path 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer' -Name 'AltTabSettings' -ErrorAction SilentlyContinue";

                        ProcessStartInfo psiAltTab = new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            Arguments = $"-Command \"{comando}\"",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        };

                        using (Process alteracaoTab = Process.Start(psiAltTab))
                        {
                            string stderr = alteracaoTab.StandardError.ReadToEnd();
                            alteracaoTab.WaitForExit();

                            if (alteracaoTab.ExitCode != 0 && !string.IsNullOrWhiteSpace(stderr))
                            {
                                AnsiConsole.MarkupLine($"[red]❌ Erro ao {(otimizar ? "alterar" : "reverter")} o Alt+Tab: {stderr.Trim()}[/]");
                                erros.Add("Alteração de AltTab");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("[green]✓ Alteração aplicada com sucesso[/]");
                            }
                        }

                        // Passo 2: Encerrar Windows Explorer
                        ctx.Status("[yellow]Encerrando Windows Explorer...[/]");
                        
                        ProcessStartInfo psiKillExplorer = new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            Arguments = "-Command \"Get-Process explorer -ErrorAction SilentlyContinue | Stop-Process -Force\"",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        };

                        using (Process encerraExpl = Process.Start(psiKillExplorer))
                        {
                            encerraExpl.WaitForExit();

                            if (encerraExpl.ExitCode != 0)
                            {
                                AnsiConsole.MarkupLine("[red]❌ Erro ao encerrar o Windows Explorer[/]");
                                erros.Add("Encerrar Windows Explorer");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("[green]✓ Windows Explorer encerrado com sucesso[/]");
                            }
                        }

                        // Passo 3: Aguardar 2 segundos
                        ctx.Status("[yellow]Aguardando...[/]");
                        System.Threading.Thread.Sleep(2000);

                        // Passo 4: Reiniciar Windows Explorer
                        ctx.Status("[yellow]Reiniciando Windows Explorer...[/]");
                        
                        ProcessStartInfo psiStartExplorer = new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            Arguments = "-Command \"Start-Process explorer.exe\"",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        };

                        using (Process reincExplo = Process.Start(psiStartExplorer))
                        {
                            reincExplo.WaitForExit();

                            if (reincExplo.ExitCode != 0)
                            {
                                AnsiConsole.MarkupLine("[red]❌ Erro ao reiniciar o Windows Explorer[/]");
                                erros.Add("Reiniciar Windows Explorer");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("[green]✓ Windows Explorer reiniciado com sucesso[/]");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red]❌ Erro ao executar: {ex.Message}[/]");
                        erros.Add(ex.Message);
                    }
                });

            if (erros.Count > 0)
            {
                AnsiConsole.MarkupLine($"[red]❌ Ocorreu um erro ao executar: {string.Join(", ", erros)}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[green]✓ Processo concluído com sucesso![/]");
            }
        }

        // Opção 04: Desativar Telemetria
        private static void DesativarTelemetria()
        {
            List<string> erros = new List<string>();
            
            AnsiConsole.Write(
                new FigletText("Telemetria")
                    .LeftJustified()
                    .Color(Color.Purple));

            AnsiConsole.MarkupLine("[cyan]Esta função altera políticas do Windows para melhorar a privacidade,[/]");
            AnsiConsole.MarkupLine("[cyan]desativando coleta de dados, anúncios e conexões automáticas com servidores da Microsoft.[/]\n");
            AnsiConsole.MarkupLine("[yellow]⚠️ Essa otimização é recomendada apenas se você deseja priorizar privacidade e desempenho.[/]\n");

            var opcao = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[purple]Escolha a opção:[/]")
                    .AddChoices(new[] { 
                        "Desativar Telemetria e Coleta de Dados", 
                        "Reverter (Restaurar configurações originais)",
                        "Cancelar"
                    }));

            if (opcao == "Cancelar")
            {
                return;
            }

            bool desativar = opcao.StartsWith("Desativar");

            Console.Clear();
            AnsiConsole.Write(
                new FigletText(desativar ? "Desativando" : "Revertendo")
                    .LeftJustified()
                    .Color(Color.Yellow));

            AnsiConsole.Status()
                .Start($"[yellow]🔄 {(desativar ? "Desativando" : "Revertendo")} telemetria...[/]", ctx => 
                {
                    try
                    {
                        // Passo 1: AllowTelemetry
                        ctx.Status($"[yellow]{(desativar ? "Desativando" : "Reativando")} coleta de dados (AllowTelemetry)...[/]");
                        
                        string cmd1 = desativar 
                            ? @"REG ADD ""HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection"" /v ""AllowTelemetry"" /t REG_DWORD /d 0 /f"
                            : @"REG ADD ""HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection"" /v ""AllowTelemetry"" /t REG_DWORD /d 3 /f";

                        ProcessStartInfo psiTelemetry = new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            Arguments = $"-Command \"{cmd1}\"",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        };

                        using (Process telemetria = Process.Start(psiTelemetry))
                        {
                            string stderr = telemetria.StandardError.ReadToEnd();
                            telemetria.WaitForExit();

                            if (telemetria.ExitCode != 0)
                            {
                                AnsiConsole.MarkupLine($"[red]❌ Erro ao {(desativar ? "desativar" : "reativar")} Telemetria: {stderr.Trim()}[/]");
                                erros.Add("AllowTelemetry");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine($"[green]✓ Telemetria {(desativar ? "desativada" : "reativada")} com sucesso[/]");
                            }
                        }

                        // Passo 2: AllowAppDataCollection
                        ctx.Status($"[yellow]{(desativar ? "Desativando" : "Reativando")} coleta de dados de aplicativos (AllowAppDataCollection)...[/]");
                        
                        string cmd2 = desativar 
                            ? @"REG ADD ""HKLM\SOFTWARE\Policies\Microsoft\Windows\System"" /v ""AllowAppDataCollection"" /t REG_DWORD /d 0 /f"
                            : @"REG DELETE ""HKLM\SOFTWARE\Policies\Microsoft\Windows\System"" /v ""AllowAppDataCollection"" /f";

                        ProcessStartInfo psiAppData = new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            Arguments = $"-Command \"{cmd2}\"",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        };

                        using (Process appData = Process.Start(psiAppData))
                        {
                            string stderr = appData.StandardError.ReadToEnd();
                            appData.WaitForExit();

                            if (appData.ExitCode != 0 && desativar)
                            {
                                AnsiConsole.MarkupLine($"[red]❌ Erro ao {(desativar ? "desativar" : "reativar")} coleta de dados de aplicativos: {stderr.Trim()}[/]");
                                erros.Add("AllowAppDataCollection");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine($"[green]✓ Coleta de dados de aplicativos {(desativar ? "desativada" : "reativada")} com sucesso[/]");
                            }
                        }

                        // Passo 3: DisableWindowsAdvertising
                        ctx.Status($"[yellow]{(desativar ? "Bloqueando" : "Reativando")} anúncios e personalização (DisableWindowsAdvertising)...[/]");
                        
                        string cmd3 = desativar 
                            ? @"REG ADD ""HKLM\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo"" /v ""DisableWindowsAdvertising"" /t REG_DWORD /d 1 /f"
                            : @"REG DELETE ""HKLM\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo"" /v ""DisableWindowsAdvertising"" /f";

                        ProcessStartInfo psiAds = new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            Arguments = $"-Command \"{cmd3}\"",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        };

                        using (Process ads = Process.Start(psiAds))
                        {
                            string stderr = ads.StandardError.ReadToEnd();
                            ads.WaitForExit();

                            if (ads.ExitCode != 0 && desativar)
                            {
                                AnsiConsole.MarkupLine($"[red]❌ Erro ao {(desativar ? "desativar" : "reativar")} anúncios: {stderr.Trim()}[/]");
                                erros.Add("DisableWindowsAdvertising");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine($"[green]✓ Publicidade e rastreamento {(desativar ? "desativados" : "reativados")} com sucesso[/]");
                            }
                        }

                        // Passo 4: DisableMicrosoftConsumerExperience
                        ctx.Status($"[yellow]{(desativar ? "Desativando" : "Reativando")} experiências do consumidor (DisableMicrosoftConsumerExperience)...[/]");
                        
                        string cmd4 = desativar 
                            ? @"REG ADD ""HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent"" /v ""DisableMicrosoftConsumerExperience"" /t REG_DWORD /d 1 /f"
                            : @"REG DELETE ""HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent"" /v ""DisableMicrosoftConsumerExperience"" /f";

                        ProcessStartInfo psiConsumer = new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            Arguments = $"-Command \"{cmd4}\"",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        };

                        using (Process consumerExp = Process.Start(psiConsumer))
                        {
                            string stderr = consumerExp.StandardError.ReadToEnd();
                            consumerExp.WaitForExit();

                            if (consumerExp.ExitCode != 0 && desativar)
                            {
                                AnsiConsole.MarkupLine($"[red]❌ Erro ao {(desativar ? "desativar" : "reativar")} experiência do consumidor: {stderr.Trim()}[/]");
                                erros.Add("DisableMicrosoftConsumerExperience");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine($"[green]✓ Experiências do consumidor {(desativar ? "desativadas" : "reativadas")} com sucesso[/]");
                            }
                        }

                        // Passo 5: DoNotConnectToWindowsUpdateInternetLocations
                        ctx.Status($"[yellow]{(desativar ? "Impedindo" : "Reativando")} conexão com servidores da Microsoft (DoNotConnectToWindowsUpdateInternetLocations)...[/]");
                        
                        string cmd5 = desativar 
                            ? @"REG ADD ""HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"" /v ""DoNotConnectToWindowsUpdateInternetLocations"" /t REG_DWORD /d 1 /f"
                            : @"REG DELETE ""HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"" /v ""DoNotConnectToWindowsUpdateInternetLocations"" /f";

                        ProcessStartInfo psiWinUpdate = new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            Arguments = $"-Command \"{cmd5}\"",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        };

                        using (Process winUpdate = Process.Start(psiWinUpdate))
                        {
                            string stderr = winUpdate.StandardError.ReadToEnd();
                            winUpdate.WaitForExit();

                            if (winUpdate.ExitCode != 0 && desativar)
                            {
                                AnsiConsole.MarkupLine($"[red]❌ Erro ao {(desativar ? "desativar" : "reativar")} conexão com Windows Update: {stderr.Trim()}[/]");
                                erros.Add("DoNotConnectToWindowsUpdateInternetLocations");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine($"[green]✓ Conexões automáticas com Windows Update {(desativar ? "desativadas" : "reativadas")} com sucesso[/]");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red]❌ Erro ao executar: {ex.Message}[/]");
                        erros.Add(ex.Message);
                    }
                });

            if (erros.Count > 0)
            {
                AnsiConsole.MarkupLine($"[red]❌ Falha parcial — comandos com erro: {string.Join(", ", erros)}[/]");
            }
            else
            {
                if (desativar)
                {
                    AnsiConsole.MarkupLine("[green]✓ Telemetria e coleta de dados desativadas com sucesso![/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[green]✓ Configurações de telemetria restauradas para o padrão original![/]");
                }
            }
        }

        // Opção 05: Desativar Serviços Inúteis
        private static void DesativarServicosInuteis()
        {
            List<string> erros = new List<string>();
            
            AnsiConsole.Write(
                new FigletText("Servicos Windows")
                    .LeftJustified()
                    .Color(Color.Orange1));

            AnsiConsole.MarkupLine("[cyan]Esta função desativa ou restaura serviços do Windows[/]");
            AnsiConsole.MarkupLine("[cyan]para melhorar o desempenho e reduzir consumo de recursos.[/]\n");
            AnsiConsole.MarkupLine("[yellow]⚠️ Use com cautela: alguns serviços desativados podem afetar recursos[/]");
            AnsiConsole.MarkupLine("[yellow]   como impressão, diagnósticos ou atualizações.[/]\n");

            var opcao = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[orange1]Escolha a opção:[/]")
                    .AddChoices(new[] { 
                        "Desativar Serviços", 
                        "Reverter Otimização",
                        "Cancelar"
                    }));

            if (opcao == "Cancelar")
            {
                return;
            }

            bool desativar = opcao.StartsWith("Desativar");

            Console.Clear();
            AnsiConsole.Write(
                new FigletText(desativar ? "Desativando" : "Revertendo")
                    .LeftJustified()
                    .Color(Color.Yellow));

            // Verificação do utilitário sc.exe
            AnsiConsole.MarkupLine("[yellow]🔍 Verificando utilitário do Windows (sc.exe)...[/]");
            
            string scPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "sc.exe");
            if (!File.Exists(scPath))
            {
                AnsiConsole.MarkupLine("[red]❌ O utilitário 'sc.exe' não foi encontrado no sistema![/]");
                AnsiConsole.MarkupLine("[yellow]⚠️ Verifique se o Windows está instalado corretamente ou se há restrições de PATH.[/]");
                return;
            }
            else
            {
                AnsiConsole.MarkupLine("[green]✓ Utilitário 'sc.exe' encontrado e disponível.[/]\n");
            }

            // Definição dos serviços
            Dictionary<string, string> servicos = desativar 
                ? new Dictionary<string, string>
                {
                    { "Spooler", "disabled" },           // Spooler de impressão
                    { "wisvc", "disabled" },             // Windows Insider Service
                    { "WerSvc", "disabled" },            // Relatório de Erros do Windows
                    { "WbioSrvc", "disabled" },          // Serviço de Biometria
                    { "DiagTrack", "disabled" },         // Telemetria
                    { "dmwappushservice", "disabled" },  // Push de notificações
                    { "wuauserv", "disabled" },          // Windows Update
                    { "dosvc", "disabled" }              // Otimização de Entrega
                }
                : new Dictionary<string, string>
                {
                    { "Spooler", "auto" },               // Automático
                    { "wisvc", "demand" },               // Manual
                    { "WerSvc", "demand" },              // Manual
                    { "WbioSrvc", "demand" },            // Manual
                    { "DiagTrack", "demand" },           // Manual
                    { "dmwappushservice", "demand" },    // Manual
                    { "wuauserv", "auto" },              // Automático
                    { "dosvc", "demand" }                // Manual
                };

            AnsiConsole.Status()
                .Start($"[yellow]🔄 {(desativar ? "Desativando" : "Revertendo")} serviços...[/]", ctx => 
                {
                    try
                    {
                        int passo = 1;
                        foreach (var servico in servicos)
                        {
                            string nome = servico.Key;
                            string modo = servico.Value;

                            if (desativar)
                            {
                                // Parar o serviço primeiro
                                ctx.Status($"[yellow]Parando serviço {nome}...[/]");
                                
                                ProcessStartInfo psiStop = new ProcessStartInfo
                                {
                                    FileName = "sc.exe",
                                    Arguments = $"stop {nome}",
                                    UseShellExecute = false,
                                    RedirectStandardOutput = true,
                                    RedirectStandardError = true,
                                    CreateNoWindow = true
                                };

                                using (Process stopProcess = Process.Start(psiStop))
                                {
                                    stopProcess.WaitForExit();
                                    
                                    if (stopProcess.ExitCode != 0)
                                    {
                                        AnsiConsole.MarkupLine($"[yellow]⚠️ Serviço {nome} pode já estar parado ou indisponível.[/]");
                                    }
                                    else
                                    {
                                        AnsiConsole.MarkupLine($"[green]✓ Serviço {nome} parado com sucesso[/]");
                                    }
                                }

                                // Configurar o serviço
                                ctx.Status($"[yellow]Configurando serviço {nome} para {modo}...[/]");
                                
                                ProcessStartInfo psiConfig = new ProcessStartInfo
                                {
                                    FileName = "sc.exe",
                                    Arguments = $"config {nome} start= {modo}",
                                    UseShellExecute = false,
                                    RedirectStandardOutput = true,
                                    RedirectStandardError = true,
                                    CreateNoWindow = true
                                };

                                using (Process configProcess = Process.Start(psiConfig))
                                {
                                    string stderr = configProcess.StandardError.ReadToEnd();
                                    configProcess.WaitForExit();

                                    if (configProcess.ExitCode != 0)
                                    {
                                        AnsiConsole.MarkupLine($"[red]❌ Erro ao configurar {nome}: {stderr.Trim()}[/]");
                                        erros.Add(nome);
                                    }
                                    else
                                    {
                                        AnsiConsole.MarkupLine($"[green]✓ {nome} configurado para {modo} com sucesso[/]");
                                    }
                                }
                            }
                            else // Reverter
                            {
                                // Reconfigurar o serviço
                                ctx.Status($"[yellow]Reconfigurando serviço {nome} para {modo}...[/]");
                                
                                ProcessStartInfo psiConfig = new ProcessStartInfo
                                {
                                    FileName = "sc.exe",
                                    Arguments = $"config {nome} start= {modo}",
                                    UseShellExecute = false,
                                    RedirectStandardOutput = true,
                                    RedirectStandardError = true,
                                    CreateNoWindow = true
                                };

                                using (Process configProcess = Process.Start(psiConfig))
                                {
                                    string stderr = configProcess.StandardError.ReadToEnd();
                                    configProcess.WaitForExit();

                                    if (configProcess.ExitCode != 0)
                                    {
                                        AnsiConsole.MarkupLine($"[red]❌ Erro ao reconfigurar {nome}: {stderr.Trim()}[/]");
                                        erros.Add(nome);
                                        continue;
                                    }
                                    else
                                    {
                                        AnsiConsole.MarkupLine($"[green]✓ Serviço {nome} configurado para {modo} com sucesso[/]");
                                    }
                                }

                                // Iniciar o serviço
                                ctx.Status($"[yellow]Iniciando serviço {nome}...[/]");
                                
                                ProcessStartInfo psiStart = new ProcessStartInfo
                                {
                                    FileName = "sc.exe",
                                    Arguments = $"start {nome}",
                                    UseShellExecute = false,
                                    RedirectStandardOutput = true,
                                    RedirectStandardError = true,
                                    CreateNoWindow = true
                                };

                                using (Process startProcess = Process.Start(psiStart))
                                {
                                    startProcess.WaitForExit();
                                    
                                    if (startProcess.ExitCode != 0)
                                    {
                                        AnsiConsole.MarkupLine($"[yellow]⚠️ Serviço {nome} não pôde ser iniciado (pode estar desnecessário ou já desativado).[/]");
                                    }
                                    else
                                    {
                                        AnsiConsole.MarkupLine($"[green]✓ Serviço {nome} iniciado com sucesso[/]");
                                    }
                                }
                            }

                            passo++;
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red]❌ Erro ao executar: {ex.Message}[/]");
                        erros.Add(ex.Message);
                    }
                });

            if (erros.Count > 0)
            {
                AnsiConsole.MarkupLine($"[red]❌ Falha parcial — serviços com erro: {string.Join(", ", erros)}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]✓ {(desativar ? "Desativação de serviços concluída" : "Serviços restaurados para o padrão")}![/]");
            }
        }

        // Opção 06: Debloater
        private static void Debloater()
        {
            List<string> erros = new List<string>();
            
            AnsiConsole.Write(
                new FigletText("Debloater")
                    .LeftJustified()
                    .Color(Color.Red));

            AnsiConsole.MarkupLine("[cyan]Remove aplicativos padrão do Windows e desativa recursos desnecessários[/]");
            AnsiConsole.MarkupLine("[cyan]como Copilot, Cortana, OfficeHub e outros bloatware.[/]\n");
            AnsiConsole.MarkupLine("[yellow]⚠️ ATENÇÃO: Esta operação remove apps do sistema. Use com cautela![/]\n");

            var opcao = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]Escolha a opção:[/]")
                    .AddChoices(new[] { 
                        "Remover Apps Padrão", 
                        "Reinstalar Apps Padrão",
                        "Cancelar"
                    }));

            if (opcao == "Cancelar")
            {
                return;
            }

            bool remover = opcao.StartsWith("Remover");

            Console.Clear();
            AnsiConsole.Write(
                new FigletText(remover ? "Removendo" : "Reinstalando")
                    .LeftJustified()
                    .Color(Color.Yellow));

            if (remover)
            {
                AnsiConsole.Status()
                    .Start("[yellow]🗑️ Removendo aplicativos...[/]", ctx => 
                    {
                        try
                        {
                            string[] apps = new string[]
                            {
                                "*Microsoft.Windows.Cortana*",
                                "*Microsoft.MicrosoftOfficeHub*",
                                "*Microsoft.YourPhone*",
                                "*Microsoft.Messaging*",
                                "*Microsoft.WindowsMaps*",
                                "*Microsoft.ZuneMusic*",
                                "*Microsoft.Getstarted*",
                                "*microsoft.windowscommunicationsapps*",
                                "*Microsoft.WindowsAlarms*",
                                "*Microsoft.3DBuilder*",
                                "*Microsoft.BingNews*",
                                "*Microsoft.OneDriveSync*"
                            };

                            string[] nomes = new string[]
                            {
                                "Cortana", "OfficeHub", "Phone Link", "Mensagens",
                                "Mapas", "Groove Music", "Get Started", "Mail e Calendar",
                                "Alarmes", "3D Builder", "Bing News", "OneDrive"
                            };

                            for (int i = 0; i < apps.Length; i++)
                            {
                                ctx.Status($"[yellow]Removendo {nomes[i]}...[/]");
                                
                                ProcessStartInfo psi = new ProcessStartInfo
                                {
                                    FileName = "powershell.exe",
                                    Arguments = $"-Command \"Get-AppxPackage {apps[i]} | Remove-AppxPackage -ErrorAction SilentlyContinue\"",
                                    UseShellExecute = false,
                                    RedirectStandardError = true,
                                    CreateNoWindow = true
                                };

                                using (Process proc = Process.Start(psi))
                                {
                                    string stderr = proc.StandardError.ReadToEnd();
                                    proc.WaitForExit();

                                    if (!string.IsNullOrWhiteSpace(stderr))
                                    {
                                        AnsiConsole.MarkupLine($"[yellow]⚠️ {nomes[i]} - pode não estar instalado[/]");
                                    }
                                    else
                                    {
                                        AnsiConsole.MarkupLine($"[green]✓ {nomes[i]} removido[/]");
                                    }
                                }
                            }

                            // Desativar Copilot
                            ctx.Status("[yellow]Desativando Copilot...[/]");
                            
                            string[] comandosCopilot = new string[]
                            {
                                @"reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"" /v ShowCopilotButton /t REG_DWORD /d 0 /f",
                                @"reg add ""HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Copilot"" /v TurnOffWindowsCopilot /t REG_DWORD /d 1 /f",
                                @"reg add ""HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search"" /v AllowCortana /t REG_DWORD /d 0 /f",
                                @"reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"" /v SystemPaneSuggestionsEnabled /t REG_DWORD /d 0 /f"
                            };

                            foreach (var cmd in comandosCopilot)
                            {
                                ProcessStartInfo psiReg = new ProcessStartInfo
                                {
                                    FileName = "powershell.exe",
                                    Arguments = $"-Command \"{cmd}\"",
                                    UseShellExecute = false,
                                    CreateNoWindow = true
                                };

                                using (Process proc = Process.Start(psiReg))
                                {
                                    proc.WaitForExit();
                                }
                            }

                            AnsiConsole.MarkupLine("[green]✓ Copilot e Cortana desativados[/]");
                        }
                        catch (Exception ex)
                        {
                            AnsiConsole.MarkupLine($"[red]❌ Erro: {ex.Message}[/]");
                            erros.Add(ex.Message);
                        }
                    });
            }
            else
            {
                AnsiConsole.Status()
                    .Start("[yellow]🔄 Reinstalando aplicativos...[/]", ctx => 
                    {
                        try
                        {
                            ProcessStartInfo psi = new ProcessStartInfo
                            {
                                FileName = "powershell.exe",
                                Arguments = "-Command \"Get-AppxPackage -AllUsers | Foreach {Add-AppxPackage -DisableDevelopmentMode -Register \\\"$($_.InstallLocation + '\\\\AppXManifest.xml')\\\" -ErrorAction SilentlyContinue}\"",
                                UseShellExecute = false,
                                RedirectStandardError = true,
                                CreateNoWindow = true
                            };

                            using (Process proc = Process.Start(psi))
                            {
                                string stderr = proc.StandardError.ReadToEnd();
                                proc.WaitForExit();

                                if (!string.IsNullOrWhiteSpace(stderr))
                                {
                                    AnsiConsole.MarkupLine("[yellow]⚠️ Alguns apps podem não ter sido reinstalados[/]");
                                }
                                else
                                {
                                    AnsiConsole.MarkupLine("[green]✓ Apps reinstalados com sucesso[/]");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            AnsiConsole.MarkupLine($"[red]❌ Erro: {ex.Message}[/]");
                            erros.Add(ex.Message);
                        }
                    });
            }

            if (erros.Count > 0)
            {
                AnsiConsole.MarkupLine($"[red]❌ Ocorreram erros: {string.Join(", ", erros)}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]✓ {(remover ? "Remoção" : "Reinstalação")} concluída com sucesso![/]");
            }
        }

        // Opção 07: Desativar Overlays
        private static void DesativarOverlays()
        {
            List<string> erros = new List<string>();
            
            AnsiConsole.Write(
                new FigletText("Overlays")
                    .LeftJustified()
                    .Color(Color.Cyan1));

            AnsiConsole.MarkupLine("[cyan]Desativa overlays de jogos como Game Bar e Game Mode[/]");
            AnsiConsole.MarkupLine("[cyan]que podem afetar o desempenho durante jogos.[/]\n");

            var opcao = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[cyan1]Escolha a opção:[/]")
                    .AddChoices(new[] { 
                        "Desativar Overlays", 
                        "Reverter (Reativar)",
                        "Cancelar"
                    }));

            if (opcao == "Cancelar")
            {
                return;
            }

            bool desativar = opcao.StartsWith("Desativar");

            Console.Clear();
            AnsiConsole.Write(
                new FigletText(desativar ? "Desativando" : "Reativando")
                    .LeftJustified()
                    .Color(Color.Yellow));

            AnsiConsole.Status()
                .Start($"[yellow]🎮 {(desativar ? "Desativando" : "Reativando")} overlays...[/]", ctx => 
                {
                    try
                    {
                        string valor = desativar ? "0" : "1";
                        
                        string[] comandos = new string[]
                        {
                            $@"reg add ""HKCU\Software\Microsoft\GameBar"" /v ""AllowAutoGameMode"" /t REG_DWORD /d {valor} /f",
                            $@"reg add ""HKCU\Software\Microsoft\GameBar"" /v ""AutoGameModeEnabled"" /t REG_DWORD /d {valor} /f",
                            $@"reg add ""HKCU\Software\Microsoft\GameBar"" /v ""ShowStartupPanel"" /t REG_DWORD /d {valor} /f",
                            $@"reg add ""HKCU\System\GameConfigStore"" /v ""GameDVR_Enabled"" /t REG_DWORD /d {valor} /f",
                            $@"reg add ""HKCU\Software\Microsoft\GameBar"" /v ""GamePanelStartupTipIndex"" /t REG_DWORD /d {valor} /f"
                        };

                        string[] descricoes = new string[]
                        {
                            "AllowAutoGameMode", "AutoGameModeEnabled", "ShowStartupPanel",
                            "GameDVR", "Xbox Game Bar"
                        };

                        for (int i = 0; i < comandos.Length; i++)
                        {
                            ctx.Status($"[yellow]{(desativar ? "Desativando" : "Reativando")} {descricoes[i]}...[/]");
                            
                            ProcessStartInfo psi = new ProcessStartInfo
                            {
                                FileName = "powershell.exe",
                                Arguments = $"-Command \"{comandos[i]}\"",
                                UseShellExecute = false,
                                RedirectStandardError = true,
                                CreateNoWindow = true
                            };

                            using (Process proc = Process.Start(psi))
                            {
                                string stderr = proc.StandardError.ReadToEnd();
                                proc.WaitForExit();

                                if (proc.ExitCode != 0)
                                {
                                    AnsiConsole.MarkupLine($"[red]❌ Erro em {descricoes[i]}: {stderr.Trim()}[/]");
                                    erros.Add(descricoes[i]);
                                }
                                else
                                {
                                    AnsiConsole.MarkupLine($"[green]✓ {descricoes[i]} {(desativar ? "desativado" : "reativado")}[/]");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red]❌ Erro: {ex.Message}[/]");
                        erros.Add(ex.Message);
                    }
                });

            if (erros.Count > 0)
            {
                AnsiConsole.MarkupLine($"[red]❌ Falha parcial: {string.Join(", ", erros)}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]✓ Overlays {(desativar ? "desativados" : "reativados")} com sucesso![/]");
            }
        }

        // Opção 08: Desativar UAC
        private static void DesativarUAC()
        {
            List<string> erros = new List<string>();
            
            AnsiConsole.Write(
                new FigletText("UAC")
                    .LeftJustified()
                    .Color(Color.Orange1));

            AnsiConsole.MarkupLine("[cyan]Desativa o Controle de Conta de Usuário (UAC)[/]");
            AnsiConsole.MarkupLine("[cyan]que exibe prompts de confirmação para ações administrativas.[/]\n");
            AnsiConsole.MarkupLine("[yellow]⚠️ ATENÇÃO: Desativar o UAC reduz a segurança do sistema![/]\n");

            var opcao = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[orange1]Escolha a opção:[/]")
                    .AddChoices(new[] { 
                        "Desativar UAC", 
                        "Reverter (Reativar UAC)",
                        "Cancelar"
                    }));

            if (opcao == "Cancelar")
            {
                return;
            }

            bool desativar = opcao.StartsWith("Desativar");

            Console.Clear();
            AnsiConsole.Write(
                new FigletText(desativar ? "Desativando" : "Reativando")
                    .LeftJustified()
                    .Color(Color.Yellow));

            Console.WriteLine();
            AnsiConsole.MarkupLine("[cyan]═══════════════════════════════════════════════════════════════════════════════[/]");
            
            try
            {
                // Verificação de integridade
                AnsiConsole.MarkupLine("[yellow]🔍 Verificando integridade do sistema (sfc /scannow)...[/]");
                AnsiConsole.WriteLine();
                
                ProcessStartInfo psiSfc = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c sfc /scannow",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (Process sfc = Process.Start(psiSfc))
                {
                    // Ler saída em tempo real
                    using (var reader = sfc.StandardOutput)
                    {
                        string linha;
                        while ((linha = reader.ReadLine()) != null)
                        {
                            // Destacar linhas importantes
                            if (linha.Contains("Scanning", StringComparison.OrdinalIgnoreCase) || 
                                linha.Contains("Found", StringComparison.OrdinalIgnoreCase) ||
                                linha.Contains("Verifying", StringComparison.OrdinalIgnoreCase) ||
                                linha.Contains("Repairing", StringComparison.OrdinalIgnoreCase))
                            {
                                AnsiConsole.MarkupLine($"[blue]{linha}[/]");
                            }
                            else if (linha.Contains("100%", StringComparison.OrdinalIgnoreCase) ||
                                     linha.Contains("completed", StringComparison.OrdinalIgnoreCase))
                            {
                                AnsiConsole.MarkupLine($"[green]{linha}[/]");
                            }
                            else if (linha.Contains("Error", StringComparison.OrdinalIgnoreCase) ||
                                     linha.Contains("failed", StringComparison.OrdinalIgnoreCase))
                            {
                                AnsiConsole.MarkupLine($"[red]{linha}[/]");
                            }
                            else if (!string.IsNullOrWhiteSpace(linha))
                            {
                                Console.WriteLine(linha);
                            }
                        }
                    }
                    
                    sfc.WaitForExit();
                    
                    if (sfc.ExitCode == 0)
                    {
                        AnsiConsole.MarkupLine("\n[green]✓ Verificação de integridade concluída com sucesso![/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"\n[yellow]⚠️ Verificação de integridade concluída com código de saída: {sfc.ExitCode}[/]");
                    }
                }

                Console.WriteLine();
                AnsiConsole.MarkupLine("[cyan]═══════════════════════════════════════════════════════════════════════════════[/]");
                AnsiConsole.WriteLine();

                // Alterar UAC
                AnsiConsole.MarkupLine($"[yellow]🔒 {(desativar ? "Desativando" : "Reativando")} UAC...[/]");
                AnsiConsole.WriteLine();
                
                string valor = desativar ? "0" : "1";
                string cmd = $@"reg add ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"" /v EnableLUA /t REG_DWORD /d {valor} /f";
                
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {cmd}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (Process proc = Process.Start(psi))
                {
                    // Ler saída em tempo real
                    using (var reader = proc.StandardOutput)
                    {
                        string linha;
                        while ((linha = reader.ReadLine()) != null)
                        {
                            if (!string.IsNullOrWhiteSpace(linha))
                            {
                                AnsiConsole.MarkupLine($"[blue]{linha}[/]");
                            }
                        }
                    }

                    string stderr = proc.StandardError.ReadToEnd();
                    proc.WaitForExit();

                    if (proc.ExitCode != 0)
                    {
                        AnsiConsole.MarkupLine($"[red]❌ Erro ao {(desativar ? "desativar" : "reativar")} UAC: {stderr.Trim()}[/]");
                        erros.Add("EnableLUA");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[green]✓ UAC {(desativar ? "desativado" : "reativado")} com sucesso[/]");
                    }
                }

                Console.WriteLine();
                AnsiConsole.MarkupLine("[cyan]═══════════════════════════════════════════════════════════════════════════════[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Erro: {ex.Message}[/]");
                erros.Add(ex.Message);
            }

            Console.WriteLine();
            if (erros.Count > 0)
            {
                AnsiConsole.MarkupLine($"[red]❌ Ocorreram erros: {string.Join(", ", erros)}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]✓ Operação concluída! Reinicie o PC para aplicar as mudanças.[/]");
            }
        }

        // Opção 09: Desativar Hibernação
        private static void DesativarHibernacao()
        {
            List<string> erros = new List<string>();
            
            AnsiConsole.Write(
                new FigletText("Hibernacao")
                    .LeftJustified()
                    .Color(Color.Yellow));

            AnsiConsole.MarkupLine("[cyan]Desativa a hibernação do Windows e libera espaço em disco[/]");
            AnsiConsole.MarkupLine("[cyan]removendo o arquivo hiberfil.sys.[/]\n");

            var opcao = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Escolha a opção:[/]")
                    .AddChoices(new[] { 
                        "Desativar Hibernação", 
                        "Ativar Hibernação",
                        "Cancelar"
                    }));

            if (opcao == "Cancelar")
            {
                return;
            }

            bool desativar = opcao.StartsWith("Desativar");

            Console.Clear();
            AnsiConsole.Write(
                new FigletText(desativar ? "Desativando" : "Ativando")
                    .LeftJustified()
                    .Color(Color.Yellow));

            AnsiConsole.Status()
                .Start($"[yellow]💤 {(desativar ? "Desativando" : "Ativando")} hibernação...[/]", ctx => 
                {
                    try
                    {
                        ProcessStartInfo psi = new ProcessStartInfo
                        {
                            FileName = "powercfg",
                            Arguments = desativar ? "/hibernate off" : "/hibernate on",
                            UseShellExecute = false,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        };

                        using (Process proc = Process.Start(psi))
                        {
                            string stderr = proc.StandardError.ReadToEnd();
                            proc.WaitForExit();

                            if (proc.ExitCode != 0)
                            {
                                AnsiConsole.MarkupLine($"[red]❌ Erro ao {(desativar ? "desativar" : "ativar")} hibernação: {stderr.Trim()}[/]");
                                erros.Add("Hibernação");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine($"[green]✓ Hibernação {(desativar ? "desativada" : "ativada")} com sucesso[/]");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red]❌ Erro: {ex.Message}[/]");
                        erros.Add(ex.Message);
                    }
                });

            if (erros.Count > 0)
            {
                AnsiConsole.MarkupLine($"[red]❌ Ocorreram erros: {string.Join(", ", erros)}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]✓ Hibernação {(desativar ? "desativada" : "ativada")} com sucesso![/]");
            }
        }

        // Opção 10: Desativar Indexação
        private static void DesativarIndexacao()
        {
            List<string> erros = new List<string>();
            
            AnsiConsole.Write(
                new FigletText("Indexacao")
                    .LeftJustified()
                    .Color(Color.Blue));

            AnsiConsole.MarkupLine("[cyan]Desativa o serviço Windows Search que indexa arquivos[/]");
            AnsiConsole.MarkupLine("[cyan]para buscas mais rápidas, mas consome recursos.[/]\n");

            var opcao = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Escolha a opção:[/]")
                    .AddChoices(new[] { 
                        "Desativar Indexação", 
                        "Ativar Indexação",
                        "Cancelar"
                    }));

            if (opcao == "Cancelar")
            {
                return;
            }

            bool desativar = opcao.StartsWith("Desativar");

            Console.Clear();
            AnsiConsole.Write(
                new FigletText(desativar ? "Desativando" : "Ativando")
                    .LeftJustified()
                    .Color(Color.Yellow));

            AnsiConsole.Status()
                .Start($"[yellow]🔍 {(desativar ? "Desativando" : "Ativando")} indexação...[/]", ctx => 
                {
                    try
                    {
                        if (desativar)
                        {
                            // Parar serviço
                            ctx.Status("[yellow]Parando serviço Windows Search...[/]");
                            
                            ProcessStartInfo psiStop = new ProcessStartInfo
                            {
                                FileName = "sc.exe",
                                Arguments = "stop WSearch",
                                UseShellExecute = false,
                                RedirectStandardError = true,
                                CreateNoWindow = true
                            };

                            using (Process stop = Process.Start(psiStop))
                            {
                                stop.WaitForExit();
                                AnsiConsole.MarkupLine("[green]✓ Serviço parado[/]");
                            }

                            // Desativar serviço
                            ctx.Status("[yellow]Desativando serviço...[/]");
                            
                            ProcessStartInfo psiConfig = new ProcessStartInfo
                            {
                                FileName = "sc.exe",
                                Arguments = "config WSearch start= disabled",
                                UseShellExecute = false,
                                RedirectStandardError = true,
                                CreateNoWindow = true
                            };

                            using (Process config = Process.Start(psiConfig))
                            {
                                string stderr = config.StandardError.ReadToEnd();
                                config.WaitForExit();

                                if (config.ExitCode != 0)
                                {
                                    AnsiConsole.MarkupLine($"[red]❌ Erro ao desativar: {stderr.Trim()}[/]");
                                    erros.Add("WSearch");
                                }
                                else
                                {
                                    AnsiConsole.MarkupLine("[green]✓ Indexação desativada com sucesso[/]");
                                }
                            }
                        }
                        else
                        {
                            // Reativar serviço
                            ctx.Status("[yellow]Reativando serviço...[/]");
                            
                            ProcessStartInfo psiConfig = new ProcessStartInfo
                            {
                                FileName = "sc.exe",
                                Arguments = "config WSearch start= auto",
                                UseShellExecute = false,
                                RedirectStandardError = true,
                                CreateNoWindow = true
                            };

                            using (Process config = Process.Start(psiConfig))
                            {
                                config.WaitForExit();
                                AnsiConsole.MarkupLine("[green]✓ Serviço configurado para automático[/]");
                            }

                            // Iniciar serviço
                            ctx.Status("[yellow]Iniciando serviço Windows Search...[/]");
                            
                            ProcessStartInfo psiStart = new ProcessStartInfo
                            {
                                FileName = "sc.exe",
                                Arguments = "start WSearch",
                                UseShellExecute = false,
                                CreateNoWindow = true
                            };

                            using (Process start = Process.Start(psiStart))
                            {
                                start.WaitForExit();
                                AnsiConsole.MarkupLine("[green]✓ Indexação reativada com sucesso[/]");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red]❌ Erro: {ex.Message}[/]");
                        erros.Add(ex.Message);
                    }
                });

            if (erros.Count > 0)
            {
                AnsiConsole.MarkupLine($"[red]❌ Ocorreram erros: {string.Join(", ", erros)}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]✓ Operação concluída com sucesso![/]");
            }
        }

        // Opção 11: Desativar Hyper-V
        private static void DesativarHyperV()
        {
            List<string> erros = new List<string>();
            
            AnsiConsole.Write(
                new FigletText("Hyper-V")
                    .LeftJustified()
                    .Color(Color.Green));

            AnsiConsole.MarkupLine("[cyan]Desativa o Hyper-V e recursos de virtualização[/]");
            AnsiConsole.MarkupLine("[cyan]que podem impactar o desempenho em jogos.[/]\n");
            AnsiConsole.MarkupLine("[yellow]⚠️ Desative apenas se não usar máquinas virtuais ou WSL2![/]\n");

            var opcao = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Escolha a opção:[/]")
                    .AddChoices(new[] { 
                        "Desativar Hyper-V", 
                        "Ativar Hyper-V",
                        "Cancelar"
                    }));

            if (opcao == "Cancelar")
            {
                return;
            }

            bool desativar = opcao.StartsWith("Desativar");

            Console.Clear();
            AnsiConsole.Write(
                new FigletText(desativar ? "Desativando" : "Ativando")
                    .LeftJustified()
                    .Color(Color.Yellow));

            Console.WriteLine();
            AnsiConsole.MarkupLine("[cyan]═══════════════════════════════════════════════════════════════════════════════[/]");

            var stopwatch = Stopwatch.StartNew();
            
            string[] features = new string[] { "Microsoft-Hyper-V-All", "VirtualMachinePlatform", "HypervisorPlatform" };
            string[] nomes = new string[] { "Hyper-V", "Virtual Machine Platform", "Hypervisor Platform" };
            string acao = desativar ? "Disable" : "Enable";
            int totalPassos = features.Length;

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
                    var task = ctx.AddTask($"[cyan]{(desativar ? "Desativando" : "Ativando")} Hyper-V...[/]", maxValue: totalPassos);

                    for (int i = 0; i < features.Length; i++)
                    {
                        task.Description = $"[cyan]Passo {i + 1}/{totalPassos}: {(desativar ? "Desativando" : "Ativando")} {nomes[i]}...[/]";
                        task.Value = i;

                        try
                        {
                            ProcessStartInfo psi = new ProcessStartInfo
                            {
                                FileName = "powershell.exe",
                                Arguments = $"-Command \"{acao}-WindowsOptionalFeature -Online -FeatureName {features[i]} -NoRestart\"",
                                UseShellExecute = false,
                                RedirectStandardOutput = true,
                                RedirectStandardError = true,
                                CreateNoWindow = true
                            };

                            using (Process proc = Process.Start(psi))
                            {
                                // Ler saída em tempo real
                                using (var reader = proc.StandardOutput)
                                {
                                    string linha;
                                    while ((linha = reader.ReadLine()) != null)
                                    {
                                        // Destacar linhas importantes
                                        if (linha.Contains("Success", StringComparison.OrdinalIgnoreCase) ||
                                            linha.Contains("Enabled", StringComparison.OrdinalIgnoreCase) ||
                                            linha.Contains("Disabled", StringComparison.OrdinalIgnoreCase))
                                        {
                                            AnsiConsole.MarkupLine($"[green]{linha}[/]");
                                        }
                                        else if (linha.Contains("Error", StringComparison.OrdinalIgnoreCase) ||
                                                 linha.Contains("failed", StringComparison.OrdinalIgnoreCase) ||
                                                 linha.Contains("Exception", StringComparison.OrdinalIgnoreCase))
                                        {
                                            AnsiConsole.MarkupLine($"[red]{linha}[/]");
                                        }
                                        else if (!string.IsNullOrWhiteSpace(linha))
                                        {
                                            AnsiConsole.MarkupLine($"[blue]{linha}[/]");
                                        }
                                    }
                                }

                                string stderr = proc.StandardError.ReadToEnd();
                                proc.WaitForExit();

                                if (proc.ExitCode != 0)
                                {
                                    if (!string.IsNullOrWhiteSpace(stderr) && !stderr.Contains("already", StringComparison.OrdinalIgnoreCase))
                                    {
                                        AnsiConsole.MarkupLine($"[yellow]⚠️ {nomes[i]} - {stderr.Trim()}[/]");
                                        erros.Add(nomes[i]);
                                    }
                                    else
                                    {
                                        AnsiConsole.MarkupLine($"[yellow]ℹ️ {nomes[i]} - já estava {(desativar ? "desativado" : "ativado")}[/]");
                                    }
                                }
                                else
                                {
                                    AnsiConsole.MarkupLine($"[green]✓ {nomes[i]} {(desativar ? "desativado" : "ativado")} com sucesso[/]");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            AnsiConsole.MarkupLine($"[red]❌ Erro ao {(desativar ? "desativar" : "ativar")} {nomes[i]}: {ex.Message}[/]");
                            erros.Add(nomes[i]);
                        }
                    }

                    task.Value = totalPassos;
                    task.StopTask();
                });

            stopwatch.Stop();
            Console.WriteLine();
            AnsiConsole.MarkupLine("[cyan]═══════════════════════════════════════════════════════════════════════════════[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[cyan]⏱️ Tempo total: {stopwatch.Elapsed:mm\\:ss}[/]");

            if (erros.Count > 0)
            {
                AnsiConsole.MarkupLine($"[yellow]⚠️ Ocorreram avisos: {string.Join(", ", erros)}[/]");
                AnsiConsole.MarkupLine("[yellow]💡 Alguns recursos podem não estar disponíveis nesta versão do Windows[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]✓ Operação concluída com sucesso![/]");
            }

            AnsiConsole.MarkupLine("[yellow]⚠️ Reinicie o PC para aplicar as mudanças.[/]");
        }

        // Opção 12: Desativar Aero Peek
        private static void DesativarAeroPeek()
        {
            List<string> erros = new List<string>();
            
            AnsiConsole.Write(
                new FigletText("Aero Peek")
                    .LeftJustified()
                    .Color(Color.Magenta1));

            AnsiConsole.MarkupLine("[cyan]Desativa o Aero Peek (preview de janelas ao passar o mouse)[/]");
            AnsiConsole.MarkupLine("[cyan]para melhorar o desempenho visual.[/]\n");

            var opcao = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[magenta1]Escolha a opção:[/]")
                    .AddChoices(new[] { 
                        "Desativar Aero Peek", 
                        "Ativar Aero Peek",
                        "Cancelar"
                    }));

            if (opcao == "Cancelar")
            {
                return;
            }

            bool desativar = opcao.StartsWith("Desativar");

            Console.Clear();
            AnsiConsole.Write(
                new FigletText(desativar ? "Desativando" : "Ativando")
                    .LeftJustified()
                    .Color(Color.Yellow));

            AnsiConsole.Status()
                .Start($"[yellow]👁️ {(desativar ? "Desativando" : "Ativando")} Aero Peek...[/]", ctx => 
                {
                    try
                    {
                        string valor = desativar ? "0" : "1";
                        string cmd = $@"reg add ""HKCU\Software\Microsoft\Windows\DWM"" /v EnableAeroPeek /t REG_DWORD /d {valor} /f";
                        
                        ProcessStartInfo psi = new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            Arguments = $"-Command \"{cmd}\"",
                            UseShellExecute = false,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        };

                        using (Process proc = Process.Start(psi))
                        {
                            string stderr = proc.StandardError.ReadToEnd();
                            proc.WaitForExit();

                            if (proc.ExitCode != 0)
                            {
                                AnsiConsole.MarkupLine($"[red]❌ Erro: {stderr.Trim()}[/]");
                                erros.Add("EnableAeroPeek");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine($"[green]✓ Aero Peek {(desativar ? "desativado" : "ativado")} com sucesso[/]");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red]❌ Erro: {ex.Message}[/]");
                        erros.Add(ex.Message);
                    }
                });

            if (erros.Count > 0)
            {
                AnsiConsole.MarkupLine($"[red]❌ Ocorreram erros: {string.Join(", ", erros)}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]✓ Operação concluída com sucesso![/]");
            }
        }

        // Opção 13: Desativar Download Maps Manager
        private static void DesativarMapsManager()
        {
            List<string> erros = new List<string>();
            
            AnsiConsole.Write(
                new FigletText("Maps Manager")
                    .LeftJustified()
                    .Color(Color.Purple));

            AnsiConsole.MarkupLine("[cyan]Desativa o serviço Download Maps Manager (MapsBroker)[/]");
            AnsiConsole.MarkupLine("[cyan]que gerencia downloads de mapas offline.[/]\n");

            var opcao = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[purple]Escolha a opção:[/]")
                    .AddChoices(new[] { 
                        "Desativar Serviço", 
                        "Ativar Serviço",
                        "Cancelar"
                    }));

            if (opcao == "Cancelar")
            {
                return;
            }

            bool desativar = opcao.StartsWith("Desativar");

            Console.Clear();
            AnsiConsole.Write(
                new FigletText(desativar ? "Desativando" : "Ativando")
                    .LeftJustified()
                    .Color(Color.Yellow));

            AnsiConsole.Status()
                .Start($"[yellow]🗺️ {(desativar ? "Desativando" : "Ativando")} Maps Manager...[/]", ctx => 
                {
                    try
                    {
                        if (desativar)
                        {
                            // Parar serviço
                            ctx.Status("[yellow]Parando serviço MapsBroker...[/]");
                            
                            ProcessStartInfo psiStop = new ProcessStartInfo
                            {
                                FileName = "sc.exe",
                                Arguments = "stop MapsBroker",
                                UseShellExecute = false,
                                CreateNoWindow = true
                            };

                            using (Process stop = Process.Start(psiStop))
                            {
                                stop.WaitForExit();
                                AnsiConsole.MarkupLine("[green]✓ Serviço parado[/]");
                            }

                            // Desativar
                            ctx.Status("[yellow]Desativando serviço...[/]");
                            
                            ProcessStartInfo psiConfig = new ProcessStartInfo
                            {
                                FileName = "sc.exe",
                                Arguments = "config MapsBroker start= disabled",
                                UseShellExecute = false,
                                RedirectStandardError = true,
                                CreateNoWindow = true
                            };

                            using (Process config = Process.Start(psiConfig))
                            {
                                string stderr = config.StandardError.ReadToEnd();
                                config.WaitForExit();

                                if (config.ExitCode != 0)
                                {
                                    AnsiConsole.MarkupLine($"[red]❌ Erro: {stderr.Trim()}[/]");
                                    erros.Add("MapsBroker");
                                }
                                else
                                {
                                    AnsiConsole.MarkupLine("[green]✓ Maps Manager desativado[/]");
                                }
                            }
                        }
                        else
                        {
                            // Reativar
                            ctx.Status("[yellow]Reativando serviço...[/]");
                            
                            ProcessStartInfo psiConfig = new ProcessStartInfo
                            {
                                FileName = "sc.exe",
                                Arguments = "config MapsBroker start= auto",
                                UseShellExecute = false,
                                CreateNoWindow = true
                            };

                            using (Process config = Process.Start(psiConfig))
                            {
                                config.WaitForExit();
                                AnsiConsole.MarkupLine("[green]✓ Serviço configurado[/]");
                            }

                            // Iniciar
                            ctx.Status("[yellow]Iniciando serviço...[/]");
                            
                            ProcessStartInfo psiStart = new ProcessStartInfo
                            {
                                FileName = "sc.exe",
                                Arguments = "start MapsBroker",
                                UseShellExecute = false,
                                CreateNoWindow = true
                            };

                            using (Process start = Process.Start(psiStart))
                            {
                                start.WaitForExit();
                                AnsiConsole.MarkupLine("[green]✓ Maps Manager reativado[/]");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red]❌ Erro: {ex.Message}[/]");
                        erros.Add(ex.Message);
                    }
                });

            if (erros.Count > 0)
            {
                AnsiConsole.MarkupLine($"[red]❌ Ocorreram erros: {string.Join(", ", erros)}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]✓ Operação concluída com sucesso![/]");
            }
        }

        // Opção 14: Desativar SmartScreen
        private static void DesativarSmartScreen()
        {
            List<string> erros = new List<string>();
            
            AnsiConsole.Write(
                new FigletText("SmartScreen")
                    .LeftJustified()
                    .Color(Color.Red));

            AnsiConsole.MarkupLine("[cyan]Desativa o Windows SmartScreen que verifica arquivos e apps[/]");
            AnsiConsole.MarkupLine("[cyan]baixados da internet em busca de ameaças.[/]\n");
            AnsiConsole.MarkupLine("[yellow]⚠️ ATENÇÃO: Desativar o SmartScreen reduz a proteção contra malware![/]\n");

            var opcao = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]Escolha a opção:[/]")
                    .AddChoices(new[] { 
                        "Desativar SmartScreen", 
                        "Ativar SmartScreen",
                        "Cancelar"
                    }));

            if (opcao == "Cancelar")
            {
                return;
            }

            bool desativar = opcao.StartsWith("Desativar");

            Console.Clear();
            AnsiConsole.Write(
               
                new FigletText(desativar ? "Desativando" : "Ativando")
                    .LeftJustified()
                    .Color(Color.Yellow));

            AnsiConsole.Status()
                .Start($"[yellow]🛡️ {(desativar ? "Desativando" : "Ativando")} SmartScreen...[/]", ctx => 
                {
                    try
                    {
                        // Explorer
                        ctx.Status("[yellow]Configurando SmartScreen (Explorer)...[/]");
                        
                        string valorExplorer = desativar ? "Off" : "RequireAdmin";
                        string cmd1 = $@"reg add ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer"" /v SmartScreenEnabled /t REG_SZ /d {valorExplorer} /f";
                        
                        ProcessStartInfo psi1 = new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            Arguments = $"-Command \"{cmd1}\"",
                            UseShellExecute = false,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        };

                        using (Process proc1 = Process.Start(psi1))
                        {
                            string stderr = proc1.StandardError.ReadToEnd();
                            proc1.WaitForExit();

                            if (proc1.ExitCode != 0)
                            {
                                AnsiConsole.MarkupLine($"[red]❌ Erro (Explorer): {stderr.Trim()}[/]");
                                erros.Add("SmartScreen Explorer");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine($"[green]✓ SmartScreen (Explorer) {(desativar ? "desativado" : "ativado")}[/]");
                            }
                        }

                        // System
                        ctx.Status("[yellow]Configurando SmartScreen (System)...[/]");
                        
                        string valorSystem = desativar ? "0" : "1";
                        string cmd2 = $@"reg add ""HKLM\SOFTWARE\Policies\Microsoft\Windows\System"" /v EnableSmartScreen /t REG_DWORD /d {valorSystem} /f";
                        
                        ProcessStartInfo psi2 = new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            Arguments = $"-Command \"{cmd2}\"",
                            UseShellExecute = false,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        };

                        using (Process proc2 = Process.Start(psi2))
                        {
                            string stderr = proc2.StandardError.ReadToEnd();
                            proc2.WaitForExit();

                            if (proc2.ExitCode != 0)
                            {
                                AnsiConsole.MarkupLine($"[red]❌ Erro (System): {stderr.Trim()}[/]");
                                erros.Add("SmartScreen System");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine($"[green]✓ SmartScreen (System) {(desativar ? "desativado" : "ativado")}[/]");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red]❌ Erro: {ex.Message}[/]");
                        erros.Add(ex.Message);
                    }
                });

            if (erros.Count > 0)
            {
                AnsiConsole.MarkupLine($"[red]❌ Falha parcial: {string.Join(", ", erros)}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]✓ SmartScreen {(desativar ? "desativado" : "ativado")} com sucesso![/]");
            }
        }
    }
}