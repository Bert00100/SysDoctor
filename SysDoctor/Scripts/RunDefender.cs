namespace SysDoctor.Scripts
{
    class RunDefender
    {
        public static void Executar()
        {
            AnsiConsole.MarkupLine("[blue]🛡️ Windows Defender[/]");
            AnsiConsole.WriteLine();

            try
            {
                AnsiConsole.MarkupLine("[cyan]🔧 Abrindo Windows Defender...[/]");

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "windowsdefender:",
                        UseShellExecute = true
                    }
                };

                process.Start();

                AnsiConsole.MarkupLine("[green]✅ Windows Defender aberto com sucesso![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Erro ao abrir Windows Defender: {ex.Message}[/]");
            }
        }
    }
}
