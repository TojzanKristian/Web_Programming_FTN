using System;

namespace VebProdavnica.Models
{
    public enum Statusi { AKTIVNA, IZVRŠENA, OTKAZANA }
    public class Porudzbina
    {
        //● Proizvod
        //● Količina
        //● Kupac
        //● Datum porudzbine
        //● Status(AKTIVNA, IZVRŠENA, OTKAZANA)

        #region Polja
        private Proizvod proizvod;
        private double kolicina;
        private Korisnik kupac;
        private DateTime datumPorudzbine;
        private Statusi status;
        #endregion

        #region Konstruktori
        public Porudzbina()
        {
        }

        public Porudzbina(Proizvod proizvod, double kolicina, Korisnik kupac, DateTime datumPorudzbine, Statusi status)
        {
            this.Proizvod = proizvod;
            this.Kolicina = kolicina;
            this.Kupac = kupac;
            this.DatumPorudzbine = datumPorudzbine;
            this.Status = status;
        }
        #endregion

        #region Svojstva
        public Proizvod Proizvod { get => proizvod; set => proizvod = value; }
        public double Kolicina { get => kolicina; set => kolicina = value; }
        public Korisnik Kupac { get => kupac; set => kupac = value; }
        public DateTime DatumPorudzbine { get => datumPorudzbine; set => datumPorudzbine = value; }
        public Statusi Status { get => status; set => status = value; }
        #endregion
    }
}