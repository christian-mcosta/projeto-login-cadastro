using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace desenvolvimento_teste
{
    public partial class Form1_Login : Form
    {
        private readonly BancoDados bancoDados;
        private readonly ServicoLog servicoLog;
        public Form1_Login(BancoDados bancoDados,ServicoLog servicoLog)
        {
            InitializeComponent();
            this.bancoDados = bancoDados;
            this.servicoLog = servicoLog;

            // Associa o evento FormClosing ao método FormLogin_FormClosing
            this.FormClosing += FormLogin_FormClosing;
        }

        private async void bt_login_Click(object sender, EventArgs e)
        {
            string usuario = tb_login_nome.Text;
            string senha = tb_login_senha.Text;
            
                // Chama o método de verificação de credenciais na classe BancoDados
                bool credenciaisValidas = await bancoDados.VerificarCredenciaisAsync(usuario, senha);
                //this.Hide();
            // retorna true ou false
            if (credenciaisValidas)
                {
                    MessageBox.Show("Login bem-sucedido!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Abre a tela de cadastro
                    Form1_cadastro formCadastro = new Form1_cadastro(bancoDados, servicoLog);
                    formCadastro.Show();
                    this.Hide();
                }   
            
            else
            {
                MessageBox.Show("Usuário ou senha inválidos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.Show(); // Mostrar novamente a tela de login
            }

        }

        private void bt_cadastro_Click(object sender, EventArgs e)
        {
            // Abrir a tela de cadastro
            Form1_cadastro formCadastro = new Form1_cadastro(bancoDados, servicoLog);
            formCadastro.Show();

            this.Hide();



        }
        private void Form1_Login_Load(object sender, EventArgs e)
        {
        }
        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Certifique-se de verificar se o motivo do fechamento é o usuário clicando no botão fechar
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Encerra completamente a aplicação
                Application.Exit();
            }
        }
    }
}
