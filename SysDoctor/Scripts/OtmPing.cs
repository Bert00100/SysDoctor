namespace SysDoctor.Scripts
{
    class OtmPing
    {
        public static void Executar()
        {
            AnsiConsole.MarkupLine("[blue]Otimizar Ping[/]");
            AnsiConsole.WriteLine();

            var stopwatch = Stopwatch.StartNew();
            var erros = new List<string>();
            string dnsJumperPath = null;

            try
            {
                // Busca o arquivo DnsJumper.exe em todas as subpastas de DNS
                string pastaDNS = Path.Combine("Scripts", "Apps", "DNS");
                string[] encontrados = Directory.Exists(pastaDNS)
                    ? Directory.GetFiles(pastaDNS, "DnsJumper.exe", SearchOption.AllDirectories)
                    : Array.Empty<string>();

                if (encontrados.Length == 0)
                {
                    DebugWarning("DNS Jumper não encontrado em nenhuma subpasta de Scripts/Apps/DNS.");
                    erros.Add("DNS JUMPER não encontrado");
                }
                else
                {
                    dnsJumperPath = Path.GetFullPath(encontrados[0]);
                    var fileInfo = new FileInfo(dnsJumperPath);

                    // Se estiver oculto, remove o atributo oculto
                    if (fileInfo.Attributes.HasFlag(FileAttributes.Hidden))
                    {
                        try
                        {
                            fileInfo.Attributes &= ~FileAttributes.Hidden;
                            DebugSuccess("Atributo oculto removido de DnsJumper.exe.");
                        }
                        catch (Exception ex)
                        {
                            DebugWarning($"Não foi possível remover atributo oculto: {ex.Message}");
                        }
                    }

                    DebugSuccess($"DNS Jumper localizado: {dnsJumperPath}");
                }

                stopwatch.Stop();
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"[cyan]⏱️ Tempo de preparação: {stopwatch.Elapsed.Seconds} segundos[/]");
                AnsiConsole.WriteLine();

                if (erros.Count > 0)
                {
                    AnsiConsole.MarkupLine($"[red]❌ Ocorreu um erro ao executar: {string.Join(", ", erros)}[/]");
                    AnsiConsole.MarkupLine("[cyan]💡 Dica:[/]");
                    AnsiConsole.MarkupLine("[dim]  1. Baixe o DNS Jumper de: https://www.sordum.org/7952/dns-jumper-v2-3/[/]");
                    AnsiConsole.MarkupLine($"[dim]  2. Coloque o DnsJumper.exe em alguma subpasta de: {Path.GetFullPath(pastaDNS)}[/]");
                }
                else
                {
                    ExecutarDnsJumper(dnsJumperPath);
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]💥 Erro durante a otimização de ping: {ex.Message}[/]");
            }
        }

        private static void ExecutarDnsJumper(string caminhoExecutavel)
        {
            try
            {
                if (string.IsNullOrEmpty(caminhoExecutavel) || !File.Exists(caminhoExecutavel))
                {
                    AnsiConsole.MarkupLine("[red]❌ Não foi possível localizar o DNS Jumper.[/]");
                    return;
                }

                DebugSuccess($"Iniciando DNS Jumper: {Path.GetFileName(caminhoExecutavel)}");
                AnsiConsole.MarkupLine("[yellow]⚠️  Aguardando o fechamento do DNS Jumper...[/]");
                AnsiConsole.WriteLine();

                var processStartInfo = new ProcessStartInfo
                {
                    FileName = caminhoExecutavel,
                    UseShellExecute = true,
                    WorkingDirectory = Path.GetDirectoryName(caminhoExecutavel),
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Normal
                };

                using (var process = Process.Start(processStartInfo))
                {
                    if (process != null)
                    {
                        process.WaitForExit();
                        AnsiConsole.WriteLine();
                        DebugSuccess("Otimização finalizada!");
                        AnsiConsole.MarkupLine("[green]✅ DNS Jumper foi fechado.[/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]❌ Não foi possível iniciar o DNS Jumper.[/]");
                    }
                }
            }
            catch (System.ComponentModel.Win32Exception winEx)
            {
                if (winEx.NativeErrorCode == 1223)
                {
                    AnsiConsole.MarkupLine("[yellow]⚠️  Execução cancelada pelo usuário.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]❌ Erro ao executar DNS Jumper: {winEx.Message}[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Erro ao executar DNS Jumper: {ex.Message}[/]");
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