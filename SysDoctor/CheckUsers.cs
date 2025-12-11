namespace SysDoctor
{
    class CheckUsers
    {
        public static async Task<bool> Executar()
        {
            Console.Write("Digite o email de compra: ");
            var email = Console.ReadLine();

            using (var client = new HttpClient())
            {
                var url = "https://sysdoctor.online/src/api/v1/check_users.php";

                var data = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("email", email)
                });

                try
                {
                    var response = await client.PostAsync(url, data);

                    // Verificar o código HTTP retornado
                    switch ((int)response.StatusCode)
                    {
                        case 200:
                            Console.WriteLine("✔ Email já existe (usuário permitido).");
                            return true; // Acesso permitido

                        case 404:
                            Console.WriteLine("✖ Usuário não permitido: email inexistente.");
                            return false; // Acesso não permitido

                        case 400:
                            Console.WriteLine("⚠ Erro: email é obrigatório.");
                            return false; // Acesso não permitido

                        default:
                            Console.WriteLine($"Resposta inesperada: {(int)response.StatusCode} - {response.ReasonPhrase}");
                            return false; // Acesso não permitido
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao acessar API: " + ex.Message);
                    return false; // Acesso não permitido
                }
            }
        }
    }
}
