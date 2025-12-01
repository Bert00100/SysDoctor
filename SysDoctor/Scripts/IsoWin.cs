namespace SysDoctor.Scripts
{
    class IsoWin
    {
        public static void Executar()
        {
            try
            {
                Console.Clear();
                AnsiConsole.MarkupLine("[cyan]📦 Preparando instalação da ISO Windows 11 Pro...[/]");

                // Nome do arquivo ISO esperado
                string isoFileName = "Win11pro.iso";

                // Possíveis caminhos de origem (procura no diretório da aplicação e em Scripts/ISOs)
                var candidates = new[]
                {
                    Path.Combine(AppContext.BaseDirectory, "Scripts", "ISOs", isoFileName),
                    Path.Combine(AppContext.BaseDirectory, "ISOs", isoFileName),
                    Path.Combine(Directory.GetCurrentDirectory(), "Scripts", "ISOs", isoFileName),
                    Path.Combine(Directory.GetCurrentDirectory(), "ISOs", isoFileName),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ISOs", isoFileName)
                };

                string? sourceIso = candidates.FirstOrDefault(File.Exists);

                if (string.IsNullOrEmpty(sourceIso))
                {
                    AnsiConsole.MarkupLine($"[red]❌ Arquivo '{isoFileName}' não encontrado nas pastas esperadas.[/]");
                    AnsiConsole.MarkupLine("[yellow]Verifique se a ISO está em 'Scripts\\ISOs' ou copie-a manualmente para uma das pastas e tente novamente.[/]");
                    return;
                }

                // Pasta de destino: Documentos\isoSysdoctor
                string destFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "isoSysdoctor");
                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                    AnsiConsole.MarkupLine($"[green]📁 Pasta criada: {destFolder}[/]");
                }

                string destPath = Path.Combine(destFolder, isoFileName);

                // Se já existir, perguntar se quer sobrescrever
                if (File.Exists(destPath))
                {
                    AnsiConsole.MarkupLine($"[yellow]⚠️ O arquivo já existe em: {destPath}[/]");
                    AnsiConsole.Markup("[dim]Deseja sobrescrever? (S/N): [/]");
                    var key = Console.ReadLine()?.Trim().ToUpperInvariant();
                    if (key != "S" && key != "SIM" && key != "Y" && key != "YES")
                    {
                        AnsiConsole.MarkupLine("[yellow]Operação cancelada pelo usuário.[/]");
                        return;
                    }
                }

                // Copiar com progresso
                var fileInfo = new FileInfo(sourceIso);
                long totalBytes = fileInfo.Length;
                const int bufferSize = 1024 * 1024; // 1 MB

                AnsiConsole.Progress()
                    .AutoClear(false)
                    .Columns(new ProgressColumn[]
                    {
                        new TaskDescriptionColumn(),
                        new ProgressBarColumn(),
                        new PercentageColumn(),
                        new RemainingTimeColumn(),
                        new SpinnerColumn(),
                    })
                    .Start(ctx =>
                    {
                        var task = ctx.AddTask($"[cyan]Copiando {isoFileName}[/]", maxValue: totalBytes);

                        using (var source = new FileStream(sourceIso, FileMode.Open, FileAccess.Read, FileShare.Read))
                        using (var dest = new FileStream(destPath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            var buffer = new byte[bufferSize];
                            long totalRead = 0;
                            int read;

                            while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                dest.Write(buffer, 0, read);
                                totalRead += read;
                                task.Value = Math.Min(totalRead, totalBytes);
                            }
                        }

                        task.StopTask();
                    });

                AnsiConsole.MarkupLine("\n[green]✅ ISO instalada com sucesso em:[/] ");
                AnsiConsole.MarkupLine($"[white]{destPath}[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]💥 Erro ao instalar a ISO: {ex.Message}[/]");
            }
            finally
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[dim]Pressione qualquer tecla para continuar...[/]");
                Console.ReadKey();
            }
        }
    }
}