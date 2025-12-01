using SysDoctor.Scripts;

namespace SysDoctor
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            // Verificação obrigatória de privilégios de administrador
            if (!IsRunningAsAdministrador())
            {
                Console.Clear();
                AnsiConsole.MarkupLine("[red]❌ ACESSO NEGADO![/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[yellow]⚠️  Este programa REQUER privilégios de Administrador para funcionar![/]");
                AnsiConsole.MarkupLine("[cyan]📋 Para executar o programa corretamente:[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[white]1. Clique com o botão direito no executável[/]");
                AnsiConsole.MarkupLine("[white]2. Selecione 'Executar como administrador'[/]");
                AnsiConsole.MarkupLine("[white]3. Confirme a solicitação do UAC[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[dim]O programa será encerrado agora.[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.Markup("[dim]Pressione qualquer tecla para sair...[/]");
                Console.ReadKey();
                
                // Encerra o programa forçadamente
                Environment.Exit(1);
                return;
            }

            // Continua a execução do programa apenas se for administrador
            bool continuar = true;

            while (continuar)
            {
                // Verificação contínua de privilégios de administrador
                if (!IsRunningAsAdministrador())
                {
                    Console.Clear();
                    AnsiConsole.MarkupLine("[red]❌ PRIVILÉGIOS DE ADMINISTRADOR PERDIDOS![/]");
                    AnsiConsole.MarkupLine("[yellow]O programa será encerrado por segurança.[/]");
                    AnsiConsole.WriteLine();
                    AnsiConsole.Markup("[dim]Pressione qualquer tecla para sair...[/]");
                    Console.ReadKey();
                    Environment.Exit(1);
                    return;
                }

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
                        Console.Clear();
                        ClearRAM.Executar();
                        break;
                    case 5:
                        Console.Clear();
                        SpeedTest.Executar();
                        break;
                    case 6:
                        Console.Clear();
                        ClearEthernet.Executar();
                        break;
                    case 7:
                        Console.Clear();
                        await TestPing.Executar();
                        break;
                    case 8:
                        Console.Clear();
                        OtmPing.Executar();
                        break;
                    case 9:
                        Console.Clear();
                        OtmWifi.Executar();
                        break;
                    case 10:
                        Console.Clear();
                        MapNet.Executar();
                        break;
                    case 11:
                        Console.Clear();
                        checkTemperature.Executar();
                        break;
                    case 12:
                        Console.Clear();
                        OtmWindows.Executar();
                        break;
                    case 13:
                        Console.Clear();
                        PointReset.Executar();
                        break;
                    case 14:
                        Console.Clear();
                        ConfigPosInstall.Executar();
                        break;
                    case 15:
                        Console.Clear();
                        UpdateWindows.Executar();
                        break;
                    case 16:
                        Console.Clear();
                        RunDefender.Executar();
                        break;
                    case 17:
                        Console.Clear();
                        IsoWin.Executar();
                        break;
                    case 18:
                        Console.Clear();
                        PackPrograms.Executar();
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
                "[[ 8 ]] Ottimizar Ping (DNS Jumper)",
                "[[ 10 ]] Mapa de Conexão",
                "[[ 12 ]] Otimizar Windows",
                "[[ 14 ]] Configuração Pós-Instalação",
                "[[ 16 ]] Rodar Windows Defender",
                "[[ 18 ]] ISO Windows 11 Pro otm"
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