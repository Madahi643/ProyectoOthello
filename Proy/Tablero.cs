using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proy
{
    public class Tablero
    {
        List<string> colores;
        List<string> columna;
        List<int> fila;
        string[,] matriz;
        int fiNegras;
        int fiBlancas;
        string fiGano;
        int ganaron;
        int perdieron;
        string fiPerdio;

        public Tablero()
        {
            columna = new List<string>();
            fila = new List<int>();
            colores = new List<string>();
            matriz = new string[9, 9];
            llenarMatriz();
            fiNegras = 0;
            fiBlancas = 0;
            fiGano = "";
            fiPerdio = "";
            ganaron = 0;
            perdieron = 0;

        }



        public List<string> getColumna()
        {
            return this.columna;
        }
        public void setColumna(string letra)
        {
            this.columna.Add(letra);
        }
        public List<int> getFila()
        {
            return this.fila;
        }
        public void setFila(int numero)
        {
            this.fila.Add(numero);
        }
        public List<string> getColores()
        {
            return this.colores;
        }
        public void setColores(string color)
        {
            this.colores.Add(color);
        }
        public string[,] getMatriz()
        {
            return matriz;
        }

        public void setMatriz(int fila, int columna, string colorFicha)
        {
            matriz[fila, columna] = colorFicha;
        }

        public string obtener(int fila, int columna)
        {
            string pal;
            pal = matriz[fila, columna];
            return pal;
        }

        public void llenarMatriz()

        {
            string let = "";
            for (int f = 0; f < this.matriz.GetLength(0); f++)
            {
                for (int c = 0; c < this.matriz.GetLength(1); c++)
                {
                    if (f == 0 & c != 0)
                    {

                        let = abecedario(c);
                        matriz[f, c] = let;

                    }
                    else if (f != 0 & c == 0)
                    {
                        let = numeros(f);
                        matriz[f, c] = let;

                    }

                    else
                    {
                        matriz[f, c] = "nada";
                    }

                }
            }
        }

        public string numeros(int i)
        {
            string letra = "";
            if (i == 1)
            {
                letra = "1";
            }
            else if (i == 2)
            {
                letra = "2";
            }
            else if (i == 3)
            {
                letra = "3";
            }
            else if (i == 4)
            {
                letra = "4";
            }
            else if (i == 5)
            {
                letra = "5";
            }
            else if (i == 6)
            {
                letra = "6";
            }
            else if (i == 7)
            {
                letra = "7";
            }
            else if (i == 8)
            {
                letra = "8";
            }
            return letra;

        }



        public string abecedario(int i)
        {
            string letra = "";
            if (i == 1)
            {
                letra = "A";
            }
            else if (i == 2)
            {
                letra = "B";
            }
            else if (i == 3)
            {
                letra = "C";
            }
            else if (i == 4)
            {
                letra = "D";
            }
            else if (i == 5)
            {
                letra = "E";
            }
            else if (i == 6)
            {
                letra = "F";
            }
            else if (i == 7)
            {
                letra = "G";
            }
            else if (i == 8)
            {
                letra = "H";
            }
            return letra;
        }

        public void tabNuevo()
        {

            for (int f = 0; f < this.matriz.GetLength(0); f++)
            {
                for (int c = 0; c < this.matriz.GetLength(1); c++)
                {

                    if (f == 4 & c == 4)
                    {
                        matriz[f, c] = "blanco";
                    }
                    else if (f == 4 & c == 5)
                    {
                        matriz[f, c] = "negro";
                    }
                    else if (f == 5 & c == 4)
                    {
                        matriz[f, c] = "negro";
                    }
                    else if (f == 5 & c == 5)
                    {
                        matriz[f, c] = "blanco";
                    }
                }
            }
        }

        public void ganador()
        {
            fiNegras = 0;
            fiBlancas = 0;
            int na = 0;
            for (int f = 0; f < this.matriz.GetLength(0); f++)
            {
                for (int c = 0; c < this.matriz.GetLength(1); c++)
                {
                    if (matriz[f, c] == "negro")
                    {

                        fiNegras = fiNegras + 1;
                    }
                    else if (matriz[f, c] == "blanco")
                    {
                        fiBlancas = fiBlancas + 1;
                    }
                    else if (matriz[f, c] == "nada")
                    {
                        na = na + 1;
                    }

                }
            }
            na = na - 1;

            if (fiBlancas > fiNegras)
            {
                fiGano = "blanco";
                fiPerdio = "negro";
                fiBlancas = fiBlancas + na;
                ganaron = fiBlancas;
                perdieron = fiNegras;
            }
            else if (fiBlancas < fiNegras)
            {
                fiGano = "negro";
                fiPerdio = "blanco";
                fiNegras = fiNegras + na;
                ganaron = fiNegras;
                perdieron = fiBlancas;
            }
            else if (fiBlancas == fiNegras)
            {
                fiGano = "Empate";
            }
        }

        public string getFiGano()
        {
            ganador();
            return fiGano;

        }

        public string getFiPerdio ()
        {
            return fiPerdio;

        }

        public int getFiNegras()
        {

            return fiNegras;
        }

        public int getFiBlancas()
        {

            return fiBlancas;
        }
        public int getGanaron()
        {

            return ganaron;
        }

        public int getPerdieron()
        {

            return perdieron;
        }
        public int contarFichas()
        {
            int i = 0;
            int na = 0;
            for (int f = 0; f < matriz.GetLength(0); f++)
            {
                for (int c = 0; c < matriz.GetLength(1); c++)
                {
                    if (matriz[f, c] == "negro")
                    {
                        i = i + 1;
                    }
                    else if (matriz[f, c] == "blanco")
                    {
                        i = i + 1;
                    }
                }
            }

            return i;
        }
    }
}