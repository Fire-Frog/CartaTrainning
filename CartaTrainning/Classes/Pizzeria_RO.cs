using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartaTrainning.Classes
{
    public class Ingrediente
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public int precio_small { get; set; }
        public int precio_big { get; set; }
        public bool carta { get; set; }
    }

    public class Pizza
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public int precio_small { get; set; }
        public int precio_big { get; set; }
        public bool vegetariana { get; set; }
        public List<int> ingredientes { get; set; }
        public List<Ingrediente> ingredientes_classes { get; set; }
    }

    public class Pizzeria_RO
    {
        public List<Ingrediente> ingredientes { get; set; }
        public List<Pizza> pizzas { get; set; }
        public static Pizzeria_RO LoadJSON()
        {
            var textjson = System.IO.File.ReadAllText("Resources/format_pizzeria.json");
            var jsonObj = JsonConvert.DeserializeObject<Pizzeria_RO>(textjson);
            return jsonObj;
        }
    }

    public static class BaseLabels
    {
        public static string[] Questions
        {
            get
            {
                return new string[] {
                    "¿Qué ingredientes tiene la siguiente Pizza?\n'{0}'",
                    "¿El ingrediente '{0}', qué valor tiene en ambos tamaños?",
                    "¿Qué valor tiene la pizza '{0}' en ambos tamaños?"
                };
            }
        }

        public static string[] Levels
        {
            get
            {
                return new string[] {
                    "EZ: 4 respuestas disponibles. Ayuda disponible",
                    "Medium: 5 respuestas disponibles",
                    "Hard: Seleccion de respuestas"
                };
            }
        }

        public static string[] ValidacionOK
        {
            get
            {
                return new string[]
                {
                    "Pepega",
                    "not bad",
                    "ez",
                    "gg wp",
                    "too ez",
                    "naisla",
                    "good",
                    "2ez4rtz",
                    "fkin ez"
                };
            }
        }

        public static string[] ValidacionError
        {
            get
            {
                return new string[]
                {
                    "LUL",
                    "gitgud",
                    "lmao",
                    "nub",
                    "get rekt",
                    "u not even trying",
                    "reported",
                    "ayylmao",
                    "ur so bad lol"
                };
            }
        }
    }
}
