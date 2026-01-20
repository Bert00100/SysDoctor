namespace SysDoctor.Scripts
{
    class UpdateWindows
    {
        public static void Executar()
        {
            AnsiConsole.MarkupLine("[blue]🔄 Windows Update[/]");
            AnsiConsole.WriteLine();

            try
            {
                AnsiConsole.MarkupLine("[cyan]🔧 Abrindo Windows Update...[/]");

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "ms-settings:windowsupdate",
                        UseShellExecute = true
                    }
                };

                process.Start();

                AnsiConsole.MarkupLine("[green]✅ Windows Update aberto com sucesso![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Erro ao abrir Windows Update: {ex.Message}[/]");
            }
        }
    }
}
