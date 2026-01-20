 using Spectre.Console;
using System.Diagnostics;

namespace SysDoctor.Scripts
{
    public class PackPrograms
    {
        public static void Executar()
        {
            bool continuar = true;

            while (continuar)
            {
                Console.Clear();
                ExibirMenu();

                var opcao = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[cyan]Escolha um programa:[/]")
                        .AddChoices(ObterOpcoes()));

                if (opcao == "0 - Voltar")
                {
                    continuar = false;
                    break;
                }

                // Extrai o número da opção
                int numero = int.Parse(opcao.Split(' ')[0]);
                ProcessarEscolha(numero);

                if (continuar)
                {
                    AnsiConsole.MarkupLine("[dim]Pressione qualquer tecla para continuar...[/]");
                    Console.ReadKey();
                }
            }
        }

        private static void ExibirMenu()
        {
            AnsiConsole.Write(
                new FigletText("Pack de Programas")
                    .LeftJustified()
                    .Color(Color.Green));

            AnsiConsole.MarkupLine("[cyan]📦 Repositório de ferramentas essenciais para Windows[/]\n");
        }

        private static string[] ObterOpcoes()
        {
            return new[]
            {
                "1 - CPU-Z",
                "2 - GPU-Z",
                "3 - HWiNFO",
                "4 - CrystalDiskInfo",
                "5 - CrystalDiskMark",
                "6 - MemTest86",
                "7 - FurMark",
                "8 - Prime95",
                "9 - Speccy",
                "10 - BleachBit",
                "11 - TreeSize Free",
                "12 - WizTree",
                "13 - Autoruns",
                "14 - Process Explorer",
                "15 - Everything",
                "16 - Geek Uninstaller",
                "17 - Revo Uninstaller Free",
                "18 - WinDirStat",
                "19 - CCleaner",
                "20 - Glary Utilities",
                "21 - Wireshark",
                "22 - NirSoft Network Tools",
                "23 - Angry IP Scanner",
                "24 - PuTTY",
                "25 - WinSCP",
                "26 - FileZilla",
                "27 - NetSpot",
                "28 - GlassWire",
                "29 - Advanced IP Scanner",
                "30 - VeraCrypt",
                "31 - Macrium Reflect Free",
                "32 - Clonezilla",
                "33 - Recuva",
                "34 - TestDisk",
                "35 - MiniTool Partition Wizard Free",
                "36 - AOMEI Backupper Standard",
                "37 - AnyDesk",
                "38 - TeamViewer",
                "39 - UltraVNC",
                "40 - mRemoteNG",
                "41 - 7-Zip",
                "42 - WinRAR",
                "43 - Rufus",
                "44 - BalenaEtcher",
                "45 - Hiren's BootCD PE",
                "46 - Ninite",
                "47 - Snappy Driver Installer Origin",
                "48 - USBDeview",
                "49 - Sumatra PDF",
                "50 - Notepad++",
                "0 - Voltar"
            };
        }

        private static void ProcessarEscolha(int opcao)
        {
            Console.Clear();
            
            string nome = "";
            string url = "";
            string categoria = "";

            switch (opcao)
            {
                case 1:
                    nome = "CPU-Z";
                    url = "https://www.cpuid.com/softwares/cpu-z.html";
                    categoria = "Diagnóstico e Monitoramento";
                    break;
                case 2:
                    nome = "GPU-Z";
                    url = "https://www.techpowerup.com/gpuz/";
                    categoria = "Diagnóstico e Monitoramento";
                    break;
                case 3:
                    nome = "HWiNFO";
                    url = "https://www.hwinfo.com/download/";
                    categoria = "Diagnóstico e Monitoramento";
                    break;
                case 4:
                    nome = "CrystalDiskInfo";
                    url = "https://crystalmark.info/en/software/crystaldiskinfo/";
                    categoria = "Diagnóstico e Monitoramento";
                    break;
                case 5:
                    nome = "CrystalDiskMark";
                    url = "https://crystalmark.info/en/software/crystaldiskmark/";
                    categoria = "Diagnóstico e Monitoramento";
                    break;
                case 6:
                    nome = "MemTest86";
                    url = "https://www.memtest86.com/download.htm";
                    categoria = "Diagnóstico e Monitoramento";
                    break;
                case 7:
                    nome = "FurMark";
                    url = "https://geeks3d.com/furmark/";
                    categoria = "Diagnóstico e Monitoramento";
                    break;
                case 8:
                    nome = "Prime95";
                    url = "https://www.mersenne.org/download/";
                    categoria = "Diagnóstico e Monitoramento";
                    break;
                case 9:
                    nome = "Speccy";
                    url = "https://www.ccleaner.com/speccy";
                    categoria = "Diagnóstico e Monitoramento";
                    break;
                case 10:
                    nome = "BleachBit";
                    url = "https://www.bleachbit.org/download";
                    categoria = "Limpeza e Otimização";
                    break;
                case 11:
                    nome = "TreeSize Free";
                    url = "https://www.jam-software.com/treesize_free";
                    categoria = "Limpeza e Otimização";
                    break;
                case 12:
                    nome = "WizTree";
                    url = "https://wiztreefree.com/";
                    categoria = "Limpeza e Otimização";
                    break;
                case 13:
                    nome = "Autoruns";
                    url = "https://docs.microsoft.com/en-us/sysinternals/downloads/autoruns";
                    categoria = "Limpeza e Otimização";
                    break;
                case 14:
                    nome = "Process Explorer";
                    url = "https://docs.microsoft.com/en-us/sysinternals/downloads/process-explorer";
                    categoria = "Limpeza e Otimização";
                    break;
                case 15:
                    nome = "Everything";
                    url = "https://www.voidtools.com/downloads/";
                    categoria = "Limpeza e Otimização";
                    break;
                case 16:
                    nome = "Geek Uninstaller";
                    url = "https://geekuninstaller.com/download";
                    categoria = "Limpeza e Otimização";
                    break;
                case 17:
                    nome = "Revo Uninstaller Free";
                    url = "https://www.revouninstaller.com/products/revo-uninstaller-free/";
                    categoria = "Limpeza e Otimização";
                    break;
                case 18:
                    nome = "WinDirStat";
                    url = "https://windirstat.net/download.html";
                    categoria = "Limpeza e Otimização";
                    break;
                case 19:
                    nome = "CCleaner";
                    url = "https://www.ccleaner.com/ccleaner/download";
                    categoria = "Limpeza e Otimização";
                    break;
                case 20:
                    nome = "Glary Utilities";
                    url = "https://www.glarysoft.com/glary-utilities/";
                    categoria = "Limpeza e Otimização";
                    break;
                case 21:
                    nome = "Wireshark";
                    url = "https://www.wireshark.org/download.html";
                    categoria = "Rede e Conectividade";
                    break;
                case 22:
                    nome = "NirSoft Network Tools";
                    url = "https://www.nirsoft.net/utils/";
                    categoria = "Rede e Conectividade";
                    break;
                case 23:
                    nome = "Angry IP Scanner";
                    url = "https://angryip.org/download/";
                    categoria = "Rede e Conectividade";
                    break;
                case 24:
                    nome = "PuTTY";
                    url = "https://www.putty.org/";
                    categoria = "Rede e Conectividade";
                    break;
                case 25:
                    nome = "WinSCP";
                    url = "https://winscp.net/eng/download.php";
                    categoria = "Rede e Conectividade";
                    break;
                case 26:
                    nome = "FileZilla";
                    url = "https://filezilla-project.org/download.php?type=client";
                    categoria = "Rede e Conectividade";
                    break;
                case 27:
                    nome = "NetSpot";
                    url = "https://www.netspotapp.com/";
                    categoria = "Rede e Conectividade";
                    break;
                case 28:
                    nome = "GlassWire";
                    url = "https://www.glasswire.com/download/";
                    categoria = "Rede e Conectividade";
                    break;
                case 29:
                    nome = "Advanced IP Scanner";
                    url = "https://www.advanced-ip-scanner.com/";
                    categoria = "Rede e Conectividade";
                    break;
                case 30:
                    nome = "VeraCrypt";
                    url = "https://www.veracrypt.fr/en/Downloads.html";
                    categoria = "Backup e Recuperação";
                    break;
                case 31:
                    nome = "Macrium Reflect Free";
                    url = "https://www.macrium.com/reflectfree";
                    categoria = "Backup e Recuperação";
                    break;
                case 32:
                    nome = "Clonezilla";
                    url = "https://clonezilla.org/downloads.php";
                    categoria = "Backup e Recuperação";
                    break;
                case 33:
                    nome = "Recuva";
                    url = "https://www.ccleaner.com/recuva/download";
                    categoria = "Backup e Recuperação";
                    break;
                case 34:
                    nome = "TestDisk";
                    url = "https://www.cgsecurity.org/wiki/TestDisk_Download";
                    categoria = "Backup e Recuperação";
                    break;
                case 35:
                    nome = "MiniTool Partition Wizard Free";
                    url = "https://www.partitionwizard.com/free-partition-manager.html";
                    categoria = "Backup e Recuperação";
                    break;
                case 36:
                    nome = "AOMEI Backupper Standard";
                    url = "https://www.aomeitech.com/ab/standard.html";
                    categoria = "Backup e Recuperação";
                    break;
                case 37:
                    nome = "AnyDesk";
                    url = "https://anydesk.com/pt/downloads";
                    categoria = "Acesso Remoto";
                    break;
                case 38:
                    nome = "TeamViewer";
                    url = "https://www.teamviewer.com/pt-br/download/";
                    categoria = "Acesso Remoto";
                    break;
                case 39:
                    nome = "UltraVNC";
                    url = "https://www.uvnc.com/downloads/";
                    categoria = "Acesso Remoto";
                    break;
                case 40:
                    nome = "mRemoteNG";
                    url = "https://mremoteng.org/download";
                    categoria = "Acesso Remoto";
                    break;
                case 41:
                    nome = "7-Zip";
                    url = "https://www.7-zip.org/download.html";
                    categoria = "Utilitários Diversos";
                    break;
                case 42:
                    nome = "WinRAR";
                    url = "https://www.win-rar.com/download.html";
                    categoria = "Utilitários Diversos";
                    break;
                case 43:
                    nome = "Rufus";
                    url = "https://rufus.ie/pt_BR/";
                    categoria = "Utilitários Diversos";
                    break;
                case 44:
                    nome = "BalenaEtcher";
                    url = "https://www.balena.io/etcher";
                    categoria = "Utilitários Diversos";
                    break;
                case 45:
                    nome = "Hiren's BootCD PE";
                    url = "https://www.hirensbootcd.org/download/";
                    categoria = "Utilitários Diversos";
                    break;
                case 46:
                    nome = "Ninite";
                    url = "https://ninite.com/";
                    categoria = "Utilitários Diversos";
                    break;
                case 47:
                    nome = "Snappy Driver Installer Origin";
                    url = "https://www.snappy-driver-installer.org/";
                    categoria = "Utilitários Diversos";
                    break;
                case 48:
                    nome = "USBDeview";
                    url = "https://www.nirsoft.net/utils/usbdeview.html";
                    categoria = "Utilitários Diversos";
                    break;
                case 49:
                    nome = "Sumatra PDF";
                    url = "https://www.sumatrapdfreader.org/download-free-pdf-viewer";
                    categoria = "Utilitários Diversos";
                    break;
                case 50:
                    nome = "Notepad++";
                    url = "https://notepad-plus-plus.org/downloads/";
                    categoria = "Utilitários Diversos";
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]❌ Opção inválida![/]");
                    return;
            }

            // Exibir detalhes do programa
            AnsiConsole.Write(
                new Panel($"[bold green]{nome}[/]\n[dim]{categoria}[/]")
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Green));

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[cyan]Link para Download:[/]");
            AnsiConsole.MarkupLine($"[blue underline]{url}[/]\n");

            // Opções de ação
            var acao = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]O que deseja fazer?[/]")
                    .AddChoices(new[] { 
                        "Abrir no navegador", 
                        "Copiar link", 
                        "Voltar"
                    }));

            switch (acao)
            {
                case "Abrir no navegador":
                    AbrirNoNavegador(url);
                    break;
                case "Copiar link":
                    CopiarLinkParaClipboard(url);
                    break;
            }
        }

        private static void AbrirNoNavegador(string url)
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
                AnsiConsole.MarkupLine($"[red]❌ Erro ao abrir navegador: {ex.Message}[/]");
            }
        }

        private static void CopiarLinkParaClipboard(string url)
        {
            try
            {
                // Cria um processo para copiar para clipboard no Windows
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell",
                        Arguments = $"-Command \"'{url}' | Set-Clipboard\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };
                proc.Start();
                proc.WaitForExit();

                AnsiConsole.MarkupLine("[green]✓ Link copiado para a área de transferência![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Erro ao copiar link: {ex.Message}[/]");
            }
        }
    }
}
