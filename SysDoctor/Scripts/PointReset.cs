namespace SysDoctor.Scripts
{
    class PointReset
    {
        public static void Executar()
        {
            AnsiConsole.MarkupLine("[blue]Ponto de Restauração do Windows[/]");
            AnsiConsole.WriteLine();

            AnsiConsole.MarkupLine("[cyan]Escolha uma opção:[/]");
            AnsiConsole.MarkupLine("[dim]1 - Criar Ponto de Restauração[/]");
            AnsiConsole.MarkupLine("[dim]2 - Restaurar Sistema[/]");
            AnsiConsole.WriteLine();

            var escolha = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Selecione:[/]")
                    .AddChoices(new[] {
                        "Criar Ponto de Restauração",
                        "Restaurar Sistema"
                    }));

            try
            {
                switch (escolha)
                {
                    case "Criar Ponto de Restauração":
                        AbrirCriarPontoRestauracao();
                        break;
                    case "Restaurar Sistema":
                        AbrirRestaurarSistema();
                        break;
                    
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Erro ao abrir utilitário: {ex.Message}[/]");
            }
        }

        private static void AbrirCriarPontoRestauracao()
        {
            try
            {
                AnsiConsole.MarkupLine("[cyan]🔧 Abrindo utilitário de criação de ponto de restauração...[/]");
                
                // Comando para criar ponto de restauração via GUI
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"Start-Process 'SystemPropertiesProtection.exe' -Verb RunAs\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                process.WaitForExit();

                AnsiConsole.MarkupLine("[green]✅ Utilitário aberto com sucesso![/]");
                AnsiConsole.MarkupLine("[yellow]💡 Clique no botão 'Criar...' para criar um ponto de restauração.[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Erro: {ex.Message}[/]");
            }
        }

        private static void AbrirRestaurarSistema()
        {
            try
            {
                AnsiConsole.MarkupLine("[cyan]🔧 Abrindo utilitário de restauração do sistema...[/]");
                
                // Abrir o assistente de restauração do sistema
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "rstrui.exe",
                        UseShellExecute = true,
                        Verb = "runas" // Requer privilégios de administrador
                    }
                };

                process.Start();

                AnsiConsole.MarkupLine("[green]✅ Assistente de restauração aberto com sucesso![/]");
                AnsiConsole.MarkupLine("[yellow]💡 Siga as instruções na tela para restaurar o sistema.[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Erro: {ex.Message}[/]");
                AnsiConsole.MarkupLine("[yellow]💡 Tente executar o programa como Administrador.[/]");
            }
        }

    }
}