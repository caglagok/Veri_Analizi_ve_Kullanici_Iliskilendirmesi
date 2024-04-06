using System;
using System.Collections.Generic;
using Proje3;

namespace Proje3
{
    public class interestGraph
    {
        string dosyaYolu = "C:\\Users\\CASPER\\source\\repos\\Proje3\\Proje3\\interest.txt";

        private Dictionary<string, KullaniciNode> users;
        private List<Edge> edges;

        public interestGraph()
        {
            users = new Dictionary<string, KullaniciNode>();
            edges = new List<Edge>();
        }
        public void ProcessUserData(string username, Dictionary<string, HashSet<string>> userNamesByInterest)
        {
            BFSForInterests(username, userNamesByInterest);
        }

        public void AddUser(Kullanici user)
        {

            if (user != null && !string.IsNullOrEmpty(user.Username))
            {
                if (!users.ContainsKey(user.Username))
                {
                    KullaniciNode node = new KullaniciNode(user);
                    users.Add(user.Username, node);
                }
            }
            else
            {
                Console.WriteLine("Hata: Eklenen kullanıcı null veya kullanıcı adı boş.");

            }
        }

        public void BFSForInterests(string startUser, Dictionary<string, HashSet<string>> userNamesByInterest)
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

                foreach (var interestEntry in currentUser.GlobalInterest)
                {
                    var interest = interestEntry.Key;
                    var userNames = userNamesByInterest.ContainsKey(interest) ? userNamesByInterest[interest] : new HashSet<string>();

                    userNames.Add(currentUser.Username);
                    userNamesByInterest[interest] = userNames;
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
            using (StreamWriter writer = new StreamWriter(dosyaYolu) { AutoFlush = true })
            {
                TextWriter originalConsoleOut = Console.Out;
                Console.SetOut(writer);
                Console.SetOut(originalConsoleOut);

                foreach (var entry in userNamesByInterest)
                {
                    var interest = entry.Key;
                    var userNames = entry.Value;

                    Console.WriteLine($"Ilgi Alani: {interest}");
                    writer.WriteLine($"Ilgi Alani: {interest}");
                    Console.WriteLine($"Kullanan Kullanıcılar: {string.Join(", ", userNames)}");
                    writer.WriteLine($"Kullanan Kullanıcılar: {string.Join(", ", userNames)}");
                    Console.WriteLine();

                }
            }
        }

        public List<KullaniciNode> GetNeighborNodes(KullaniciNode node)
        {
            var neighbors = new List<KullaniciNode>();

            foreach (var edge in edges)
            {
                if (edge.Follower == node)
                {
                    neighbors.Add(edge.Following);
                }
                else if (edge.Following == node)
                {
                    neighbors.Add(edge.Follower);
                }
            }

            return neighbors;
        }

        public class KullaniciNode
        {
            public Kullanici User { get; }

            public KullaniciNode(Kullanici user)
            {
                User = user;
            }
        }

        public class Edge
        {
            public KullaniciNode Follower { get; }
            public KullaniciNode Following { get; }

            public Edge(KullaniciNode follower, KullaniciNode following)
            {
                Follower = follower;
                Following = following;
            }
        }
    }
}