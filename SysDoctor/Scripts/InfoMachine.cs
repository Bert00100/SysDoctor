namespace SysDoctor.Scripts
{
    class InfoMachine
    {
        public static void Executar()
        {
            AnsiConsole.MarkupLine("[blue] Informação da Máquina[/]");

            try
            {
                // Nome da máquina e usuário
                string machineName = Environment.MachineName;
                string userMachine = Environment.UserName;
                string serialMachine = GetMachineSerialNumber();

                // Informações adicionais
                string totalRam = GetTotalRAM();
                string diskInfo = GetDiskInfo();
                string gpuInfo = GetGPUInfo();
                string internalIP = GetIPAddress();
                string externalIP = GetExternalIPAddress();
                string dnsAddress = GetDNSAddress();
                string macAddress = GetMACAddress();

                // Exibe as informações
                AnsiConsole.MarkupLine($"[green]Nome da Máquina:[/] {machineName}");
                AnsiConsole.MarkupLine($"[green]Nome de Usuário:[/] {userMachine}");
                AnsiConsole.MarkupLine($"[green]Número de Série:[/] {serialMachine}");
                AnsiConsole.MarkupLine($"[green]Memória RAM Total:[/] {totalRam}");
                AnsiConsole.MarkupLine($"[green]Informações do Disco (SSD/HD):[/] {diskInfo}");
                AnsiConsole.MarkupLine($"[green]Placa Gráfica:[/] {gpuInfo}");
                AnsiConsole.MarkupLine($"[green]Endereço IP Interno:[/] {internalIP}");
                AnsiConsole.MarkupLine($"[green]Endereço IP Externo:[/] {externalIP}");
                AnsiConsole.MarkupLine($"[green]Servidor DNS:[/] {dnsAddress}");
                AnsiConsole.MarkupLine($"[green]Endereço MAC:[/] {macAddress}");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Erro ao obter informações: {ex.Message}[/]");
            }
        }

        private static string GetMachineSerialNumber()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BIOS"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return obj["SerialNumber"]?.ToString() ?? "Desconhecido";
                    }
                }
            }
            catch
            {
                return "Erro ao obter o número de série";
            }
            
            return "Desconhecido"; // Adicionado retorno padrão
        }

        private static string GetTotalRAM()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory"))
                {
                    long totalRam = 0;
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        totalRam += Convert.ToInt64(obj["Capacity"]);
                    }
                    return $"{totalRam / (1024 * 1024 * 1024)} GB";
                }
            }
            catch
            {
                return "Erro ao obter a memória RAM";
            }
        }

        private static string GetDiskInfo()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT Model, Size FROM Win32_DiskDrive"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string model = obj["Model"]?.ToString() ?? "Desconhecido";
                        long size = Convert.ToInt64(obj["Size"] ?? 0);
                        return $"{model} ({size / (1024 * 1024 * 1024)} GB)";
                    }
                }
            }
            catch
            {
                return "Erro ao obter informações do disco";
            }

            return "Desconhecido";
        }

        private static string GetGPUInfo()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_VideoController"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return obj["Name"]?.ToString() ?? "Desconhecido";
                    }
                }
            }
            catch
            {
                return "Erro ao obter informações da GPU";
            }
            
            return "Desconhecido"; // Adicionado retorno padrão
        }

        private static string GetIPAddress()
        {
            try
            {
                string hostName = Dns.GetHostName();
                var ipEntry = Dns.GetHostEntry(hostName);
                foreach (var ip in ipEntry.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch
            {
                return "Erro ao obter o endereço IP";
            }

            return "Desconhecido";
        }

        private static string GetExternalIPAddress()
        {
            try
            {
                using (var client = new WebClient())
                {
                    // Consulta uma API pública para obter o IP externo
                    string externalIP = client.DownloadString("https://api.ipify.org").Trim();
                    return externalIP;
                }
            }
            catch
            {
                return "Erro ao obter o IP externo";
            }
        }

        private static string GetDNSAddress()
        {
            try
            {
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (var adapter in networkInterfaces)
                {
                    var ipProperties = adapter.GetIPProperties();
                    foreach (var dns in ipProperties.DnsAddresses)
                    {
                        if (dns.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            return dns.ToString();
                        }
                    }
                }
            }
            catch
            {
                return "Erro ao obter o servidor DNS";
            }

            return "Desconhecido";
        }

        private static string GetMACAddress()
        {
            try
            {
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (var adapter in networkInterfaces)
                {
                    if (adapter.OperationalStatus == OperationalStatus.Up)
                    {
                        return string.Join("-", adapter.GetPhysicalAddress().GetAddressBytes().Select(b => b.ToString("X2")));
                    }
                }
            }
            catch
            {
                return "Erro ao obter o endereço MAC";
            }

            return "Desconhecido";
        }
    }
}