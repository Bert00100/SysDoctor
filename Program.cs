using System.Diagnostics;

void ClearDiskAsAdmin()
{
    Console.WriteLine("=== INICIANDO LIMPEZA DE DISCO ===");

    // Primeiro comando: Optimize-Volume
    ExecutePowerShellAsAdmin("Import-Module Storage; Optimize-Volume -DriveLetter C -ReTrim -Verbose",
                           "OTIMIZAÇÃO DE VOLUME (TRIM)");

    // Segundo comando: Disk Cleanup
    ExecutePowerShellAsAdmin("Start-Process cleanmgr -ArgumentList '/sagerun:1' -Wait",
                           "LIMPEZA DE DISCO");

    // Terceiro comando: Desfragmentação
    ExecutePowerShellAsAdmin("defrag C: /U /V",
                           "DESFRAGMENTAÇÃO");

    Console.WriteLine("=== TODAS AS OPERAÇÕES CONCLUÍDAS ===");
    Console.WriteLine("Pressione Enter para sair...");
    Console.ReadLine();
}

void ExecutePowerShellAsAdmin(string command, string operationName)
{
    Console.WriteLine($"\n--- {operationName} ---");
    Console.WriteLine($"Comando: {command}");
    Console.WriteLine("Solicitando permissões de administrador...");

    var process = new Process();
    process.StartInfo.FileName = "powershell";
    process.StartInfo.Arguments = $"-Command \"{command}\"";
    process.StartInfo.UseShellExecute = true;
    process.StartInfo.Verb = "runas"; //Abre a Janela para pedir para executar como ADM
    process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

    try
    {
        Console.WriteLine("Iniciando processo...");
        bool started = process.Start(); //inicia o comando no powershell

        if (started)
        {
            Console.WriteLine("Processo iniciado com sucesso!");
            Console.WriteLine("Aguardando conclusão...");
            process.WaitForExit(); //Faz o programa esperar o comano a terminar

            //ExitCode é o código de saída que o progresso devolve

            Console.WriteLine($"Processo finalizado com código de saída: {process.ExitCode}");

            if (process.ExitCode == 0)
            {
                Console.WriteLine($"✓ {operationName} concluído com sucesso!");
            }
            else
            {
                Console.WriteLine($"⚠ {operationName} concluído com avisos (código: {process.ExitCode})");
            }
        }
        else
        {
            Console.WriteLine("Falha ao iniciar o processo.");
        }
    }
    catch (System.ComponentModel.Win32Exception ex)
    {
        Console.WriteLine($"❌ A execução como administrador foi cancelada ou falhou: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Erro inesperado: {ex.Message}");
    }
    finally
    {
        process?.Close();
    }

    Console.WriteLine($"--- FIM DA {operationName} ---");
}

// Executar a função principal
ClearDiskAsAdmin();