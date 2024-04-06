using System;
using System.Collections.Generic;
using Proje3;

namespace Proje3
{
    public class BolgeVeDil
    {
        string dosyaYolu = "C:\\Users\\CASPER\\source\\repos\\Proje3\\Proje3\\bolgevedil2.txt";

        private Dictionary<string, KullaniciNode> users;
        private List<Edge> edges;

        public BolgeVeDil()
        {
            users = new Dictionary<string, KullaniciNode>();
            edges = new List<Edge>();
        }
        public void ProcessUserData(string username, Dictionary<string, HashSet<string>> usernamedil, Dictionary<string, HashSet<string>> usernamebolge)
        {
            BFSForInterests(username, usernamedil, usernamebolge);
        }

        public void AddUser(Kullanici user, Dictionary<string, string> Languagesss, Dictionary<string, string> Regionsss)
        {
            if (user != null && !string.IsNullOrEmpty(user.Username))
            {
                if (!users.ContainsKey(user.Username))
                {
                    KullaniciNode node = new KullaniciNode(user, Languagesss, Regionsss);
                    users.Add(user.Username, node);
                }
            }
            else
            {
                Console.WriteLine("Hata: Eklenen kullanıcı null veya kullanıcı adı boş.");
            }
        }

        public void BFSForInterests(string startUser, Dictionary<string, HashSet<string>> usernamedil, Dictionary<string, HashSet<string>> usernamebolge)
        {
            if (!users.ContainsKey(startUser))
            {
                Console.WriteLine($"Başlangıç kullanıcısı bulunamadı: {startUser}");
                return;
            }

            var visited = new HashSet<string>();
            var queue = new Queue<KullaniciNode>();
            queue.Enqueue(users[startUser]);
            visited.Add(startUser);

            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                var currentUser = currentNode.User;

                foreach (var diller in currentUser.Languagesss)
                {
                    var dils = diller.Key;
                    var userNames = usernamedil.ContainsKey(dils) ? usernamedil[dils] : new HashSet<string>();

                    userNames.Add(currentUser.Username);
                    usernamedil[dils] = userNames;
                    foreach (var bolgeler in currentUser.Regionsss)
                    {
                        var bolges = bolgeler.Key;
                        var usernames = usernamebolge.ContainsKey(bolges) ? usernamebolge[bolges] : new HashSet<string>();

                        userNames.Add(currentUser.Username);
                        usernamebolge[bolges] = userNames;
                    }
                   
                }

                foreach (var neighborNode in GetNeighborNodes(currentNode))
                {
                    var neighborUser = neighborNode.User;

                    if (!visited.Contains(neighborUser.Username))
                    {
                        queue.Enqueue(neighborNode);
                        visited.Add(neighborUser.Username);
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(dosyaYolu, true))
            {
                foreach (var entry in usernamedil)
                {
                    var dils = entry.Key;
                    var usernames = entry.Value;

                    foreach (var entrybolge in usernamebolge)
                    {
                        var bolges = entrybolge.Key;
                        var usernamesss = entrybolge.Value;
                        if (usernames.SetEquals(usernamesss))
                        {
                            Console.WriteLine($"Dil: {dils}, Bölge: {bolges}");
                            writer.WriteLine($"Dil: {dils}, Bölge: {bolges}");
                            Console.WriteLine($"Kullanan Kullanıcılar: {string.Join(", ", usernames)}");
                            writer.WriteLine($"Kullanan Kullanıcılar: {string.Join(", ", usernames)}");
                            
                        }
                    }
                }
            }
        }

        public List<KullaniciNode> GetNeighborNodes(KullaniciNode node)
        {
            var neighbors = new List<KullaniciNode>();

            foreach (var edge in edges)
            {
                if (edge.Region == node)
                {
                    neighbors.Add(edge.Languages);
                }
                else if (edge.Languages == node)
                {
                    neighbors.Add(edge.Region);
                }
            }

            return neighbors;
        }

        public class KullaniciNode
        {
            public Kullanici User { get; }
            public Dictionary<string, string> Languagesss { get; }
            public Dictionary<string, string> Regionsss { get; }
            public KullaniciNode(Kullanici user, Dictionary<string, string> Languagesss, Dictionary<string, string> Regionsss)
            {
                User = user;
                this.Languagesss = Languagesss;
                this.Regionsss = Regionsss;
            }
        }

        public class Edge
        {
            public KullaniciNode Languages { get; }
            public KullaniciNode Region { get; }

            public Edge(KullaniciNode language, KullaniciNode region)
            {
                Languages = language;
                Region = region;
            }
        }
    }
}