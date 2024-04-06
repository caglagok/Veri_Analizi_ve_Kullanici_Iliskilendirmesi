/*
using System;
using Proje3;


namespace Proje3
{
    public class UserGraph
    {

        string dosyaYolu = "C:\\Users\\CASPER\\source\\repos\\Proje3\\Proje3\\graf.txt";
        string dosyaYolu1 = "C:\\Users\\CASPER\\source\\repos\\Proje3\\Proje3\\benzer.txt";
        string dosyaYolu2 = "C:\\Users\\CASPER\\source\\repos\\Proje3\\Proje3\\tekkisi.txt";

        private Dictionary<string, KullaniciNode> users;
        private List<Edge> edges;
        public UserGraph()
        {
            users = new Dictionary<string, KullaniciNode>();
            edges = new List<Edge>();
        }

        public void AddUser(Kullanici user)
        {
            if (!users.ContainsKey(user.Username))
            {
                KullaniciNode node = new KullaniciNode(user);
                users.Add(user.Username, node);
            }
        }

        public void AddRelationship(string followerUsername, string followingUsername)
        {
            if (users.ContainsKey(followerUsername) && users.ContainsKey(followingUsername))
            {
                edges.Add(new Edge(users[followerUsername], users[followingUsername]));
            }
        }
        public void TakipciVeTakipEdilenleriBul(string kullaniciAdi)
        {
            using (StreamWriter writer = new StreamWriter(dosyaYolu2) { AutoFlush = true })
            {
                TextWriter originalConsoleOut = Console.Out;
                Console.SetOut(writer);
                Console.SetOut(originalConsoleOut);

                if (users.ContainsKey(kullaniciAdi))
                {
                    var kullaniciDugumu = users[kullaniciAdi];
                    var takipciler = edges.Where(e => e.Following == kullaniciDugumu).Select(e => e.Follower.User.Username);
                    var takipEdilenler = edges.Where(e => e.Follower == kullaniciDugumu).Select(e => e.Following.User.Username);

                    Console.WriteLine();
                    Console.WriteLine($"{kullaniciAdi}'ın Takipçileri:");
                    writer.WriteLine($"{kullaniciAdi}'ın Takipçileri:");

                    foreach (var takipci in takipciler)
                    {
                        Console.WriteLine(takipci);
                        writer.WriteLine(takipci);
                    }

                    Console.WriteLine($"{kullaniciAdi}'ın Takip Ettikleri:");
                    writer.WriteLine($"{kullaniciAdi}'ın Takip Ettikleri:");

                    foreach (var takipEdilen in takipEdilenler)
                    {
                        Console.WriteLine(takipEdilen);
                        writer.WriteLine(takipEdilen);
                        Console.WriteLine();
                    }
                    writer.Close();
                }
                else
                {
                    Console.WriteLine($"Kullanıcı bulunamadı: {kullaniciAdi}");
                }
            }
        }

        public void BFS(string baslangicKullaniciAdi, int hedefTakipciSayisi)
        {
            if (!users.ContainsKey(baslangicKullaniciAdi))
            {
                Console.WriteLine($"Başlangıç kullanıcısı bulunamadı: {baslangicKullaniciAdi}");
                return;
            }
            using (StreamWriter writer = new StreamWriter(dosyaYolu1, true) { AutoFlush = true })
            {
                TextWriter originalConsoleOut = Console.Out;
                Console.SetOut(writer);
                Console.SetOut(originalConsoleOut);

                var baslangicDugumu = users[baslangicKullaniciAdi];
                var ziyaretEdildi = new HashSet<string>();
                var kuyruk = new Queue<KullaniciNode>();
                kuyruk.Enqueue(baslangicDugumu);
                ziyaretEdildi.Add(baslangicKullaniciAdi);

                var targetUser = users[baslangicKullaniciAdi];
                while (kuyruk.Count > 0)
                {
                    var currentDugum = kuyruk.Dequeue();

                    if (IsCloseEnough(currentDugum.User.followers_count, hedefTakipciSayisi))
                    {
                        //Console.WriteLine($"Kullanıcı: {currentDugum.User.Username}, Takipçi Sayısı: {currentDugum.User.followers_count}");
                        //writer.WriteLine($"Kullanıcı: {currentDugum.User.Username}, Takipçi Sayısı: {currentDugum.User.followers_count}");
                    }

                    foreach (var user in users.Values)
                    {
                        if (user != targetUser)
                        {
                            if (IsCloseEnough(user.User.followers_count, targetUser.User.followers_count))
                            {
                                Console.WriteLine($"{targetUser.User.Username} ve {user.User.Username} benzer takipçi sayısına sahip");
                                writer.WriteLine($"{targetUser.User.Username} ve {user.User.Username} benzer takipçi sayısına sahip");
                            }
                        }
                    }
                    foreach (var komsuDugum in GetKomsuDugumler(currentDugum))
                    {
                        if (!ziyaretEdildi.Contains(komsuDugum.User.Username))
                        {
                            kuyruk.Enqueue(komsuDugum);
                            ziyaretEdildi.Add(komsuDugum.User.Username);
                        }
                    }
                }
            }
        }
        public void DisplayGraph()
        {
            using (StreamWriter writer = new StreamWriter(dosyaYolu) { AutoFlush = true })
            {
                TextWriter originalConsoleOut = Console.Out;
                Console.SetOut(writer);
                Console.SetOut(originalConsoleOut);

                Console.WriteLine("\nGraf:");
                writer.WriteLine("\nGraf:");
                foreach (var edge in edges)
                {
                    Console.WriteLine($"{edge.Follower.User.Username} takip ediyor {edge.Following.User.Username}");
                    writer.WriteLine($"{edge.Follower.User.Username} takip ediyor {edge.Following.User.Username}");
                }
            }
        }
        private IEnumerable<KullaniciNode> GetKomsuDugumler(KullaniciNode dugum)
        {
            return edges.Where(e => e.Follower == dugum || e.Following == dugum)
                        .Select(e => e.Follower == dugum ? e.Following : e.Follower);
        }

        private bool IsCloseEnough(int followersCount1, int followersCount2)
        {
            const int tolerance = 10;
            return Math.Abs(followersCount1 - followersCount2) <= tolerance;
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
*/