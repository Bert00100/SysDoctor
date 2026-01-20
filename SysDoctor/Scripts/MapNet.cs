namespace SysDoctor.Scripts
{
    class MapNet
    {
        public static void Executar()
        {
            AnsiConsole.MarkupLine("[blue] Mapa de Conexão [/]");

            Console.Write("Digite o endereço que quer Mapear: ");
            string endereco = Console.ReadLine();

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var erros = new List<string>();

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c tracert {endereco}",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };

                process.Start();

                int saltos = 0;
                while (!process.StandardOutput.EndOfStream)
                {
                    string linha = process.StandardOutput.ReadLine();
                    if (!string.IsNullOrWhiteSpace(linha))
                    {
                        if (char.IsDigit(linha.TrimStart()[0]))
                        {
                            saltos++;
                        }
                        AnsiConsole.WriteLine(linha);
                    }
                }

                process.WaitForExit();

                stopwatch.Stop();

                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"[cyan]⏱️ Tempo total: {stopwatch.Elapsed:mm\\:ss}[/]");

                if (saltos == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]⚠️ Nenhum salto encontrado. Verifique o endereço.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[green]✅ Mapeamento de rede concluído![/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]💥 Erro durante o mapeamento de rede: {ex.Message}[/]");
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