//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Proy.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class partidas
    {
        public decimal idPartida { get; set; }
        public string tipoPartia { get; set; }
        public decimal movRealizados { get; set; }
        public string estadoPartida { get; set; }
        public string nombreUsuario { get; set; }
    
        public virtual usuarios usuarios { get; set; }
    }
}
