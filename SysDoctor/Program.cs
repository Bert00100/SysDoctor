using System.Security.Principal;

namespace SysDoctor
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Verifica se está rodando como administrador
            if (!IsRunningAsAdministrator())
            {
                RestartAsAdministrator();
                return;
            }

            bool continuar = true;

            while (continuar)
            {
                // Exibe o ASCII Art
                MostrarAsciiArt();

                // Exibe o menu dividido em duas colunas
                MostrarMenu();

                AnsiConsole.Markup("[cyan] Digite sua opção: [/]");
                int.TryParse(Console.ReadLine(), out int opcao);

                switch (opcao)
                {
                    case 1:
                        InfoMachine.Executar();
                        break;
                    case 2:
                        ClearDisk.Executar();
                        break;
                    case 3:
                        // Scanner do Windows
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

        private static bool IsRunningAsAdministrator()
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
                    Verb = "runas" // Solicita elevação de privilégios
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

            AnsiConsole.MarkupLine("[blue]" + asciiArt + "[/]");
            AnsiConsole.MarkupLine("[yellow]" + " ".PadLeft(30) + "Reparador e Otimizador de Windows[/]");
            AnsiConsole.WriteLine();
        }

        private static void MostrarMenu()
        {
            var opcoesEsq = new[]
            {
                "[[ 1 ]] Informação da Máquina",
                // "[[ 3 ]] Scanner do Windows", 
                // "[[ 5 ]] SpeedTest",
                // "[[ 7 ]] Teste de Ping",
                // "[[ 9 ]] Otimizar Wifi",
                // "[[ 11 ]] Verificar Temperatura",
                // "[[ 13 ]] Criar Ponto de Restauração",
                // "[[ 15 ]] Atualizar Windows"
            };

            var opcoesDir = new[]
            {
                "[[ 2 ]] Limpar SSD/HD",
                // "[[ 4 ]] Limpar Memória RAM",
                // "[[ 6 ]] Limpar Caches de Wifi/Ethernet", 
                // "[[ 8 ]] Otimizar Ping",
                // "[[ 10 ]] Mapa de Conexão",
                // "[[ 12 ]] Otimizar Windows",
                // "[[ 14 ]] Configuração Pós-Instalação",
                // "[[ 16 ]] Rodar Windows Defender"
            };

            // Coluna esquerda
            var colunaEsq = new Panel(string.Join("\n", opcoesEsq))
                .Header(" Opções")
                .Border(BoxBorder.Rounded)
                .Padding(1, 1);
            
            // Coluna direita  
            var colunaDir = new Panel(string.Join("\n", opcoesDir))
                .Header(" Opções")
                .Border(BoxBorder.Rounded)
                .Padding(1, 1);

            // Cria duas colunas para o menu
            var columns = new Columns(new Panel[] { colunaEsq, colunaDir });
            
            AnsiConsole.Write(columns);
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[dim][[ 0 ]] Sair[/]");
            AnsiConsole.WriteLine();
        }
    }
}