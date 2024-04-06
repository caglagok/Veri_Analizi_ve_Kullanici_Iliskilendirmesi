using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Proje3
{
    public class Kullanici
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public int followers_count { get; set; }
        public int following_count { get; set; }
        public string Language { get; set; }
        public string Region { get; set; }
        public string[] Tweets { get; set; }
        public string[] following { get; set; }
        public string[] followers { get; set; }
        public Dictionary<string, int> GlobalInterest { get; set; }
        public Dictionary<string, string> Languagesss { get; set; }
        public Dictionary<string, string> Regionsss { get; set; }

        public Kullanici()
        {
            Languagesss = new Dictionary<string, string>();
            Regionsss = new Dictionary<string, string>();
        }

    }
}