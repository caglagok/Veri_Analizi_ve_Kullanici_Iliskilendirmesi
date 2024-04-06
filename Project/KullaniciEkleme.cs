using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proje3;

namespace Proje3
{
    public class KullaniciEkleme
    {
        public class Dugum
        {
            public Kullanici Veri;
            public Dugum Sonraki;
        }
        public Dugum Root { get; private set; }

        public void SonaEkle(Kullanici yeniVeri)
        {
            Dugum yeniDugum = new Dugum();
            Dugum sonuncu = Root;

            yeniDugum.Veri = yeniVeri;
            yeniDugum.Sonraki = null;

            if (Root == null)
            {
                Root = yeniDugum;
                return;
            }

            while (sonuncu.Sonraki != null)
                sonuncu = sonuncu.Sonraki;

            sonuncu.Sonraki = yeniDugum;
        }

        public void DictionaryToLinkedList(Dictionary<Kullanici, Dictionary<string, int>> userInterests)
        {
            foreach (var entry in userInterests)
            {
                foreach (var interestEntry in entry.Value.OrderByDescending(x => x.Value).Take(10))
                {
                    SonaEkle(entry.Key);
                }
            }
        }
    }
}
