using System.Text.RegularExpressions;
using tskobic_zadaca_3.Composite;
using tskobic_zadaca_3.Modeli;
using tskobic_zadaca_3.Observer;
using tskobic_zadaca_3.Singleton;
using tskobic_zadaca_3.Static;
using tskobic_zadaca_3.Visitor;

namespace tskobic_zadaca_3.Citaci
{
    public class CitacRezervacija : ICitac
    {
        public void ProcitajPodatke(string putanja)
        {
            try
            {
                if (!ProvjeriInformativniRedak(putanja))
                {
                    return;
                }
                string[] retci = File.ReadAllLines(putanja);

                for (int i = 0; i < retci.Length; i++)
                {
                    string[] celije = retci[i].Split(";");
                    if (celije.Length != 3)
                    {
                        Ispis.GreskaBrojCelija(retci[i], putanja);
                    }
                    else
                    {
                        if (!int.TryParse(celije[0], out int idBrod))
                        {
                            Ispis.GreskaPretvorbeUInt(retci[i], celije[0], putanja);
                            continue;
                        }
                        if (!Utils.ProvjeriPretvorbuUDatum(celije[1], out DateTime datumOd))
                        {
                            Ispis.GreskaPretvorbeUDatum(retci[i], celije[1], putanja);
                            continue;
                        }
                        if (!int.TryParse(celije[2], out int satiTrajanjaPriveza))
                        {
                            Ispis.GreskaPretvorbeUInt(retci[i], celije[2], putanja);
                            continue;
                        }
                        TimeOnly vrijemeOd = TimeOnly.FromTimeSpan(datumOd.TimeOfDay);
                        if (vrijemeOd >= vrijemeOd.AddHours(satiTrajanjaPriveza))
                        {
                            continue;
                        }
                        TimeOnly vrijemeDo = vrijemeOd.AddHours(satiTrajanjaPriveza);
                        BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
                        Brod? brod = bls.BrodskaLuka!.Brodovi.Find(x => x.ID == idBrod);
                        if (brod != null)
                        {
                            List<Vez> vezovi = Utils.PronadjiVezove(brod);
                            if (vezovi.Count > 0)
                            {
                                List<Vez> fVezoviRasporedi = vezovi.FindAll(vez => bls.BrodskaLuka.Rasporedi.Any(x => x.IdVez == vez.ID
                                && x.DaniUTjednu.Contains(datumOd.DayOfWeek) && x.VrijemeOd <= vrijemeDo && vrijemeOd <= x.VrijemeDo));

                                List<Vez> fVezoviRezervacije = vezovi.FindAll(vez => bls.BrodskaLuka.Rezervacije.Any(x => x.IdVez == vez.ID
                                && x.DatumOd.Date == datumOd.Date
                                && TimeOnly.FromTimeSpan(x.DatumOd.TimeOfDay) <= vrijemeDo
                                && vrijemeOd <= TimeOnly.FromTimeSpan(x.DatumOd.TimeOfDay).AddHours(x.SatiTrajanja)));

                                List<Vez> slobodniVezovi = vezovi.Except(fVezoviRasporedi).Except(fVezoviRezervacije).ToList();

                                if (slobodniVezovi.Count > 0)
                                {
                                    Vez vez = Utils.PronadjiNajekonomicnijiVez(vezovi);
                                    bls.BrodskaLuka.Rezervacije.Add(new Rezervacija(vez.ID, idBrod, datumOd, satiTrajanjaPriveza));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public bool ProvjeriInformativniRedak(string putanja)
        {
            Regex rg = new Regex(Konstante.InformativniRedak);
            string redak = File.ReadLines(putanja).First();
            if (!rg.IsMatch(redak))
            {
                Ispis.GreskaNeispravanInformativniRedak(redak, putanja);
                return false;
            }

            return true;
        }
    }
}
