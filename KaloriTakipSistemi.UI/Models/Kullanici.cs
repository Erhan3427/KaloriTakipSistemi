﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaloriTakipSistemi.UI.Models
{ // burada classlarımızı olsuturuyoruz ve tablolarda istenilenlere göre propertylerimizi ekliyoruz
    public class Kullanici
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Sifre { get; set; }
        public string KullaniciAdi { get; set; }
        public byte Yas { get; set; }
        public bool HesapDurumu { get; set; }

        public string ?DondurmaSebebi  { get; set; }
        public List<KullaniciYemek> KullaniciYemek { get; set; }
    }
}
