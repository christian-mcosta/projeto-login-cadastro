using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desenvolvimento_teste
{
    public class ServicoLog
    {
        private readonly BancoDados bancoDados;

        public ServicoLog(BancoDados bancoDados)
        {
            this.bancoDados = bancoDados;
        }

        public async Task RegistrarOperacaoAsync(string tipoOperacao)
        {
            // Chama o método de registro de operações na classe BancoDados
            await bancoDados.RegistrarOperacaoAsync(tipoOperacao);
        }

    }
    
}
