﻿namespace Web_Shop.Models
{
    public class Recenzija
    {
        #region Polja
        private string proizvod;
        private string recenzent;
        private string naslov;
        private string sadrzajRecenzije;
        private string slika;
        private bool prikazi = false; // za prikaz recenzije svima
        private bool samoAdmin = false; // za prikaz recenzije adminu ako je odbije
        #endregion

        #region Konstruktori
        public Recenzija()
        {
        }

        public Recenzija(string proizvod, string recenzent, string naslov, string sadrzajRecenzije, string slika)
        {
            this.Proizvod = proizvod;
            this.Recenzent = recenzent;
            this.Naslov = naslov;
            this.SadrzajRecenzije = sadrzajRecenzije;
            this.Slika = slika;
        }
        #endregion

        #region Svojstva
        public string Proizvod { get => proizvod; set => proizvod = value; }
        public string Recenzent { get => recenzent; set => recenzent = value; }
        public string Naslov { get => naslov; set => naslov = value; }
        public string SadrzajRecenzije { get => sadrzajRecenzije; set => sadrzajRecenzije = value; }
        public string Slika { get => slika; set => slika = value; }
        public bool Prikazi { get => prikazi; set => prikazi = value; }
        public bool SamoAdmin { get => samoAdmin; set => samoAdmin = value; }
        #endregion
    }
}