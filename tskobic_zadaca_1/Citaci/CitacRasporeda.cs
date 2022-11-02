using tskobic_zadaca_1.Modeli;
using tskobic_zadaca_1.Singleton;
using tskobic_zadaca_1.Static;

namespace tskobic_zadaca_1.Citaci
{
    public class CitacRasporeda
    {
        public void ProcitajPodatke(string putanja)
        {
            string[] retci = File.ReadAllLines(putanja);
            List<Raspored> rasporedi = new List<Raspored>();

            for (int i = 1; i < retci.Length; i++)
            {
                string[] celije = retci[i].Split(";");
                if (celije.Length != 5)
                {
                    Ispis.GreskaBrojCelija(retci[i], putanja);
                }
                else
                {
                    if (!int.TryParse(celije[0], out int idVez))
                    {
                        Ispis.GreskaPretvorbeUInt(retci[i], celije[0], putanja);
                        continue;
                    }
                    if (!int.TryParse(celije[1], out int idBrod))
                    {
                        Ispis.GreskaPretvorbeUInt(retci[i], celije[1], putanja);
                        continue;
                    }
                    List<string> dani = celije[2].Split(",").ToList();
                    if (dani.Count > 7)
                    {
                        Ispis.GreskaPretvorbeUDane(retci[i], celije[2], putanja);
                        continue;
                    }
                    bool greska = false;
                    List<DayOfWeek> daniUTjednu = new List<DayOfWeek>();
                    foreach (var dan in dani)
                    {
                        if (!int.TryParse(dan, out int rezultat))
                        {
                            greska = true;
                            Ispis.GreskaPretvorbeUDane(retci[i], celije[2], putanja);
                            break;
                        }
                        daniUTjednu.Add((DayOfWeek)rezultat);
                    }
                    if (greska)
                    {
                        continue;
                    }
                    if (dani.FindAll(x => int.Parse(x) < 0 || int.Parse(x) > 6).Count != 0)
                    {
                        Ispis.GreskaPretvorbeUDane(retci[i], celije[2], putanja);
                        continue;
                    }
                    if (!TimeOnly.TryParse(celije[3], out TimeOnly vrijemeOd))
                    {
                        Ispis.GreskaPretvorbeUSate(retci[i], celije[3], putanja);
                        continue;
                    }
                    if (!TimeOnly.TryParse(celije[4], out TimeOnly vrijemeDo))
                    {
                        Ispis.GreskaPretvorbeUSate(retci[i], celije[4], putanja);
                        continue;
                    }
                    if (vrijemeOd >= vrijemeDo)
                    {
                        continue;
                    }
                    BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
                    Brod? brod = bls.BrodskaLuka.Brodovi.Find(x => x.ID == idBrod);
                    Vez? vez = bls.BrodskaLuka.Vezovi.Find(x => x.ID == idVez);
                    if (brod != null && vez != null && ProvjeriBrodIVez(brod, vez))
                    {
                        Raspored? raspored = bls.BrodskaLuka.Rasporedi.Find(x => x.DaniUTjednu.SequenceEqual(daniUTjednu)
                        && x.VrijemeOd == vrijemeOd && x.VrijemeDo == vrijemeDo);
                        if (raspored == null)
                        {
                            bls.BrodskaLuka.Rasporedi.Add(new Raspored(idVez, idBrod, daniUTjednu, vrijemeOd, vrijemeDo));
                        }
                    }
                }
            }
        }

        public bool ProvjeriBrodIVez(Brod brod, Vez vez)
        {
            bool provjera = false;
            string[]? brodovi = null;
            switch (vez.Vrsta)
            {
                case "PU":
                    {
                        brodovi = Konstante.PutnickiBrodovi;
                        break;
                    }
                case "PO":
                    {
                        brodovi = Konstante.PoslovniBrodovi;
                        break;
                    }
                case "OS":
                    {
                        brodovi = Konstante.OstaliBrodovi;
                        break;
                    }
            }
            if (brodovi!.Contains(brod.Vrsta)
                    && vez.MaksDubina >= brod.Gaz && vez.MaksSirina >= brod.Sirina && vez.MaksDuljina >= brod.Duljina)
            {
                provjera = true;

            }

            return provjera;
        }
    }
}
