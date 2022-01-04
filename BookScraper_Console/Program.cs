using C_Our_Souls_DAL.Data;
using C_Our_Souls_DAL.Data.UnitOfWork;
using C_Our_Souls_DAL.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookScraper_Console
{
    public class Program
    {
        public static IUnitOfWork uow = new UnitOfWork(new DatabaseContext());
        public static string[] LeeftijdsKlasse = new string[] {
        "3 tot 7 jaar",
"8 tot 11 jaar",
"12 tot 15 jaar",
"16 tot 18 jaar",
"Ouder dan 18 jaar"
        };
        public static void Main(string[] args)
        {
            Console.WriteLine("Creating general data...");
            CreateGeneralData();
            Console.WriteLine("Starting scraper...");
            ScrapeListPage();
            Console.WriteLine("Creating mediums");
            CreateMediums();
            Console.ReadKey();
        }
        public static void CreateGeneralData()
        {
            //Generate general test data
            var soort = new Soort()
            {
                Naam = "Boek"
            };
            uow.SoortRepository.Add(soort);
            uow.Save();
            var extra = new Extra()
            {
                Type = "ISBN",
                Soort = soort
            };
            soort.Extras.Add(extra);
            uow.SoortRepository.Update(soort);

            var Functie = new Functie()
            {
                Naam = "Auteur"
            };
            uow.FunctieRepository.Add(Functie);

            LeeftijdsKlasse.ToList().ForEach(l =>
            {
                uow.LeeftijdsKlasseRepository.Add(new LeeftijdsKlasse()
                {
                    Omschrijving = l
                });
            });

            uow.Save();
        }
        public static void ScrapeListPage()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("https://www.hebban.nl/1000/2018");

            IList<HtmlNode> rows = doc.QuerySelectorAll("div .row-fluid .item");

            //Books
            foreach (var row in rows)
            {
                var bookUrl = row.QuerySelector(".neutral").GetAttributeValue("href", "");

                var MediumDetail = CreateMediumDetail(bookUrl);
                if (MediumDetail != null)
                {
                    uow.MediumDetailRepository.Add(MediumDetail);
                    uow.Save();
                    Console.WriteLine($"Boek {MediumDetail.Title} is toegevoegd aan de database!");
                    Console.WriteLine(bookUrl);
                }
            }
            uow.Save();
        }

        public static MediumDetail CreateMediumDetail(string url)
        {
            //Load book page
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);

            //Random
            Random rnd = new Random();

            MediumDetail md = new MediumDetail();
            //Fill title
            var title = GetTitle(doc);
            if (!string.IsNullOrEmpty(title))
            {
                md.Title = title;
                //Fill genres
                var genreString = GetGenre(doc);
                if (!string.IsNullOrEmpty(genreString))
                {
                    if (genreString.Contains("&"))
                    {
                        var genres = genreString.Replace("amp;", "").ToLower().Split('&');
                        genres.ToList().ForEach(gString =>
                        {
                            var g = uow.CategorieRepository.Get(c => c.Omschrijving == gString.ToLower().Trim()).FirstOrDefault();
                            if (g == null)
                                g = CreateCategory(gString);

                            md.MediumCategorieen.Add(new MediumCategorie() { Categorie = g, MediumDetail = md });
                        });
                    }
                    else
                    {
                        var g = uow.CategorieRepository.Get(c => c.Omschrijving == genreString.ToLower().Trim()).FirstOrDefault();
                        if (g == null)
                            g = CreateCategory(genreString);

                        md.MediumCategorieen.Add(new MediumCategorie() { Categorie = g, MediumDetail = md });
                    }
                    //Fill medewerkers
                    var auteur = GetAuthor(doc);
                    if (!string.IsNullOrEmpty(auteur))
                    {
                        var medewerker = uow.MedewerkerRepository.Get(m => m.Naam == auteur.ToLower().Trim()).FirstOrDefault();
                        if (medewerker == null)
                        {
                            medewerker = CreateMedewerker(auteur);
                        }
                        md.MediumDetailMedewerker.Add(new MediumDetailMedewerker() { Medewerker = medewerker, MediumDetail = md });
                        var korteInhoud = getKorteInhoud(doc);
                        var boekSoort = uow.SoortRepository.Get(s => s.Naam == "Boek").First();
                        if (!string.IsNullOrEmpty(korteInhoud))
                        {
                            md.KorteInhoud = korteInhoud;
                            md.Soort = boekSoort;

                            //Fill leeftijdsklasse
                            var leeftijdsklasseString = LeeftijdsKlasse[rnd.Next(0, 5)];
                            md.LeeftijdsKlasse = uow.LeeftijdsKlasseRepository.Get(l => l.Omschrijving == leeftijdsklasseString).First();
                            //Fill EAN
                            string randomEan = string.Empty;
                            for (int i = 0; i < 18; i++)
                                randomEan = String.Concat(randomEan, rnd.Next(10).ToString());
                            md.Ean = randomEan;

                            //Fill Extras
                            var isbn = GetIsbn(doc);
                            if (!string.IsNullOrEmpty(isbn))
                            {
                                var isbnExtra = uow.ExtraRepository.Get(e => e.Type == "ISBN" && e.SoortId == boekSoort.Id).First();
                                md.MediumDetailExtras.Add(new MediumDetailExtra() { Extra = isbnExtra, Beschrijving = isbn, MediumDetail = md });
                                //md.MediumDetailExtras.
                            }
                            return md;
                        }
                    }
                }
            }
            return null;
        }
        public static Categorie CreateCategory(string naam)
        {
            Categorie c = new Categorie()
            {
                Omschrijving = naam.Trim()
            };
            uow.CategorieRepository.Add(c);
            return c;
        }
        public static Medewerker CreateMedewerker(string naam)
        {
            var f = uow.FunctieRepository.Get(_f => _f.Naam == "Auteur").First();
            var m = new Medewerker()
            {
                Naam = naam.Trim(),
                Functie = f
            };
            return m;
        }
        public static string GetTitle(HtmlDocument doc)
        {
            var titleContainer = doc.QuerySelector(".work-article__title");
            if (titleContainer != null)
                return titleContainer.InnerText;
            else
                return null;
        }
        public static string GetGenre(HtmlDocument doc)
        {
            var genreNaamContainer = doc.QuerySelector("[itemprop=\"genre\"]");
            if (genreNaamContainer != null)
                return genreNaamContainer.InnerText;
            else
                return null;
        }
        public static string GetAuthor(HtmlDocument doc)
        {
            var authorContainer = doc.QuerySelector("[itemprop=\"author\"]");
            if (authorContainer != null)
                return authorContainer.InnerText;
            else
                return null;
        }

        public static string getKorteInhoud(HtmlDocument doc)
        {
            var KorteInhoudContainer = doc.QuerySelector("[itemprop=\"description\"]");
            if (KorteInhoudContainer != null)
                return KorteInhoudContainer.InnerText.Trim();
            else
                return null;
        }
        public static string GetIsbn(HtmlDocument doc)
        {
            var isbnNodes = doc.QuerySelectorAll("[itemprop=\"isbn\"]");
            if (isbnNodes != null)
                return isbnNodes[0].InnerText;
            else
                return null;
        }

        public static void CreateMediums()
        {
            var mediumDetails = uow.MediumDetailRepository.Get().ToList();
            mediumDetails.ForEach(md =>
            {
                Random rnd = new Random();
                int max = rnd.Next(1, 5);
                for (int i = 0; i < max; i++)
                {
                    md.Medium.Add(new Medium()
                    {
                        MediumDetail = md,
                        EindeLevensduur = DateTime.Now.AddDays(365 * 5),
                        Registratie = DateTime.Now,
                        Verkoopprijs = 5
                    });
                }
                uow.MediumDetailRepository.Update(md);
            });
            uow.Save();
        }
    }
}
