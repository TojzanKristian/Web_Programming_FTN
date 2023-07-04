using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web_Shop.Models;

namespace Web_Shop
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //#region Podaci za rad aplikacije
            //XML xmlFile = new XML();

            //string p1 = "Pictures\\Capriolo.jpg";
            //string p2 = "Pictures\\Dyson-v11.jpg";
            //string p3 = "Pictures\\Iphone5.jpg";
            //string p4 = "Pictures\\Lenovo-IdeaPad-5.jpg";
            //string p5 = "Pictures\\Miki.jpg";
            //string p6 = "Pictures\\Nozevi-set.jpg";
            //string p7 = "Pictures\\Samsung_tv.jpg";
            //string p8 = "Pictures\\Tastatura.jpg";

            //Recenzija recenzija = new Recenzija("", "", "", "Nema recenzije!", "");

            //List<Proizvod> Proizvodi = new List<Proizvod>
            //{
            //new Proizvod() { Naziv = "SAMSUNG TV", Cena = 150000, Kolicina = 1, Opis = "Skoro nov, minimalno korišćen.", Slika = p7, DatumPostavljanja = new DateTime(2021, 06, 13, 0, 0, 0), Grad = "Vrbas", Dostupan = "Da", Recenzije = new List<Recenzija> { recenzija } },
            //new Proizvod() { Naziv = "Dyson usisivač", Cena = 72000, Kolicina = 3, Opis = "Polovan, korišćen 2 godine, ali se za to vreme nije pokvario.", Slika = p2, DatumPostavljanja = new DateTime(2022, 10, 02, 0, 0, 0), Grad = "Niš", Dostupan = "Da", Recenzije = new List<Recenzija> { recenzija } },
            //new Proizvod() { Naziv = "Noževi", Cena = 1200, Kolicina = 5, Opis = "Cena celog seta je 5000din. Cena po komadu 1200din. U odličnom stanju su, naoštreni.", Slika = p6, DatumPostavljanja = new DateTime(2020, 05, 09, 0, 0, 0), Grad = "Sombor", Dostupan = "Da", Recenzije = new List<Recenzija> { recenzija } },
            //new Proizvod() { Naziv = "Lenovo laptop", Cena = 80000, Kolicina = 1, Opis = "Polovan. Cenu je moguće minimalno smanjiti po dogovoru.", Slika = p4, DatumPostavljanja = new DateTime(2022, 01, 19, 0, 0, 0), Grad = "Novi Sad", Dostupan = "Da", Recenzije = new List<Recenzija> { recenzija } },
            //new Proizvod() { Naziv = "Caprilolo bicikli", Cena = 12000, Kolicina = 1, Opis = "Nov. Kupljen pre 3meseca postavljanja objave, korišćen samo jednom. Moguća zamena.", Slika = p1, DatumPostavljanja = new DateTime(2021, 05, 22, 0, 0, 0), Grad = "Apatin", Dostupan = "Da", Recenzije = new List<Recenzija> { recenzija } },
            //new Proizvod() { Naziv = "Iphone 5", Cena = 52000, Kolicina = 1, Opis = "Polovan. Moguća zamena za drugi telefon ili dogovor o menjanju za nešto drugo.", Slika = p3, DatumPostavljanja = new DateTime(2022, 07, 07, 0, 0, 0), Grad = "Subotica", Dostupan = "Da", Recenzije = new List<Recenzija> { recenzija } },
            //new Proizvod() { Naziv = "Tastatura", Cena = 2850, Kolicina = 1, Opis = "Marka : Teclado Gamer Hyperx Alloy Origins Core Aqua Preto. Polovan, ali u dobrom stanju.", Slika = p8, DatumPostavljanja = new DateTime(2021, 10, 17, 0, 0, 0), Grad = "Jagodina", Dostupan = "Da", Recenzije = new List<Recenzija> { recenzija } },
            //new Proizvod() { Naziv = "Mickey Mouse pliš igračka", Cena = 1590, Kolicina = 2, Opis = "Nov. Preporučen za decu stariju od 2 godine.", Slika = p5, DatumPostavljanja = new DateTime(2022, 08, 05, 0, 0, 0), Grad = "Bečej", Dostupan = "Da", Recenzije = new List<Recenzija> { recenzija } },
            //};

            //xmlFile.SerializeObject<List<Proizvod>>(Proizvodi, "Proizvodi.xml");

            //List<Korisnik> ListaAdministratora = new List<Korisnik>
            //{
            //    new Korisnik() { KorisnickoIme = "admin1", Lozinka = "adminftn1", Ime = "Siniša", Prezime = "Nikolić", Pol = "muški", Email = "sinisa_nikolic@uns.ac.rs", DatumRodjenja = new DateTime(1984, 04, 23, 0, 0, 0), Uloga = Uloge.Administrator },
            //    new Korisnik() { KorisnickoIme = "admin2", Lozinka = "adminftn2", Ime = "Tamara", Prezime = "Kovačević", Pol = "ženski", Email = "tamara.kovacevic@uns.ac.rs", DatumRodjenja = new DateTime(1998, 07, 13, 0, 0, 0), Uloga = Uloge.Administrator },
            //};

            //xmlFile.SerializeObject<List<Korisnik>>(ListaAdministratora, "Administratori.xml");

            //List<Korisnik> Korisnici = new List<Korisnik>
            //{
            //    new Korisnik() { KorisnickoIme = "nidžo", Lozinka = "n12345n6789", Ime = "Nikola", Prezime = "Nikolić", Pol = "muški", Email = "nNesic@gamil.com", DatumRodjenja = new DateTime(1999, 03, 13, 0, 0, 0), Uloga = Uloge.Kupac, OmiljeniProizvodi = new List<Proizvod> { Proizvodi[0] } },
            //    new Korisnik() { KorisnickoIme = "ana42", Lozinka = "anaKraljica", Ime = "Ana", Prezime = "Anić", Pol = "ženski", Email = "ana@gmail.com", DatumRodjenja = new DateTime(2002, 08, 22, 0, 0, 0), Uloga = Uloge.Kupac, OmiljeniProizvodi = new List<Proizvod> { Proizvodi[3] } },
            //    new Korisnik() { KorisnickoIme = "nemac", Lozinka = "123456", Ime = "Nemanja", Prezime = "Nešić", Pol = "muški", Email = "nemac123@gmail.com", DatumRodjenja = new DateTime(2001, 11, 05, 0, 0, 0), Uloga = Uloge.Prodavac, ObjavljeniProizvodi = new List<Proizvod> { Proizvodi[1], Proizvodi[3] } },
            //    new Korisnik() { KorisnickoIme = "mici", Lozinka = "987456", Ime = "Milica", Prezime = "Micić", Pol = "ženski", Email = "mm00@gmail.com", DatumRodjenja = new DateTime(2000, 05, 15, 0, 0, 0), Uloga = Uloge.Prodavac, ObjavljeniProizvodi = new List<Proizvod> { Proizvodi[0], Proizvodi[7], Proizvodi[5] } },
            //    new Korisnik() { KorisnickoIme = "marko_kraljina", Lozinka = "m&m666", Ime = "Marko", Prezime = "Marković", Pol = "muški", Email = "marko33@gmail.com", DatumRodjenja = new DateTime(1998, 08, 01, 0, 0, 0), Uloga = Uloge.Kupac, OmiljeniProizvodi = new List<Proizvod> { Proizvodi[5] } },
            //    new Korisnik() { KorisnickoIme = "anjica", Lozinka = "123anja", Ime = "Anja", Prezime = "Lalović", Pol = "ženski", Email = "anja@gmail.com", DatumRodjenja = new DateTime(2002, 12, 21, 0, 0, 0), Uloga = Uloge.Prodavac, ObjavljeniProizvodi = new List<Proizvod> { Proizvodi[2], Proizvodi[4], Proizvodi[6] } },
            //};

            //List<Porudzbina> Poruceno = new List<Porudzbina>
            //{
            //    new Porudzbina() { Proizvod = Proizvodi[1], Kolicina = 1, Kupac = Korisnici[0].KorisnickoIme, DatumPorudzbine = new DateTime(2023, 05, 15), Status = Statusi.AKTIVNA },
            //    new Porudzbina() { Proizvod = Proizvodi[7], Kolicina = 1, Kupac = Korisnici[1].KorisnickoIme, DatumPorudzbine = new DateTime(2023, 05, 25), Status = Statusi.AKTIVNA },
            //    new Porudzbina() { Proizvod = Proizvodi[4], Kolicina = 1, Kupac = Korisnici[4].KorisnickoIme, DatumPorudzbine = new DateTime(2023, 06, 16), Status = Statusi.AKTIVNA },
            //};

            //Korisnici = new List<Korisnik>
            //{
            //    new Korisnik() { KorisnickoIme = "nidžo", Lozinka = "n12345n6789", Ime = "Nikola", Prezime = "Nikolić", Pol = "muški", Email = "nNesic@gamil.com", DatumRodjenja = new DateTime(1999, 03, 13, 0, 0, 0), Uloga = Uloge.Kupac, OmiljeniProizvodi = new List<Proizvod> { Proizvodi[0] }, Porudzbine = new List<Porudzbina> { Poruceno[0] } },
            //    new Korisnik() { KorisnickoIme = "ana42", Lozinka = "anaKraljica", Ime = "Ana", Prezime = "Anić", Pol = "ženski", Email = "ana@gmail.com", DatumRodjenja = new DateTime(2002, 08, 22, 0, 0, 0), Uloga = Uloge.Kupac, OmiljeniProizvodi = new List<Proizvod> { Proizvodi[3] },  Porudzbine = new List<Porudzbina> { Poruceno[1] } },
            //    new Korisnik() { KorisnickoIme = "nemac", Lozinka = "123456", Ime = "Nemanja", Prezime = "Nešić", Pol = "muški", Email = "nemac123@gmail.com", DatumRodjenja = new DateTime(2001, 11, 05, 0, 0, 0), Uloga = Uloge.Prodavac, ObjavljeniProizvodi = new List<Proizvod> { Proizvodi[1], Proizvodi[3] } },
            //    new Korisnik() { KorisnickoIme = "mici", Lozinka = "987456", Ime = "Milica", Prezime = "Micić", Pol = "ženski", Email = "mm00@gmail.com", DatumRodjenja = new DateTime(2000, 05, 15, 0, 0, 0), Uloga = Uloge.Prodavac, ObjavljeniProizvodi = new List<Proizvod> { Proizvodi[0], Proizvodi[7], Proizvodi[5] } },
            //    new Korisnik() { KorisnickoIme = "marko_kraljina", Lozinka = "m&m666", Ime = "Marko", Prezime = "Marković", Pol = "muški", Email = "marko33@gmail.com", DatumRodjenja = new DateTime(1998, 08, 01, 0, 0, 0), Uloga = Uloge.Kupac, OmiljeniProizvodi = new List<Proizvod> { Proizvodi[5] },  Porudzbine = new List<Porudzbina> { Poruceno[2] } },
            //    new Korisnik() { KorisnickoIme = "anjica", Lozinka = "123anja", Ime = "Anja", Prezime = "Lalović", Pol = "ženski", Email = "anja@gmail.com", DatumRodjenja = new DateTime(2002, 12, 21, 0, 0, 0), Uloga = Uloge.Prodavac, ObjavljeniProizvodi = new List<Proizvod> { Proizvodi[2], Proizvodi[4], Proizvodi[6] } },
            //};

            //xmlFile.SerializeObject<List<Korisnik>>(Korisnici, "Korisnici.xml");

            //#endregion
        }
    }
}