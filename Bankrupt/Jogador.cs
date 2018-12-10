using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankrupt
{
    class Jogador
    {
        public string tipo;
        public int quantCoins;
        public int posicao;
        public Boolean quebrou;
        public int id;

        public Jogador()
        {
            this.quantCoins = 300;
            this.posicao = 0;
            this.quebrou = false;
        }

    }
}
