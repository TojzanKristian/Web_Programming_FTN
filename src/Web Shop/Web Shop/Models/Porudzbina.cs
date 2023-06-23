using System;

namespace Web_Shop.Models
{
    public enum Statusi { AKTIVNA, IZVRŠEN, OTKAZAN }
    public class Porudzbina
    {
        #region Polja
        private Proizvod proizvod;
        private int kolicina;
        private string kupac;
        private DateTime datumPorudzbine;
        private Statusi status;
        #endregion

        #region Konstruktori
        public Porudzbina()
        {
        }

        public Porudzbina(Proizvod proizvod, int kolicina, string kupac, DateTime datumPorudzbine, Statusi status)
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
        public int Kolicina { get => kolicina; set => kolicina = value; }
        public string Kupac { get => kupac; set => kupac = value; }
        public DateTime DatumPorudzbine { get => datumPorudzbine; set => datumPorudzbine = value; }
        public Statusi Status { get => status; set => status = value; }
        #endregion
    }
}