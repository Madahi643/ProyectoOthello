using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Proy.Models;


namespace Proy.Controllers
{
    public class proyecOthelloController : Controller
    {
        static int fichaGano;
        static int fichaPerdio;
        static BaseDatos baseD;
        static string empate;
        static int totalFichas;
        static string ganador;
        static string perdedor;
        static Tablero tab;
        static Usuario jugador1;
        static Usuario jugador2;
        static string ficha1;
        static string ficha2;
        //static Tablero tabler;
        static bool tabNuev;
        static int turno;
        // GET: proyecOthello
        public ActionResult vista1()
        {

            return View();
        }



        [HttpPost]
        public ActionResult vista1(string nombreUsuario, string contraseña)
        {
            // string consultar = bd.selectstring("select * from usuarios where nombre_usuario '"+nombreUsuario +"'");
            // var user = bd.usuarios.FirstOrDefault(n => n.nombre_usuario == nombreUsuario && n.contraseña == contraseña);
            string str = Request.Params["btn1"];
            if (str == "ingresar")
            {
                if (nombreUsuario != "" && contraseña != "")
                {
                    BaseDatos bd = new BaseDatos();
                    baseD = bd;
                    var usuario = bd.usuarios.FirstOrDefault(e => e.nombre_usuario == nombreUsuario && e.contraseña == contraseña);
                    if (usuario != null)
                    {
                        Usuario usu1 = new Usuario(nombreUsuario);
                        jugador1 = usu1;
                        ViewBag.NombreUsuario = nombreUsuario;
                        ViewBag.Contraseña = contraseña;
                        return RedirectToAction("inicio", "proyecOthello");
                    }
                    else
                    {
                        ViewBag.NombreUsuario = "Tu usuario no existe puedes registrarte";
                        return View();
                    }
                }
                else
                {

                    ViewBag.NombreUsuario = "Llene los campos";
                    return View();
                }

            }
            else
            {
                ViewBag.NombreUsuario = "ya te voy a registrar";
                return RedirectToAction("registrar", "proyecOthello");
            }

        }


        public ActionResult registrar(string proceso = "")
        {
            if (proceso == "Registrado")
            {
                return RedirectToAction("vista1", "proyecOthello");
            }
            else {
                ViewBag.proceso = proceso;
                return View();
            }
        }

        [HttpPost]
        public ActionResult registro(string nombreUsuario, string nombreJugador, string apellidoJugador, string dia, string mes, string año, string pais, string correo, string contraseña)
        {
            string proceso = "";
            string str = Request.Params["btn1"];
            if (str == "registrar")
            {
                if (nombreUsuario != "" & nombreJugador != "" & apellidoJugador != "" & dia != "" & mes != "" & año != "" & pais != "" & correo != "" & contraseña != "")
                {
                    BaseDatos bd = new BaseDatos();
                    baseD = bd;
                    var usuario = bd.usuarios.FirstOrDefault(e => e.nombre_usuario == nombreUsuario);
                    if (usuario == null)
                    {
                        DateTime fecha = new DateTime(int.Parse(año), int.Parse(mes), int.Parse(dia));
                        usuarios usu = new usuarios();
                        usu.nombre_usuario = nombreUsuario;
                        usu.nombres_usuarios = nombreJugador;
                        usu.apellidos_usuario = apellidoJugador;
                        usu.contraseña = contraseña;
                        usu.correo_electronico = correo;
                        usu.fecha_nacimiento = fecha;
                        usu.pais = pais;
                        bd.usuarios.Add(usu);
                        bd.SaveChanges();
                        proceso = "Registrado";
                    }
                    else
                    {
                        proceso = "El nombre de usuario ya existe";

                    }

                    return RedirectToAction("registrar", new { proceso });
                }
                else
                {
                    proceso = "LLene todos los campos";
                    return RedirectToAction("registrar", new { proceso });
                }
            }
            else {

                return RedirectToAction("registrar", "proyecOthello");
            }
        }




        public ActionResult inicio()
        {

            return View();
        }


        [HttpPost]
        public ActionResult inicioU()
        {
            string str = Request.Params["btn1"];
            if (str == "Jugar")
            {
                return RedirectToAction("opciones", "proyecOthello");
            }
            else if (str == "Salir")
            {
                return RedirectToAction("vista1", "proyecOthello");
            }
            else
            {
                return View();
            }
        }
        public ActionResult opciones()
        {
            return View();
        }

        [HttpPost]
        public ActionResult opciones1()
        {
            string str = Request.Params["btn1"];
            if (str == "EMPEZAR PARTIDA")
            {
                return RedirectToAction("opcionesJuego", "proyecOthello");
            }
            else
            {
                return RedirectToAction("vista1", "proyecOthello");
            }
        }

        public ActionResult opcionesJuego()
        {

            return View();

        }

        [HttpPost]
        public ActionResult entradaFichas()
        {
            string str = Request.Params["btn1"];
            if (str == "PARTIDA CONTRA JUGADOR")
            {
                return RedirectToAction("otroJugador", "proyecOthello");
            }
            else if (str == "PARTIDA CONTRA MAQUINA")
            {
                return RedirectToAction("leerXml", "proyecOthello");
            }
            else
            {
                return RedirectToAction("vista1", "proyecOthello");
            }
        }

        public ActionResult otroJugador()
        {

            return View();

        }
        [HttpPost]
        public ActionResult nuevoJug(string nombreJugador)
        {

            string str = Request.Params["btn1"];
            if (nombreJugador != "")
            {
                if (str == "listo")
                {
                    Usuario usu2 = new Usuario(nombreJugador);
                    jugador2 = usu2;
                    return RedirectToAction("leerXml", "proyecOthello");
                }
                else
                {
                    return RedirectToAction("vista1", "proyecOthello");
                }
            }
            else
            {
                return View();
            }
        }


        //CON XML O SIN XML
        public ActionResult leerXml()
        {
            return View();

        }
        [HttpPost]
        public ActionResult leerXml(string nombreArchivo)
        {
            string ruta = "";
            bool existeArchivo = false;
            string str = Request.Params["btn1"];
            if (str == "OK")
            {
                if (nombreArchivo != "")
                {
                    existeArchivo = existe(nombreArchivo);
                    if (existeArchivo == true)
                    {
                        tabNuev = true;
                        //string rutaSitio = Server.MapPath("~/");
                        //string pathArchivo = Path.Combine(rutaSitio + nombreArchivo);
                        return RedirectToAction("escogerTipoFicha");
                    }
                    else
                    {
                        ViewBag.Error = "Error, No existe el archivo XML";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Error = "Error, llene el campo para el XML";
                    return View();
                }
            }
            else if (str == "EMPEZAR PARTIDA SIN XML")
            {
                tabNuev = false;
                Tablero ta = new Tablero();
                tab = ta;
                return RedirectToAction("escogerTipoFicha");
            }
            else
            {
                //aqui va la direccion para cuando vaya sin el xml
                return RedirectToAction("vista1", "proyecOthello");
            }
        }

        public ActionResult escogerTipoFicha()
        {
            return View();
        }

        [HttpPost]
        public ActionResult fichasEscoger()
        {
            string str = Request.Params["btn1"];
            if (str == "FICHAS ALEATORIAMENTE")
            {
                if (tabNuev == false)
                {
                    turno = 0;
                    pintarTableroNuevo();
                    colorFicha();
                    return RedirectToAction("pintarTablero");
                }
                else
                {
                    turno = 0;
                    colorFicha();
                    return RedirectToAction("pintarTablero");

                }
            }
            else if (str == "FICHAS MANUALMENTE")
            {
                return RedirectToAction("fichasManual", "proyecOthello");
            }
            else
            {
                return RedirectToAction("inicio", "proyecOthello");
            }

        }

        public ActionResult fichasManual()
        {
            ViewBag.NomJug1 = jugador1.getNombreUsuario();
            return View();

        }
        [HttpPost]
        public ActionResult fichasManualResp()
        {
            if (tabNuev == false)
            {
                pintarTableroNuevo();
            }

            string str = Request.Params["btn1"];
            if (str == "FICHA BLANCA")
            {

                ficha1 = "blanco";
                ficha2 = "negro";
                jugador1.setColorFicha(ficha1);
                jugador2.setColorFicha(ficha2);
                return RedirectToAction("pintarTablero");
            }
            else
            {
                ficha1 = "negro";
                ficha2 = "blanco";
                jugador1.setColorFicha(ficha1);
                jugador2.setColorFicha(ficha2);
                return RedirectToAction("pintarTablero");
            }
        }


        public bool existe(string rut)
        {
            bool existe = false;
            string rutaSitio = Server.MapPath("~/");
            string pathArchivo = Path.Combine(rutaSitio + "XML" + "\\"+ rut);
            if (System.IO.File.Exists(pathArchivo))
            {
                existe = true;
                leerArchivo(pathArchivo);
            }
            else
            {
                existe = false;
            }
            return existe;
        }


        public void leerArchivo(string ruta)
        {
            bool cumpleComlumna = true;
            bool cumpleFila = true;
            int i = 0;
            int columnaNuevo = 0;
            string colorNuevo = "";
            int filaNuevo = 0;
            XmlReader reader = XmlReader.Create(ruta);
            Tablero ta = new Tablero();
            tab = ta;
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name.ToString())
                    {
                        case "color":
                            colorNuevo = reader.ReadString();
                            tab.setColores(colorNuevo);
                            i = i + 1;
                            break;
                        case "columna":
                            string a = reader.ReadString();
                            tab.setColumna(a);
                            columnaNuevo = convertirLetra(a);
                            if (columnaNuevo == 10)
                            {
                                cumpleComlumna = false;
                            }
                            else
                            {
                                cumpleComlumna = true;
                            }
                            i = i + 1;
                            break;
                        case "fila":
                            int b = int.Parse(reader.ReadString());
                            tab.setFila(b);
                            filaNuevo = b;
                            if (b <= 0 | b >= 9)
                            {
                                cumpleFila = false;
                            }
                            else
                            {
                                cumpleFila = true;
                            }
                            i = i + 1;
                            break;
                    }
                    if (i == 3)
                    {
                        if (cumpleComlumna == true & cumpleFila == true)
                        {
                            i = 0;
                            tab.setMatriz(filaNuevo, columnaNuevo, colorNuevo);
                        }
                        else
                        {
                            i = 0;
                            cumpleComlumna = true;
                            cumpleFila = true;
                        }
                    }


                }
            }
        }

        public void colorFicha()
        {
            Random aleatorio = new Random();
            int numero;
            numero = aleatorio.Next(0, 2);
            if (numero == 0)
            {
                ficha1 = "negro";
                ficha2 = "blanco";
                jugador1.setColorFicha(ficha1);
                jugador2.setColorFicha(ficha2);
            }
            else
            {
                ficha1 = "blanco";
                ficha2 = "negro";
                jugador1.setColorFicha(ficha1);
                jugador2.setColorFicha(ficha2);
            }
        }


        public ActionResult pintarTableroNuevo()
        {
            Tablero ta = new Tablero();
            tab = ta;
            tab.tabNuevo();
            return RedirectToAction("pintarTablero", "proyecOthello");
        }



        public ActionResult pintarTablero(string mensaje = "")
        {
            int i = turno;
            ViewBag.Error = mensaje;
            if (turno == 0)
            {
                ViewBag.turno = jugador1.getNombreUsuario() + "____Color Ficha:" + ficha1;
                turno = 2;
                ViewBag.Usuario1 = jugador1.getNombreUsuario() + "_____ Color Ficha:" + ficha1;
                ViewBag.Usuario2 = jugador2.getNombreUsuario() + "_____Color Ficha:" + ficha2;
                ViewBag.contador1 = jugador1.getTiroRealizados();
                ViewBag.contador2 = jugador2.getTiroRealizados();
                ViewBag.Tablero = tab;
                return View();
            }
            else if (turno == 1) {
                ViewBag.turno = jugador1.getNombreUsuario() + "____Color Ficha:" + ficha1;
                turno = 2;
                ViewBag.Usuario1 = jugador1.getNombreUsuario() + "____ Color Ficha:" + ficha1;
                ViewBag.Usuario2 = jugador2.getNombreUsuario() + "____Color Ficha:" + ficha2;
                ViewBag.Tablero = tab;
                ViewBag.contador1 = jugador1.getTiroRealizados();
                ViewBag.contador2 = jugador2.getTiroRealizados();
                return View();
            }
            else
            {
                ViewBag.turno = jugador2.getNombreUsuario() + "____Color Ficha:" + ficha2;
                turno = 1;
                ViewBag.Usuario1 = jugador1.getNombreUsuario() + "____Color Ficha:" + ficha1;
                ViewBag.Usuario2 = jugador2.getNombreUsuario() + "____Color Ficha:" + ficha2;
                ViewBag.Tablero = tab;
                ViewBag.contador1 = jugador1.getTiroRealizados();
                ViewBag.contador2 = jugador2.getTiroRealizados();
                return View();
            }

        }

        [HttpPost]
        public ActionResult pareja(string fil, string col)
        {
            
            string str = Request.Params["btn1"];
            if (str == "Tirar" )
            {
                bool error = false;
                error = validar(fil, col);
                
                if (error == true)
                {
                    return RedirectToAction("pintarTablero", new { mensaje = "La Posicion que desea es invalida" });
                }
                else
                {
                    int r = tab.contarFichas();
                    if (r <= 63)
                    {
                        return RedirectToAction("pintarTablero", "proyecOthello");
                    }
                    else {

                        return RedirectToAction("finPartida", "proyecOthello");

                    }

                    
                }
            }
            else if (str == "Generar XML") {

                return RedirectToAction("genXml", "proyecOthello");
            }
            else {

                return RedirectToAction("finPartida", "proyecOthello");
            }
        }

        public ActionResult genXml()
        {
            generarArchivo(); 
            return View();

        }

        public void generarArchivo()
        {
            string rutaSitio = Server.MapPath("~/");
            string pathArchivo = Path.Combine(rutaSitio + "XML" + "\\" + jugador1.getNombreUsuario() + "_" + jugador2.getNombreUsuario()+".xml");
            string[,] matriz = tab.getMatriz();

            XmlDocument document = new XmlDocument();


            XmlElement tablero = document.CreateElement(string.Empty, "tablero", string.Empty);
            document.AppendChild(tablero);

            for (int f = 0; f < matriz.GetLength(0); f++)
            {
                for (int c = 0; c < matriz.GetLength(1); c++)
                {
                    if (matriz[f, c] == "negro")
                    {                                   
                        string letra = convertirNum(c);
                        XmlElement ficha = document.CreateElement(string.Empty, "ficha", string.Empty);
                        tablero.AppendChild(ficha);

                        XmlElement color = document.CreateElement(string.Empty, "color", string.Empty);
                        XmlText textColor = document.CreateTextNode("negro");
                        color.AppendChild(textColor);
                        ficha.AppendChild(color);

                        XmlElement columna = document.CreateElement(string.Empty, "columna", string.Empty);
                        XmlText textColumna = document.CreateTextNode(letra);
                        columna.AppendChild(textColumna);
                        ficha.AppendChild(columna);

                        XmlElement fila = document.CreateElement(string.Empty, "fila", string.Empty);
                        XmlText textFila = document.CreateTextNode(f.ToString());
                        fila.AppendChild(textFila);
                        ficha.AppendChild(fila);
                    }
                    else if (matriz[f, c] == "blanco") {
                        string letra = convertirNum(c);
                        XmlElement ficha = document.CreateElement(string.Empty, "ficha", string.Empty);
                        tablero.AppendChild(ficha);

                        XmlElement color = document.CreateElement(string.Empty, "color", string.Empty);
                        XmlText textColor = document.CreateTextNode("blanco");
                        color.AppendChild(textColor);
                        ficha.AppendChild(color);

                        XmlElement columna = document.CreateElement(string.Empty, "columna", string.Empty);
                        XmlText textColumna = document.CreateTextNode(letra);
                        columna.AppendChild(textColumna);
                        ficha.AppendChild(columna);

                        XmlElement fila = document.CreateElement(string.Empty, "fila", string.Empty);
                        XmlText textFila = document.CreateTextNode(f.ToString());
                        fila.AppendChild(textFila);
                        ficha.AppendChild(fila);

                    }

                }
            }

            document.Save(pathArchivo);
          

        }
        public string convertirNum(int numero)
        {

            string colum = "";
            if (numero == 1)
            {
                colum = "A";
            }
            else if (numero==2)
            {
                colum = "B";
            }
            else if (numero==3)
            {
                colum = "C";
            }
            else if (numero==4)
            {
                colum = "D";
            }
            else if (numero==5)
            {
                colum ="E";
            }
            else if (numero==6)
            {
                colum = "F";
            }
            else if (numero==7)
            {
                colum = "G";
            }
            else if (numero==8)
            {
                colum = "H";
            }
            else
            {
                colum ="Z";
            }
            return colum;
        }

    
        public ActionResult finPartida(string mensaje = "")
        {
            tab.ganador();
            ContarGanador();
            ViewBag.Usuario1 = jugador1.getNombreUsuario() + "_____ Color Ficha:" + ficha1;
            ViewBag.Usuario2 = jugador2.getNombreUsuario() + "_____Color Ficha:" + ficha2;
            ViewBag.gano = "Ganador: " + ganador + "___Numero de fichas: " + fichaGano;
            ViewBag.perdio ="Perdio: "+ perdedor + "___Numero de fichas: " + fichaPerdio;
            ViewBag.empato =empate;
            ViewBag.cualquieraF = tab.getFiBlancas();
            ViewBag.tablero = tab;
            return View(); 

        }

        [HttpPost]
        public ActionResult ultimasOpc(string mensaje = "")
        {
            string str = Request.Params["btn1"];
            if (str == "IR A INICIO")
            {
                return RedirectToAction("inicio", "proyecOthello");
            }
            else {
                limpiar();
                return RedirectToAction("vista1", "proyecOthello");
            }

            }


        public void limpiar() {
           fichaGano=0;
           fichaPerdio =0;
           baseD= null;
           empate="";
           totalFichas=0;
           ganador="";
           perdedor="";
           tab=null;
           jugador1=null;
           jugador2=null;
           ficha1=null;
           ficha2=null;
           int turno = 0;
            //static bool tabNuev;
        }

            public void ContarGanador()
           {
            string gano;
            int negr = 0;
            int blan = 0;

            gano = tab.getFiGano();
            negr = tab.getFiNegras();
            blan = tab.getFiBlancas();
            if (gano=="negro" & ficha1=="negro")
            {
                ganador = jugador1.getNombreUsuario();
                perdedor = jugador2.getNombreUsuario();
                fichaGano = tab.getFiNegras();
                fichaPerdio = tab.getFiBlancas();

            }
            else if (gano=="negro" & ficha2=="negro")
            {
                ganador = jugador2.getNombreUsuario();
                perdedor = jugador1.getNombreUsuario();
                fichaGano = tab.getFiNegras();
                fichaPerdio = tab.getFiBlancas();
            }
            else if (gano=="blanco" & ficha1=="blanco")
            {
                ganador = jugador1.getNombreUsuario();
                perdedor = jugador2.getNombreUsuario();
                fichaGano = tab.getFiBlancas();
                fichaPerdio = tab.getFiNegras();
            }
            else if (gano=="blanco" & ficha2=="blanco")
            {
                ganador = jugador2.getNombreUsuario();
                perdedor = jugador1.getNombreUsuario();
                fichaGano = tab.getFiBlancas();
                fichaPerdio = tab.getFiNegras();
            }
            else if (gano.Equals("Empate")) {
                empate = "SE EMPATO LA PARTIDA";
            }
            

        }

        public bool validar(string fila, string columna)
        {
           
                bool error = false;
                bool pasa = false;
                int colu;
                colu = convertirLetra(columna);
                int fil = int.Parse(fila);
                string l;
                string cf = "";
          
            if (colu <= 8 & fil > 0 & fil < 9)
            {
                l = tab.obtener(fil, colu);
                if (l.Equals("nada"))
                {
                    if (turno == 2)
                    {
                        jugador1.aumentarTiro();
                        cf = ficha1;
                        tab.setMatriz(fil, colu, ficha1);
                        totalFichas = totalFichas + 1;
                    }
                    else
                    {
                        jugador2.aumentarTiro();
                        cf = ficha2;
                        tab.setMatriz(fil, colu, ficha2);
                        totalFichas = totalFichas + 1;
                    }

                    if (colu >= 3)
                    {
                        verificaIzquierda(fil, colu, cf);
                    }
                    if (colu <= 6) {
                        verificaDerecha(fil, colu, cf);
                    }
                    if (fil >= 3) {
                        verificaArriba(fil, colu, cf);
                    }
                    if (fil <= 6) {
                        verificaAbajo(fil, colu, cf);
                    }
                    if (colu <= 6 & fil >= 3) {

                        verificaDiag1(fil, colu, cf);
                    }
                    if (colu <= 6 & fil <= 6) {
                        verificaDiag2(fil, colu, cf);
                    }
                    if (colu>=3 & fil>=3) {
                        verificaDiag3(fil, colu, cf);
                    }
                    if (colu>=3 & fil<=6) {
                        verificaDiag4(fil, colu, cf);
                    }

                }
                else
                {
                    error = true;
                    pasa = true;
                }
            }
            else
            {
                error = true;
                pasa = true;
            }
            
            return error;
        }


        public void verificaIzquierda(int fila, int columna, string cf)
        {
            bool nada = false;
            bool otraFicha = false;
            bool encontre = false;
            List<int> fi = new List<int>();
            int i = 0;
            int a = columna;

            while (nada == false & otraFicha == false)
            {
                a = a - 1;
                if (a > 0)
                {
                    if (tab.obtener(fila, a).Equals(cf))
                    {
                        if (i == 0)
                        {
                            nada = true;
                        }
                        else
                        {
                            otraFicha = true;
                        }
                    }
                    else if (tab.obtener(fila, a).Equals("nada"))
                    {
                        nada = true;
                    }
                    else
                    {
                        fi.Add(a);
                    }
                    i = i + 1;
                }
                else
                {
                    nada = true;
                }
            }
            if (nada == false & otraFicha == true)
            {
                foreach (int fil in fi) {
                    if (cf.Equals("negro"))
                    {
                        tab.setMatriz(fila, fil, "negro");
                    }
                    else {
                        tab.setMatriz(fila, fil, "blanco");
                    }

                }
            }
        }

        public void verificaDerecha(int fila, int columna, string cf)
        {
            bool nada = false;
            bool otraFicha = false;
            bool encontre = false;
            List<int> fi = new List<int>();
            int i = 0;
            int a = columna;


            while (nada == false & otraFicha == false)
            {
                a = a + 1;
                if (a <= 8)
                {
                    if (tab.obtener(fila, a).Equals(cf))
                    {
                        if (i == 0)
                        {
                            nada = true;
                        }
                        else
                        {
                            otraFicha = true;
                        }
                    }
                    else if (tab.obtener(fila, a).Equals("nada"))
                    {
                        nada = true;
                    }
                    else
                    {
                        fi.Add(a);
                    }
                    i = i + 1;
                }
                else
                {
                    nada = true;
                }
            }

            if (nada == false & otraFicha == true)
            {
                foreach (int fil in fi)
                {
                    if (cf.Equals("negro"))
                    {
                        tab.setMatriz(fila, fil, "negro");
                    }
                    else
                    {
                        tab.setMatriz(fila, fil, "blanco");
                    }

                }
            }

        }

        public void verificaArriba(int fila, int columna, string cf)
        {
            bool nada = false;
            bool otraFicha = false;
            bool encontre = false;
            List<int> fi = new List<int>();
            int i = 0;
            int a = fila;


            while (nada == false & otraFicha == false)
            {
                a = a - 1;
                if (a > 0)
                {
                    if (tab.obtener(a, columna).Equals(cf))
                    {
                        if (i == 0)
                        {
                            nada = true;
                        }
                        else
                        {
                            otraFicha = true;
                        }
                    }
                    else if (tab.obtener(a, columna).Equals("nada"))
                    {
                        nada = true;
                    }
                    else
                    {
                        fi.Add(a);
                    }
                    i = i + 1;
                }
                else
                {
                    nada = true;
                }

            }

            if (nada == false & otraFicha == true)
            {
                foreach (int fil in fi)
                {
                    if (cf.Equals("negro"))
                    {
                        tab.setMatriz(fil, columna, "negro");
                    }
                    else
                    {
                        tab.setMatriz(fil, columna, "blanco");
                    }

                }
            }

        }


    
        public void verificaAbajo(int fila, int columna, string cf)
        {
            bool nada = false;
            bool otraFicha = false;
            bool encontre = false;
            List<int> fi = new List<int>();
            int i = 0;
            int a = fila;


            while (nada == false & otraFicha == false)
            {
                a = a + 1;
                if (a <= 8)
                {
                    if (tab.obtener(a, columna).Equals(cf))
                    {
                        if (i == 0)
                        {
                            nada = true;
                        }
                        else
                        {
                            otraFicha = true;
                        }
                    }
                    else if (tab.obtener(a, columna).Equals("nada"))
                    {
                        nada = true;
                    }
                    else
                    {
                        fi.Add(a);
                    }
                    i = i + 1;
                }
                else
                {
                    nada = true;
                }
            }
                if (nada == false & otraFicha == true)
                    {
                        foreach (int fil in fi)
                        {
                            if (cf.Equals("negro"))
                            {
                                tab.setMatriz(fil, columna, "negro");
                            }
                            else
                            {
                                tab.setMatriz(fil, columna, "blanco");
                            }

                        }
                    }
                  }
        public void verificaDiag1(int fila, int columna, string cf)
        {
            bool nada = false;
            bool otraFicha = false;
            bool encontre = false;
            List<int> fi = new List<int>();
            List<int> co = new List<int>();
            int i = 0;
            int a = fila;
            int b = columna;

            while (nada == false & otraFicha == false)
            {
                a = a - 1;
                b = b + 1;
                if (a > 0 & b <= 8)
                {
                    if (tab.obtener(a, b).Equals(cf))
                    {
                        if (i == 0)
                        {
                            nada = true;
                        }
                        else
                        {
                            otraFicha = true;
                        }
                    }
                    else if (tab.obtener(a, b).Equals("nada"))
                    {
                        nada = true;
                    }
                    else
                    {
                        fi.Add(a);
                        co.Add(b);
                    }
                    i = i + 1;
                }
                else
                {
                    nada = true;
                }
            }

                if (nada == false & otraFicha == true)
                {
                    foreach (int fil in fi)
                    {
                        foreach (int col in co)
                        {
                            if (cf.Equals("negro"))
                            {
                                tab.setMatriz(fil, col, "negro");
                            }
                            else
                            {
                                tab.setMatriz(fil, col, "blanco");
                            }

                        }
                    }
                }          
            
        }

        public void verificaDiag2(int fila, int columna, string cf)
        {
            bool nada = false;
            bool otraFicha = false;
            bool encontre = false;
            List<int> fi = new List<int>();
            List<int> co = new List<int>();
            int i = 0;
            int a = fila;
            int b = columna;

            while (nada == false & otraFicha == false)
            {
                a = a + 1;
                b = b + 1;
                if (a <=8  & b<=8)
                {
                    if (tab.obtener(a, b).Equals(cf))
                    {
                        if (i == 0)
                        {
                            nada = true;
                        }
                        else
                        {
                            otraFicha = true;
                        }
                    }
                    else if (tab.obtener(a, b).Equals("nada"))
                    {
                        nada = true;
                    }
                    else
                    {
                        fi.Add(a);
                        co.Add(b);
                    }
                    i = i + 1;
                }
                else
                {
                    nada = true;
                }
            }

            if (nada == false & otraFicha == true)
            {
                foreach (int fil in fi)
                {
                    foreach (int col in co)
                    {
                        if (cf.Equals("negro"))
                        {
                            tab.setMatriz(fil, col, "negro");
                        }
                        else
                        {
                            tab.setMatriz(fil, col, "blanco");
                        }

                    }
                }
            }

        }

        public void verificaDiag3(int fila, int columna, string cf)
        {
            bool nada = false;
            bool otraFicha = false;
            bool encontre = false;
            List<int> fi = new List<int>();
            List<int> co = new List<int>();
            int i = 0;
            int a = fila;
            int b = columna;

            while (nada == false & otraFicha == false)
            {
                a = a - 1;
                b = b - 1;
                if (a >0 & b>0 )
                {
                    if (tab.obtener(a, b).Equals(cf))
                    {
                        if (i == 0)
                        {
                            nada = true;
                        }
                        else
                        {
                            otraFicha = true;
                        }
                    }
                    else if (tab.obtener(a, b).Equals("nada"))
                    {
                        nada = true;
                    }
                    else
                    {
                        fi.Add(a);
                        co.Add(b);
                    }
                    i = i + 1;
                }
                else
                {
                    nada = true;
                }
            }

            if (nada == false & otraFicha == true)
            {
                foreach (int fil in fi)
                {
                    foreach (int col in co)
                    {
                        if (cf.Equals("negro"))
                        {
                            tab.setMatriz(fil, col, "negro");
                        }
                        else
                        {
                            tab.setMatriz(fil, col, "blanco");
                        }

                    }
                }
            }

        }

        public void verificaDiag4(int fila, int columna, string cf)
        {
            bool nada = false;
            bool otraFicha = false;
            bool encontre = false;
            List<int> fi = new List<int>();
            List<int> co = new List<int>();
            int i = 0;
            int a = fila;
            int b = columna;

            while (nada == false & otraFicha == false)
            {
                a = a + 1;
                b = b - 1;
                if (a <= 8 & b > 0)
                {
                    if (tab.obtener(a, b).Equals(cf))
                    {
                        if (i == 0)
                        {
                            nada = true;
                        }
                        else
                        {
                            otraFicha = true;
                        }
                    }
                    else if (tab.obtener(a, b).Equals("nada"))
                    {
                        nada = true;
                    }
                    else
                    {
                        fi.Add(a);
                        co.Add(b);
                    }
                    i = i + 1;
                }
                else
                {
                    nada = true;
                }
            }

            if (nada == false & otraFicha == true)
            {
                foreach (int fil in fi)
                {
                    foreach (int col in co)
                    {
                        if (cf.Equals("negro"))
                        {
                            tab.setMatriz(fil, col, "negro");
                        }
                        else
                        {
                            tab.setMatriz(fil, col, "blanco");
                        }

                    }
                }
            }

        }



        public int convertirLetra(string letra)
        {
            
            int colum = 0;
            if (letra.Equals("A"))
            {
                colum = 1;
            }
            else if (letra.Equals("B"))
            {
                colum = 2;
            }
            else if (letra.Equals("C"))
            {
                colum = 3;
            }
            else if (letra.Equals("D"))
            {
                colum = 4;
            }
            else if (letra.Equals("E"))
            {
                colum = 5;
            }
            else if (letra.Equals("F"))
            {
                colum = 6;
            }
            else if (letra.Equals("G"))
            {
                colum = 7;
            }
            else if (letra.Equals("H"))
            {
                colum = 8;
            }
            else {
                colum = 10;
            }
            return colum;
        }
    }
}