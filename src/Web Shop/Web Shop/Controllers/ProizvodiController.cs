using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Web_Shop.Models;

namespace Web_Shop.Controllers
{
    public class ProizvodiController : ApiController
    {
        // Funkcija koja vraća sve proizvode za prikaz
        public IHttpActionResult GetAllProizvodi()
        {
            return Json(Proizvodi.ListaProizvoda);
        }

        // Funkcija za pretragu proizvoda na početnoj stranici
        [HttpGet]
        [Route("api/proizvodi/search")]
        public IHttpActionResult PretragaProizvoda(string naziv, string grad, double? minCena, double? maxCena)
        {
            List<Proizvod> result = LogikaPretrage(naziv, grad, minCena, maxCena);
            return Json(result);
        }
        private List<Proizvod> LogikaPretrage(string naziv, string grad, double? minCena, double? maxCena)
        {
            List<Proizvod> retV = new List<Proizvod>();
            foreach (Proizvod p in Proizvodi.ListaProizvoda)
            {
                if (naziv != null)
                {
                    if (p.Naziv.Contains(naziv))
                    {
                        retV.Add(p);
                    }
                }
                if (grad != null)
                {
                    if (p.Grad.Contains(grad))
                    {
                        retV.Add(p);
                    }
                }
                if (minCena != null && maxCena != null)
                {
                    if (p.Cena > minCena && p.Cena < maxCena)
                    {
                        retV.Add(p);
                    }
                }
            }
            return retV;
        }

        // Funkcija za sortiranje proizvoda na početnoj stranici
        [HttpGet]
        [Route("api/proizvodi/sort")]
        public IHttpActionResult SortiranjeProizvoda(string sortN, string sortC, string sortD)
        {
            List<Proizvod> result = LogikaSortiranja(sortN, sortC, sortD);
            return Json(result);
        }
        private List<Proizvod> LogikaSortiranja(string sortN, string sortC, string sortD)
        {
            List<Proizvod> retV = new List<Proizvod>();

            if (sortN != null)
            {
                if (sortN.Contains("rastuce"))
                {
                    retV = Proizvodi.ListaProizvoda.OrderBy(p => p.Naziv).ToList();
                }
                if (sortN.Contains("opadajuce"))
                {
                    retV = Proizvodi.ListaProizvoda.OrderByDescending(p => p.Naziv).ToList();
                }
            }
            if (sortC != null)
            {
                if (sortC.Contains("rastuce"))
                {
                    retV = Proizvodi.ListaProizvoda.OrderBy(p => p.Cena).ToList();
                }
                if (sortC.Contains("opadajuce"))
                {
                    retV = Proizvodi.ListaProizvoda.OrderByDescending(p => p.Cena).ToList();
                }
            }
            if (sortD != null)
            {
                if (sortD.Contains("rastuce"))
                {
                    retV = Proizvodi.ListaProizvoda.OrderBy(p => p.DatumPostavljanja).ToList();
                }
                if (sortD.Contains("opadajuce"))
                {
                    retV = Proizvodi.ListaProizvoda.OrderByDescending(p => p.DatumPostavljanja).ToList();
                }
            }
            return retV;
        }

        // Funkcija za dodavanje proizvoda u sistem od strane prodavca
        [HttpPost]
        [Route("api/proizvodi/dodaj")]
        public IHttpActionResult Dodaj(Proizvod p)
        {
            p.Recenzije = new List<Recenzija>();
            string filePath = PromenaPutanjeSlike(p.Slika);
            p.Slika = filePath;
            if (p != null)
            {
                if (p.Kolicina > 0)
                    p.Dostupan = "Da";
                if (p.Kolicina == 0)
                    p.Dostupan = "Ne";
                p.Recenzije.Add(new Recenzija("", "", "", "Nema recenzije!", ""));
                Proizvodi.ListaProizvoda.Add(p);
            }
            XML file = new XML();
            file.SerializeObject<List<Proizvod>>(Proizvodi.ListaProizvoda, "Proizvodi.xml");

            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            if (p != null)
            {
                if (p.Kolicina > 0)
                    p.Dostupan = "Da";
                if (p.Kolicina == 0)
                    p.Dostupan = "Ne";
                x[0].ObjavljeniProizvodi.Add(p);
            }
            file.SerializeObject<List<Korisnik>>(x, "Pomoc.xml");

            if (p == null)
            {
                return BadRequest("Dodavanje nije moguće !!");
            }
            var response = new
            {
                Data = x[0].ObjavljeniProizvodi,
                Message = "Uspešno!"
            };
            return Json(response);
        }

        // Funkcija koja menja putanju slike
        private string PromenaPutanjeSlike(string path)
        {
            string[] s = path.Split('\\'); //base64Image.Replace("C:\\fakepath\\", filePath);
            return "Pictures\\" + s[2];
        }
    }
}