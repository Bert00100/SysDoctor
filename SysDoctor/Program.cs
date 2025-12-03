using SysDoctor.Scripts;
using Spectre.Console;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Text;

namespace SysDoctor
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                // Configurar o terminal para suportar UTF-8 e emojis
                ConfigurarTerminalParaEmojis();

                // Continua a execução do programa
                bool continuar = true;

                while (continuar)
                {

                    Console.Clear();
                    
                    // Exibe status de administrador no topo
                    MostrarStatusAdministrador();
                    
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
                            ExecutarComTratamento(() => InfoMachine.Executar(), "Informação da Máquina");
                            break;
                        case 2:
                            Console.Clear();
                            ExecutarComTratamento(() => ClearDisk.Executar(), "Limpeza de Disco");
                            break;
                        case 3:
                            Console.Clear();
                            ExecutarComTratamento(() => ScanWindow.Executar(), "Scanner do Windows");
                            break;
                        case 4:
                            Console.Clear();
                            ExecutarComTratamento(() => ClearRAM.Executar(), "Limpeza de RAM");
                            break;
                        case 5:
                            Console.Clear();
                            ExecutarComTratamento(() => SpeedTest.Executar(), "SpeedTest");
                            break;
                        case 6:
                            Console.Clear();
                            ExecutarComTratamento(() => ClearEthernet.Executar(), "Limpeza de Ethernet");
                            break;
                        case 7:
                            Console.Clear();
                            ExecutarComTratamentoAsync(async () => await TestPing.Executar(), "Teste de Ping");
                            break;
                        case 8:
                            Console.Clear();
                            ExecutarComTratamento(() => OtmPing.Executar(), "Otimizar Ping");
                            break;
                        case 9:
                            Console.Clear();
                            ExecutarComTratamento(() => OtmWifi.Executar(), "Otimizar Wifi");
                            break;
                        case 10:
                            Console.Clear();
                            ExecutarComTratamento(() => MapNet.Executar(), "Mapa de Conexão");
                            break;
                        case 11:
                            Console.Clear();
                            ExecutarComTratamento(() => checkTemperature.Executar(), "Verificar Temperatura");
                            break;
                        case 12:
                            Console.Clear();
                            ExecutarComTratamento(() => OtmWindows.Executar(), "Otimizar Windows");
                            break;
                        case 13:
                            Console.Clear();
                            ExecutarComTratamento(() => PointReset.Executar(), "Criar Ponto de Restauração");
                            break;
                        case 14:
                            Console.Clear();
                            ExecutarComTratamento(() => ConfigPosInstall.Executar(), "Configuração Pós-Instalação");
                            break;
                        case 15:
                            Console.Clear();
                            ExecutarComTratamento(() => UpdateWindows.Executar(), "Atualizar Windows");
                            break;
                        case 16:
                            Console.Clear();
                            ExecutarComTratamento(() => RunDefender.Executar(), "Rodar Windows Defender");
                            break;
                        case 17:
                            Console.Clear();
                            ExecutarComTratamento(() => IsoWin.Executar(), "ISO Windows");
                            break;
                        case 18:
                            Console.Clear();
                            ExecutarComTratamento(() => PackPrograms.Executar(), "Pack de Programas");
                            break;
                        case 0:
                            Console.Clear();
                            CentralizarTexto("[red]Encerrando...[/]", true);
                            continuar = false;
                            break;
                        default:
                            Console.Clear();
                            if (opcao > 18 || opcao < 0)
                            {
                                CentralizarTexto("[red]❌ Opção inválida! Digite apenas números de 1 a 18 ou 0 para sair.[/]", true);
                            }
                            else
                            {
                                CentralizarTexto("[red]❌ Opção inválida![/]", true);
                            }
                            AnsiConsole.WriteLine();
                            CentralizarTexto("[dim]Pressione qualquer tecla para continuar...[/]", true);
                            Console.ReadKey();
                            break;
                    }

                    if (continuar)
                    {
                        AnsiConsole.WriteLine();
                        CentralizarTexto("[dim]Pressione qualquer tecla para continuar...[/]", true);
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\nERRO FATAL:\n" + ex.ToString());
                Console.ResetColor();
                Console.WriteLine("\nPressione qualquer tecla para sair...");
                Console.ReadKey();
            }
        }

        private static void ConfigurarTerminalParaEmojis()
        {
            try
            {
                // Configura o terminal para UTF-8
                Console.OutputEncoding = Encoding.UTF8;
                Console.InputEncoding = Encoding.UTF8;
                
                // Ativa suporte a caracteres Unicode no Windows
                if (OperatingSystem.IsWindows())
                {
                    // Define o code page para UTF-8
                    SetConsoleOutputCP(65001);
                    SetConsoleCP(65001);
                }
            }
            catch
            {
                // Se falhar, continua sem configuração especial
            }
        }

        // APIs do Windows para suporte a emojis
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleOutputCP(uint wCodePageID);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCP(uint wCodePageID);

        private static bool IsRunningAsAdministrador()
        {
            try
            {
                // Verifica se está no Windows primeiro
                if (!OperatingSystem.IsWindows())
                    return false;

                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                // Se houver qualquer erro, considera como não sendo administrador
                return false;
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
            
            // Encontra a linha mais larga para usar como referência para centralização
            int larguraMaxima = 0;
            foreach (var linha in linhas)
            {
                if (!string.IsNullOrEmpty(linha.Trim()))
                {
                    larguraMaxima = Math.Max(larguraMaxima, linha.Length);
                }
            }
            
            // Centraliza baseado na largura máxima encontrada
            int paddingBase = Math.Max(0, (larguraTerminal - larguraMaxima) / 2);
            
            foreach (var linha in linhas)
            {
                if (!string.IsNullOrEmpty(linha.Trim()))
                {
                    AnsiConsole.MarkupLine("[blue]" + new string(' ', paddingBase) + linha + "[/]");
                }
                else if (!string.IsNullOrEmpty(linha))
                {
                    // Para linhas que contêm apenas espaços, mantém uma linha em branco
                    AnsiConsole.WriteLine();
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
                "[[ 15 ]] Atualizar Windows",
                "[[ 17 ]] Pack de programas"
            };

            var opcoesDir = new[]
            {
                "[[ 2 ]] Limpar SSD/HD",
                "[[ 4 ]] Limpar Memória RAM",
                "[[ 6 ]] Limpar Caches de Wifi/Ethernet", 
                "[[ 8 ]] Otimizar Ping ",
                "[[ 10 ]] Mapa de Conexão",
                "[[ 12 ]] Otimizar Windows",
                "[[ 14 ]] Configuração Pós-Instalação",
                "[[ 16 ]] Rodar Windows Defender",
                //"[[ 18 ]] ISO Windows 11 Pro otm"
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

        private static void MostrarStatusAdministrador()
        {
            Console.WriteLine();
            int larguraTerminal = Console.WindowWidth;
            
            bool temPrivilegios = IsRunningAsAdministrador();
            
            if (temPrivilegios)
            {
                // Linha superior
                string linhaSuperior = "╔═══════════════════════════════════════════════════════════╗";
                int paddingSuperior = Math.Max(0, (larguraTerminal - linhaSuperior.Length) / 2);
                Console.WriteLine(new string(' ', paddingSuperior) + linhaSuperior);
                
                // Status de administrador
                string statusAdmin = "║           🛡️  EXECUTANDO COMO ADMINISTRADOR ✅           ║";
                int paddingStatus = Math.Max(0, (larguraTerminal - statusAdmin.Length) / 2);
                AnsiConsole.MarkupLine(new string(' ', paddingStatus) + "[bold green]║           🛡️  EXECUTANDO COMO ADMINISTRADOR ✅           ║[/]");
                
                // Linha inferior
                string linhaInferior = "╚═══════════════════════════════════════════════════════════╝";
                int paddingInferior = Math.Max(0, (larguraTerminal - linhaInferior.Length) / 2);
                Console.WriteLine(new string(' ', paddingInferior) + linhaInferior);
            }
            else
            {
                // Linha superior
                string linhaSuperior = "╔═══════════════════════════════════════════════════════════╗";
                int paddingSuperior = Math.Max(0, (larguraTerminal - linhaSuperior.Length) / 2);
                Console.WriteLine(new string(' ', paddingSuperior) + linhaSuperior);
                
                // Status sem administrador
                string statusAdmin = "║    ⚠️  SEM PRIVILÉGIOS DE ADMINISTRADOR - NÃO RECOMENDADO   ║";
                int paddingStatus = Math.Max(0, (larguraTerminal - statusAdmin.Length) / 2);
                AnsiConsole.MarkupLine(new string(' ', paddingStatus) + "[bold yellow]║    ⚠️  SEM PRIVILÉGIOS DE ADMINISTRADOR - NÃO RECOMENDADO   ║[/]");
                
                // Linha inferior
                string linhaInferior = "╚═══════════════════════════════════════════════════════════╝";
                int paddingInferior = Math.Max(0, (larguraTerminal - linhaInferior.Length) / 2);
                Console.WriteLine(new string(' ', paddingInferior) + linhaInferior);
            }
            
            Console.WriteLine();
        }

        private static void ExecutarComTratamento(Action acao, string nomeFuncionalidade)
        {
            try
            {
                acao.Invoke();
            }
            catch (UnauthorizedAccessException)
            {
                AnsiConsole.MarkupLine("[red]❌ ACESSO NEGADO[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[yellow]⚠️  A funcionalidade '[yellow]" + nomeFuncionalidade + "[/]' requer privilégios de administrador![/]");
                AnsiConsole.MarkupLine("[cyan]💡 Para usar esta funcionalidade:[/]");
                AnsiConsole.MarkupLine("[white]1. Execute o programa como administrador[/]");
                AnsiConsole.MarkupLine("[white]2. Ou tente novamente com privilégios elevados[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine("[red]❌ ERRO ao executar " + nomeFuncionalidade + "[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[yellow]Erro: " + ex.Message + "[/]");
            }
        }

        private static void ExecutarComTratamentoAsync(Func<Task> acao, string nomeFuncionalidade)
        {
            try
            {
                acao.Invoke().Wait();
            }
            catch (AggregateException aex) when (aex.InnerException is UnauthorizedAccessException)
            {
                AnsiConsole.MarkupLine("[red]❌ ACESSO NEGADO[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[yellow]⚠️  A funcionalidade '[yellow]" + nomeFuncionalidade + "[/]' requer privilégios de administrador![/]");
                AnsiConsole.MarkupLine("[cyan]💡 Para usar esta funcionalidade:[/]");
                AnsiConsole.MarkupLine("[white]1. Execute o programa como administrador[/]");
                AnsiConsole.MarkupLine("[white]2. Ou tente novamente com privilégios elevados[/]");
            }
            catch (UnauthorizedAccessException)
            {
                AnsiConsole.MarkupLine("[red]❌ ACESSO NEGADO[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[yellow]⚠️  A funcionalidade '[yellow]" + nomeFuncionalidade + "[/]' requer privilégios de administrador![/]");
                AnsiConsole.MarkupLine("[cyan]💡 Para usar esta funcionalidade:[/]");
                AnsiConsole.MarkupLine("[white]1. Execute o programa como administrador[/]");
                AnsiConsole.MarkupLine("[white]2. Ou tente novamente com privilégios elevados[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine("[red]❌ ERRO ao executar " + nomeFuncionalidade + "[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[yellow]Erro: " + ex.Message + "[/]");
            }
        }
    }
}