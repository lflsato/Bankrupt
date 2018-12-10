using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankrupt
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Inicializando o controlador de estatisticas do jogo Bankrupt!");

            int numTimeout = 0;
            int somaTurnos = 0;
            int numPartida;
            float mediaPartidas;
            float porcVitoriaImpulsivo = 0;
            float porcVitoriaExigente = 0;
            float porcVitoriaCauteloso = 0;
            float porcVitoriaAleatorio = 0;
            string maisVence = "impulsivo";

            for (numPartida = 0; numPartida < 300; numPartida++)
            { 
                IniciaJogo jogo = new IniciaJogo();
                if (jogo.resultado.timeout)
                    numTimeout++;
                somaTurnos += jogo.resultado.numeroTurnos;
                if (jogo.resultado.vencedor.tipo.Equals("impulsivo"))
                    porcVitoriaImpulsivo++;
                else if (jogo.resultado.vencedor.tipo.Equals("exigente"))
                    porcVitoriaExigente++;
                else if (jogo.resultado.vencedor.tipo.Equals("cauteloso"))
                    porcVitoriaCauteloso++;
                else if (jogo.resultado.vencedor.tipo.Equals("aleatorio"))
                    porcVitoriaAleatorio++;

            }
            mediaPartidas = somaTurnos/ numPartida;
            porcVitoriaImpulsivo = (porcVitoriaImpulsivo / numPartida) * 100;
            porcVitoriaExigente = (porcVitoriaExigente / numPartida) * 100;
            porcVitoriaCauteloso = (porcVitoriaCauteloso / numPartida) * 100;
            porcVitoriaAleatorio = (porcVitoriaAleatorio / numPartida) * 100;

            //verificando o que mais vence
            if (porcVitoriaExigente > porcVitoriaImpulsivo)
                maisVence = "exigente";
            if (porcVitoriaCauteloso > porcVitoriaExigente)
                maisVence = "cauteloso";
            if (porcVitoriaAleatorio > porcVitoriaCauteloso)
                maisVence = "aleatorio";

            Console.WriteLine("Relatorio de todas as partidas:");
            Console.WriteLine("Partidas por timeout: " + numTimeout);
            Console.WriteLine("Média de turnos por partida: " + mediaPartidas);
            Console.WriteLine("Média de vitoria de impulsivo: " + porcVitoriaImpulsivo + "%");
            Console.WriteLine("Média de vitoria de exigente: " + porcVitoriaExigente + "%");
            Console.WriteLine("Média de vitoria de cauteloso: " + porcVitoriaCauteloso + "%");
            Console.WriteLine("Média de vitoria de aleatorio: " + porcVitoriaAleatorio + "%");
            Console.WriteLine("O que mais vence é: " + maisVence);
            Console.ReadKey();
        }
    }
}
