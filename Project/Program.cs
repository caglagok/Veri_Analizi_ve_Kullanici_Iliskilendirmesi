using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ZemberekDotNet.Morphology;
using ZemberekDotNet.Morphology.Analysis;
using System.Collections;
using System.Linq;
using Proje3;

namespace Proje3
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string dosyaYolu = "C:\\Users\\CASPER\\source\\repos\\Proje3\\Proje3\\kayıt.txt";

            string jsonFilePath = @"C:\\Users\\CASPER\\Desktop\\twitter_data.json";

            string jsonContent = File.ReadAllText(jsonFilePath);
            var users = JsonConvert.DeserializeObject<List<Kullanici>>(jsonContent);

            Hashtable userInterests = new Hashtable();
            interestGraph interestGraphhh = new interestGraph();
            HashSet<string> hashSet = new HashSet<string>();
            //UserGraph userGraphhh = new UserGraph();

            Dictionary<string, HashSet<string>> userNamesByInterest = new Dictionary<string, HashSet<string>>();
            Dictionary<string, HashSet<string>> usernamedil = new Dictionary<string, HashSet<string>>();
            Dictionary<string, HashSet<string>> usernamebolge = new Dictionary<string, HashSet<string>>();


            BolgeVeDil bolgevedilll = new BolgeVeDil();
            olusanhash<string, Dictionary<string, int>> userInterests2 = new olusanhash<string, Dictionary<string, int>>();
            Hashtable dil = new Hashtable();
            Hashtable bolge = new Hashtable();

            if (users.Count > 0)
            {
                for (int i = 0; i < Math.Min(100, users.Count); i++)
                {
                    var currentUser = users[i];
                    var interests = AnalyzeUserInterests(currentUser);

                    Kullanici kullanici = new Kullanici();

                    userInterests.Add(currentUser, interests);

                    kullanici.Username = currentUser.Username;
                    kullanici.Name = currentUser.Name;
                    kullanici.followers_count = currentUser.followers_count;
                    kullanici.following_count = currentUser.following_count;
                    kullanici.Language = currentUser.Language;
                    kullanici.Region = currentUser.Region;
                    kullanici.Tweets = currentUser.Tweets;
                    kullanici.followers = currentUser.followers;
                    kullanici.following = currentUser.following;

                    //kullanici.Languagesss[currentUser.Username] = kullanici.Language;
                    //kullanici.Regionsss[currentUser.Username] = kullanici.Region;

                    foreach (DictionaryEntry entry in userInterests)
                    {
                        var user = entry.Key as Kullanici;
                        var interestss = (Dictionary<string, int>)entry.Value;

                        kullanici.Languagesss[user.Username] = kullanici.Language;
                        kullanici.Regionsss[user.Username] = kullanici.Region;

                        userInterests2.Add(user.Username, interestss);
                    }



                    dil.Add(kullanici, kullanici.Languagesss);
                    bolge.Add(kullanici, kullanici.Regionsss);
/*
                    userGraphhh.AddUser(kullanici);
                    userGraphhh.BFS(kullanici.Username, kullanici.followers_count);

                    if (currentUser.following != null)
                    {
                        foreach (var followingUsername in kullanici.following)
                        {
                            userGraphhh.AddRelationship(kullanici.Username, followingUsername);
                        }
                    }
                    if (currentUser.followers != null)
                    {
                        foreach (var followersUsername in kullanici.followers)
                        {
                            userGraphhh.AddRelationship(kullanici.Username, followersUsername);
                        }
                    }*/
                }
                //userInterests2.PrintKeyValuePairs();
                Dictionary<string, int> globalInterests = new Dictionary<string, int>();

                using (StreamWriter writer = new StreamWriter(dosyaYolu) { AutoFlush = true })
                {
                    TextWriter originalConsoleOut = Console.Out;
                    Console.SetOut(writer);
                    Console.SetOut(originalConsoleOut);

                    foreach (DictionaryEntry entry in userInterests)
                    {
                        var user = entry.Key as Kullanici;
                        var interestss = (Dictionary<string, int>)entry.Value;

                        Console.WriteLine($"Kullanici: {user.Username}");
                        writer.WriteLine($"Kullanici: {user.Username}");

                        foreach (var interestEntry in interestss.OrderByDescending(x => x.Value).Take(5))
                        {
                            Console.WriteLine($"  {interestEntry.Key}: {interestEntry.Value} kez");
                            writer.WriteLine($"  {interestEntry.Key}: {interestEntry.Value} kez");

                            if (globalInterests.ContainsKey(interestEntry.Key))
                            {
                                globalInterests[interestEntry.Key] += interestEntry.Value;
                            }
                            else
                            {
                                globalInterests[interestEntry.Key] = interestEntry.Value;
                            }

                            if (interestss.ContainsKey(interestEntry.Key))
                            {
                                interestss[interestEntry.Key] += interestEntry.Value;
                            }
                            else
                            {
                                interestss[interestEntry.Key] = interestEntry.Value;
                            }

                            if (!userNamesByInterest.ContainsKey(interestEntry.Key))
                            {
                                userNamesByInterest[interestEntry.Key] = new HashSet<string>();
                            }
                            userNamesByInterest[interestEntry.Key].Add(user.Username);
                        }

                        Kullanici kullanicinew = new Kullanici();
                        kullanicinew.GlobalInterest = globalInterests;
                        kullanicinew.Username = user.Username;

                        //user.GlobalInterest = globalInterests;
                        interestGraphhh.AddUser(kullanicinew);
                        //interestGraph interestGraphInstance = new interestGraph();
                        interestGraphhh.BFSForInterests(user.Username, userNamesByInterest);

                    }
                    Console.WriteLine("\nGlobal İlgi Alanlari:");
                    writer.Write("\nGlobal İlgi Alanlari:");
                    foreach (var globalInterestEntry in globalInterests.OrderByDescending(x => x.Value).Take(20))
                    {
                        Console.WriteLine($"  {globalInterestEntry.Key}: {globalInterestEntry.Value} kez");
                        writer.WriteLine($"  {globalInterestEntry.Key}: {globalInterestEntry.Value} kez");
                    }

                    //userGraphhh.DisplayGraph();
                    Console.WriteLine();
                    Console.Write("Takipci ve takip ettikleri için bir kullanici adi girin: ");
                    string hedefKullaniciAdi = Console.ReadLine();

                    //userGraphhh.TakipciVeTakipEdilenleriBul(hedefKullaniciAdi);
                }
            }
            else
            {
                Console.WriteLine("JSON dosyasında hiç kullanıcı yok.");
            }


            foreach (DictionaryEntry entry in dil)
            {
                var user = (Kullanici)entry.Key;
                var language = (Dictionary<string, string>)entry.Value;

                //Console.WriteLine($"Kullanici: {user.Username}");
                //writer.WriteLine($"Kullanici: {user.Username}");
                foreach (var entrys in language)
                {
                    if (language.ContainsKey(entrys.Key))
                    {
                        language[entrys.Key] += entrys.Value;
                    }
                    else
                    {
                        language[entrys.Key] = entrys.Value;
                    }
                    if (!usernamedil.ContainsKey(entrys.Key))
                    {
                        usernamedil[entrys.Key] = new HashSet<string>();
                    }
                    
                    usernamedil[entrys.Key].Add(user.Username);

                    Kullanici kullanicinewww = new Kullanici();

                    //kullanicinewww.Username = user.Username;
                    //bolgevedilll.AddUser(kullanicinewww, kullanicinewww.Languagesss, kullanicinewww.Regionsss);

                    foreach (DictionaryEntry entryy in bolge)
                    {
                        var userx = entry.Key as Kullanici;
                        var region = (Dictionary<string, string>)entry.Value;

                        //Console.WriteLine($"Kullanici: {user.Username}");
                        //writer.WriteLine($"Kullanici: {user.Username}");
                        foreach (var entryss in region)
                        {
                            if (region.ContainsKey(entrys.Key))
                            {
                                region[entrys.Key] += entrys.Value;
                            }
                            else
                            {
                                region[entrys.Key] = entrys.Value;
                            }
                            if (!usernamebolge.ContainsKey(entrys.Key))
                            {
                                usernamebolge[entrys.Key] = new HashSet<string>();
                            }

                            usernamebolge[entrys.Key].Add(user.Username);
                        }

                        // Kullanici kullanicinewww = new Kullanici();
                        //interestGraph interestGraphInstance = new interestGraph();

                    }
                    kullanicinewww.Username = user.Username;
                    bolgevedilll.AddUser(kullanicinewww, kullanicinewww.Languagesss, kullanicinewww.Regionsss);
                }

                //Kullanici kullanicinewww = new Kullanici()
                //kullanicinewww.Username = user.Username;
                //bolgevedilll.AddUser(kullanicinewww);
                //interestGraph interestGraphInstance = new interestGraph();


                bolgevedilll.BFSForInterests(user.Username, usernamedil, usernamebolge);
            }

            Console.ReadLine();


            Dictionary<string, int> AnalyzeUserInterests(Kullanici user)
            {
                var allTweets = user.Tweets ?? Array.Empty<string>();
                var allWords = allTweets.SelectMany(tweet => tweet.Split(' '));

                TurkishMorphology morphology = TurkishMorphology.CreateWithDefaults();

                var turkishWords = allWords
                    .Where(word => IsTurkishWord(word, morphology))
                    .Where(word => !IsStopWord(word.ToLowerInvariant()))
                    .Where(word => !IsVerbEndingWithMekMak(word))
                    .ToList();

                Dictionary<string, int> interests = new Dictionary<string, int>();

                foreach (var turkishWord in turkishWords)
                {
                    var roots = GetStems(turkishWord, morphology);

                    foreach (var root in roots)
                    {
                        if (interests.ContainsKey(root))
                        {
                            interests[root]++;
                        }
                        else
                        {
                            interests[root] = 1;
                        }
                    }
                }

                var mergedInterests = interests.GroupBy(pair => pair.Key.ToLower())
                                               .ToDictionary(group => group.Key, group => group.Sum(pair => pair.Value));

                return mergedInterests;
            }
            List<string> GetStems(string word, TurkishMorphology morphology)
            {
                WordAnalysis result = morphology.Analyze(word);

                var stems = result.Where(analysis => analysis.GetLemmas().Any())
                                  .Select(analysis => analysis.GetLemmas().First())
                                  .ToList();

                return stems;
            }

            bool IsTurkishWord(string word, TurkishMorphology morphology)
            {
                WordAnalysis result = morphology.Analyze(word);

                return result.Any();
            }
            bool IsStopWord(string word)
            {
                List<string> stopWords = new List<string> {"bul", "ara", "etmek", "eylemek", "olmak", "aa", "acaba", "ait", "altı", "a", "altmış", "ama", "amma","anca", "ancağ", "ancak", "artık", "asla", "aslında", "az", "b", "bana", "bari", "başkası", "bazen",
                    "bazı", "bazıları", "bazısı", "be", "belki", "ben", "bende", "benden", "beni", "benim", "beş", "bide", "bile", "bin", "bir", "birazı", "birçoğ", "birçoğu", "birçok", "birçokları", "biri", "birisi", "birkaç",
                    "birkaçı", "birkez", "birşey", "birşeyi", "biz", "bizden", "bize", "bizi", "bizim", "böyle", "böylece", "değişik", "son", "gel", "ayn", "yer", "bu", "buna", "bunda", "bundan", "bunu", "bunun", "burada",
                    "bütün", "c", "ç", "çoğu", "çoğuna", "çoğunu", "çok", "çünkü", "d", "da", "daha", "dahi", "dandini","de", "defa", "değ", "değil", "değin", "dek", "demek", "diğer", "diğeri", "diğerleri", "diye", "dk",
                    "dha", "doğrusu", "doksan", "dokuz", "dolayı", "dört", "e", "eğer", "eh", "elbette", "elli", "en", "etkili", "f", "fakat", "fakad", "falan", "falanca", "felan", "filan", "filanca", "g", "ğ", "gene","gereğ", "gibi", "göre", "görece",
                    "h", "hakeza", "hakkında", "hâlâ", "halbuki", "hangi", "hangisi","hani", "hasebiyle", "hatime", "hatta", "hele", "hem", "henüz", "hep", "hepsi", "hepsine", "hepsini",
                    "her", "her biri", "herkes", "herkese", "herkesi", "hiç", "hiç kimse", "hiçbiri", "hiçbirine", "hiçbirini", "hoş", "i", "ı", "ın", "için", "içinde", "içre", "iki", "ila", "ile", "imdi", "indinde", "intağ", "intak",
                    "ise", "işte", "ister", "j", "k", "kaç", "kaçı", "kadar", "kah", "karşın", "katrilyon", "kelli", "kendi","kendine", "kendini", "keşke", "keşki", "kez", "keza", "kezaliğ", "kezalik", "ki", "kim", "kimden",
                    "kime", "kimi", "kimin", "kimisi", "kimse", "kırk", "l", "67", "lakin", "m", "madem", "mademki", "mamafih","meğer", "meğerki", "meğerse", "mi", "mı", "milyar", "milyon", "mu", "mü", "n", "nasıl", "nde", "ne", "ne kadar",
                    "ne zaman", "neden", "nedense", "nedir", "nerde", "nere", "nerede", "nereden", "nereli", "neresi", "nereye", "nesi", "neye", "neyi", "neyse", "niçin", "ni", "nı", "nin", "nın", "nitekim", "niye", "o", "ö", "öbürkü",
                    "öbürü", "on", "ön", "ona", "önce", "onda", "ondan", "onlar", "onlara", "onlardan", "onlari", "onların","onu", "onun", "orada", "ötekisi", "ötürü", "otuz", "öyle", "oysa", "oysaki", "p", "pad", "pat", "peki",
                    "r", "rağmen", "s", "ş", "sakın", "sana", "sanki", "şayet", "sekiz", "seksen", "sen", "senden", "seni", "senin", "şey", "şeyden", "şeye", "şeyi", "şeyler", "şimdi", "siz", "sizden", "size", "sizi", "sizin",
                    "son", "sonra", "68", "şöyle", "şu", "şuna", "şunda", "şundan", "şunu", "şunun", "t", "ta", "tabi","tamam", "tl", "trilyon", "tüm", "tümü", "u", "ü", "üç", "üsd", "üst", "uyarınca", "üzere", "v", "var", "ve", "velev", "velhasıl", "velhasılıkelam",
                    "vesselam", "veya", "veyahud", "veyahut", "y", "ya", "ya da", "yani", "yazığ", "yazık", "yedi", "yekdiğeri", "yerine", "yetmiş", "yine", "yirmi", "yoksa", "yukarda",
                    "yukardan", "yukarıda", "yukarıdan", "yüz", "z", "zaten", "zinhar", "zira", "tür", "cins", "ad", "iyi","kötü", "çeşit", "ol", "tut", "değer", "ad", "çeşit", "örnek"
                };

                return stopWords.Contains(word);
            }
            bool IsVerbEndingWithMekMak(string word)
            {
                return word.EndsWith("mek", StringComparison.OrdinalIgnoreCase) || word.EndsWith("mak", StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}