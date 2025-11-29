using System;
using System.Diagnostics;
using System.Security.Principal;
using Spectre.Console;

namespace SysDoctor
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Verifica se está rodando como administrador
            if (!IsRunningAsAdministrador())
            {
                RestartAsAdministrator();
                return;
            }

            bool continuar = true;

            while (continuar)
            {
                Console.Clear();
                
                // Exibe o ASCII Art
                MostrarAsciiArt();

                // Exibe o menu dividido em duas colunas
                MostrarMenu();

                // Centraliza o input
                CentralizarInput();

                int.TryParse(Console.ReadLine(), out int opcao);

                switch (opcao)
                {
                    case 1:
                        Console.Clear();
                        InfoMachine.Executar();
                        break;
                    case 2:
                        Console.Clear();
                        ClearDisk.Executar();
                        break;
                    case 3:
                        Console.Clear();
                        ScanWindow.Executar();
                        break;
                    case 4:
                        // Limpar Memória RAM
                        break;
                    case 5:
                        // SpeedTest
                        break;
                    case 6:
                        // Limpar Caches de Wifi/Ethernet
                        break;
                    case 7:
                        // Teste de Ping
                        break;
                    case 8:
                        // Otimizar Ping
                        break;
                    case 9:
                        // Otimizar Wifi
                        break;
                    case 10:
                        // Mapa de Conexão
                        break;
                    case 11:
                        // Verificar Temperatura
                        break;
                    case 12:
                        // Otimizar Windows
                        break;
                    case 13:
                        // Criar Ponto de Restauração
                        break;
                    case 14:
                        // Configuração Pós-Instalação
                        break;
                    case 15:
                        // Atualizar Windows
                        break;
                    case 16:
                        // Rodar Windows Defender
                        break;
                    case 0:
                        Console.Clear();
                        CentralizarTexto("[red]Encerrando...[/]", true);
                        continuar = false;
                        break;
                    default:
                        CentralizarTexto("[red]Opção inválida![/]", true);
                        break;
                }

                if (continuar)
                {
                    AnsiConsole.WriteLine();
                    CentralizarTexto("[dim]Pressione qualquer tecla para continuar...[/]", true); // Mudei de false para true
                    Console.ReadKey();
                }
            }
        }

        private static bool IsRunningAsAdministrador()
        {
            try
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }

        private static void RestartAsAdministrator()
        {
            try
            {
                AnsiConsole.MarkupLine("[yellow]⚠️  O programa precisa de privilégios de Administrador![/]");
                AnsiConsole.MarkupLine("[cyan]Reiniciando como Administrador...[/]");
                
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = Environment.ProcessPath ?? System.Reflection.Assembly.GetExecutingAssembly().Location,
                    UseShellExecute = true,
                    Verb = "runas"
                };

                Process.Start(processInfo);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Erro ao tentar reiniciar como Administrador: {ex.Message}[/]");
                AnsiConsole.MarkupLine("[yellow]Por favor, execute o programa manualmente como Administrador.[/]");
                AnsiConsole.Markup("[dim]Pressione qualquer tecla para sair...[/]");
                Console.ReadKey();
            }
        }

        private static void MostrarAsciiArt()
        {
            string asciiArt = @"
         ___    _  _    ___         ____     _____     ___    ____    _____    ____
        / __)  ( \/ )  / __)       (  _ \   (  _  )   / __)  (_  _)  (  _  )  (  _ \
        \__ \   \  /   \__ \        )(_) )   )(_)(   ( (__     )(     )(_)(    )   /
        (___/   (__)   (___/  ___  (____/   (_____)   \___)   (__)   (_____)  (_)\_)
            ";

            var linhas = asciiArt.Split('\n');
            int larguraTerminal = Console.WindowWidth;
            
            foreach (var linha in linhas)
            {
                if (!string.IsNullOrWhiteSpace(linha))
                {
                    int padding = Math.Max(0, (larguraTerminal - linha.Length) / 2);
                    AnsiConsole.MarkupLine("[blue]" + new string(' ', padding) + linha + "[/]");
                }
            }
            
            AnsiConsole.WriteLine();
            
            string subtitulo = "🛠️ Reparador e Otimizador de Windows";
            int paddingSubtitulo = Math.Max(0, (larguraTerminal - subtitulo.Length) / 2);
            AnsiConsole.MarkupLine("[yellow]" + new string(' ', paddingSubtitulo) + subtitulo + "[/]");
            AnsiConsole.WriteLine();
        }

        private static void MostrarMenu()
        {
            var opcoesEsq = new[]
            {
                "[[ 1 ]] Informação da Máquina",
                "[[ 3 ]] Scanner do Windows", 
                "[[ 5 ]] SpeedTest",
                "[[ 7 ]] Teste de Ping",
                "[[ 9 ]] Otimizar Wifi",
                "[[ 11 ]] Verificar Temperatura",
                "[[ 13 ]] Criar Ponto de Restauração",
                "[[ 15 ]] Atualizar Windows"
            };

            var opcoesDir = new[]
            {
                "[[ 2 ]] Limpar SSD/HD",
                "[[ 4 ]] Limpar Memória RAM",
                "[[ 6 ]] Limpar Caches de Wifi/Ethernet", 
                "[[ 8 ]] Otimizar Ping",
                "[[ 10 ]] Mapa de Conexão",
                "[[ 12 ]] Otimizar Windows",
                "[[ 14 ]] Configuração Pós-Instalação",
                "[[ 16 ]] Rodar Windows Defender"
            };

            // Cria uma tabela para melhor organização
            var table = new Table()
                .HideHeaders()
                .Border(TableBorder.None)
                .AddColumn(new TableColumn("").Width(40).PadRight(2))
                .AddColumn(new TableColumn("").Width(40))
                .AddRow(
                    // Coluna esquerda
                    new Panel(
                        string.Join("\n", opcoesEsq)
                    )
                    .Header("[blue]🔧 Ferramentas do Sistema[/]")
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Blue)
                    .Padding(1, 1),
                    
                    // Coluna direita
                    new Panel(
                        string.Join("\n", opcoesDir)
                    )
                    .Header("[green]🧹 Limpeza e Otimização[/]")
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Green)
                    .Padding(1, 1)
                );

            // Centraliza a tabela
            var centeredTable = Align.Center(table);
            AnsiConsole.Write(centeredTable);
            AnsiConsole.WriteLine();
            
            CentralizarTexto("[dim]📋 [[ 0 ]] Sair[/]", true);
            AnsiConsole.WriteLine();
        }

        private static void CentralizarTexto(string texto, bool usarMarkup)
        {
            // Remove tags de marcação para calcular o tamanho real do texto
            var textoLimpo = System.Text.RegularExpressions.Regex.Replace(texto, @"\[.*?\]", "");
    
            int larguraTerminal = Console.WindowWidth;
            int padding = Math.Max(0, (larguraTerminal - textoLimpo.Length) / 2);
    
            if (usarMarkup)
            {
                AnsiConsole.MarkupLine(new string(' ', padding) + texto);
            }
            else
            {
                // Remove as tags manualmente se não usar markup
                var textoSemTags = System.Text.RegularExpressions.Regex.Replace(texto, @"\[.*?\]", "");
                Console.WriteLine(new string(' ', padding) + textoSemTags);
            }
        }

        private static void CentralizarInput()
        {
            string textoOpcao = "🎯 Digite sua opção: ";
            int larguraTerminal = Console.WindowWidth;
            int padding = Math.Max(0, (larguraTerminal - textoOpcao.Length) / 2);
            
            Console.Write(new string(' ', padding));
            AnsiConsole.Markup("[cyan]🎯 Digite sua opção: [/]");
        }
    }
}