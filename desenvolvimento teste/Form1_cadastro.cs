using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace desenvolvimento_teste
{

    public partial class Form1_cadastro : Form
    {

        private readonly BancoDados bancoDados;
        private readonly ServicoLog servicoLog;

        public Form1_cadastro(BancoDados bancoDados, ServicoLog servicoLog)
        {
            InitializeComponent();
            this.bancoDados = bancoDados;
            this.servicoLog = servicoLog;
            // Adicione o evento CellClick e SelectionChanged ao DataGridView
            dataGridViewUsuarios.CellClick += dataGridViewUsuarios_CellClick;
            dataGridViewUsuarios.SelectionChanged += DataGridViewUsuarios_SelectionChanged;
            // Associa o evento KeyPress ao método tb_senha_KeyPress
            tb_senha.KeyPress += tb_senha_KeyPress;
            // Associa o evento FormClosing ao método Form1_cadastro_FormClosing
            this.FormClosing += Form1_cadastro_FormClosing;
        } 
        private async void Form1_cadastro_Load(object sender, EventArgs e)
        {
            try
            {
                // Tenta carregar os dados dos usuários ao carregar o formulário
                await CarregarDadosAsync();
            }
            catch (Exception ex)
            {
                // Captura exceções e trata-as, por exemplo, exibindo uma mensagem de erro
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ControlBox.ToString();
        }
        // Método para carregar dados dos usuários de forma assíncrona
        private async Task CarregarDadosAsync()
        {
            try
            {
                // Tenta obter os usuários do banco de dados e exibir no DataGridView
                List<Usuario> usuarios = await bancoDados.ObterUsuariosAsync();
                dataGridViewUsuarios.DataSource = usuarios;
            }
            catch (Exception ex)
            {
                // Captura exceções e trata-as, por exemplo, exibindo uma mensagem de erro
                MessageBox.Show($"Erro ao obter usuários: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void bt_salvar_Click(object sender, EventArgs e)
        {
            try
            {
                // Tenta salvar um novo usuário ao clicar no botão Salvar
                string nome = tb_nome.Text;
                string senha = tb_senha.Text;
               
                if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(senha))
                {
                    // Exibe uma mensagem se campos obrigatórios não estiverem preenchidos
                    MessageBox.Show("Todos os campos são obrigatórios.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //verifiva se o usuario colocou a senha maior que zero
                if (senha.Length == 0)
                {
                    MessageBox.Show("A senha deve ter mais de 1 caractere.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Verifica se o usuario já existe no banco de dados
                bool usuarioExistente = await bancoDados.VerificarUsuarioExistenteAsync(senha);
                if (usuarioExistente)
                {
                    MessageBox.Show("Esse usuario já foi utilizado. Por favor, escolha outro nome.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Verifica se a senha já existe no banco de dados
                bool senhaExistente = await bancoDados.VerificarSenhaExistenteAsync(senha);
                if (senhaExistente)
                {
                    MessageBox.Show("Essa senha já foi utilizada. Por favor, escolha outra senha.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Verifica se a senha contém apenas números inteiros
                if (!int.TryParse(senha, out _))
                {
                    MessageBox.Show("A senha deve conter apenas números inteiros.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Cria um novo usuário
                Usuario novoUsuario = new Usuario { Nome = nome, Senha = senha };

                //verifica se a conexão do bancodados não esta retornando vazio
                if (bancoDados != null)
                {
                   // Tenta inserir o usuário no banco de dados
                    int id = await bancoDados.InserirUsuarioAsync(novoUsuario);
                    
                        // Limpa os campos e recarrega os dados
                        LimparCampos();
                    await CarregarDadosAsync();
                }
                else
                {
                    MessageBox.Show("A instância de BancoDados não foi inicializada corretamente.");
                }
                //verifica se a conexão do bancodados não esta retornando vazio
                if (servicoLog != null)
                {
                    //await iLog.RegistrarOperacaoAsync("insert");
                    await servicoLog.RegistrarOperacaoAsync("Insert");

                }
                else
                {
                    MessageBox.Show("A instância de BancoDados não foi inicializada corretamente.");
                }
            }

            catch (Exception ex)
            {
                // Captura exceções e trata-as, por exemplo, exibindo uma mensagem de erro
                MessageBox.Show($"Erro ao salvar usuário: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    
    private void LimparCampos()
    {
        tb_id.Text = string.Empty;
        tb_nome.Text = string.Empty;
        tb_senha.Text = string.Empty;
    }

    private async void bt_alterar_Click(object sender, EventArgs e)
    {
        try
        {
            // Obtém os dados dos campos do formulário
            int id = Convert.ToInt32(tb_id.Text);
            string nome = tb_nome.Text;
            string senha = tb_senha.Text;

            if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(senha))
            {
                // Exibe uma mensagem se campos obrigatórios não estiverem preenchidos
                MessageBox.Show("Todos os campos são obrigatórios.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Cria um novo usuário com os dados atualizados
            Usuario usuarioAtualizado = new Usuario { Id = id, Nome = nome, Senha = senha };

            // Tenta alterar o usuário no banco de dados
            await bancoDados.AlterarUsuarioAsync(usuarioAtualizado);

            // Tenta registrar a operação de alteração no log
            await servicoLog.RegistrarOperacaoAsync("Update");

            // Limpa os campos e recarrega os dados
            LimparCampos();
            await CarregarDadosAsync();
        }
        catch (Exception ex)
        {
            // Captura exceções e trata-as, por exemplo, exibindo uma mensagem de erro
            Console.WriteLine($"Erro ao alterar usuário: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            MessageBox.Show("Todos os campos são obrigatórios.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
    }

    private async void bt_excluir_Click(object sender, EventArgs e)
    {
        try
        {
            // Obtém o ID do usuário a ser excluído
            int id = Convert.ToInt32(tb_id.Text);

            // Tenta excluir o usuário do banco de dados
            await bancoDados.ExcluirUsuarioAsync(id);

            // Tenta registrar a operação de exclusão no log
            await servicoLog.RegistrarOperacaoAsync("Delete");

            // Limpa os campos e recarrega os dados
            LimparCampos();
            await CarregarDadosAsync();
        }
        catch (Exception ex)
        {
            // Captura exceções e trata-as, por exemplo, exibindo uma mensagem de erro
            Console.WriteLine($"Erro ao excluir usuário: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            MessageBox.Show("Todos os campos são obrigatórios.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
    
        
        }
        private void Form1_cadastro_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Exibe novamente a tela de login quando a tela de cadastro é fechada
            Form1_Login formLogin = new Form1_Login(bancoDados, servicoLog);
            formLogin.Show();
        }
        private void DataGridViewUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            // Verifique se há pelo menos uma linha selecionada
            if (dataGridViewUsuarios.SelectedRows.Count > 0)
            {
                // Obtém o valor da coluna 'Id','Nome', 'senha' da linha selecionada
                string idSelecionado = dataGridViewUsuarios.SelectedRows[0].Cells["Id"].Value.ToString();
                string nomeSelecionado = dataGridViewUsuarios.SelectedRows[0].Cells["Nome"].Value.ToString();
                string senhaSelecionado = dataGridViewUsuarios.SelectedRows[0].Cells["Senha"].Value.ToString();
                // Preenche o campo 'tb_id,tb_nome,tb_senha' com o valor selecionado
                tb_id.Text = idSelecionado;
                tb_nome.Text = nomeSelecionado;
                tb_senha.Text = senhaSelecionado;
            }
            if (dataGridViewUsuarios.SelectedRows.Count == 0)
            {
                // Habilita o botão salvar quando não há linhas selecionadas
                bt_salvar.Enabled = true;

            }
        }
        private void dataGridViewUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica se uma célula (não a cabeçalho) foi clicada
            if (e.RowIndex >= 0)
            {
                // Desativa o botão salvar quando o usuário seleciona uma linha
                bt_salvar.Enabled = false;
            }
        }
        private void tb_senha_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite apenas números inteiros e teclas de controle (Backspace, Delete)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Ignora a tecla pressionada
            }
        }
    }
}  

