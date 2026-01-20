using System.Diagnostics;
using Spectre.Console;

namespace SysDoctor.Scripts
{
    class IsoWin
    {
        public static void Executar()
        {
            try
            {
                Console.Clear();
                AnsiConsole.MarkupLine("[cyan]📦 ISO do windows 11 pro...[/]");

                AnsiConsole.MarkupLine("[cyan]═══════════════════════════════════════════════════════════════════════════════[/]");
                Console.WriteLine(" ");

                string isoURL = "https://drive.google.com/drive/folders/12FuLRZnO1iKyqDyUPQPm18ka3HwqTD20?usp=sharing";
                
                AnsiConsole.MarkupLine("[cyan]Link para Download da ISO[/]");
                Console.WriteLine(" ");

                AnsiConsole.MarkupLine($"[blue underline]{isoURL}[/]");

                AnsiConsole.MarkupLine("[yellow]Pressione N para abrir o link no navegador ...[/]");
                var tecla = Console.ReadKey(true);

                if (tecla.Key == ConsoleKey.N)
                {
                    AbrirLinkNoBrowser(isoURL);
                }
                else
                {
                    AnsiConsole.MarkupLine("[dim]Operacao cancelada.[/]");
                }
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

        private static void AbrirLinkNoBrowser(string url)
        {
            try
            {
                AnsiConsole.MarkupLine("[yellow]⏳ Abrindo navegador...[/]");
                
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
                
                AnsiConsole.MarkupLine("[green]✓ Link aberto com sucesso![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Erro ao abrir o navegador: {ex.Message}[/]");
            }
        }
    }
}