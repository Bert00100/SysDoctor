namespace SysDoctor.Scripts
{
    public class PackPrograms
    {

        public static void Executar()
        {
            bool continuar = true;

            while (continuar)
            {
                ExibirMenu();

                Console.Write("\nDigite o número do programa (0 para voltar): ");
                string? escolha = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(escolha) || escolha == "0")
                {
                    continuar = false;
                    break;
                }

                // Validação: aceitar apenas números válidos
                if (!int.TryParse(escolha, out int numero) || numero < 1 || numero > 50)
                {
                    Console.Clear();
                    Console.WriteLine("❌ Opção inválida! Por favor, escolha um número entre 1 e 50 (ou 0 para voltar).");
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    continue;
                }

                ProcessarEscolha(numero);

                if (continuar)
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        private static void ExibirMenu()
        {
            Console.Clear();
            Console.WriteLine(@"
  ____                   _        ____
 |  _ \    __ _    ___  | | __   |  _ \   _ __    ___     __ _   _ __    __ _   _ __ ___    ___
 | |_) |  / _` |  / __| | |/ /   | |_) | | '__|  / _ \   / _` | | '__|  / _` | | '_ ` _ \  / __|
 |  __/  | (_| | | (__  |   <    |  __/  | |    | (_) | | (_| | | |    | (_| | | | | | | | \__ \
 |_|      \__,_|  \___| |_|\_\   |_|     |_|     \___/   \__, | |_|     \__,_| |_| |_| |_| |___/
                                                         |___/
");
            Console.WriteLine("Selecione um programa para ver o link de download:");
            Console.WriteLine();

            Console.WriteLine("=== DIAGNÓSTICO E MONITORAMENTO ===");
            Console.WriteLine("[ 1 ] CPU-Z");
            Console.WriteLine("[ 2 ] GPU-Z");
            Console.WriteLine("[ 3 ] HWiNFO");
            Console.WriteLine("[ 4 ] CrystalDiskInfo");
            Console.WriteLine("[ 5 ] CrystalDiskMark");
            Console.WriteLine("[ 6 ] MemTest86");
            Console.WriteLine("[ 7 ] FurMark");
            Console.WriteLine("[ 8 ] Prime95");
            Console.WriteLine("[ 9 ] Speccy");
            Console.WriteLine();

            Console.WriteLine("=== LIMPEZA E OTIMIZAÇÃO ===");
            Console.WriteLine("[ 10 ] BleachBit");
            Console.WriteLine("[ 11 ] TreeSize Free");
            Console.WriteLine("[ 12 ] WizTree");
            Console.WriteLine("[ 13 ] Autoruns");
            Console.WriteLine("[ 14 ] Process Explorer");
            Console.WriteLine("[ 15 ] Everything");
            Console.WriteLine("[ 16 ] Geek Uninstaller");
            Console.WriteLine("[ 17 ] Revo Uninstaller Free");
            Console.WriteLine("[ 18 ] WinDirStat");
            Console.WriteLine("[ 19 ] CCleaner");
            Console.WriteLine("[ 20 ] Glary Utilities");
            Console.WriteLine();

            Console.WriteLine("=== REDE E CONECTIVIDADE ===");
            Console.WriteLine("[ 21 ] Wireshark");
            Console.WriteLine("[ 22 ] NirSoft Network Tools");
            Console.WriteLine("[ 23 ] Angry IP Scanner");
            Console.WriteLine("[ 24 ] PuTTY");
            Console.WriteLine("[ 25 ] WinSCP");
            Console.WriteLine("[ 26 ] FileZilla");
            Console.WriteLine("[ 27 ] NetSpot");
            Console.WriteLine("[ 28 ] GlassWire");
            Console.WriteLine("[ 29 ] Advanced IP Scanner");
            Console.WriteLine();

            Console.WriteLine("=== BACKUP E RECUPERAÇÃO ===");
            Console.WriteLine("[ 30 ] VeraCrypt");
            Console.WriteLine("[ 31 ] Macrium Reflect Free");
            Console.WriteLine("[ 32 ] Clonezilla");
            Console.WriteLine("[ 33 ] Recuva");
            Console.WriteLine("[ 34 ] TestDisk");
            Console.WriteLine("[ 35 ] MiniTool Partition Wizard Free");
            Console.WriteLine("[ 36 ] AOMEI Backupper Standard");
            Console.WriteLine();

            Console.WriteLine("=== ACESSO REMOTO ===");
            Console.WriteLine("[ 37 ] AnyDesk");
            Console.WriteLine("[ 38 ] TeamViewer");
            Console.WriteLine("[ 39 ] UltraVNC");
            Console.WriteLine("[ 40 ] mRemoteNG");
            Console.WriteLine();

            Console.WriteLine("=== UTILITÁRIOS DIVERSOS ===");
            Console.WriteLine("[ 41 ] 7-Zip");
            Console.WriteLine("[ 42 ] WinRAR");
            Console.WriteLine("[ 43 ] Rufus");
            Console.WriteLine("[ 44 ] BalenaEtcher");
            Console.WriteLine("[ 45 ] Hiren's BootCD PE");
            Console.WriteLine("[ 46 ] Ninite");
            Console.WriteLine("[ 47 ] Snappy Driver Installer Origin");
            Console.WriteLine("[ 48 ] USBDeview");
            Console.WriteLine("[ 49 ] Sumatra PDF");
            Console.WriteLine("[ 50 ] Notepad++");
            Console.WriteLine();
            
            Console.WriteLine("[ 0 ] Voltar ao menu principal");
        }

        private static void ProcessarEscolha(int opcao)
        {
            Console.Clear();
            
            string nome = "";
            string url = "";

            switch (opcao)
            {
                case 1:
                    nome = "CPU-Z";
                    url = "https://www.cpuid.com/softwares/cpu-z.html";
                    break;
                case 2:
                    nome = "GPU-Z";
                    url = "https://www.techpowerup.com/gpuz/";
                    break;
                case 3:
                    nome = "HWiNFO";
                    url = "https://www.hwinfo.com/download/";
                    break;
                case 4:
                    nome = "CrystalDiskInfo";
                    url = "https://crystalmark.info/en/software/crystaldiskinfo/";
                    break;
                case 5:
                    nome = "CrystalDiskMark";
                    url = "https://crystalmark.info/en/software/crystaldiskmark/";
                    break;
                case 6:
                    nome = "MemTest86";
                    url = "https://www.memtest86.com/download.htm";
                    break;
                case 7:
                    nome = "FurMark";
                    url = "https://geeks3d.com/furmark/";
                    break;
                case 8:
                    nome = "Prime95";
                    url = "https://www.mersenne.org/download/";
                    break;
                case 9:
                    nome = "Speccy";
                    url = "https://www.ccleaner.com/speccy";
                    break;
                case 10:
                    nome = "BleachBit";
                    url = "https://www.bleachbit.org/download";
                    break;
                case 11:
                    nome = "TreeSize Free";
                    url = "https://www.jam-software.com/treesize_free";
                    break;
                case 12:
                    nome = "WizTree";
                    url = "https://wiztreefree.com/";
                    break;
                case 13:
                    nome = "Autoruns";
                    url = "https://docs.microsoft.com/en-us/sysinternals/downloads/autoruns";
                    break;
                case 14:
                    nome = "Process Explorer";
                    url = "https://docs.microsoft.com/en-us/sysinternals/downloads/process-explorer";
                    break;
                case 15:
                    nome = "Everything";
                    url = "https://www.voidtools.com/downloads/";
                    break;
                case 16:
                    nome = "Geek Uninstaller";
                    url = "https://geekuninstaller.com/download";
                    break;
                case 17:
                    nome = "Revo Uninstaller Free";
                    url = "https://www.revouninstaller.com/products/revo-uninstaller-free/";
                    break;
                case 18:
                    nome = "WinDirStat";
                    url = "https://windirstat.net/download.html";
                    break;
                case 19:
                    nome = "CCleaner";
                    url = "https://www.ccleaner.com/ccleaner/download";
                    break;
                case 20:
                    nome = "Glary Utilities";
                    url = "https://www.glarysoft.com/glary-utilities/";
                    break;
                case 21:
                    nome = "Wireshark";
                    url = "https://www.wireshark.org/download.html";
                    break;
                case 22:
                    nome = "NirSoft Network Tools";
                    url = "https://www.nirsoft.net/utils/";
                    break;
                case 23:
                    nome = "Angry IP Scanner";
                    url = "https://angryip.org/download/";
                    break;
                case 24:
                    nome = "PuTTY";
                    url = "https://www.putty.org/";
                    break;
                case 25:
                    nome = "WinSCP";
                    url = "https://winscp.net/eng/download.php";
                    break;
                case 26:
                    nome = "FileZilla";
                    url = "https://filezilla-project.org/download.php?type=client";
                    break;
                case 27:
                    nome = "NetSpot";
                    url = "https://www.netspotapp.com/";
                    break;
                case 28:
                    nome = "GlassWire";
                    url = "https://www.glasswire.com/download/";
                    break;
                case 29:
                    nome = "Advanced IP Scanner";
                    url = "https://www.advanced-ip-scanner.com/";
                    break;
                case 30:
                    nome = "VeraCrypt";
                    url = "https://www.veracrypt.fr/en/Downloads.html";
                    break;
                case 31:
                    nome = "Macrium Reflect Free";
                    url = "https://www.macrium.com/reflectfree";
                    break;
                case 32:
                    nome = "Clonezilla";
                    url = "https://clonezilla.org/downloads.php";
                    break;
                case 33:
                    nome = "Recuva";
                    url = "https://www.ccleaner.com/recuva/download";
                    break;
                case 34:
                    nome = "TestDisk";
                    url = "https://www.cgsecurity.org/wiki/TestDisk_Download";
                    break;
                case 35:
                    nome = "MiniTool Partition Wizard Free";
                    url = "https://www.partitionwizard.com/free-partition-manager.html";
                    break;
                case 36:
                    nome = "AOMEI Backupper Standard";
                    url = "https://www.aomeitech.com/ab/standard.html";
                    break;
                case 37:
                    nome = "AnyDesk";
                    url = "https://anydesk.com/pt/downloads";
                    break;
                case 38:
                    nome = "TeamViewer";
                    url = "https://www.teamviewer.com/pt-br/download/";
                    break;
                case 39:
                    nome = "UltraVNC";
                    url = "https://www.uvnc.com/downloads/";
                    break;
                case 40:
                    nome = "mRemoteNG";
                    url = "https://mremoteng.org/download";
                    break;
                case 41:
                    nome = "7-Zip";
                    url = "https://www.7-zip.org/download.html";
                    break;
                case 42:
                    nome = "WinRAR";
                    url = "https://www.win-rar.com/download.html";
                    break;
                case 43:
                    nome = "Rufus";
                    url = "https://rufus.ie/pt_BR/";
                    break;
                case 44:
                    nome = "BalenaEtcher";
                    url = "https://www.balena.io/etcher";
                    break;
                case 45:
                    nome = "Hiren's BootCD PE";
                    url = "https://www.hirensbootcd.org/download/";
                    break;
                case 46:
                    nome = "Ninite";
                    url = "https://ninite.com/";
                    break;
                case 47:
                    nome = "Snappy Driver Installer Origin";
                    url = "https://www.snappy-driver-installer.org/";
                    break;
                case 48:
                    nome = "USBDeview";
                    url = "https://www.nirsoft.net/utils/usbdeview.html";
                    break;
                case 49:
                    nome = "Sumatra PDF";
                    url = "https://www.sumatrapdfreader.org/download-free-pdf-viewer";
                    break;
                case 50:
                    nome = "Notepad++";
                    url = "https://notepad-plus-plus.org/downloads/";
                    break;
                default:
                    Console.WriteLine("❌ Opção inválida!");
                    return;
            }

            Console.WriteLine($"=== {nome} ===");
            Console.WriteLine();
            Console.WriteLine("Link para download:");
            Console.WriteLine(url);
            Console.WriteLine();
            Console.WriteLine("Copie e cole o link acima no seu navegador para fazer o download.");
        }
    }
}
