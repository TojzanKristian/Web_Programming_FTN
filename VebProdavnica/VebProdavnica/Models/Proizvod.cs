using System;
using System.Collections.Generic;

namespace VebProdavnica.Models
{
    public class Proizvod
    {
        //        ● Naziv
        //        ● Cena
        //        ● Količina
        //        ● Opis
        //        ● Slika
        //        ● Datum postavljanja proizvoda(čuvati u formatu dd/MM/yyyy)
        //        ● Grad
        //        ● Lista recenzija
        //        ● Status da li je dostupan

        #region Polja
        private string naziv;
        private double cena;
        private int kolicina;
        private string opis;
        private string slika;
        private DateTime datumPostavljanja;
        private string grad;
        private List<Recenzija> recenzije;
        private string dostupan;
        #endregion

        #region Konstruktori
        public Proizvod()
        {
        }

        public Proizvod(string naziv, double cena, int kolicina, string opis, string slika, DateTime datumPostavljanja, string grad, List<Recenzija> recenzije, string dostupan)
        {
            this.Naziv = naziv;
            this.Cena = cena;
            this.Kolicina = kolicina;
            this.Opis = opis;
            this.Slika = slika;
            this.DatumPostavljanja = datumPostavljanja;
            this.Grad = grad;
            this.Recenzije = recenzije;
            this.Dostupan = dostupan;
        }
        #endregion

        #region Svojstva
        public string Naziv { get => naziv; set => naziv = value; }
        public double Cena { get => cena; set => cena = value; }
        public int Kolicina { get => kolicina; set => kolicina = value; }
        public string Opis { get => opis; set => opis = value; }
        public string Slika { get => slika; set => slika = value; }
        public DateTime DatumPostavljanja { get => datumPostavljanja; set => datumPostavljanja = value; }
        public string Grad { get => grad; set => grad = value; }
        public List<Recenzija> Recenzije { get => recenzije; set => recenzije = value; }
        public string Dostupan { get => dostupan; set => dostupan = value; }
        #endregion
    }
}