using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankrupt
{
    class Propriedade
    {

        public int custoVenda;
        public int aluguel;
        public int numJogadorDono;
        public int ordem; //1 a 20
        public Propriedade(int custoVenda, int aluguel, int ordem)
        {
            this.numJogadorDono = 0; //inicialmente nao ha dono
            this.custoVenda = custoVenda;
            this.aluguel = aluguel;
            this.ordem = ordem;
        }

    }

}
