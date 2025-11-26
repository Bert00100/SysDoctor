using System;
using System.Diagnostics;
using System.Management.Automation;
using Spectre.Console;
using CommandLine;
using SysDoctor.Scripts;

namespace SysDoctor
{
    class Program
    {
        public static void Main(string[] args)
        {
            AnsiConsole.MarkupLine("[green]SysDoctor CSharp 1.0[/]");
            AnsiConsole.WriteLine();

            bool continuar = true;

            while (continuar)
            {
                AnsiConsole.MarkupLine("[yellow]Menu Principal:[/]");
                AnsiConsole.WriteLine("1 - Informações da Máquina");
                AnsiConsole.WriteLine("0 - Sair");

                AnsiConsole.Markup("[cyan] Digite sua opção: [/]");
                int.TryParse(Console.ReadLine(), out int opcao);

                switch (opcao)
                {
                    case 1:
                        InfoMachine.Executar();
                        break;
                    case 0:
                        AnsiConsole.MarkupLine("[red]Encerrando...[/]");
                        continuar = false;
                        break;
                    default:
                        AnsiConsole.MarkupLine("[red]Opção inválida![/]");
                        break;
                }

                if (continuar)
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.Markup("[dim]Pressione qualquer tecla para continuar...[/]");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
    }
}