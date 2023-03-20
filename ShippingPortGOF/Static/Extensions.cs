namespace ShippingPortGOF.Static
{
    public static class Extensions
    {
        public static string Format(this DateTime date) => date.ToString("dd.MM.yyyy. HH:mm:ss");
    }
}