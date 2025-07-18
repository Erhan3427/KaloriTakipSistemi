﻿using KaloriTakipSistemi.UI.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KaloriTakipSistemi.UI
{
    public partial class FRMKullaniciBilgileri : Form
    {
        MyDbContext _context = new MyDbContext();
        public FRMKullaniciBilgileri()
        {
            InitializeComponent();
        }


        private void FRMKullaniciBilgileri_Load(object sender, EventArgs e) // 
        {
            btnGuncelle.Enabled = false; // burda butonun başlangıçta kapalı olmasını sağlıyoruz
            var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.Id == FRMKullaniciGirisEkrani.AktifKullaniciId); // burada kullanıcının bilgilerini alıyoruz
            if (kullanici != null)
            {
                lblKullaniciAdi.Text = kullanici.KullaniciAdi; 
                txtAd.Text = kullanici.Ad;
                txtSoyad.Text = kullanici.Soyad;
                txtYas.Text = kullanici.Yas.ToString();
            }
            else
            {
                MessageBox.Show("Kullanıcı bulunamadı."); // kullanıcı bulunamadı mesajını veriyoruz 
            }



        }

        private void chbBilgilerimiGuncelle_CheckedChanged(object sender, EventArgs e)
        {
            if (chbBilgilerimiGuncelle.Checked) // burada checkbox işaretli ise
            {
                txtAd.ReadOnly = false; // burada textboxların readonly özelliğini false yapıyoruz
                txtSoyad.ReadOnly = false; // burada textboxların readonly özelliğini false yapıyoruz
                txtYas.ReadOnly = false;  // burada textboxların readonly özelliğini false yapıyoruz
                txtSifre.ReadOnly = false; // burada textboxların readonly özelliğini false yapıyoruz
                btnGuncelle.Enabled = true; // burada butonu aktif yapıyoruz
            }
            else
            {
                txtAd.ReadOnly = true;
                txtSoyad.ReadOnly = true;
                txtYas.ReadOnly = true;
                txtSifre.ReadOnly = true;
                btnGuncelle.Enabled = false;
            }


        }
        private bool BilgiGuncellemeValidasyonu()
        {
            // Boş alan kontrolollerini yapıyoruz 
            if (string.IsNullOrWhiteSpace(txtAd.Text) ||
                string.IsNullOrWhiteSpace(txtSoyad.Text) ||
                string.IsNullOrWhiteSpace(txtYas.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Ad ve soyad uzunluk kontrolü
            if (txtAd.Text.Length < 2 || txtSoyad.Text.Length < 2)
            {
                MessageBox.Show("Ad ve soyad en az 2 karakter olmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Yaş kontrolü
            if (!byte.TryParse(txtYas.Text, out byte yas) || yas < 13 || yas > 100)
            {
                MessageBox.Show("Yaş 13-100 arasında olmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txtAd.Text.Any(char.IsDigit) || txtSoyad.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Ad ve soyad sayı içermemelidir.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Şifre değişikliği varsa kontrol et
            if (string.IsNullOrWhiteSpace(txtSifre.Text))
            {
                // Şifre uzunluk kontrolü
                MessageBox.Show("Sifre Bos Olamaz!!","UYARI",  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            //şifrede noktalama işareti olmalı...

            if (txtSifre.Text.Any(char.IsPunctuation))
            {

                MessageBox.Show("Şifre de noktalama işareti olmamalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return false;

            }

            //ad ve soyad noktalama işareti kontrolü...

            if (txtAd.Text.Any(char.IsPunctuation) || txtSoyad.Text.Any(char.IsPunctuation))
            {

                MessageBox.Show("Ad ve soyad noktalama işareti olmamalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return false;

            }


            return true;
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (!BilgiGuncellemeValidasyonu()) // burada bilgi güncelleme validasyonunu kontrol ediyoruz 
                {
                    return;
                }
                var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.Id == FRMKullaniciGirisEkrani.AktifKullaniciId); // burada kullanıcı bilgilerini alıyoruz 
                if (kullanici != null)
                {
                    kullanici.Ad = txtAd.Text.Trim();
                    kullanici.Soyad = txtSoyad.Text.Trim();
                    kullanici.Yas = byte.Parse(txtYas.Text);
                    kullanici.Sifre = _context.sha256_hash(txtSifre.Text.Trim());
                    _context.SaveChanges();
                    MessageBox.Show("Bilgileriniz güncellendi.");
                }
                else
                {
                    MessageBox.Show("Kullanıcı bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme sırasında hata oluştu: " + ex.Message);

            }

        }
    }
}
