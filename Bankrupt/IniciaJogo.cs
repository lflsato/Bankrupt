using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankrupt
{
    class IniciaJogo
    {
        public Resultado resultado;

        void devolverPropriedades(int numJogadorPerdedor, Hashtable ListaPropriedades)
        {
            foreach (int k in ListaPropriedades.Keys)
            {
                Propriedade propriedadeSelecionada = (Propriedade)ListaPropriedades[k];
                if (propriedadeSelecionada.numJogadorDono == numJogadorPerdedor)
                    propriedadeSelecionada.numJogadorDono = 0;
            }
        }

        Jogador retornaVencedor(Hashtable ListaJogadores)
        {
            Jogador jogadorSelecionado = null;
            for (int jogadorIdAux = 1; jogadorIdAux <= 4; jogadorIdAux++)
            {
                jogadorSelecionado = (Jogador)ListaJogadores[jogadorIdAux];
                if (jogadorSelecionado.quebrou == false)
                    return jogadorSelecionado;
            }
            return jogadorSelecionado;
        }

        public IniciaJogo() { 
            Console.WriteLine("Novo jogo inicializado");
            int numeroJogadoresAtivos = 0;
            Boolean gameOver = false;
            Hashtable ListaPropriedades = new Hashtable(); //Uso uma hashtable para agilizar as consultas
            Hashtable ListaJogadores = new Hashtable();
            Random dadoRandomico = new Random();
            int numRodadas = 0;
            Boolean timeout = false;
            int MAXRODADAS = 1000;

            //inicializando jogadores
            Jogador jogador = new Jogador();
            jogador.tipo = "impulsivo";
            jogador.id = 1;
            ListaJogadores.Add(1, jogador);
            numeroJogadoresAtivos++;

            jogador = new Jogador();
            jogador.tipo = "exigente";
            jogador.id = 2;
            ListaJogadores.Add(2, jogador);
            numeroJogadoresAtivos++;

            jogador = new Jogador();
            jogador.tipo = "cauteloso";
            jogador.id = 3;
            ListaJogadores.Add(3, jogador);
            numeroJogadoresAtivos++;

            jogador = new Jogador();
            jogador.tipo = "aleatorio";
            jogador.id = 4;
            ListaJogadores.Add(4, jogador);
            numeroJogadoresAtivos++;

            //leia o arquivo de entrada
            string[] lines = System.IO.File.ReadAllLines(@"..\..\gameConfig.txt");

            int ordemPropriedadesAux = 0;
            foreach (string line in lines)
            {
                string[] bits = line.Split(' ');
                int venda = int.Parse(bits[0]);
                int aluguel;
                try
                {
                    aluguel = int.Parse(bits[1]);
                }
                catch (FormatException e)
                {
                    aluguel = int.Parse(bits[2]);
                }

                //instanciando propriedade e atribuindo variaveis
                Propriedade novaPropriedade = new Propriedade(venda, aluguel, ordemPropriedadesAux);
                ListaPropriedades.Add(ordemPropriedadesAux, novaPropriedade);
                ordemPropriedadesAux++;
            }

            //comeca o jogo!
            //laco para controlar o max de numero de jogadas
            for (int jogadorDaVez=1; numRodadas < MAXRODADAS &&  gameOver == false; numRodadas++, jogadorDaVez++)
            {
                if (jogadorDaVez > 4)
                    jogadorDaVez %= 4;

                //Extrai do jogador da hashtable:
                Jogador jogadorSelecionado = (Jogador)ListaJogadores[jogadorDaVez];
                Console.WriteLine("Inicio da rodada do jogador " + jogadorDaVez + ".");

                if (jogadorSelecionado.quebrou == false)
                { 
                    //jogo o dado de 6 faces
                    int face = dadoRandomico.Next(1, 6);
                    jogadorSelecionado.posicao += face;
                    if (jogadorSelecionado.posicao > 19)
                    {
                        jogadorSelecionado.posicao %= 19;
                        jogadorSelecionado.quantCoins += 100; //deu uma volta no tabuleiro, ganha 100 coins
                    }

                    Propriedade propriedadeSelecionada = (Propriedade)ListaPropriedades[jogadorSelecionado.posicao];

                    //verifica se jogador precisa pagar aluguel
                    if (propriedadeSelecionada.numJogadorDono != 1 && propriedadeSelecionada.numJogadorDono != 0)
                    {
                        if (jogadorSelecionado.quantCoins >= propriedadeSelecionada.aluguel)
                        {
                            //pague o alguel!
                            jogadorSelecionado.quantCoins -= propriedadeSelecionada.aluguel;
                            Console.WriteLine("Jogador " + jogadorSelecionado.id + " pagou o aluguel de "+ propriedadeSelecionada.aluguel +
                                " coins. Restou " + jogadorSelecionado.quantCoins + " coins de sua conta.");

                        }
                        else //jogador perdeu
                        {
                            Console.WriteLine("Jogador " + jogadorSelecionado.id + " perdeu. Suas propriedades serão devolvidas.");
                            numeroJogadoresAtivos--;
                            if (numeroJogadoresAtivos == 1)
                                gameOver = true;
                            jogadorSelecionado.quebrou = true;
                            devolverPropriedades(jogadorSelecionado.id, ListaPropriedades);
                            continue;
                        }
                    }
                    //se jogador nao precisa pagar aluguel, ele tentara comprar a propriedade se for impulsivo
                    else if (jogadorSelecionado.tipo.Equals("impulsivo") &&
                        jogadorSelecionado.quantCoins >= propriedadeSelecionada.custoVenda)
                    {
                        Console.WriteLine("Jogador " + jogadorSelecionado.id + " comprou a propriedade " + propriedadeSelecionada.ordem + ".");
                        jogadorSelecionado.quantCoins -= propriedadeSelecionada.custoVenda;
                        propriedadeSelecionada.numJogadorDono = jogadorSelecionado.id;
                        continue;
                    }
                    else if (jogadorSelecionado.tipo.Equals("exigente") &&
                        jogadorSelecionado.quantCoins >= propriedadeSelecionada.custoVenda
                        && propriedadeSelecionada.aluguel > 50)
                    {
                        Console.WriteLine("Jogador " + jogadorSelecionado.id + " comprou a propriedade " + propriedadeSelecionada.ordem + ".");
                        jogadorSelecionado.quantCoins -= propriedadeSelecionada.custoVenda;
                        propriedadeSelecionada.numJogadorDono = jogadorSelecionado.id;
                        continue;
                    }
                    else if (jogadorSelecionado.tipo.Equals("cauteloso") &&
                        jogadorSelecionado.quantCoins >= propriedadeSelecionada.custoVenda + 80)
                    {
                        Console.WriteLine("Jogador " + jogadorSelecionado.id + " comprou a propriedade " + propriedadeSelecionada.ordem + ".");
                        jogadorSelecionado.quantCoins -= propriedadeSelecionada.custoVenda;
                        propriedadeSelecionada.numJogadorDono = jogadorSelecionado.id;
                        continue;
                    }
                    else if (jogadorSelecionado.tipo.Equals("aleatorio") &&
                    jogadorSelecionado.quantCoins >= propriedadeSelecionada.custoVenda)
                    {
                        face = dadoRandomico.Next(1, 2);
                        if (face == 1)
                        {
                            Console.WriteLine("Jogador " + jogadorSelecionado.id + " comprou a propriedade "+ propriedadeSelecionada.ordem+".");
                            jogadorSelecionado.quantCoins -= propriedadeSelecionada.custoVenda;
                            propriedadeSelecionada.numJogadorDono = jogadorSelecionado.id;
                        }
                        continue;
                    }
                }
            }
            if (numRodadas == MAXRODADAS)
            {
                Console.WriteLine("Chegou no limite de " + MAXRODADAS + " jogadas");
                timeout = true;
            }

            Console.WriteLine("Game over\n");
            this.resultado = new Resultado(retornaVencedor(ListaJogadores), timeout, numRodadas);
        }
    }
}
