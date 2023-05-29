namespace VebProdavnica.Models
{
    public class Recenzija
    {
        //● Proizvod
        //● Recenzent(kupac koji je napisao recenziju)
        //● Naslov
        //● Sadržaj recenzije
        //● Slika

        #region Polja
        private Proizvod proizvod;
        private Korisnik recenzent;
        private string naslov;
        private string sadrzajRecenzije;
        private string slika;
        #endregion

        #region Konstruktori
        public Recenzija()
        {
        }

        public Recenzija(Proizvod proizvod, Korisnik recenzent, string naslov, string sadrzajRecenzije, string slika)
        {
            this.Proizvod = proizvod;
            this.Recenzent = recenzent;
            this.Naslov = naslov;
            this.SadrzajRecenzije = sadrzajRecenzije;
            this.Slika = slika;
        }
        #endregion

        #region Svojstva
        public Proizvod Proizvod { get => proizvod; set => proizvod = value; }
        public Korisnik Recenzent { get => recenzent; set => recenzent = value; }
        public string Naslov { get => naslov; set => naslov = value; }
        public string SadrzajRecenzije { get => sadrzajRecenzije; set => sadrzajRecenzije = value; }
        public string Slika { get => slika; set => slika = value; }
        #endregion
    }
}