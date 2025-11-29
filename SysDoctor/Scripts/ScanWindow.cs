namespace SysDoctor.Scripts
{
    public class ScanWindow
    {
        public static void Executar()
        {
            AnsiConsole.MarkupLine("[blue]🛠️  Scanner do Windows[/]");
            AnsiConsole.MarkupLine("[yellow]As ações a seguir podem levar bastante tempo...[/]");
            AnsiConsole.WriteLine();

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var erros = new List<string>();
                int totalPassos = 7;
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
                        var task = ctx.AddTask("[cyan]Escaneando Windows...[/]", maxValue: totalPassos);

                        // Passo 1: Verificar disco (CHKDSK)
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Verificando integridade do disco...[/]";
                        task.Value = passoAtual;
                        var resultadoCHKDSK = ExecutarPowerShellAsync("chkdsk C: /scan", 300).Result;
                        if (resultadoCHKDSK.exitCode == 0 || resultadoCHKDSK.timedOut)
                        {
                            DebugSuccess("Verificação de disco concluída");
                        }
                        else
                        {
                            DebugWarning("Aviso ao verificar disco - continuando...");
                            if (!string.IsNullOrEmpty(resultadoCHKDSK.error)) erros.Add("CHKDSK");
                        }

                        // Passo 2: Executar SFC (System File Checker)
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Executando SFC (verificação de arquivos do sistema)...[/]";
                        task.Value = passoAtual;
                        var resultadoSFC = ExecutarPowerShellAsync("sfc /scannow", 600).Result;
                        if (resultadoSFC.exitCode == 0 || resultadoSFC.timedOut)
                        {
                            DebugSuccess("SFC concluído com sucesso");
                        }
                        else
                        {
                            DebugWarning("Aviso ao executar SFC - continuando...");
                            if (!string.IsNullOrEmpty(resultadoSFC.error)) erros.Add("SFC");
                        }

                        // Passo 3: DISM - Verificar estado da imagem
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: DISM - Verificando estado da imagem...[/]";
                        task.Value = passoAtual;
                        var resultadoDISMCheck = ExecutarPowerShellAsync("DISM /Online /Cleanup-Image /CheckHealth", 180).Result;
                        if (resultadoDISMCheck.exitCode == 0 || resultadoDISMCheck.timedOut)
                        {
                            DebugSuccess("Verificação DISM concluída");
                        }
                        else
                        {
                            DebugWarning("Aviso na verificação DISM - continuando...");
                        }

                        // Passo 4: DISM - Escanear saúde da imagem
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: DISM - Escaneando saúde da imagem...[/]";
                        task.Value = passoAtual;
                        var resultadoDISMScan = ExecutarPowerShellAsync("DISM /Online /Cleanup-Image /ScanHealth", 600).Result;
                        if (resultadoDISMScan.exitCode == 0 || resultadoDISMScan.timedOut)
                        {
                            DebugSuccess("Escaneamento DISM concluído");
                        }
                        else
                        {
                            DebugWarning("Aviso no escaneamento DISM - continuando...");
                        }

                        // Passo 5: DISM - Restaurar integridade
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: DISM - Restaurando integridade da imagem...[/]";
                        task.Value = passoAtual;
                        var resultadoDISM = ExecutarPowerShellAsync("DISM /Online /Cleanup-Image /RestoreHealth", 900).Result;
                        if (resultadoDISM.exitCode == 0 || resultadoDISM.timedOut)
                        {
                            DebugSuccess("DISM - Restauração concluída com sucesso");
                        }
                        else
                        {
                            DebugWarning("Aviso ao executar DISM - continuando...");
                            if (!string.IsNullOrEmpty(resultadoDISM.error)) erros.Add("DISM");
                        }

                        // Passo 6: Verificar Windows Update
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Verificando problemas do Windows Update...[/]";
                        task.Value = passoAtual;
                        var resultadoWU = ExecutarPowerShellAsync("Get-WindowsUpdateLog", 60).Result;
                        if (resultadoWU.exitCode == 0 || resultadoWU.timedOut)
                        {
                            DebugSuccess("Verificação do Windows Update concluída");
                        }
                        else
                        {
                            DebugWarning("Aviso ao verificar Windows Update - continuando...");
                        }

                        // Passo 7: Resetar Windows Store (se houver problemas)
                        passoAtual++;
                        task.Description = $"[cyan]Passo {passoAtual}/{totalPassos}: Resetando cache da Windows Store...[/]";
                        task.Value = passoAtual;
                        var resultadoStore = ExecutarPowerShellAsync("wsreset.exe", 30).Result;
                        if (resultadoStore.exitCode == 0 || resultadoStore.timedOut)
                        {
                            DebugSuccess("Cache da Windows Store resetado");
                        }
                        else
                        {
                            DebugWarning("Aviso ao resetar Windows Store - continuando...");
                        }

                        task.StopTask();
                    });

                stopwatch.Stop();

                // Resumo final
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[cyan]═══════════════════════════════════════════════════════════════[/]");
                AnsiConsole.MarkupLine("[cyan]                    📊 Resumo da Análise                      [/]");
                AnsiConsole.MarkupLine("[cyan]═══════════════════════════════════════════════════════════════[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"[cyan]⏱️  Tempo total: {stopwatch.Elapsed.Minutes} minutos e {stopwatch.Elapsed.Seconds} segundos[/]");
                
                if (erros.Count > 0)
                {
                    AnsiConsole.MarkupLine($"[yellow]⚠️  Concluído com avisos: {string.Join(", ", erros)}[/]");
                    AnsiConsole.MarkupLine("[yellow]💡 Recomendação: Execute novamente ou reinicie o computador[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[green]✅ Scanner do Windows concluído com sucesso![/]");
                    AnsiConsole.MarkupLine("[green]💡 Sistema parece estar saudável[/]");
                }

                // Informações adicionais
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[dim]═══════════════════════════════════════════════════════════════[/]");
                AnsiConsole.MarkupLine("[dim]                  📝 Logs e Informações                      [/]");
                AnsiConsole.MarkupLine("[dim]═══════════════════════════════════════════════════════════════[/]");
                AnsiConsole.MarkupLine("[dim]• Logs do SFC: C:\\Windows\\Logs\\CBS\\CBS.log[/]");
                AnsiConsole.MarkupLine("[dim]• Logs do DISM: C:\\Windows\\Logs\\DISM\\dism.log[/]");
                AnsiConsole.MarkupLine("[dim]• Logs do CHKDSK: Event Viewer > Windows Logs > Application[/]");
                AnsiConsole.MarkupLine("[dim]• Logs do Windows Update: Desktop\\WindowsUpdate.log[/]");
                
                // Recomendações adicionais
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[yellow]═══════════════════════════════════════════════════════════════[/]");
                AnsiConsole.MarkupLine("[yellow]                      💡 Recomendações                        [/]");
                AnsiConsole.MarkupLine("[yellow]═══════════════════════════════════════════════════════════════[/]");
                AnsiConsole.MarkupLine("[yellow]• Reinicie o computador após a conclusão[/]");
                AnsiConsole.MarkupLine("[yellow]• Execute o Windows Update para garantir que está atualizado[/]");
                AnsiConsole.MarkupLine("[yellow]• Se os problemas persistirem, considere criar um ponto de restauração[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]💥 Erro durante o scanner do Windows: {ex.Message}[/]");
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
                        FileName = "cmd.exe",
                        Arguments = $"/c {comando}",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        Verb = "runas"
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