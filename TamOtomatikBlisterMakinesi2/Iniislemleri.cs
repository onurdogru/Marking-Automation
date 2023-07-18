using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;


namespace TamOtomatikBlisterMakinesi2
{
    public static class Iniislemleri
    {
        //
        //************************PROJE YOLU*************************************//
        //
        //Sınıfımızı Extension Method olarak kullanmak istediğimiz için static tanımlıyoruz.

        static string dizinYolu = "C:\\Proje";
        static string dosyaAdi = "C:\\Proje\\ayarlar.ini";
        static string dosyaAdi2 = "C:\\Proje\\ayarlar2.ini"; //2 ayarlar dosyası olmasının nedeni, her kaydedilen ayarlar1'e tek seferlik, ayarlar2'ye ise her kaydedileni gönderiyör.





        //
        //**************************YAZMA İŞLEMLERİ******************************//
        //

        //Yazma işlemleri için gerekli olan dll'i import edip, ini için WritePrivateProfileString metodunun görüntüsünü aldık
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool WritePrivateProfileString(string kategori, string anahtar, string deger, string dosyaAdi);

        //Yazma işlemleri için gerekli olan dll'i import edip, ini için GetPrivateProfileString metodunun görüntüsünü aldık
        [DllImport("kernel32.dll")]
        static extern uint GetPrivateProfileString(string kategori, string anahtar, string lpDefault, StringBuilder sb, int sbKapasite, string dosyaAdi);

        public static bool VeriYaz(string kategori, string anahtar, string deger)
        {
            // Eski verileri korumak için dosyanın içeriğini kaydedin
            // Yeni verileri dosyaya yazmak için StreamWriter kullanın
            string[] eskiVeriler = File.ReadAllLines(dosyaAdi);
            //Eski verileri dosyaya geri yazın
            foreach (string veri in eskiVeriler)
                using (StreamWriter sw = File.AppendText(dosyaAdi2))
                {
                    sw.WriteLine(veri);
                }


            if (!Directory.Exists(dizinYolu)) //Dizin yoksa oluşturalım.
                Directory.CreateDirectory(dizinYolu);

            return WritePrivateProfileString(kategori, anahtar, deger, dosyaAdi);
        }






        //
        //*********************************************OKUMA İŞLEMLERİ******************************************//
        //
        public static string VeriOku(string kategori, string anahtar)
        {
            //Okunacak veriyi okumak ve kapasitesini sınırlandırmak ve performans için StringBuilder sınıfını kullanıyoruz.
            StringBuilder sb = new StringBuilder(500);

            GetPrivateProfileString(kategori, anahtar, "", sb, sb.Capacity, dosyaAdi);

            string veri = sb.ToString();
            sb.Clear();
            return veri;
        }
    }
}

