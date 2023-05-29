using System;
using System.Collections.Generic;

namespace VebProdavnica.Models
{
    public enum Uloge { Kupac, Prodavac, Administrator }
    public class Korisnik
    {
        //● Korisničko ime(jedinstveno)
        //● Lozinka
        //● Ime
        //● Prezime
        //● Pol
        //● Email
        //● Datum rođenja(čuvati u formatu dd/MM/yyyy)
        //● Uloga(Kupac, Prodavac ili Administrator)
        //● Listu porudzbina(ukoliko korisnik ima ulogu Kupac)
        //● Listu omiljenih proizvoda(ukoliko korisnik ima ulogu Kupac)
        //● Listu objavljenih proizvoda(ukoliko korisnik ima ulogu Prodavac)

        #region Polja
        private string korisnickoIme;
        private string lozinka;
        private string ime;
        private string prezime;
        private string pol;
        private string email;
        private DateTime datumRodjenja;
        private Uloge uloga;
        private List<Porudzbina> porudzbine;
        private List<Proizvod> omiljeniProizvodi;
        private List<Proizvod> objavljenihProizvodi;
        #endregion

        #region Konstruktori
        public Korisnik()
        {
        }

        public Korisnik(string korisnickoIme, string lozinka, string ime, string prezime, string pol, string email, DateTime datumRodjenja, Uloge uloga, List<Porudzbina> porudzbine, List<Proizvod> omiljeniProizvodi, List<Proizvod> objavljenihProizvodi)
        {
            this.KorisnickoIme = korisnickoIme;
            this.Lozinka = lozinka;
            this.Ime = ime;
            this.Prezime = prezime;
            this.Pol = pol;
            this.Email = email;
            this.DatumRodjenja = datumRodjenja;
            this.Uloga = uloga;
            this.Porudzbine = porudzbine;
            this.OmiljeniProizvodi = omiljeniProizvodi;
            this.ObjavljenihProizvodi = objavljenihProizvodi;
        }
        #endregion

        #region Svojtva
        public string KorisnickoIme { get => korisnickoIme; set => korisnickoIme = value; }
        public string Lozinka { get => lozinka; set => lozinka = value; }
        public string Ime { get => ime; set => ime = value; }
        public string Prezime { get => prezime; set => prezime = value; }
        public string Pol { get => pol; set => pol = value; }
        public string Email { get => email; set => email = value; }
        public DateTime DatumRodjenja { get => datumRodjenja; set => datumRodjenja = value; }
        public Uloge Uloga { get => uloga; set => uloga = value; }
        public List<Porudzbina> Porudzbine { get => porudzbine; set => porudzbine = value; }
        public List<Proizvod> OmiljeniProizvodi { get => omiljeniProizvodi; set => omiljeniProizvodi = value; }
        public List<Proizvod> ObjavljenihProizvodi { get => objavljenihProizvodi; set => objavljenihProizvodi = value; }
        #endregion
    }
}