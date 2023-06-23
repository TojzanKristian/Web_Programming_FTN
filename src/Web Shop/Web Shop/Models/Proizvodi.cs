using System.Collections.Generic;

namespace Web_Shop.Models
{
    public class Proizvodi
    {
        public static XML file = new XML();

        public static List<Proizvod> ListaProizvoda { get; set; } = file.DeSerializeObject<List<Proizvod>>("Proizvodi.xml");
    }
}