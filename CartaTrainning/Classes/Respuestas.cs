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
        public bool EsRespuestaCorrecta { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(nombre);
            if (!EsRespuestaCorrecta) sb.Append(" (X)");

            return sb.ToString();
        }
    }
}
