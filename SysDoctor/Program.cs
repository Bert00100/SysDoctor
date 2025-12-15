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
            ConfigurarTerminalParaEmojis();

            // PASSO 1: Verificar email ANTES de mostrar qualquer coisa
            bool acessoPermitido = false;

            while (!acessoPermitido)
            {
                acessoPermitido = await CheckUsers.Executar();

                if (!acessoPermitido)
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLine("[yellow]Por favor, tente novamente com um email válido.[/]");
                    AnsiConsole.WriteLine();
                }
            }

            // PASSO 2: Após email validado, mostrar o app
            AnsiConsole.Clear();
            MostrarAsciiArt();
            MostrarStatusAdministrador();
            
            // PASSO 3: Loop do menu principal
            await MenuPrincipalAsync();
        }

        private static async Task MenuPrincipalAsync()
        {
            bool sair = false;

            while (!sair)
            {
                MostrarAsciiArt();
                MostrarMenu();
                CentralizarInput();

                string opcao = Console.ReadLine();
                AnsiConsole.WriteLine();

                switch (opcao)
                {
                    case "1":
                        await ExecutarComTratamentoAsync(() => Task.Run(() => InfoMachine.Executar()), "Informação da Máquina");
                        break;

                    case "2":
                        await ExecutarComTratamentoAsync(() => Task.Run(() => ClearDisk.Executar()), "Limpar SSD/HD");
                        break;

                    case "3":
                        await ExecutarComTratamentoAsync(() => Task.Run(() => ScanWindow.Executar()), "Scanner do Windows");
                        break;

                    case "4":
                        await ExecutarComTratamentoAsync(() => ClearRAM.Executar(), "Limpar Memória RAM");
                        break;

                    case "5":
                        await ExecutarComTratamentoAsync(() => Task.Run(() => SpeedTest.Executar()), "SpeedTest");
                        break;

                    case "6":
                        await ExecutarComTratamentoAsync(() => Task.Run(() => ClearEthernet.Executar()), "Limpar Caches de Wifi/Ethernet");
                        break;

                    case "7":
                        await ExecutarComTratamentoAsync(() => TestPing.Executar(), "Teste de Ping");
                        break;

                    case "8":
                        await ExecutarComTratamentoAsync(() => Task.Run(() => OtmPing.Executar()), "Otimizar Ping");
                        break;

                    case "9":
                        await ExecutarComTratamentoAsync(() => Task.Run(() => OtmWifi.Executar()), "Otimizar Wifi");
                        break;

                    case "10":
                        await ExecutarComTratamentoAsync(() => Task.Run(() => MapNet.Executar()), "Mapa de Conexão");
                        break;

                    case "11":
                        await ExecutarComTratamentoAsync(() => Task.Run(() => checkTemperature.Executar()), "Verificar Temperatura");
                        break;

                    case "12":
                        await ExecutarComTratamentoAsync(() => Task.Run(() => OtmWindows.Executar()), "Otimizar Windows");
                        break;

                    case "13":
                        await ExecutarComTratamentoAsync(() => Task.Run(() => PointReset.Executar()), "Criar Ponto de Restauração");
                        break;

                    case "14":
                        await ExecutarComTratamentoAsync(() => Task.Run(() => ConfigPosInstall.Executar()), "Configuração Pós-Instalação");
                        break;

                    case "15":
                        await ExecutarComTratamentoAsync(() => Task.Run(() => UpdateWindows.Executar()), "Atualizar Windows");
                        break;

                    case "16":
                        await ExecutarComTratamentoAsync(() => Task.Run(() => RunDefender.Executar()), "Rodar Windows Defender");
                        break;

                    case "17":
                        await ExecutarComTratamentoAsync(() => Task.Run(() => PackPrograms.Executar()), "Pack de programas");
                        break;

                    case "18":
                        await ExecutarComTratamentoAsync(() => Task.Run(() => IsoWin.Executar()), "ISO Windows 11 Pro");
                        break;

                    case "0":
                        AnsiConsole.MarkupLine("[yellow]👋 Saindo do SysDoctor...[/]");
                        sair = true;
                        break;

                    default:
                        AnsiConsole.MarkupLine("[red]❌ Opção inválida! Tente novamente.[/]");
                        break;
                }

                if (!sair)
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLine("[dim]Pressione ENTER para continuar...[/]");
                    Console.ReadLine();
                    AnsiConsole.Clear();
                }
            }
        }

        private static void ConfigurarTerminalParaEmojis()
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
                Console.InputEncoding = Encoding.UTF8;
                
                if (OperatingSystem.IsWindows())
                {
                    SetConsoleOutputCP(65001);
                    SetConsoleCP(65001);
                }
            }
            catch
            {
                // Se falhar, continua sem configuração especial
            }
        }

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleOutputCP(uint wCodePageID);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCP(uint wCodePageID);

        private static bool IsRunningAsAdministrador()
        {
            try
            {
                if (!OperatingSystem.IsWindows())
                    return false;

                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
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
            
            int larguraMaxima = 0;
            foreach (var linha in linhas)
            {
                if (!string.IsNullOrEmpty(linha.Trim()))
                {
                    larguraMaxima = Math.Max(larguraMaxima, linha.Length);
                }
            }
            
            int paddingBase = Math.Max(0, (larguraTerminal - larguraMaxima) / 2);
            
            foreach (var linha in linhas)
            {
                if (!string.IsNullOrEmpty(linha.Trim()))
                {
                    AnsiConsole.MarkupLine("[blue]" + new string(' ', paddingBase) + linha + "[/]");
                }
                else if (!string.IsNullOrEmpty(linha))
                {
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
                "[[ 18 ]] ISO Windows 11 Pro otm"
            };

            var table = new Table()
                .HideHeaders()
                .Border(TableBorder.None)
                .AddColumn(new TableColumn("").Width(40).PadRight(2))
                .AddColumn(new TableColumn("").Width(40))
                .AddRow(
                    new Panel(
                        string.Join("\n", opcoesEsq)
                    )
                    .Header("[blue]🔧 Ferramentas do Sistema[/]")
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Blue)
                    .Padding(1, 1),
                    
                    new Panel(
                        string.Join("\n", opcoesDir)
                    )
                    .Header("[green]🧹 Limpeza e Otimização[/]")
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Green)
                    .Padding(1, 1)
                );

            var centeredTable = Align.Center(table);
            AnsiConsole.Write(centeredTable);
            AnsiConsole.WriteLine();
            
            CentralizarTexto("[dim]📋 [[ 0 ]] Sair[/]", true);
            AnsiConsole.WriteLine();
        }

        private static void CentralizarTexto(string texto, bool usarMarkup)
        {
            var textoLimpo = System.Text.RegularExpressions.Regex.Replace(texto, @"\[.*?\]", "");
    
            int larguraTerminal = Console.WindowWidth;
            int padding = Math.Max(0, (larguraTerminal - textoLimpo.Length) / 2);
    
            if (usarMarkup)
            {
                AnsiConsole.MarkupLine(new string(' ', padding) + texto);
            }
            else
            {
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
                string linhaSuperior = "╔═══════════════════════════════════════════════════════════╗";
                int paddingSuperior = Math.Max(0, (larguraTerminal - linhaSuperior.Length) / 2);
                Console.WriteLine(new string(' ', paddingSuperior) + linhaSuperior);
                
                string statusAdmin = "║           🛡️  EXECUTANDO COMO ADMINISTRADOR ✅           ║";
                int paddingStatus = Math.Max(0, (larguraTerminal - statusAdmin.Length) / 2);
                AnsiConsole.MarkupLine(new string(' ', paddingStatus) + "[bold green]║           🛡️  EXECUTANDO COMO ADMINISTRADOR ✅           ║[/]");
                
                string linhaInferior = "╚═══════════════════════════════════════════════════════════╝";
                int paddingInferior = Math.Max(0, (larguraTerminal - linhaInferior.Length) / 2);
                Console.WriteLine(new string(' ', paddingInferior) + linhaInferior);
            }
            else
            {
                string linhaSuperior = "╔═══════════════════════════════════════════════════════════╗";
                int paddingSuperior = Math.Max(0, (larguraTerminal - linhaSuperior.Length) / 2);
                Console.WriteLine(new string(' ', paddingSuperior) + linhaSuperior);
                
                string statusAdmin = "║    ⚠️  SEM PRIVILÉGIOS DE ADMINISTRADOR - NÃO RECOMENDADO   ║";
                int paddingStatus = Math.Max(0, (larguraTerminal - statusAdmin.Length) / 2);
                AnsiConsole.MarkupLine(new string(' ', paddingStatus) + "[bold yellow]║    ⚠️  SEM PRIVILÉGIOS DE ADMINISTRADOR - NÃO RECOMENDADO   ║[/]");
                
                string linhaInferior = "╚═══════════════════════════════════════════════════════════╝";
                int paddingInferior = Math.Max(0, (larguraTerminal - linhaInferior.Length) / 2);
                Console.WriteLine(new string(' ', paddingInferior) + linhaInferior);
            }
            
            Console.WriteLine();
        }

        private static async Task ExecutarComTratamentoAsync(Func<Task> acao, string nomeFuncionalidade)
        {
            try
            {
                await acao.Invoke();
            }
            catch (AggregateException aex) when (aex.InnerException is UnauthorizedAccessException)
            {
                AnsiConsole.MarkupLine("[red]❌ ACESSO NEGADO[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"[yellow]⚠️  A funcionalidade '[yellow]{nomeFuncionalidade}[/]' requer privilégios de administrador![/]");
                AnsiConsole.MarkupLine("[cyan]💡 Para usar esta funcionalidade:[/]");
                AnsiConsole.MarkupLine("[white]1. Execute o programa como administrador[/]");
                AnsiConsole.MarkupLine("[white]2. Ou tente novamente com privilégios elevados[/]");
            }
            catch (UnauthorizedAccessException)
            {
                AnsiConsole.MarkupLine("[red]❌ ACESSO NEGADO[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"[yellow]⚠️  A funcionalidade '[yellow]{nomeFuncionalidade}[/]' requer privilégios de administrador![/]");
                AnsiConsole.MarkupLine("[cyan]💡 Para usar esta funcionalidade:[/]");
                AnsiConsole.MarkupLine("[white]1. Execute o programa como administrador[/]");
                AnsiConsole.MarkupLine("[white]2. Ou tente novamente com privilégios elevados[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ ERRO ao executar {nomeFuncionalidade}[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"[yellow]Erro: {ex.Message}[/]");
            }
        }
    }
}