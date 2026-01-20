namespace SysDoctor.Scripts
{
    class ConfigPosInstall
    {
        private static string pastaScripts = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "scriptsSysdoctor");

        public static void Executar()
        {
            bool continuar = true;

            while (continuar)
            {
                Console.Clear();
                Console.WriteLine(@"
  ____              __ _        ____             ___           _        _            
 / ___|___  _ __   / _(_) __ _ |  _ \ ___  ___   |_ _|_ __  ___| |_ __ _| |           
| |   / _ \| '_ \ | |_| |/ _` || |_) / _ \/ __|   | || '_ \/ __| __/ _` | |           
| |__| (_) | | | ||  _| | (_| ||  __/ (_) \__ \   | || | | \__ \ || (_| | |           
 \____\___/|_| |_||_| |_|\__, ||_|   \___/|___/  |___|_| |_|___/\__\__,_|_|           
                         |___/                                                        
");
                Console.WriteLine("=== CONFIGURAÇÃO PÓS-INSTALAÇÃO ===");
                Console.WriteLine();

                // Verifica se a pasta existe
                if (!Directory.Exists(pastaScripts))
                {
                    CriarPastaScripts();
                    continuar = false;
                    break;
                }

                // Lista scripts disponíveis
                var scripts = ListarScripts();

                if (scripts.Count == 0)
                {
                    Console.WriteLine($"📁 Pasta encontrada em: {pastaScripts}");
                    Console.WriteLine();
                    Console.WriteLine("⚠️  Nenhum script encontrado (.bat ou .ps1)");
                    Console.WriteLine();
                    Console.WriteLine("💡 Adicione seus scripts de configuração na pasta para executá-los aqui.");
                    Console.WriteLine();
                    Console.WriteLine("[ 0 ] Voltar ao menu principal");
                    Console.WriteLine();
                    Console.Write("Digite sua opção: ");
                    
                    var input = Console.ReadLine();
                    if (input == "0" || string.IsNullOrEmpty(input))
                    {
                        continuar = false;
                    }
                    continue;
                }

                // Exibe menu de scripts
                ExibirMenuScripts(scripts);

                Console.WriteLine();
                Console.Write("Digite o número do script para executar (0 para voltar): ");
                
                if (int.TryParse(Console.ReadLine(), out int opcao))
                {
                    if (opcao == 0)
                    {
                        continuar = false;
                    }
                    else if (opcao >= 1 && opcao <= scripts.Count)
                    {
                        ExecutarScript(scripts[opcao - 1]);
                    }
                    else
                    {
                        Console.WriteLine("❌ Opção inválida!");
                        Console.WriteLine();
                        Console.WriteLine("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("❌ Entrada inválida!");
                    Console.WriteLine();
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        private static void CriarPastaScripts()
        {
            try
            {
                Directory.CreateDirectory(pastaScripts);
                
                Console.WriteLine("📁 Pasta 'scriptsSysdoctor' criada com sucesso!");
                Console.WriteLine($"📍 Local: {pastaScripts}");
                Console.WriteLine();
                Console.WriteLine("💡 Esta pasta é onde você pode colocar seus scripts de configuração:");
                Console.WriteLine("   • Arquivos .bat (Batch)");
                Console.WriteLine("   • Arquivos .ps1 (PowerShell)");
                Console.WriteLine();
                Console.WriteLine("🔧 Use esta funcionalidade para:");
                Console.WriteLine("   • Instalar programas automaticamente");
                Console.WriteLine("   • Configurar o Windows após formatação");
                Console.WriteLine("   • Executar comandos de otimização personalizados");
                Console.WriteLine("   • Automatizar tarefas repetitivas");
                Console.WriteLine();
                Console.WriteLine("⚠️  Todos os scripts serão executados como Administrador!");
                Console.WriteLine();
                Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao criar pasta: {ex.Message}");
                Console.WriteLine();
                Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                Console.ReadKey();
            }
        }

        private static List<string> ListarScripts()
        {
            var scripts = new List<string>();
            
            try
            {
                // Busca arquivos .bat
                var batFiles = Directory.GetFiles(pastaScripts, "*.bat");
                scripts.AddRange(batFiles);
                
                // Busca arquivos .ps1
                var ps1Files = Directory.GetFiles(pastaScripts, "*.ps1");
                scripts.AddRange(ps1Files);
                
                // Ordena alfabeticamente
                scripts.Sort();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao listar scripts: {ex.Message}");
            }
            
            return scripts;
        }

        private static void ExibirMenuScripts(List<string> scripts)
        {
            Console.WriteLine($"📁 Scripts encontrados em: {pastaScripts}");
            Console.WriteLine();
            Console.WriteLine("=== SCRIPTS DISPONÍVEIS ===");

            for (int i = 0; i < scripts.Count; i++)
            {
                var nomeArquivo = Path.GetFileName(scripts[i]);
                var extensao = Path.GetExtension(scripts[i]).ToUpper();
                var icone = extensao == ".BAT" ? "📄" : "🔷";
                
                Console.WriteLine($"[ {i + 1} ] {icone} {nomeArquivo} ({extensao})");
            }

            Console.WriteLine();
            Console.WriteLine("[ 0 ] Voltar ao menu principal");
        }

        private static void ExecutarScript(string caminhoScript)
        {
            try
            {
                var nomeScript = Path.GetFileName(caminhoScript);
                var extensao = Path.GetExtension(caminhoScript).ToLower();
                
                Console.Clear();
                Console.WriteLine($"🚀 Executando: {nomeScript}");
                Console.WriteLine();
                Console.WriteLine("⚠️  ATENÇÃO: O script será executado como Administrador!");
                Console.WriteLine();
                Console.Write("Deseja continuar? (S/N): ");
                
                var resposta = Console.ReadLine()?.ToUpper();
                if (resposta != "S" && resposta != "SIM" && resposta != "Y" && resposta != "YES")
                {
                    Console.WriteLine("❌ Execução cancelada.");
                    Console.WriteLine();
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine();
                Console.WriteLine("▶️  Iniciando execução...");
                Console.WriteLine();

                ProcessStartInfo processInfo = new ProcessStartInfo();
                
                if (extensao == ".bat")
                {
                    // Executa arquivo .bat
                    processInfo.FileName = "cmd.exe";
                    processInfo.Arguments = $"/c \"{caminhoScript}\"";
                }
                else if (extensao == ".ps1")
                {
                    // Executa arquivo .ps1
                    processInfo.FileName = "powershell.exe";
                    processInfo.Arguments = $"-ExecutionPolicy Bypass -File \"{caminhoScript}\"";
                }
                else
                {
                    Console.WriteLine("❌ Tipo de arquivo não suportado!");
                    Console.WriteLine();
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    return;
                }

                // Configurações para executar como administrador
                processInfo.UseShellExecute = true;
                processInfo.Verb = "runas";
                processInfo.WorkingDirectory = Path.GetDirectoryName(caminhoScript);

                var processo = Process.Start(processInfo);
                
                if (processo != null)
                {
                    Console.WriteLine("✅ Script iniciado com sucesso!");
                    Console.WriteLine("⏳ Aguardando conclusão...");
                    
                    // Aguarda o processo terminar
                    processo.WaitForExit();
                    
                    Console.WriteLine();
                    if (processo.ExitCode == 0)
                    {
                        Console.WriteLine("✅ Script executado com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine($"⚠️  Script concluído com código de saída: {processo.ExitCode}");
                    }
                }
                else
                {
                    Console.WriteLine("❌ Falha ao iniciar o script!");
                }
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.NativeErrorCode == 1223) // ERROR_CANCELLED
                {
                    Console.WriteLine("❌ Execução cancelada pelo usuário (UAC).");
                }
                else
                {
                    Console.WriteLine($"❌ Erro do Windows: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao executar script: {ex.Message}");
            }
            
            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}