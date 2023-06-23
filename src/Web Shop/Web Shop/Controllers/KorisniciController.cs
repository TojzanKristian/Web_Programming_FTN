using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using Web_Shop.Models;

public static class Globals
{
    private static List<Korisnik> korisnici = new List<Korisnik>();
    private static Korisnik prijavljen;
    private static string nazivProizvodaZaIzmenu;

    public static Korisnik Prijavljen { get => prijavljen; set => prijavljen = value; }
    public static List<Korisnik> Korisnici { get => korisnici; set => korisnici = value; }
    public static string NazivProizvodaZaIzmenu { get => nazivProizvodaZaIzmenu; set => nazivProizvodaZaIzmenu = value; }
}

namespace Web_Shop.Controllers
{
    public class KorisniciController : ApiController
    {
        private readonly XML file = new XML();

        // Prikaz korisnika Adminu
        public List<Korisnik> Get()
        {
            return file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
        }

        // Pretraga proizvoda od strane admina
        [HttpGet]
        [Route("api/korisnici/admin/proizvodi/pretraga")]
        public IHttpActionResult PretragaProizvoda(string status)
        {
            List<Proizvod> result = LogikaPretrageProizvoda(status);

            return Json(result);
        }
        private List<Proizvod> LogikaPretrageProizvoda(string status)
        {
            string s = char.ToUpper(status[0]) + status.Substring(1);
            List<Proizvod> retV = new List<Proizvod>();
            List<Proizvod> proizvodi = file.DeSerializeObject<List<Proizvod>>("Proizvodi.xml");
            foreach (Proizvod p in proizvodi)
            {
                if (status != null)
                {
                    if (p.Dostupan.Equals(s))
                    {
                        retV.Add(p);
                    }
                }
            }
            return retV;
        }

        // Sortiranje proizvoda od strane admina
        [HttpGet]
        [Route("api/korisnici/admin/proizvodi/sort")]
        public IHttpActionResult SortProizvoda(string nazivSort, string cenaSort, string datumPSort)
        {
            List<Proizvod> result = LogikaSortiranjaProivoda(nazivSort, cenaSort, datumPSort);

            return Json(result);
        }
        private List<Proizvod> LogikaSortiranjaProivoda(string nazivSort, string cenaSort, string datumPSort)
        {
            List<Proizvod> retV = new List<Proizvod>();

            if (nazivSort != null)
            {
                if (nazivSort.Contains("rastuce"))
                {
                    retV = Proizvodi.ListaProizvoda.OrderBy(p => p.Naziv).ToList();
                }
                if (nazivSort.Contains("opadajuce"))
                {
                    retV = Proizvodi.ListaProizvoda.OrderByDescending(p => p.Naziv).ToList();
                }
            }
            if (cenaSort != null)
            {
                if (cenaSort.Contains("rastuce"))
                {
                    retV = Proizvodi.ListaProizvoda.OrderBy(p => p.Cena).ToList();
                }
                if (cenaSort.Contains("opadajuce"))
                {
                    retV = Proizvodi.ListaProizvoda.OrderByDescending(p => p.Cena).ToList();
                }
            }
            if (datumPSort != null)
            {
                if (datumPSort.Contains("rastuce"))
                {
                    retV = Proizvodi.ListaProizvoda.OrderBy(p => p.DatumPostavljanja).ToList();
                }
                if (datumPSort.Contains("opadajuce"))
                {
                    retV = Proizvodi.ListaProizvoda.OrderByDescending(p => p.DatumPostavljanja).ToList();
                }
            }
            return retV;
        }

        // Pretraga korisnika od strane admina
        [HttpGet]
        [Route("api/korisnici/admin/search")]
        public IHttpActionResult Search(string ime, string prezime, string datumR, string uloge)
        {
            List<Korisnik> result = LogikaPretrage(ime, prezime, datumR, uloge);

            return Json(result);
        }
        private List<Korisnik> LogikaPretrage(string ime, string prezime, string datumR, string uloge)
        {
            List<Korisnik> retV = new List<Korisnik>();
            List<Korisnik> korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            foreach (Korisnik k in korisnici)
            {
                if (ime != null)
                {
                    if (k.Ime.Contains(ime))
                    {
                        retV.Add(k);
                    }
                }
                if (prezime != null)
                {
                    if (k.Prezime.Contains(prezime))
                    {
                        retV.Add(k);
                    }
                }
                if (datumR != null)
                {
                    DateTime datum = DateTime.Parse(datumR);
                    if (k.DatumRodjenja == datum)
                    {
                        retV.Add(k);
                    }
                }
                if (uloge != null)
                {
                    Uloge pom = Uloge.Administrator;
                    if (uloge.Contains("kupac"))
                    {
                        pom = Uloge.Kupac;
                    }
                    if (uloge.Contains("prodavac"))
                    {
                        pom = Uloge.Prodavac;
                    }
                    if (k.Uloga == pom)
                    {
                        retV.Add(k);
                    }
                }
            }
            return retV;
        }

        // Sortiranje korisnika od strane admina
        [HttpGet]
        [Route("api/korisnici/admin/sort")]
        public IHttpActionResult Sort(string imeSort, string datumSort, string ulogeSort)
        {
            List<Korisnik> result = LogikaSortiranja(imeSort, datumSort, ulogeSort);

            return Json(result);
        }
        private List<Korisnik> LogikaSortiranja(string imeSort, string datumSort, string ulogeSort)
        {
            List<Korisnik> retV = new List<Korisnik>();
            List<Korisnik> korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");

            if (imeSort != null)
            {
                if (imeSort.Contains("rastuce"))
                {
                    retV = korisnici.OrderBy(k => k.Ime).ToList();
                }
                if (imeSort.Contains("opadajuce"))
                {
                    retV = korisnici.OrderByDescending(k => k.Ime).ToList();
                }
            }
            if (datumSort != null)
            {
                if (datumSort.Contains("rastuce"))
                {
                    retV = korisnici.OrderBy(k => k.DatumRodjenja).ToList();
                }
                if (datumSort.Contains("opadajuce"))
                {
                    retV = korisnici.OrderByDescending(k => k.DatumRodjenja).ToList();
                }
            }
            if (ulogeSort != null)
            {
                if (ulogeSort.Contains("rastuce"))
                {
                    retV = korisnici.OrderBy(k => k.Uloga).ToList();
                }
                if (ulogeSort.Contains("opadajuce"))
                {
                    retV = korisnici.OrderByDescending(k => k.Uloga).ToList();
                }
            }
            return retV;
        }

        // Prijava
        public IHttpActionResult Get(string korisnickoIme, string lozinka)
        {
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            int brojac = 0;
            foreach (Korisnik pom in Globals.Korisnici)
            {
                if (pom.KorisnickoIme.Contains(korisnickoIme) && pom.Lozinka.Contains(lozinka))
                {
                    Globals.Prijavljen = pom;
                    brojac++;
                }
            }

            List<Korisnik> admini = file.DeSerializeObject<List<Korisnik>>("Administratori.xml");
            foreach (Korisnik pom in admini)
            {
                if (pom.KorisnickoIme.Contains(korisnickoIme) && pom.Lozinka.Contains(lozinka))
                {
                    Globals.Prijavljen = pom;
                    brojac++;
                }
            }

            if (brojac == 0)
            {
                return BadRequest($"Korisnik sa korisničkim imenom {korisnickoIme} ne postoji ! Otvori će vam se stranica za registraciju....");
            }

            List<Korisnik> x = new List<Korisnik>
            {
                Globals.Prijavljen
            };
            file.SerializeObject<List<Korisnik>>(x, "Pomoc.xml");

            var temp = Globals.Prijavljen;
            var response = new
            {
                Data = temp,
                Message = "Uspešno!"
            };

            return Json(response);
        }

        // Preuzimanje porudžbina kod kupca
        [HttpGet]
        [Route("api/korisnici/porudzbine")]
        public IHttpActionResult GetPorudzbine()
        {
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            Globals.Prijavljen = x[0];
            List<Porudzbina> poruceno = new List<Porudzbina>();

            foreach (Korisnik pom in Globals.Korisnici)
            {
                if (pom.KorisnickoIme.Contains(Globals.Prijavljen.KorisnickoIme))
                {
                    poruceno = pom.Porudzbine;
                }
            }

            var response = new
            {
                Data = poruceno,
                Message = "Uspešno!"
            };

            return Json(response);
        }

        // Preuzimanje svih porudžbina za admina
        [HttpGet]
        [Route("api/korisnici/sveporudzbine")]
        public IHttpActionResult SvePorudzbine()
        {
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            List<Porudzbina> sve = new List<Porudzbina>();

            foreach (Korisnik pom in Globals.Korisnici)
            {
                foreach (Porudzbina item in pom.Porudzbine)
                {
                    sve.Add(item);
                }
            }

            var response = new
            {
                Data = sve,
                Message = "Uspešno!"
            };

            return Json(response);
        }

        // Preuzimanje omiljenih proizvoda kupca
        [HttpGet]
        [Route("api/korisnici/omiljeni")]
        public IHttpActionResult GetOmiljeni()
        {
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            Globals.Prijavljen = x[0];
            List<Proizvod> omiljeni = new List<Proizvod>();

            foreach (Korisnik pom in Globals.Korisnici)
            {
                if (pom.KorisnickoIme.Contains(Globals.Prijavljen.KorisnickoIme))
                {
                    omiljeni = Globals.Prijavljen.OmiljeniProizvodi;
                }
            }

            var response = new
            {
                Data = omiljeni,
                Message = "Uspešno!"
            };

            return Json(response);
        }

        [HttpPost]
        [Route("api/korisnici/dodajuOmiljne")]
        public IHttpActionResult DodajUOmiljne([FromBody] JObject data)
        {
            string naziv = data.GetValue("naziv").ToString();
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            Globals.Prijavljen = x[0];

            Proizvod pom = null;
            for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
            {
                if (Proizvodi.ListaProizvoda[i].Naziv.Contains(naziv))
                {
                    pom = Proizvodi.ListaProizvoda[i];
                }
            }

            if (pom != null)
            {
                for (int i = 0; i < Globals.Korisnici.Count; i++)
                {
                    Korisnik k = Globals.Korisnici[i];
                    if (k.KorisnickoIme.Contains(Globals.Prijavljen.KorisnickoIme))
                    {
                        x[0].OmiljeniProizvodi.Add(pom);
                    }
                }
            }

            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");
            file.SerializeObject<List<Korisnik>>(x, "Pomoc.xml");

            return Ok();
        }

        // Poručivanje proizvoda od strane Kupca
        [HttpPost]
        [Route("api/korisnici/naruci")]
        public IHttpActionResult NaruciProizvod([FromBody] JObject data)
        {
            string naziv = data.GetValue("naziv").ToString();
            int kolicina = int.Parse(data.GetValue("kolicina").ToString());
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            Globals.Prijavljen = x[0];
            Proizvod pomoc = null;

            foreach (Proizvod p in Proizvodi.ListaProizvoda)
            {
                if (p.Naziv.Contains(naziv) && p.Kolicina > 0 && p.Kolicina >= kolicina && p.Dostupan.Contains("Da"))
                {
                    pomoc = p;
                }
            }

            if (pomoc == null)
            {
                return BadRequest("Proizvod ili nije dostupan ili ste uneli pogrešnu količinu !");
            }

            Porudzbina poruceno = new Porudzbina(pomoc, kolicina, null, DateTime.Now, Statusi.AKTIVNA);

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                if (Globals.Korisnici[i].KorisnickoIme.Equals(Globals.Prijavljen.KorisnickoIme))
                {
                    if (pomoc != null)
                    {
                        poruceno = new Porudzbina(pomoc, kolicina, Globals.Korisnici[i].KorisnickoIme, DateTime.Now, Statusi.AKTIVNA);
                        Globals.Korisnici[i].Porudzbine.Add(poruceno);
                    }
                }
            }

            if (pomoc != null)
            {
                x[0].Porudzbine.Add(poruceno);
            }

            for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
            {
                if (pomoc.Naziv.Contains(Proizvodi.ListaProizvoda[i].Naziv))
                {
                    Proizvodi.ListaProizvoda[i].Kolicina -= kolicina;
                    if (Proizvodi.ListaProizvoda[i].Kolicina == 0)
                    {
                        Proizvodi.ListaProizvoda[i].Dostupan = "Ne";
                    }
                }
            }

            for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
            {
                if (Proizvodi.ListaProizvoda[i].Kolicina == 0)
                {
                    Proizvodi.ListaProizvoda.Remove(Proizvodi.ListaProizvoda[i]);
                }
            }

            file.SerializeObject<List<Proizvod>>(Proizvodi.ListaProizvoda, "Proizvodi.xml");
            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");
            file.SerializeObject<List<Korisnik>>(x, "Pomoc.xml");

            return Ok();
        }

        // Prikaz objavljenih proizvoda za prodavca
        [HttpGet]
        [Route("api/korisnici/objavljeni")]
        public List<Proizvod> Objavljeni()
        {
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            return x[0].ObjavljeniProizvodi;
        }

        // Registracija
        public IHttpActionResult Post(Korisnik k)
        {
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");

            foreach (Korisnik pom in Globals.Korisnici)
            {
                if (pom.KorisnickoIme.Contains(k.KorisnickoIme))
                {
                    return BadRequest($"Korisnik sa korisničkim imenom {k.KorisnickoIme} već postoji !");
                }
            }

            Globals.Korisnici.Add(k);
            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");

            return Ok();
        }

        // Dodavanje prodavca od strane admina
        [HttpPost]
        [Route("api/korisnici/dodatProdavac")]
        public IHttpActionResult DodatProdavac(Korisnik korisnik)
        {
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");

            foreach (Korisnik pom in Globals.Korisnici)
            {
                if (pom.KorisnickoIme.Contains(korisnik.KorisnickoIme))
                {
                    return BadRequest($"Korisnik sa korisničkim imenom {korisnik.KorisnickoIme} već postoji !");
                }
            }

            Globals.Korisnici.Add(korisnik);
            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");

            return Json(Globals.Korisnici);
        }


        // Za prikaz profila
        [HttpGet]
        [Route("api/korisnici/preuzmi")]
        public IHttpActionResult GetKorisnik()
        {
            string filePath = GetProjectLocation() + "bin\\Debug\\Pomoc.xml";
            if (File.Exists(filePath))
            {
                List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
                Globals.Prijavljen = x[0];
            }
            else
            {
                return BadRequest("Niste se prijavili !!");
            }

            if (Globals.Prijavljen == null)
            {
                return BadRequest("Niste se prijavili !!");
            }

            var temp = Globals.Prijavljen;
            var response = new
            {
                Data = temp,
                Message = "Uspešno!"
            };

            return Json(response);
        }

        // Za izmenu korisnika kod admina
        [HttpGet]
        [Route("api/korisnici/preuzmiOznacenogKorisnika")]
        public IHttpActionResult PreuzmiKorisnika()
        {
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            List<string> pom = file.DeSerializeObject<List<string>>("Naziv.xml");
            string ime = pom[0];
            Korisnik temp = null;

            foreach (Korisnik k in Globals.Korisnici)
            {
                if (k.Ime.Equals(ime))
                {
                    temp = k;
                }
            }

            var response = new
            {
                Data = temp,
                Message = "Uspešno!"
            };

            return Json(response);
        }

        // Za izmenu profila
        public IHttpActionResult Put(Korisnik k)
        {
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            Globals.Prijavljen = x[0];
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                if (Globals.Korisnici[i].KorisnickoIme.Equals(k.KorisnickoIme))
                {
                    return BadRequest("Korisnik sa korisničkim imenom već postoji !!");
                }
                if (Globals.Korisnici[i].Ime.Equals(k.Ime))
                {
                    Globals.Korisnici[i].KorisnickoIme = k.KorisnickoIme;
                    Globals.Korisnici[i].Lozinka = k.Lozinka;
                    Globals.Korisnici[i].Prezime = k.Prezime;
                    Globals.Korisnici[i].Pol = k.Pol;
                    Globals.Korisnici[i].Email = k.Email;
                    Globals.Korisnici[i].DatumRodjenja = k.DatumRodjenja;
                }
            }

            if (Globals.Prijavljen.Ime.Equals(k.Ime))
            {
                Globals.Prijavljen.KorisnickoIme = k.KorisnickoIme;
                Globals.Prijavljen.Lozinka = k.Lozinka;
                Globals.Prijavljen.Prezime = k.Prezime;
                Globals.Prijavljen.Pol = k.Pol;
                Globals.Prijavljen.Email = k.Email;
                Globals.Prijavljen.DatumRodjenja = k.DatumRodjenja;
            }


            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");
            file.SerializeObject<List<Korisnik>>(x, "Pomoc.xml");

            return Ok();
        }

        // Za korisnika kod admina
        [HttpPut]
        [Route("api/korisnici/izmenaKodAdmina")]
        public IHttpActionResult IzmenaKorisnika(Korisnik k)
        {
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                if (Globals.Korisnici[i].KorisnickoIme.Equals(k.KorisnickoIme))
                {
                    return BadRequest("Korisnik sa korisničkim imenom već postoji !!");
                }
                if (Globals.Korisnici[i].Ime.Equals(k.Ime))
                {
                    Globals.Korisnici[i].KorisnickoIme = k.KorisnickoIme;
                    Globals.Korisnici[i].Lozinka = k.Lozinka;
                    Globals.Korisnici[i].Prezime = k.Prezime;
                    Globals.Korisnici[i].Pol = k.Pol;
                    Globals.Korisnici[i].Email = k.Email;
                    Globals.Korisnici[i].DatumRodjenja = k.DatumRodjenja;
                }
            }

            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");

            return Ok();
        }

        // Pretraga kod prodavca
        [HttpGet]
        [Route("api/korisnici/search")]
        public IHttpActionResult SearchProducts(string dostupnost)
        {
            List<Proizvod> result = SearchLogic(dostupnost);

            return Json(result);
        }
        private List<Proizvod> SearchLogic(string dostupnost)
        {
            List<Proizvod> retV = new List<Proizvod>();
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            Globals.Prijavljen = x[0];
            foreach (Proizvod p in Globals.Prijavljen.ObjavljeniProizvodi)
            {
                if (p.Dostupan.Contains(char.ToUpper(dostupnost[0]) + dostupnost.Substring(1)))
                {
                    retV.Add(p);
                }
            }
            return retV;
        }

        // Sortiranje kod prodavca
        [HttpGet]
        [Route("api/korisnici/sort")]
        public IHttpActionResult SortProducts(string sortN, string sortC, string sortD)
        {
            List<Proizvod> result = SortLogic(sortN, sortC, sortD);

            return Json(result);
        }
        private List<Proizvod> SortLogic(string sortN, string sortC, string sortD)
        {
            List<Proizvod> retV = new List<Proizvod>();
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            Globals.Prijavljen = x[0];

            if (sortN != null)
            {
                if (sortN.Contains("rastuce"))
                {
                    retV = Globals.Prijavljen.ObjavljeniProizvodi.OrderBy(p => p.Naziv).ToList();
                }
                if (sortN.Contains("opadajuce"))
                {
                    retV = Globals.Prijavljen.ObjavljeniProizvodi.OrderByDescending(p => p.Naziv).ToList();
                }
            }
            if (sortC != null)
            {
                if (sortC.Contains("rastuce"))
                {
                    retV = Globals.Prijavljen.ObjavljeniProizvodi.OrderBy(p => p.Cena).ToList();
                }
                if (sortC.Contains("opadajuce"))
                {
                    retV = Globals.Prijavljen.ObjavljeniProizvodi.OrderByDescending(p => p.Cena).ToList();
                }
            }
            if (sortD != null)
            {
                if (sortD.Contains("rastuce"))
                {
                    retV = Globals.Prijavljen.ObjavljeniProizvodi.OrderBy(p => p.DatumPostavljanja).ToList();
                }
                if (sortD.Contains("opadajuce"))
                {
                    retV = Globals.Prijavljen.ObjavljeniProizvodi.OrderByDescending(p => p.DatumPostavljanja).ToList();
                }
            }
            return retV;
        }

        // Brisanje proizvoda od strane prodavca
        [HttpDelete]
        [Route("api/korisnici/brisanje")]
        public IHttpActionResult Brisanje([FromBody] JObject data)
        {
            string naziv = data.GetValue("naziv").ToString();
            int br = 0;

            for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
            {
                if (Proizvodi.ListaProizvoda[i].Naziv.Contains(naziv) && (Proizvodi.ListaProizvoda[i].Dostupan.Contains("Da") || Proizvodi.ListaProizvoda[i].Dostupan.Contains("da")))
                {
                    Proizvodi.ListaProizvoda.Remove(Proizvodi.ListaProizvoda[i]);
                    br++;
                }
            }

            file.SerializeObject<List<Proizvod>>(Proizvodi.ListaProizvoda, "Proizvodi.xml");
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");

            for (int i = 0; i < x[0].ObjavljeniProizvodi.Count; i++)
            {
                if (x[0].ObjavljeniProizvodi[i].Naziv.Contains(naziv) && (x[0].ObjavljeniProizvodi[i].Dostupan.Contains("Da") || x[0].ObjavljeniProizvodi[i].Dostupan.Contains("da")))
                {
                    x[0].ObjavljeniProizvodi.Remove(x[0].ObjavljeniProizvodi[i]);
                }
            }

            file.SerializeObject<List<Korisnik>>(x, "Pomoc.xml");

            if (br == 0)
            {
                return BadRequest("Brisanje nije moguće !!");
            }

            var response = new
            {
                Data = x[0].ObjavljeniProizvodi,
                Message = "Uspešno!"
            };

            return Json(response);
        }

        // Brisanje proizvoda od strane admina
        [HttpDelete]
        [Route("api/korisnici/brisanjeProizvodaAdmin")]
        public IHttpActionResult BrisanjeProizvodaAdmin([FromBody] JObject data)
        {
            string naziv = data.GetValue("naziv").ToString();
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            int br = 0;

            for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
            {
                if (Proizvodi.ListaProizvoda[i].Naziv.Contains(naziv))
                {
                    Proizvodi.ListaProizvoda.Remove(Proizvodi.ListaProizvoda[i]);
                    br++;
                }
            }

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                for (int j = 0; j < Globals.Korisnici[i].ObjavljeniProizvodi.Count; j++)
                {
                    if (Globals.Korisnici[i].ObjavljeniProizvodi[j].Naziv.Contains(naziv))
                    {
                        Globals.Korisnici[i].ObjavljeniProizvodi.Remove(Globals.Korisnici[i].ObjavljeniProizvodi[j]);
                    }
                }

                for (int j = 0; j < Globals.Korisnici[i].OmiljeniProizvodi.Count; j++)
                {
                    if (Globals.Korisnici[i].OmiljeniProizvodi[j].Naziv.Contains(naziv))
                    {
                        Globals.Korisnici[i].OmiljeniProizvodi.Remove(Globals.Korisnici[i].OmiljeniProizvodi[j]);
                    }
                }

                for (int k = 0; k < Globals.Korisnici[i].Porudzbine.Count; k++)
                {
                    if (Globals.Korisnici[i].Porudzbine[k].Proizvod.Naziv.Contains(naziv))
                    {
                        Globals.Korisnici[i].Porudzbine.Remove(Globals.Korisnici[i].Porudzbine[k]);
                    }
                }
            }

            file.SerializeObject<List<Proizvod>>(Proizvodi.ListaProizvoda, "Proizvodi.xml");
            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");


            if (br == 0)
            {
                return BadRequest("Brisanje nije moguće !!");
            }

            return Ok();
        }

        // Brisanje korisnika od strane admina
        [HttpDelete]
        [Route("api/korisnici/brisanjeKorisnikaAdmin")]
        public IHttpActionResult BrisanjeKorisnika([FromBody] JObject data)
        {
            string ime = data.GetValue("naziv").ToString();
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                if (Globals.Korisnici[i].Ime.Equals(ime) && Globals.Korisnici[i].Uloga == Uloge.Prodavac)
                {
                    Globals.Korisnici.Remove(Globals.Korisnici[i]);
                }
            }

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                if (Globals.Korisnici[i].Ime.Equals(ime) && Globals.Korisnici[i].Uloga == Uloge.Kupac)
                {
                    for (int j = 0; j < Globals.Korisnici[i].Porudzbine.Count; j++)
                    {
                        if (Globals.Korisnici[i].Porudzbine[j].Status == Statusi.AKTIVNA)
                        {
                            for (int k = 0; k < Proizvodi.ListaProizvoda.Count; k++)
                            {
                                if (Proizvodi.ListaProizvoda[k].Naziv.Equals(Globals.Korisnici[i].Porudzbine[j].Proizvod.Naziv))
                                {
                                    Proizvodi.ListaProizvoda[k].Kolicina += Globals.Korisnici[i].Porudzbine[j].Kolicina;
                                }
                            }
                        }
                    }
                    Globals.Korisnici.Remove(Globals.Korisnici[i]);
                }
            }

            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");
            file.SerializeObject<List<Proizvod>>(Proizvodi.ListaProizvoda, "Proizvodi.xml");

            return Ok();
        }

        // Priprema za Izmenu proizvoda
        [HttpPost]
        [Route("api/korisnici/pripremaZaIzmenu")]
        public IHttpActionResult PripremaIzmene([FromBody] JObject data)
        {
            List<string> naziv = new List<string> { data.GetValue("naziv").ToString() };
            file.SerializeObject<List<string>>(naziv, "Naziv.xml");
            return Ok();
        }

        // Preuzimanje podataka o proizvodu koji se menja kod prodavca
        [HttpGet]
        [Route("api/korisnici/preuzmiIzmenu")]
        public IHttpActionResult PreuzmiIzmenu()
        {
            Proizvod temp = null;
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            List<string> pom = file.DeSerializeObject<List<string>>("Naziv.xml");
            Globals.NazivProizvodaZaIzmenu = pom[0];

            for (int i = 0; i < x[0].ObjavljeniProizvodi.Count; i++)
            {
                if (x[0].ObjavljeniProizvodi[i].Naziv.Contains(Globals.NazivProizvodaZaIzmenu))
                {
                    temp = x[0].ObjavljeniProizvodi[i];
                }
            }

            if (temp == null)
            {
                return BadRequest("Nije odabran proizvod !!");
            }

            var response = new
            {
                Data = temp,
                Message = "Uspešno!"
            };

            return Json(response);
        }

        // Preuzimanje podataka o proizvodu koji se menja kod admina
        [HttpGet]
        [Route("api/korisnici/preuzmiIzmenuAdmin")]
        public IHttpActionResult IzmenaAdmin()
        {
            Proizvod temp = null;
            List<string> pom = file.DeSerializeObject<List<string>>("Naziv.xml");
            Globals.NazivProizvodaZaIzmenu = pom[0];

            for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
            {
                if (Proizvodi.ListaProizvoda[i].Naziv.Contains(Globals.NazivProizvodaZaIzmenu))
                {
                    temp = Proizvodi.ListaProizvoda[i];
                }
            }

            if (temp == null)
            {
                return BadRequest("Nije odabran proizvod !!");
            }

            var response = new
            {
                Data = temp,
                Message = "Uspešno!"
            };

            return Json(response);
        }

        // Izmena status porudžbine od strane Kupca
        [HttpPost]
        [Route("api/korisnici/izmeniStatus")]
        public IHttpActionResult IzmeniStatus([FromBody] JObject data)
        {
            string naziv = data.GetValue("naziv").ToString();
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            Globals.Prijavljen = x[0];

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                Korisnik k = Globals.Korisnici[i];
                if (k.KorisnickoIme.Contains(Globals.Prijavljen.KorisnickoIme))
                {
                    for (int j = 0; j < k.Porudzbine.Count; j++)
                    {
                        if (k.Porudzbine[j].Status == Statusi.OTKAZAN)
                        {
                            return BadRequest("Nemate pravo da izmenite status !!");
                        }
                        if (k.Porudzbine[j].Status == Statusi.AKTIVNA && k.Porudzbine[j].Proizvod.Naziv.Contains(naziv))
                        {
                            Globals.Prijavljen.Porudzbine[j].Status = Statusi.IZVRŠEN;
                            Globals.Korisnici[i].Porudzbine[j].Status = Statusi.IZVRŠEN;
                        }
                    }
                }
            }

            file.SerializeObject<List<Korisnik>>(x, "Pomoc.xml");
            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");

            return Ok();
        }

        // Izmena status porudžbine od strane admina na izvršeno
        [HttpPost]
        [Route("api/korisnici/statusIzvrsen")]
        public IHttpActionResult StatusIzvrsen([FromBody] JObject data)
        {
            string naziv = data.GetValue("naziv").ToString();
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                Korisnik k = Globals.Korisnici[i];
                for (int j = 0; j < k.Porudzbine.Count; j++)
                {
                    if (k.Porudzbine[j].Proizvod.Naziv.Contains(naziv))
                    {
                        Globals.Korisnici[i].Porudzbine[j].Status = Statusi.IZVRŠEN;
                    }
                }
            }

            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");

            return Ok();
        }

        // Izmena status porudžbine od strane admina na otkazan
        [HttpPost]
        [Route("api/korisnici/statusOtkazan")]
        public IHttpActionResult StatusOtkazan([FromBody] JObject data)
        {
            string naziv = data.GetValue("naziv").ToString();
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            int kolicina = 0;
            bool status = false;
            Porudzbina pomoc = new Porudzbina();

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                Korisnik k = Globals.Korisnici[i];
                for (int j = 0; j < k.Porudzbine.Count; j++)
                {
                    if (k.Porudzbine[j].Proizvod.Naziv.Contains(naziv) && (k.Porudzbine[j].Status == Statusi.IZVRŠEN || k.Porudzbine[j].Status == Statusi.OTKAZAN))
                    {
                        return BadRequest("Status je Izvršen ili Otkazan, nemate pravo da izmenite status !!");
                    }

                    if (k.Porudzbine[j].Proizvod.Naziv.Contains(naziv) && k.Porudzbine[j].Status == Statusi.AKTIVNA)
                    {
                        Globals.Korisnici[i].Porudzbine[j].Status = Statusi.OTKAZAN;
                        kolicina = Globals.Korisnici[i].Porudzbine[j].Kolicina;
                        pomoc = Globals.Korisnici[i].Porudzbine[j];
                        status = true;
                    }
                }
            }

            int br = 0;
            for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
            {
                if (Proizvodi.ListaProizvoda[i].Naziv.Equals(naziv) && status == true)
                {
                    Proizvodi.ListaProizvoda[i].Kolicina += kolicina;
                    br++;
                }
            }

            if (br == 0)
            {
                pomoc.Proizvod.Kolicina += kolicina;
                if (pomoc.Proizvod.Kolicina > 0)
                    pomoc.Proizvod.Dostupan = "Da";
                Proizvodi.ListaProizvoda.Add(pomoc.Proizvod);
            }

            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");
            file.SerializeObject<List<Proizvod>>(Proizvodi.ListaProizvoda, "Proizvodi.xml");

            return Ok();
        }

        // Dodavanje recenzije od strane Kupca
        [HttpPost]
        [Route("api/korisnici/dodajRecenziju")]
        public IHttpActionResult DodajRecenziju([FromBody] JObject data)
        {
            string naziv = data.GetValue("naziv").ToString();
            string naslov = data.GetValue("naslov").ToString();
            string opis = data.GetValue("opis").ToString();
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            Globals.Prijavljen = x[0];

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                Korisnik k = Globals.Korisnici[i];
                if (k.KorisnickoIme.Contains(Globals.Prijavljen.KorisnickoIme))
                {
                    for (int j = 0; j < k.Porudzbine.Count; j++)
                    {
                        if (k.Porudzbine[j].Status != Statusi.IZVRŠEN)
                        {
                            return BadRequest("Nemate pravo da dodate recenziju !!");
                        }
                        if (k.Porudzbine[j].Status == Statusi.IZVRŠEN && k.Porudzbine[j].Proizvod.Naziv.Contains(naziv))
                        {
                            Recenzija recenzija = new Recenzija(Globals.Prijavljen.Porudzbine[j].Proizvod.Naziv, Globals.Prijavljen.KorisnickoIme, naslov, opis, Globals.Prijavljen.Porudzbine[j].Proizvod.Slika);
                            Globals.Prijavljen.Porudzbine[j].Proizvod.Recenzije.Add(recenzija);
                            Globals.Korisnici[i].Porudzbine[j].Proizvod.Recenzije.Add(recenzija);
                        }
                    }
                }
            }

            file.SerializeObject<List<Korisnik>>(x, "Pomoc.xml");
            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");

            return Ok();
        }

        // Izmena proizvoda kod prodavca
        [HttpPut]
        [Route("api/korisnici/Izmena")]
        public IHttpActionResult Izmena([FromBody] JObject data)
        {
            string naziv = data.GetValue("naziv").ToString();
            double cena = double.Parse(data.GetValue("cena").ToString());
            int kolicina = int.Parse(data.GetValue("kolicina").ToString());
            string opis = data.GetValue("opis").ToString();
            DateTime datum = DateTime.Parse(data.GetValue("datumPostavljanja").ToString());
            string grad = data.GetValue("grad").ToString();
            string slika = data.GetValue("slika").ToString();
            string dostupan = data.GetValue("dostupan").ToString();
            Proizvod proizvod = new Proizvod(naziv, cena, kolicina, opis, slika, datum, grad, dostupan);
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            List<string> pom = file.DeSerializeObject<List<string>>("Naziv.xml");
            Globals.NazivProizvodaZaIzmenu = pom[0];

            for (int i = 0; i < x[0].ObjavljeniProizvodi.Count; i++)
            {
                if (x[0].ObjavljeniProizvodi[i].Naziv.Contains(Globals.NazivProizvodaZaIzmenu) && (x[0].ObjavljeniProizvodi[i].Dostupan.Contains("Da") || x[0].ObjavljeniProizvodi[i].Dostupan.Contains("da")))
                {
                    proizvod.Slika = SaveImage(proizvod.Slika);
                    if (proizvod.Dostupan.Contains("da") || proizvod.Dostupan.Contains("ne"))
                    {
                        proizvod.Dostupan = char.ToUpper(proizvod.Dostupan[0]) + proizvod.Dostupan.Substring(1);
                    }
                    x[0].ObjavljeniProizvodi[i] = proizvod;
                }
            }

            for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
            {
                if (Proizvodi.ListaProizvoda[i].Naziv.Contains(Globals.NazivProizvodaZaIzmenu) && (Proizvodi.ListaProizvoda[i].Dostupan.Contains("Da") || Proizvodi.ListaProizvoda[i].Dostupan.Contains("da")))
                {
                    Proizvodi.ListaProizvoda[i] = proizvod;
                }
            }

            file.SerializeObject<List<Korisnik>>(x, "Pomoc.xml");
            file.SerializeObject<List<Proizvod>>(Proizvodi.ListaProizvoda, "Proizvodi.xml");

            return Ok();
        }

        // Izmena proizvoda kod admin
        [HttpPut]
        [Route("api/korisnici/IzmenaProizvodaAdmin")]
        public IHttpActionResult IzmenaProizvodaAdmin(Proizvod p)
        {
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            List<string> pom = file.DeSerializeObject<List<string>>("Naziv.xml");
            Globals.NazivProizvodaZaIzmenu = pom[0];
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");

            for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
            {
                if (Proizvodi.ListaProizvoda[i].Naziv.Contains(Globals.NazivProizvodaZaIzmenu))
                {
                    p.Slika = SaveImage(p.Slika);
                    if (p.Dostupan.Contains("da") || p.Dostupan.Contains("ne"))
                    {
                        p.Dostupan = char.ToUpper(p.Dostupan[0]) + p.Dostupan.Substring(1);
                    }
                    Proizvodi.ListaProizvoda[i] = p;
                }
            }

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                for (int j = 0; j < Globals.Korisnici[i].ObjavljeniProizvodi.Count; j++)
                {
                    if (Globals.Korisnici[i].ObjavljeniProizvodi[j].Naziv.Contains(Globals.NazivProizvodaZaIzmenu))
                    {
                        p.Slika = SaveImage(p.Slika);
                        if (p.Dostupan.Contains("da") || p.Dostupan.Contains("ne"))
                        {
                            p.Dostupan = char.ToUpper(p.Dostupan[0]) + p.Dostupan.Substring(1);
                        }
                        Globals.Korisnici[i].ObjavljeniProizvodi[j] = p;
                    }
                }

                for (int j = 0; j < Globals.Korisnici[i].OmiljeniProizvodi.Count; j++)
                {
                    if (Globals.Korisnici[i].OmiljeniProizvodi[j].Naziv.Contains(Globals.NazivProizvodaZaIzmenu))
                    {
                        p.Slika = SaveImage(p.Slika);
                        if (p.Dostupan.Contains("da") || p.Dostupan.Contains("ne"))
                        {
                            p.Dostupan = char.ToUpper(p.Dostupan[0]) + p.Dostupan.Substring(1);
                        }
                        Globals.Korisnici[i].OmiljeniProizvodi[j] = p;
                    }
                }
            }

            file.SerializeObject<List<Proizvod>>(Proizvodi.ListaProizvoda, "Proizvodi.xml");
            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");

            return Ok();
        }

        // Prikaz poadataka o recenziji na stranici za izmenu kod kupca
        [HttpGet]
        [Route("api/korisnici/preuzmiRecenziju")]
        public IHttpActionResult PreuzmiRecenziju()
        {
            List<string> pom = file.DeSerializeObject<List<string>>("Naziv.xml");
            Globals.NazivProizvodaZaIzmenu = pom[0];
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            Globals.Prijavljen = x[0];

            Recenzija retV = null;

            foreach (Porudzbina p in Globals.Prijavljen.Porudzbine)
            {
                if (p.Proizvod.Naziv.Contains(Globals.NazivProizvodaZaIzmenu) && p.Proizvod.Recenzije.Count > 1)
                {
                    retV = p.Proizvod.Recenzije[p.Proizvod.Recenzije.Count - 1];
                }
            }

            var response = new
            {
                Data = retV,
                Message = "Uspešno!"
            };

            return Json(response);
        }

        // Izmena recenzije od strane kupca
        [HttpPut]
        [Route("api/korisnici/izmenaRecenzije")]
        public IHttpActionResult IzmenaRecenzije([FromBody] JObject data)
        {
            string naslov = data.GetValue("naslov").ToString();
            string opis = data.GetValue("opis").ToString();
            List<string> pom = file.DeSerializeObject<List<string>>("Naziv.xml");
            Globals.NazivProizvodaZaIzmenu = pom[0];
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            Globals.Prijavljen = x[0];
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");

            string slika = "";
            foreach (Korisnik k in Globals.Korisnici)
            {
                if (k.KorisnickoIme.Contains(Globals.Prijavljen.KorisnickoIme))
                {
                    foreach (Porudzbina p in k.Porudzbine)
                    {
                        if (p.Proizvod.Naziv.Contains(Globals.NazivProizvodaZaIzmenu))
                        {
                            slika = p.Proizvod.Slika;
                        }
                    }
                }
            }

            Recenzija r = new Recenzija(Globals.NazivProizvodaZaIzmenu, Globals.Prijavljen.KorisnickoIme, naslov, opis, slika);

            for (int i = 0; i < Globals.Prijavljen.Porudzbine.Count; i++)
            {
                Porudzbina p = Globals.Prijavljen.Porudzbine[i];
                if (p.Proizvod.Naziv.Contains(Globals.NazivProizvodaZaIzmenu))
                {
                    p.Proizvod.Recenzije[p.Proizvod.Recenzije.Count - 1] = r;
                }
            }

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                Korisnik k = Globals.Korisnici[i];
                if (k.KorisnickoIme.Contains(Globals.Prijavljen.KorisnickoIme))
                {
                    foreach (Porudzbina p in k.Porudzbine)
                    {
                        if (p.Proizvod.Naziv.Contains(Globals.NazivProizvodaZaIzmenu))
                        {
                            p.Proizvod.Recenzije[p.Proizvod.Recenzije.Count - 1] = r;
                        }
                    }
                }
            }

            file.SerializeObject<List<Korisnik>>(x, "Pomoc.xml");
            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");

            return Ok();
        }

        // Brisanje recenzije od strane kupca
        [HttpDelete]
        [Route("api/korisnici/brisanjeRecenzije")]
        public IHttpActionResult BrisanjeRecenzije([FromBody] JObject data)
        {
            string naziv = data.GetValue("naziv").ToString();
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            Globals.Prijavljen = x[0];
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");

            for (int i = 0; i < Globals.Prijavljen.Porudzbine.Count; i++)
            {
                Porudzbina p = Globals.Prijavljen.Porudzbine[i];
                if (p.Proizvod.Naziv.Contains(naziv))
                {
                    if (p.Proizvod.Recenzije.Count > 1)
                    {
                        p.Proizvod.Recenzije.RemoveRange(1, p.Proizvod.Recenzije.Count - 1);
                    }
                    else
                    {
                        return BadRequest("Nemate recenziju!!");
                    }
                }
            }

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                Korisnik k = Globals.Korisnici[i];
                if (k.KorisnickoIme.Contains(Globals.Prijavljen.KorisnickoIme))
                {
                    foreach (Porudzbina p in k.Porudzbine)
                    {
                        if (p.Proizvod.Naziv.Contains(naziv))
                        {
                            if (p.Proizvod.Recenzije.Count > 1)
                            {
                                p.Proizvod.Recenzije.RemoveRange(1, p.Proizvod.Recenzije.Count - 1);
                            }
                            else
                            {
                                return BadRequest("Nemate recenziju!!");
                            }
                        }
                    }
                }
            }

            file.SerializeObject<List<Korisnik>>(x, "Pomoc.xml");
            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");

            return Ok();
        }

        // Funkcija koja menja putanju slike
        private string SaveImage(string path)
        {
            if (path.Contains("Pictures\\"))
            {
                return path;
            }
            else
            {
                string[] s = path.Split('\\');
                //base64Image.Replace("C:\\fakepath\\", filePath);
                return "Pictures\\" + s[2];
            }
        }

        // Odjava sa profila
        [HttpPost]
        [Route("api/korisnici/odjava")]
        public IHttpActionResult Odjava()
        {
            string filePathP = GetProjectLocation() + "bin\\Debug\\Pomoc.xml";
            string filePathN = GetProjectLocation() + "bin\\Debug\\Naziv.xml";
            try
            {
                File.Delete(filePathP);
                File.Delete(filePathN);
            }
            catch (IOException exp)
            {
                return BadRequest($"Došlo je do greške -> {exp} !!");
            }
            return Ok();
        }

        // Funkcija koja vraća putanju do projekta
        private string GetProjectLocation()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return baseDirectory;
        }
    }
}