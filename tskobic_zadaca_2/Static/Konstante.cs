namespace tskobic_zadaca_2.Static
{
    public static class Konstante
    {
        public static string[] VrsteVezova { get; } = { "PU", "PO", "OS" };

        public static string[] VrsteBrodova { get; } = { "TR", "KA", "KL", "KR", "RI", "TE", "JA", "BR", "RO" };

        public static string[] PutnickiBrodovi { get; } = { "TR", "KA", "KL", "KR" };

        public static string[] PoslovniBrodovi { get; } = { "RI", "TE" };

        public static string[] OstaliBrodovi { get; } = { "JA", "BR", "RO" };

        public static string UlazniArgumenti { get; } = @"^(?:(-r [a-zA-Z_0-9.]+\.csv)(?!.*\1) )?"
            + @"(-(?:(?:mv)|[lvbmk]){1} [a-zA-Z_0-9.]+\.csv)(?!.*\2)(?: (-r [a-zA-Z_0-9.]+\.csv)(?!.*\3))?"
            + @"(?: (-(?:(?:mv)|[lvbmk]){1} [a-zA-Z_0-9.]+\.csv)(?!.*\4))(?: (-r [a-zA-Z_0-9.]+\.csv)(?!.*\5))?"
            + @"(?: (-(?:(?:mv)|[lvbmk]){1} [a-zA-Z_0-9.]+\.csv)(?!.*\6))(?: (-r [a-zA-Z_0-9.]+\.csv)(?!.*\7))?"
            + @"(?: (-(?:(?:mv)|[lvbmk]){1} [a-zA-Z_0-9.]+\.csv)(?!.*\8))(?: (-r [a-zA-Z_0-9.]+\.csv)(?!.*\9))?"
            + @"(?: (-(?:(?:mv)|[lvbmk]){1} [a-zA-Z_0-9.]+\.csv)(?!.*\10))(?: (-r [a-zA-Z_0-9.]+\.csv)(?!.*\11))?"
            + @"(?: (-(?:(?:mv)|[lvbmk]){1} [a-zA-Z_0-9.]+\.csv)(?!.*\12))(?: (-r [a-zA-Z_0-9.]+\.csv)(?!.*\13))?$";

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
    }
}
