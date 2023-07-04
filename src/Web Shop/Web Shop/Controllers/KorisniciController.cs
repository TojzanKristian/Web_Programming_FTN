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

        // Funkcija za prikaz korisnika adminu
        public IHttpActionResult Get()
        {
            return Json(file.DeSerializeObject<List<Korisnik>>("Korisnici.xml"));
        }

        // Funkcija za pretragu proizvoda od strane admina
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

        // Funkcija za sortiranje proizvoda od strane admina
        [HttpGet]
        [Route("api/korisnici/admin/proizvodi/sort")]
        public IHttpActionResult SortiranjeProizvoda(string nazivSort, string cenaSort, string datumPSort)
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

        // Funkcija za pretragu korisnika od strane admina
        [HttpGet]
        [Route("api/korisnici/admin/search")]
        public IHttpActionResult PretragaKodAdmina(string ime, string prezime, string datumR, string datumR1, string uloge)
        {
            List<Korisnik> result = LogikaPretrage(ime, prezime, datumR, datumR1, uloge);
            return Json(result);
        }
        private List<Korisnik> LogikaPretrage(string ime, string prezime, string datumR, string datumR1, string uloge)
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
                    DateTime datum1 = DateTime.Parse(datumR1);
                    if (k.DatumRodjenja >= datum && k.DatumRodjenja <= datum1)
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

        // Funkcija za sortiranje korisnika od strane admina
        [HttpGet]
        [Route("api/korisnici/admin/sort")]
        public IHttpActionResult SortiranjeKodAdmina(string imeSort, string datumSort, string ulogeSort)
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

        // Funkcija koja služi za prijavu
        public IHttpActionResult Get(string korisnickoIme, string lozinka)
        {
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            int brojac = 0;
            foreach (Korisnik pom in Globals.Korisnici)
            {
                if (pom.KorisnickoIme.Equals(korisnickoIme) && pom.Lozinka.Equals(lozinka))
                {
                    Globals.Prijavljen = pom;
                    brojac++;
                }
            }

            List<Korisnik> admini = file.DeSerializeObject<List<Korisnik>>("Administratori.xml");
            foreach (Korisnik pom in admini)
            {
                if (pom.KorisnickoIme.Equals(korisnickoIme) && pom.Lozinka.Equals(lozinka))
                {
                    Globals.Prijavljen = pom;
                    brojac++;
                }
            }

            if (brojac == 0)
            {
                return BadRequest($"Korisnik sa korisničkim imenom {korisnickoIme} ne postoji ! Pokušajte ponovo...");
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

        // Funkcija za prikaz porudžbina kod kupca
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
                if (pom.KorisnickoIme.Equals(Globals.Prijavljen.KorisnickoIme))
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

        // Funkcija za prikaz svih porudžbina u sistemu za admina
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

        // Funkcija za prikaz omiljenih proizvoda kod kupca
        [HttpGet]
        [Route("api/korisnici/omiljeni")]
        public IHttpActionResult GetOmiljeni()
        {
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            Globals.Prijavljen = x[0];
            List<Proizvod> omiljeni = new List<Proizvod>();

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                Korisnik pom = Globals.Korisnici[i];
                if (pom.KorisnickoIme.Equals(Globals.Prijavljen.KorisnickoIme))
                {
                    omiljeni = pom.OmiljeniProizvodi;
                }
            }

            var response = new
            {
                Data = omiljeni,
                Message = "Uspešno!"
            };
            return Json(response);
        }

        // Funkcija za dodavanje proizvoda u omiljene kod kupca
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
                if (Proizvodi.ListaProizvoda[i].Naziv.Equals(naziv))
                {
                    pom = Proizvodi.ListaProizvoda[i];
                }
            }

            if (pom != null)
            {
                for (int i = 0; i < Globals.Korisnici.Count; i++)
                {
                    Korisnik k = Globals.Korisnici[i];
                    if (k.KorisnickoIme.Equals(Globals.Prijavljen.KorisnickoIme))
                    {
                        x[0].OmiljeniProizvodi.Add(pom);
                        k.OmiljeniProizvodi.Add(pom);
                    }
                }
            }

            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");
            file.SerializeObject<List<Korisnik>>(x, "Pomoc.xml");
            return Ok();
        }

        // Funkcija za poručivanje proizvoda kod kupca
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
                if (p.Naziv.Equals(naziv) && p.Kolicina > 0 && p.Kolicina >= kolicina && p.Dostupan.Contains("Da") && kolicina == 1)
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

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                if (Globals.Korisnici[i].KorisnickoIme.Equals(Globals.Prijavljen.KorisnickoIme))
                {
                    for (int j = 0; j < Globals.Korisnici[i].OmiljeniProizvodi.Count; j++)
                    {
                        if (Globals.Korisnici[i].OmiljeniProizvodi[j].Naziv.Equals(naziv))
                        {
                            Globals.Korisnici[i].OmiljeniProizvodi[j].Kolicina -= kolicina;
                            if (Globals.Korisnici[i].OmiljeniProizvodi[j].Kolicina == 0)
                            {
                                Globals.Korisnici[i].OmiljeniProizvodi[j].Dostupan = "Ne";
                            }
                        }
                    }
                }
            }

            for (int j = 0; j < Globals.Prijavljen.OmiljeniProizvodi.Count; j++)
            {
                if (Globals.Prijavljen.OmiljeniProizvodi[j].Naziv.Equals(naziv))
                {
                    Globals.Prijavljen.OmiljeniProizvodi[j].Kolicina -= kolicina;
                    if (Globals.Prijavljen.OmiljeniProizvodi[j].Kolicina == 0)
                    {
                        Globals.Prijavljen.OmiljeniProizvodi[j].Dostupan = "Ne";
                    }
                }
            }

            for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
            {
                if (Proizvodi.ListaProizvoda[i].Naziv.Equals(naziv))
                {
                    Proizvodi.ListaProizvoda[i].Kolicina -= kolicina;
                }

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

        // Funkcija za prikaz objavljenih proizvoda kod prodavca
        [HttpGet]
        [Route("api/korisnici/objavljeni")]
        public List<Proizvod> Objavljeni()
        {
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            return x[0].ObjavljeniProizvodi;
        }

        // Funkcija koja obavlja registraciju na sistem
        public IHttpActionResult Post(Korisnik k)
        {
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");

            foreach (Korisnik pom in Globals.Korisnici)
            {
                if (pom.KorisnickoIme.Equals(k.KorisnickoIme))
                {
                    return BadRequest($"Korisnik sa korisničkim imenom {k.KorisnickoIme} već postoji !");
                }
            }

            Globals.Korisnici.Add(k);
            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");
            return Ok();
        }

        // Funkcija koja dodaje prodavca kod admina
        [HttpPost]
        [Route("api/korisnici/dodatProdavac")]
        public IHttpActionResult DodatProdavac(Korisnik korisnik)
        {
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            foreach (Korisnik pom in Globals.Korisnici)
            {
                if (pom.KorisnickoIme.Equals(korisnik.KorisnickoIme))
                {
                    return BadRequest($"Korisnik sa korisničkim imenom {korisnik.KorisnickoIme} već postoji !");
                }
            }

            Globals.Korisnici.Add(korisnik);
            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");
            return Json(Globals.Korisnici);
        }


        // Funkcija za prikaz profila svim korisnicima
        [HttpGet]
        [Route("api/korisnici/preuzmi")]
        public IHttpActionResult GetKorisnik()
        {
            string filePath = PutanjaDoProjekta() + "bin\\Debug\\Pomoc.xml";
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

        // Funkcija za izmenu korisnika kod admina
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

        // Funkcija za izmenu profila kod svih korisnika
        public IHttpActionResult Put(Korisnik k)
        {
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            Globals.Prijavljen = x[0];
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
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

            string korisnickoIme = k.KorisnickoIme;
            bool isDuplicate = Globals.Korisnici.Count(kr => kr.KorisnickoIme.Equals(korisnickoIme)) > 1;

            if (isDuplicate)
            {
                return BadRequest("Korisničko ime koje ste uneli već postoji !!");
            }

            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");
            file.SerializeObject<List<Korisnik>>(x, "Pomoc.xml");
            return Ok();

        }

        // Funkcija za izmenu korisnika kod admina
        [HttpPut]
        [Route("api/korisnici/izmenaKodAdmina")]
        public IHttpActionResult IzmenaKorisnika(Korisnik k)
        {
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
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

            string korisnickoIme = k.KorisnickoIme;
            bool isDuplicate = Globals.Korisnici.Count(kr => kr.KorisnickoIme.Equals(korisnickoIme)) > 1;

            if (isDuplicate)
            {
                return BadRequest("Korisničko ime koje ste uneli već postoji !!");
            }

            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");
            return Ok();
        }

        // Funkcija za pretragu kod prodavca
        [HttpGet]
        [Route("api/korisnici/search")]
        public IHttpActionResult PretragaKorisnikaAdmin(string dostupnost)
        {
            List<Proizvod> result = PretragaLogika(dostupnost);
            return Json(result);
        }
        private List<Proizvod> PretragaLogika(string dostupnost)
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

        // Funkcija za sortiranje kod prodavca
        [HttpGet]
        [Route("api/korisnici/sort")]
        public IHttpActionResult SortiranjeKorisnikaAdmin(string sortN, string sortC, string sortD)
        {
            List<Proizvod> result = SortiranjeLogika(sortN, sortC, sortD);
            return Json(result);
        }
        private List<Proizvod> SortiranjeLogika(string sortN, string sortC, string sortD)
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

        // Funkcija za brisanje proizvoda od strane prodavca
        [HttpDelete]
        [Route("api/korisnici/brisanje")]
        public IHttpActionResult Brisanje([FromBody] JObject data)
        {
            string naziv = data.GetValue("naziv").ToString();
            int br = 0;

            for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
            {
                if (Proizvodi.ListaProizvoda[i].Naziv.Equals(naziv) && (Proizvodi.ListaProizvoda[i].Dostupan.Contains("Da") || Proizvodi.ListaProizvoda[i].Dostupan.Contains("da")))
                {
                    Proizvodi.ListaProizvoda.Remove(Proizvodi.ListaProizvoda[i]);
                    br++;
                }
            }

            file.SerializeObject<List<Proizvod>>(Proizvodi.ListaProizvoda, "Proizvodi.xml");
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");

            for (int i = 0; i < x[0].ObjavljeniProizvodi.Count; i++)
            {
                if (x[0].ObjavljeniProizvodi[i].Naziv.Equals(naziv) && (x[0].ObjavljeniProizvodi[i].Dostupan.Contains("Da") || x[0].ObjavljeniProizvodi[i].Dostupan.Contains("da")))
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

        // Funkcija za brisanje proizvoda od strane admina
        [HttpDelete]
        [Route("api/korisnici/brisanjeProizvodaAdmin")]
        public IHttpActionResult BrisanjeProizvodaAdmin([FromBody] JObject data)
        {
            string naziv = data.GetValue("naziv").ToString();
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            int br = 0;

            for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
            {
                if (Proizvodi.ListaProizvoda[i].Naziv.Equals(naziv))
                {
                    Proizvodi.ListaProizvoda.Remove(Proizvodi.ListaProizvoda[i]);
                    br++;
                }
            }

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                for (int j = 0; j < Globals.Korisnici[i].ObjavljeniProizvodi.Count; j++)
                {
                    if (Globals.Korisnici[i].ObjavljeniProizvodi[j].Naziv.Equals(naziv))
                    {
                        Globals.Korisnici[i].ObjavljeniProizvodi.Remove(Globals.Korisnici[i].ObjavljeniProizvodi[j]);
                    }
                }

                //for (int j = 0; j < Globals.Korisnici[i].OmiljeniProizvodi.Count; j++)
                //{
                //    if (Globals.Korisnici[i].OmiljeniProizvodi[j].Naziv.Contains(naziv))
                //    {
                //        Globals.Korisnici[i].OmiljeniProizvodi.Remove(Globals.Korisnici[i].OmiljeniProizvodi[j]);
                //    }
                //}
            }

            file.SerializeObject<List<Proizvod>>(Proizvodi.ListaProizvoda, "Proizvodi.xml");
            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");

            if (br == 0)
            {
                return BadRequest("Brisanje nije moguće !!");
            }
            return Ok();
        }

        // Funkcija za brisanje korisnika od strane admina
        [HttpDelete]
        [Route("api/korisnici/brisanjeKorisnikaAdmin")]
        public IHttpActionResult BrisanjeKorisnika([FromBody] JObject data)
        {
            string ime = data.GetValue("naziv").ToString();
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            List<Proizvod> temp = new List<Proizvod>();

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                if (Globals.Korisnici[i].Ime.Equals(ime) && Globals.Korisnici[i].Uloga == Uloge.Prodavac)
                {
                    temp = Globals.Korisnici[i].ObjavljeniProizvodi;
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

            for (int k = 0; k < temp.Count; k++)
            {
                for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
                {
                    if (Proizvodi.ListaProizvoda[i].Naziv.Equals(temp[k].Naziv))
                    {
                        Proizvodi.ListaProizvoda.Remove(Proizvodi.ListaProizvoda[i]);
                    }
                }
            }

            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");
            file.SerializeObject<List<Proizvod>>(Proizvodi.ListaProizvoda, "Proizvodi.xml");
            return Ok();
        }

        // Funkcija koja služi kao priprema za neku izmenu
        [HttpPost]
        [Route("api/korisnici/pripremaZaIzmenu")]
        public IHttpActionResult PripremaIzmene([FromBody] JObject data)
        {
            List<string> naziv = new List<string> { data.GetValue("naziv").ToString() };
            file.SerializeObject<List<string>>(naziv, "Naziv.xml");
            return Ok();
        }

        // Funkcija za preuzimanje podataka o proizvodu koji se menja kod prodavca
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
                if (x[0].ObjavljeniProizvodi[i].Naziv.Equals(Globals.NazivProizvodaZaIzmenu))
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

        // Funkcija za preuzimanje podataka o proizvodu koji se menja kod admina
        [HttpGet]
        [Route("api/korisnici/preuzmiIzmenuAdmin")]
        public IHttpActionResult IzmenaAdmin()
        {
            Proizvod temp = null;
            List<string> pom = file.DeSerializeObject<List<string>>("Naziv.xml");
            Globals.NazivProizvodaZaIzmenu = pom[0];

            for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
            {
                if (Proizvodi.ListaProizvoda[i].Naziv.Equals(Globals.NazivProizvodaZaIzmenu))
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

        // Funkcija za izmenu status porudžbine od strane kupca
        [HttpPut]
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
                if (k.KorisnickoIme.Equals(Globals.Prijavljen.KorisnickoIme))
                {
                    for (int j = 0; j < k.Porudzbine.Count; j++)
                    {
                        if (k.Porudzbine[j].Status == Statusi.OTKAZAN)
                        {
                            return BadRequest("Nemate pravo da izmenite status !!");
                        }
                        if (k.Porudzbine[j].Status == Statusi.AKTIVNA && k.Porudzbine[j].Proizvod.Naziv.Equals(naziv))
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

        // Funkcija za izmenu status porudžbine od strane admina na izvršeno
        [HttpPut]
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
                    if (k.Porudzbine[j].Proizvod.Naziv.Equals(naziv))
                    {
                        Globals.Korisnici[i].Porudzbine[j].Status = Statusi.IZVRŠEN;
                    }
                }
            }

            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");
            return Ok();
        }

        // Funkcija za izmenu status porudžbine od strane admina na otkazan
        [HttpPut]
        [Route("api/korisnici/statusOtkazan")]
        public IHttpActionResult StatusOtkazan([FromBody] JObject data)
        {
            string naziv = data.GetValue("naziv").ToString();
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            int brojac = 0;
            bool status = false;
            Porudzbina pomoc = new Porudzbina();

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                Korisnik k = Globals.Korisnici[i];
                for (int j = 0; j < k.Porudzbine.Count; j++)
                {
                    if (k.Porudzbine[j].Proizvod.Naziv.Equals(naziv) && (k.Porudzbine[j].Status == Statusi.IZVRŠEN || k.Porudzbine[j].Status == Statusi.OTKAZAN))
                    {
                        return BadRequest("Status je Izvršen ili Otkazan, nemate pravo da izmenite status !!");
                    }

                    if (k.Porudzbine[j].Proizvod.Naziv.Equals(naziv) && k.Porudzbine[j].Status == Statusi.AKTIVNA)
                    {
                        Globals.Korisnici[i].Porudzbine[j].Status = Statusi.OTKAZAN;
                        pomoc = Globals.Korisnici[i].Porudzbine[j];
                        status = true;
                        brojac++;
                    }
                }
            }

            int br = 0;
            for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
            {
                if (Proizvodi.ListaProizvoda[i].Naziv.Equals(naziv) && status == true)
                {
                    Proizvodi.ListaProizvoda[i].Kolicina += brojac;
                    br++;
                }
            }

            if (br == 0)
            {
                pomoc.Proizvod.Kolicina = brojac;
                if (pomoc.Proizvod.Kolicina > 0)
                    pomoc.Proizvod.Dostupan = "Da";
                Proizvodi.ListaProizvoda.Add(pomoc.Proizvod);
            }

            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");
            file.SerializeObject<List<Proizvod>>(Proizvodi.ListaProizvoda, "Proizvodi.xml");
            return Ok();
        }

        // Funkcija za dodavanje recenzije od strane kupca
        [HttpPost]
        [Route("api/korisnici/dodajRecenziju")]
        public IHttpActionResult DodajRecenziju([FromBody] JObject data)
        {
            string naziv = data.GetValue("naziv").ToString();
            string naslov = data.GetValue("naslov").ToString();
            string opis = data.GetValue("opis").ToString();
            string slika = data.GetValue("slika").ToString();
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            Globals.Prijavljen = x[0];
            Recenzija recenzija = null;

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                Korisnik k = Globals.Korisnici[i];
                if (k.KorisnickoIme.Equals(Globals.Prijavljen.KorisnickoIme))
                {
                    for (int j = 0; j < k.Porudzbine.Count; j++)
                    {
                        if (k.Porudzbine[j].Proizvod.Naziv.Contains(naziv) && k.Porudzbine[j].Status != Statusi.IZVRŠEN)
                        {
                            return BadRequest("Nemate pravo da dodate recenziju !!");
                        }
                        if (k.Porudzbine[j].Status == Statusi.IZVRŠEN && k.Porudzbine[j].Proizvod.Naziv.Contains(naziv))
                        {
                            if (k.Porudzbine[j].Proizvod.Recenzije.Count >= 2)
                            {
                                return BadRequest("Već imate recenziju !!");
                            }
                            if (slika == "")
                            {
                                recenzija = new Recenzija(Globals.Prijavljen.Porudzbine[j].Proizvod.Naziv, Globals.Prijavljen.KorisnickoIme, naslov, opis, Globals.Prijavljen.Porudzbine[j].Proizvod.Slika);
                            }
                            else
                            {
                                string novaSlika = PromenaPutanjeSlike(slika);
                                recenzija = new Recenzija(Globals.Prijavljen.Porudzbine[j].Proizvod.Naziv, Globals.Prijavljen.KorisnickoIme, naslov, opis, novaSlika);
                            }
                            Globals.Prijavljen.Porudzbine[j].Proizvod.Recenzije.Add(recenzija);
                            Globals.Korisnici[i].Porudzbine[j].Proizvod.Recenzije.Add(recenzija);
                        }
                    }
                }
            }

            for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
            {
                if (Proizvodi.ListaProizvoda[i].Naziv.Contains(naziv) && recenzija != null)
                {
                    Proizvodi.ListaProizvoda[i].Recenzije.Add(recenzija);
                }
            }

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                Korisnik k = Globals.Korisnici[i];
                if (k.KorisnickoIme.Equals(Globals.Prijavljen.KorisnickoIme))
                {
                    for (int j = 0; j < k.ObjavljeniProizvodi.Count; j++)
                    {
                        if (k.ObjavljeniProizvodi[j].Naziv.Contains(naziv) && recenzija != null)
                        {
                            k.ObjavljeniProizvodi[j].Recenzije.Add(recenzija);
                        }
                    }
                }
            }

            file.SerializeObject<List<Korisnik>>(x, "Pomoc.xml");
            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");
            file.SerializeObject<List<Proizvod>>(Proizvodi.ListaProizvoda, "Proizvodi.xml");
            return Ok();
        }

        // Funkcija za izmenu proizvoda kod prodavca
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
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            Globals.NazivProizvodaZaIzmenu = pom[0];

            for (int i = 0; i < x[0].ObjavljeniProizvodi.Count; i++)
            {
                if (x[0].ObjavljeniProizvodi[i].Naziv.Equals(Globals.NazivProizvodaZaIzmenu) && (x[0].ObjavljeniProizvodi[i].Dostupan.Contains("Da") || x[0].ObjavljeniProizvodi[i].Dostupan.Contains("da")))
                {
                    if (proizvod.Slika != "")
                    {
                        proizvod.Slika = PromenaPutanjeSlike(proizvod.Slika);
                    }
                    else
                    {
                        proizvod.Slika = x[0].ObjavljeniProizvodi[i].Slika;
                    }

                    if (proizvod.Dostupan.Equals("da") || proizvod.Dostupan.Equals("ne"))
                    {
                        proizvod.Dostupan = char.ToUpper(proizvod.Dostupan[0]) + proizvod.Dostupan.Substring(1);
                    }
                    proizvod.Recenzije = x[0].ObjavljeniProizvodi[i].Recenzije;
                    x[0].ObjavljeniProizvodi[i] = proizvod;
                }
            }

            for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
            {
                if (Proizvodi.ListaProizvoda[i].Naziv.Equals(Globals.NazivProizvodaZaIzmenu) && (Proizvodi.ListaProizvoda[i].Dostupan.Contains("Da") || Proizvodi.ListaProizvoda[i].Dostupan.Contains("da")))
                {
                    Proizvodi.ListaProizvoda[i] = proizvod;
                }
            }

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                Korisnik k = Globals.Korisnici[i];

                if (k.KorisnickoIme.Equals(x[0].KorisnickoIme))
                {
                    for (int j = 0; j < k.ObjavljeniProizvodi.Count; j++)
                    {
                        if (k.ObjavljeniProizvodi[j].Naziv.Equals(Globals.NazivProizvodaZaIzmenu) && (k.ObjavljeniProizvodi[j].Dostupan.Contains("Da") || k.ObjavljeniProizvodi[j].Dostupan.Contains("da")))
                        {
                            k.ObjavljeniProizvodi[j] = proizvod;
                        }
                    }
                }
            }

            for (int i = 0; i < x[0].ObjavljeniProizvodi.Count; i++)
            {
                if (x[0].ObjavljeniProizvodi[i].Naziv.Equals(Globals.NazivProizvodaZaIzmenu) && (x[0].ObjavljeniProizvodi[i].Dostupan.Contains("Ne") || x[0].ObjavljeniProizvodi[i].Dostupan.Contains("ne")))
                {
                    return BadRequest("Proizvod nije dostupan!! Izmena nije moguća!!");
                }
            }

            file.SerializeObject<List<Korisnik>>(x, "Pomoc.xml");
            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");
            file.SerializeObject<List<Proizvod>>(Proizvodi.ListaProizvoda, "Proizvodi.xml");
            return Ok();
        }

        // Funkcija za izmenu proizvoda kod admin
        [HttpPut]
        [Route("api/korisnici/IzmenaProizvodaAdmin")]
        public IHttpActionResult IzmenaProizvodaAdmin(Proizvod p)
        {
            List<string> pom = file.DeSerializeObject<List<string>>("Naziv.xml");
            Globals.NazivProizvodaZaIzmenu = pom[0];
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");
            p.Recenzije = new List<Recenzija>();

            for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
            {
                if (Proizvodi.ListaProizvoda[i].Naziv.Equals(Globals.NazivProizvodaZaIzmenu))
                {
                    if (p.Slika != "")
                    {
                        p.Slika = PromenaPutanjeSlike(p.Slika);
                    }
                    else
                    {
                        p.Slika = Proizvodi.ListaProizvoda[i].Slika;
                    }
                    if (p.Dostupan.Contains("da") || p.Dostupan.Contains("ne"))
                    {
                        p.Dostupan = char.ToUpper(p.Dostupan[0]) + p.Dostupan.Substring(1);
                    }
                    p.Recenzije.Add(new Recenzija("", "", "", "Nema recenzije!", ""));
                    Proizvodi.ListaProizvoda[i] = p;
                }
            }

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                for (int j = 0; j < Globals.Korisnici[i].ObjavljeniProizvodi.Count; j++)
                {
                    if (Globals.Korisnici[i].ObjavljeniProizvodi[j].Naziv.Equals(Globals.NazivProizvodaZaIzmenu))
                    {
                        if (p.Slika != "")
                        {
                            p.Slika = PromenaPutanjeSlike(p.Slika);
                        }
                        else
                        {
                            p.Slika = Globals.Korisnici[i].ObjavljeniProizvodi[j].Slika;
                        }

                        if (p.Dostupan.Contains("da") || p.Dostupan.Contains("ne"))
                        {
                            p.Dostupan = char.ToUpper(p.Dostupan[0]) + p.Dostupan.Substring(1);
                        }
                        p.Recenzije.Add(new Recenzija("", "", "", "Nema recenzije!", ""));
                        Globals.Korisnici[i].ObjavljeniProizvodi[j] = p;
                    }
                }

                for (int j = 0; j < Globals.Korisnici[i].OmiljeniProizvodi.Count; j++)
                {
                    if (Globals.Korisnici[i].OmiljeniProizvodi[j].Naziv.Equals(Globals.NazivProizvodaZaIzmenu))
                    {
                        if (p.Slika != "")
                        {
                            p.Slika = PromenaPutanjeSlike(p.Slika);
                        }
                        else
                        {
                            p.Slika = Globals.Korisnici[i].OmiljeniProizvodi[j].Slika;
                        }
                        if (p.Dostupan.Contains("da") || p.Dostupan.Contains("ne"))
                        {
                            p.Dostupan = char.ToUpper(p.Dostupan[0]) + p.Dostupan.Substring(1);
                        }
                        p.Recenzije.Add(new Recenzija("", "", "", "Nema recenzije!", ""));
                        Globals.Korisnici[i].OmiljeniProizvodi[j] = p;
                    }
                }
            }

            file.SerializeObject<List<Proizvod>>(Proizvodi.ListaProizvoda, "Proizvodi.xml");
            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");
            return Ok();
        }

        // Funkcija za prikaz poadataka o recenziji na stranici za izmenu kod kupca
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
                if (p.Proizvod.Naziv.Equals(Globals.NazivProizvodaZaIzmenu) && p.Proizvod.Recenzije.Count > 1)
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

        // Funkcija za izmenu recenzije kod kupca
        [HttpPut]
        [Route("api/korisnici/izmenaRecenzije")]
        public IHttpActionResult IzmenaRecenzije([FromBody] JObject data)
        {
            string naslov = data.GetValue("naslov").ToString();
            string opis = data.GetValue("opis").ToString();
            string slika = data.GetValue("slika").ToString();
            List<string> pom = file.DeSerializeObject<List<string>>("Naziv.xml");
            Globals.NazivProizvodaZaIzmenu = pom[0];
            List<Korisnik> x = file.DeSerializeObject<List<Korisnik>>("Pomoc.xml");
            Globals.Prijavljen = x[0];
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");

            foreach (Korisnik k in Globals.Korisnici)
            {
                if (k.KorisnickoIme.Equals(Globals.Prijavljen.KorisnickoIme))
                {
                    foreach (Porudzbina p in k.Porudzbine)
                    {
                        if (p.Proizvod.Naziv.Equals(Globals.NazivProizvodaZaIzmenu))
                        {
                            if (slika == "")
                            {
                                slika = p.Proizvod.Slika;
                            }
                            else
                            {
                                slika = PromenaPutanjeSlike(slika);
                            }
                        }
                    }
                }
            }

            Recenzija r = new Recenzija(Globals.NazivProizvodaZaIzmenu, Globals.Prijavljen.KorisnickoIme, naslov, opis, slika);

            for (int i = 0; i < Globals.Prijavljen.Porudzbine.Count; i++)
            {
                Porudzbina p = Globals.Prijavljen.Porudzbine[i];
                if (p.Proizvod.Naziv.Equals(Globals.NazivProizvodaZaIzmenu))
                {
                    p.Proizvod.Recenzije[p.Proizvod.Recenzije.Count - 1] = r;
                }
            }

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                Korisnik k = Globals.Korisnici[i];
                if (k.KorisnickoIme.Equals(Globals.Prijavljen.KorisnickoIme))
                {
                    foreach (Porudzbina p in k.Porudzbine)
                    {
                        if (p.Proizvod.Naziv.Equals(Globals.NazivProizvodaZaIzmenu))
                        {
                            p.Proizvod.Recenzije[p.Proizvod.Recenzije.Count - 1] = r;
                        }
                    }
                }
            }

            for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
            {
                if (Proizvodi.ListaProizvoda[i].Naziv.Equals(Globals.NazivProizvodaZaIzmenu))
                {
                    Proizvodi.ListaProizvoda[i].Recenzije[Proizvodi.ListaProizvoda[i].Recenzije.Count - 1] = r;
                }
            }

            file.SerializeObject<List<Korisnik>>(x, "Pomoc.xml");
            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");
            file.SerializeObject<List<Proizvod>>(Proizvodi.ListaProizvoda, "Proizvodi.xml");
            return Ok();
        }

        // Funkcija za brisanje recenzije od strane kupca
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
                if (p.Proizvod.Naziv.Equals(naziv))
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
                if (k.KorisnickoIme.Equals(Globals.Prijavljen.KorisnickoIme))
                {
                    foreach (Porudzbina p in k.Porudzbine)
                    {
                        if (p.Proizvod.Naziv.Equals(naziv))
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
        private string PromenaPutanjeSlike(string path)
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

        // Funkcija koja menja status recenzije kod admina
        [HttpPut]
        [Route("api/korisnici/odobriRecenziju")]
        public IHttpActionResult OdobriRecenziju([FromBody] JObject data)
        {
            string naziv = data.GetValue("naziv").ToString();
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                Korisnik k = Globals.Korisnici[i];
                for (int j = 0; j < k.ObjavljeniProizvodi.Count; j++)
                {
                    if (k.ObjavljeniProizvodi[j].Naziv.Equals(naziv))
                    {
                        for (int l = 0; l < k.ObjavljeniProizvodi[j].Recenzije.Count; l++)
                        {
                            k.ObjavljeniProizvodi[j].Recenzije[l].Prikazi = true;
                        }
                    }
                }
            }

            for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
            {
                if (Proizvodi.ListaProizvoda[i].Naziv.Equals(naziv))
                {
                    for (int j = 0; j < Proizvodi.ListaProizvoda[i].Recenzije.Count; j++)
                    {
                        Proizvodi.ListaProizvoda[i].Recenzije[j].Prikazi = true;
                    }
                }
            }

            file.SerializeObject<List<Proizvod>>(Proizvodi.ListaProizvoda, "Proizvodi.xml");
            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");
            return Ok();
        }

        // Funkcija koja menja status recenzije kod admina
        [HttpPut]
        [Route("api/korisnici/odbijRecenziju")]
        public IHttpActionResult OdbijRecenziju([FromBody] JObject data)
        {
            string naziv = data.GetValue("naziv").ToString();
            Globals.Korisnici = file.DeSerializeObject<List<Korisnik>>("Korisnici.xml");

            for (int i = 0; i < Globals.Korisnici.Count; i++)
            {
                Korisnik k = Globals.Korisnici[i];
                for (int j = 0; j < k.ObjavljeniProizvodi.Count; j++)
                {
                    if (k.ObjavljeniProizvodi[j].Naziv.Equals(naziv))
                    {
                        for (int l = 0; l < k.ObjavljeniProizvodi[j].Recenzije.Count; l++)
                        {
                            k.ObjavljeniProizvodi[j].Recenzije[l].SamoAdmin = true;
                        }
                    }
                }
            }

            for (int i = 0; i < Proizvodi.ListaProizvoda.Count; i++)
            {
                if (Proizvodi.ListaProizvoda[i].Naziv.Equals(naziv))
                {
                    for (int j = 0; j < Proizvodi.ListaProizvoda[i].Recenzije.Count; j++)
                    {
                        Proizvodi.ListaProizvoda[i].Recenzije[j].SamoAdmin = true;
                    }
                }
            }

            file.SerializeObject<List<Proizvod>>(Proizvodi.ListaProizvoda, "Proizvodi.xml");
            file.SerializeObject<List<Korisnik>>(Globals.Korisnici, "Korisnici.xml");
            return Ok();
        }

        // Funkcija koja služi za odjavu sa profila
        [HttpPost]
        [Route("api/korisnici/odjava")]
        public IHttpActionResult Odjava()
        {
            string filePathP = PutanjaDoProjekta() + "bin\\Debug\\Pomoc.xml";
            string filePathN = PutanjaDoProjekta() + "bin\\Debug\\Naziv.xml";
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
        private string PutanjaDoProjekta()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return baseDirectory;
        }
    }
}