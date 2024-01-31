using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace desenvolvimento_teste
{
    public class BancoDados
    {
        private readonly string connectionString;

        // Construtor que recebe a string de conexão
        public BancoDados(string connectionString)
        {
            this.connectionString = connectionString;
        }
        // Método para obter uma instância de NpgsqlConnection
        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }

        // Método para obter usuários de forma assíncrona
        public async Task<List<Usuario>> ObterUsuariosAsync()
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                // Inicia uma conexão com o banco de dados PostgreSQL
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    // Abre a conexão de forma assíncrona
                    await connection.OpenAsync();

                    // Consulta SQL para obter todos os usuários
                    string query = "SELECT * FROM Usuario";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        // Lê os resultados da consulta e popula a lista de usuários
                        while (await reader.ReadAsync())
                        {
                            Usuario usuario = new Usuario
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                Senha = reader.GetString(reader.GetOrdinal("Senha"))
                            };

                            usuarios.Add(usuario);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Captura e trata exceções, por exemplo, registrando em log
                Console.WriteLine($"Erro ao obter usuários: {ex.Message}");
            }

            return usuarios;
        }

        // Método para inserir um novo usuário de forma assíncrona
        public async Task<int> InserirUsuarioAsync(Usuario usuario)
        {
            try
            {
                // Inicia uma conexão com o banco de dados PostgreSQL
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    // Abre a conexão de forma assíncrona
                    await connection.OpenAsync();

                    // Consulta SQL para inserir um novo usuário e obter o ID gerado
                    string insertQuery = "INSERT INTO Usuario (Nome, Senha) VALUES (@Nome, @Senha) RETURNING Id";
                    using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, connection))
                    {
                        // Parâmetros para a consulta
                        command.Parameters.AddWithValue("@Nome", usuario.Nome);
                        command.Parameters.AddWithValue("@Senha", usuario.Senha);

                        // Executa a consulta e adiciona o ID gerado em novoId
                        int novoId= (int)await command.ExecuteScalarAsync();

                        // Exibe uma mensagem de sucesso
                        MessageBox.Show("cadastro concluido com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Retorna o ID gerado
                        return novoId;
                    }
                }
            }
            catch (Exception ex)
            {
                // Captura e trata exceções
                Console.WriteLine($"Erro ao inserir usuário: {ex.Message}");
                MessageBox.Show($"Erro ao inserir usuário: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1; // Indica erro para o chamador
            }
        }

        // Método para alterar um usuário de forma assíncrona
        public async Task AlterarUsuarioAsync(Usuario usuario)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Consulta SQL para alterar um usuário
                    string updateQuery = "UPDATE Usuario SET Nome = @Nome, Senha = @Senha WHERE Id = @Id";
                    using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
                    {
                        // Parâmetros para a consulta
                        command.Parameters.AddWithValue("@Id", usuario.Id);
                        command.Parameters.AddWithValue("@Nome", usuario.Nome);
                        command.Parameters.AddWithValue("@Senha", usuario.Senha);

                        // Executa a consulta para alterar o usuário
                        await command.ExecuteNonQueryAsync();

                        // Exibe uma mensagem de sucesso
                        MessageBox.Show("Usuário alterado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao alterar usuário: {ex.Message}");
                // Exibe uma mensagem de erro
                MessageBox.Show($"Erro ao alterar usuário: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para excluir um usuário de forma assíncrona
        public async Task ExcluirUsuarioAsync(int usuarioId)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Consulta SQL para excluir um usuário
                    string deleteQuery = "DELETE FROM Usuario WHERE Id = @Id";
                    using (NpgsqlCommand command = new NpgsqlCommand(deleteQuery, connection))
                    {
                        // Parâmetro para a consulta
                        command.Parameters.AddWithValue("@Id", usuarioId);

                        // Executa a consulta para excluir o usuário
                        await command.ExecuteNonQueryAsync();
                        // Exibe uma mensagem de sucesso
                        MessageBox.Show("Usuário excluido com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir usuário: {ex.Message}");

                // Exibe uma mensagem de erro
                MessageBox.Show($"Erro ao inserir usuário: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Método para registrar uma operação no log de forma assíncrona
        public async Task RegistrarOperacaoAsync(string tipoOperacao)
        {
            try
            {
                using (NpgsqlConnection connection = GetConnection())
                {
                    await connection.OpenAsync();

                    // Implemente a lógica de inserção na tabela logOperacoes
                    string insertLogQuery = "INSERT INTO logOperacoes (TipoOperacao, DataHora) VALUES (@TipoOperacao, current_timestamp)";
                    using (NpgsqlCommand command = new NpgsqlCommand(insertLogQuery, connection))
                    {
                        command.Parameters.AddWithValue("@TipoOperacao", tipoOperacao);
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao registrar operação no log: {ex.Message}");
                throw;
            }
        }
        public async Task<bool> VerificarCredenciaisAsync(string usuario, string senha)
        {
            try
            {
  
                using (NpgsqlConnection connection = GetConnection())
                {
                    await connection.OpenAsync();

                    // lógica de consulta ao banco de dados para verificar as credenciais
                    string query = "SELECT COUNT(*) FROM usuario WHERE nome = @Nome AND senha = @Senha";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nome", usuario);
                        command.Parameters.AddWithValue("@Senha", senha);

                        int count = Convert.ToInt32(await command.ExecuteScalarAsync());

                        // Se o count for maior que 0, as credenciais são válidas
                        if (count > 0)
                        {
                            return true;
                        }
                        else
                        {
                            // Exibe uma mensagem se o usuário não for encontrado
                            Console.WriteLine("Erro ao verificar credenciais: {ex.Message}");
                            return false;
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                // Examine a exceção para determinar a causa específica
                Console.WriteLine($"Erro ao verificar credenciais: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar credenciais: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> VerificarSenhaExistenteAsync(string senha)
        {
            try
            {
                using (NpgsqlConnection connection = GetConnection())
                {
                    await connection.OpenAsync();

                    string query = "SELECT COUNT(*) FROM Usuario WHERE senha = @Senha";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Senha", senha);

                        int count = Convert.ToInt32(await command.ExecuteScalarAsync());

                        // Se o count for maior que 0, a senha já existe
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar senha existente: {ex.Message}");
                throw;
            }
        }
        public async Task<bool> VerificarUsuarioExistenteAsync(string senha)
        {
            try
            {
                using (NpgsqlConnection connection = GetConnection())
                {
                    await connection.OpenAsync();

                    string query = "SELECT COUNT(*) FROM Usuario WHERE nome = @Nome";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nome", senha);

                        int count = Convert.ToInt32(await command.ExecuteScalarAsync());

                        // Se o count for maior que 0, a senha já existe
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar senha existente: {ex.Message}");
                throw;
            }
        }

    }
}
