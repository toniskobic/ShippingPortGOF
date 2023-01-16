namespace tskobic_zadaca_3.Static
{
    public static class Konstante
    {
        public static string UNICODE_ESC { get; } = "\u001b[";

        public static string[] VrsteVezova { get; } = { "PU", "PO", "OS" };

        public static string[] VrsteBrodova { get; } = { "TR", "KA", "KL", "KR", "RI", "TE", "JA", "BR", "RO" };

        public static string[] PutnickiBrodovi { get; } = { "TR", "KA", "KL", "KR" };

        public static string[] PoslovniBrodovi { get; } = { "RI", "TE" };

        public static string[] OstaliBrodovi { get; } = { "JA", "BR", "RO" };

        private static string ArgumentR { get; } = @"(-r [a-zA-Z_0-9.]+\.csv)";

        private static string ObavezniArgumenti { get; } = @"(-(?:(?:(?:br) \d{2})|(?:(?:vt) "
        + @"(?:(?:50:50)|(?:25:75)|(?:75:25)){1})|(?:(?:pd) (?:(?:R:P)|(?:P:R)){1})|(?:(?:(?:mv)"
        + @"|[lvbmk]){1} [a-zA-Z_0-9.]+\.csv)){1})";

        public static string UlazniArgumenti { get; } = @"^(?:" + $"{ArgumentR}"
            + @"(?!.*\1) )?" + $"{ObavezniArgumenti}" + @"(?!.*\2)(?: " + $"{ArgumentR}"
            + @"(?!.*\3))?(?: " + $"{ObavezniArgumenti}" + @"(?!.*\4))(?: " + $"{ArgumentR}"
            + @"(?!.*\5))?(?: " + $"{ObavezniArgumenti}" + @"(?!.*\6))(?: " + $"{ArgumentR}"
            + @"(?!.*\7))?(?: " + $"{ObavezniArgumenti}" + @"(?!.*\8))(?: " + $"{ArgumentR}"
            + @"(?!.*\9))?(?: " + $"{ObavezniArgumenti}" + @"(?!.*\10))(?: " + $"{ArgumentR}"
            + @"(?!.*\11))?(?: " + $"{ObavezniArgumenti}" + @"(?!.*\12))(?: " + $"{ArgumentR}"
            + @"(?!.*\13))?(?: " + $"{ObavezniArgumenti}" + @"(?!.*\14))(?: " + $"{ArgumentR}"
            + @"(?!.*\15))?(?: " + $"{ObavezniArgumenti}" + @"(?!.*\16))(?: " + $"{ArgumentR}"
            + @"(?!.*\17))?(?: " + $"{ObavezniArgumenti}" + @"(?!.*\18))(?: " + $"{ArgumentR}"
            + @"(?!.*\19))?$";

        public static string VirtualnoVrijeme { get; } = "^VR ([1-9]|([012][0-9])|(3[01]))."
            + @"([0]{0,1}[1-9]|1[012]).\d\d\d\d. ([0-1]?[0-9]|2?[0-3]):([0-5]\d):([0-5]\d)$";

        public static string ZahtjevRezervacije { get; } = @"^UR ([a-zA-Z_0-9.]+\.csv)$";

        public static string ZahtjevRezPriveza { get; } = @"^ZD \d{1,9}$";

        public static string ZahtjevSlobPriveza { get; } = @"^ZP \d{1,9} ([0]?[1-9]|[1][0-9]|2[0-3])$$";

        public static string IspisVezova { get; } = @"^V ([A-Z]{2}) (S|Z)( ([1-9]|([012][0-9])|(3[01]))."
            + @"([0]{0,1}[1-9]|1[012]).\d\d\d\d. ([0-1]?[0-9]|2?[0-3]):([0-5]\d):([0-5]\d)){2}$";

        public static string InformativniRedak { get; } = @"^([a-zA-Z_]+)(\;[a-zA-Z_]+)*$";

        public static string Vezovi { get; } = @"^([0-9]+){1}(,[0-9]+)*$";

        public static string UredjenjeIspisa { get; } = @"^T(?: ((?:RB)|[ZP]){1}(?!.*\1))?"
            + @"(?: ((?:RB)|[ZP]){1}(?!.*\2))?(?: ((?:RB)|[ZP]){1}(?!.*\3))?$";

        public static string IspisZauzetihVezova { get; } = @"^ZA ([1-9]|([012][0-9])|(3[01]))."
            + @"([0]{0,1}[1-9]|1[012]).\d\d\d\d. ([0-1]?[0-9]|2?[0-3]):([0-5]\d)$";

        public static string SpajanjeNaKanal { get; } = @"^F \d{1,9} \d{1,9}(?: Q)*$";

        public static string StatusBroda { get; } = @"^B \d{1,9}$";

        public static string SpremanjePostojecegStanja { get; } = @"^SPS "".+""$";

        public static string VracanjeStanjaVezova { get; } = @"^VPS "".+""$";
    }
}
