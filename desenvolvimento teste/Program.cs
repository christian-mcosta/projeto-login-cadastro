using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace desenvolvimento_teste
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Cria uma instância de BancoDados
            BancoDados bancoDados = new BancoDados("Host=localhost;Username=postgres;Password=1234;Database=DBUsuarios");

            // Cria uma instância de servicoLog (se aplicável)
            ServicoLog servicoLog = new ServicoLog(bancoDados);

            // Cria uma instância de Form1_cadastro passando os objetos BancoDados e ILog como argumentos
            //Application.Run(new Form1_cadastro(bancoDados, servicoLog));
            Form1_Login loginForm = new Form1_Login(bancoDados, servicoLog);
            Application.Run(loginForm);

        }
    }
}
