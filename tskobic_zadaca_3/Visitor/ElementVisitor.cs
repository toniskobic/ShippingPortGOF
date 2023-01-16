namespace tskobic_zadaca_3.Visitor
{
    public class ElementVisitor : IElementVisitor
    {
        public DateTime Datum { get; set; }

        public ElementVisitor(DateTime datum)
        {
            Datum = datum;
        }

        public int? Visit(Privez privez)
        {
            if (privez.VrijemeOd <= Datum.AddSeconds(60) && Datum <= privez.VrijemeDo)
            {
                return privez.IdVez;
            }
            return null;
        }

        public int? Visit(Raspored raspored)
        {
            TimeOnly vrijeme = TimeOnly.FromTimeSpan(Datum.TimeOfDay);
            DayOfWeek dan = Datum.DayOfWeek;

            if (raspored.DaniUTjednu.Contains(dan) && raspored.VrijemeOd <= vrijeme.AddMinutes(1)
                && vrijeme <= raspored.VrijemeDo)
            {
                return raspored.IdVez;
            }
            return null;
        }

        public int? Visit(Rezervacija rezervacija)
        {
            TimeOnly vrijeme = TimeOnly.FromTimeSpan(Datum.TimeOfDay);
            TimeOnly vrijemeOd = TimeOnly.FromTimeSpan(rezervacija.DatumOd.TimeOfDay);
            TimeOnly vrijemeDo = vrijemeOd.AddHours(rezervacija.SatiTrajanja);

            if (rezervacija.DatumOd.Date == Datum.Date && vrijemeOd <= vrijeme.AddMinutes(1) && vrijeme <= vrijemeDo)
            {
                return rezervacija.IdVez;
            }
            return null;
        }
    }
}
