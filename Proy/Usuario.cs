using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proy
{
    public class Usuario
    {
        string nombreUsuario;
        string colorFicha;
        int tirosRealizados;

        public Usuario(string nombreUsuario)
        {
            this.nombreUsuario = nombreUsuario;
            this.tirosRealizados = 0;
            this.colorFicha = "";

        }

        public string getNombreUsuario() {

            return nombreUsuario;

        }

        public void aumentarTiro() {
            tirosRealizados = tirosRealizados + 1;
        }

        public int getTiroRealizados()
        {
            return tirosRealizados;
        }

        public void setColorFicha(string color) {

            this.colorFicha = color;

        }

    }

   
}