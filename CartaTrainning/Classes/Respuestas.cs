using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartaTrainning.Classes
{
    public class RespuestasPizzas : Pizza
    {
        public int NumeroRespuesta { get; set; }
        public int[] IngredientesUsuario { get; set; }
        public bool EsRespuestaCorrecta { get; set; } = false;
        public int precio_small_usuario { get; set; }
        public int precio_big_usuario { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(nombre);
            if (!EsRespuestaCorrecta) sb.Append(" (X)");

            return sb.ToString();
        }
    }

    public class RespuestasIngredientes : Ingrediente
    {
        public int NumeroRespuesta { get; set; }
        public bool EsRespuestaCorrecta { get; set; } = false;
        public int precio_small_usuario { get; set; }
        public int precio_big_usuario { get; set; }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(nombre);
            if (!EsRespuestaCorrecta) sb.Append(" (X)");

            return sb.ToString();
        }
    }
}
