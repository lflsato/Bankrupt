using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankrupt
{
    class Resultado
    {
        public Jogador vencedor;
        public Boolean timeout;
        public int numeroTurnos;

        public Resultado(Jogador vencedor, Boolean timeout, int numeroTurnos)
        {
            this.vencedor = vencedor;
            this.timeout = timeout;
            this.numeroTurnos = numeroTurnos;
        }
    }
}
