using tskobic_zadaca_3.Iterator;
using tskobic_zadaca_3.Modeli;
using tskobic_zadaca_3.Observer;
using tskobic_zadaca_3.Visitor;

namespace tskobic_zadaca_3.Composite
{
    public class BrodskaLukaComposite : IComponent
    {
        #region Svojstva
        public List<IComponent> Children { get; set; }

        public int ID { get; set; }

        public string Naziv { get; set; }

        public double GPSSirina { get; set; }

        public double GPSVisina { get; set; }

        public int DubinaLuke { get; set; }

        public int PutnickiVezovi { get; set; }

        public int PoslovniVezovi { get; set; }

        public int OstaliVezovi { get; set; }

        public List<Kanal> Kanali { get; set; }

        public List<Brod> Brodovi { get; set; }

        public List<Raspored> Rasporedi { get; set; }

        public List<Rezervacija> Rezervacije { get; set; }

        public List<Privez> Privezi { get; set; }

        public List<Zapis> Dnevnik { get; set; }
        #endregion

        #region Konstruktor
        public BrodskaLukaComposite(int id, string naziv, double gpsSirina, double gpsVisina, int dubinaLuke,
            int putnickiVezovi, int poslovniVezovi, int ostaliVezovi)
        {
            Children = new List<IComponent>();
            ID = id;
            Naziv = naziv;
            GPSSirina = gpsSirina;
            GPSVisina = gpsVisina;
            DubinaLuke = dubinaLuke;
            PutnickiVezovi = putnickiVezovi;
            PoslovniVezovi = poslovniVezovi;
            OstaliVezovi = ostaliVezovi;
            Kanali = new List<Kanal>();
            Brodovi = new List<Brod>();
            Rasporedi = new List<Raspored>();
            Rezervacije = new List<Rezervacija>();
            Privezi = new List<Privez>();
            Dnevnik = new List<Zapis>();
        }
        #endregion

        #region Metode
        public void Add(IComponent component)
        {
            Children.Add(component);
        }

        public void Remove(IComponent component)
        {
            Children.Remove(component);
        }

        public void RemoveAll(Predicate<IComponent> match)
        {
            Children.RemoveAll(match);
        }

        public int GetId()
        {
            return ID;
        }

        public string GetName()
        {
            return Naziv;
        }

        public bool IsComposite()
        {
            return true;
        }

        public List<IComponent> Find(Func<IComponent, bool> predicate)
        {
            IIterator iterator = CreateIterator();
            List<IComponent> list = new List<IComponent>();
            while (iterator.HasNext())
            {
                IComponent? component = (IComponent?)iterator.Next();
                if(predicate.Invoke(component!))
                {
                    list.Add(component!);
                }
            }

            return list;
        }

        public IIterator CreateIterator()
        {
            return new ComponentIterator(Children.GetEnumerator());
        }
        #endregion
    }
}
