namespace ShippingPortGOF.Static
{
    public static class Constants
    {
        public static string[] MooringTypes { get; } = { "PU", "PO", "OS" };

        public static string[] ShipTypes { get; } = { "TR", "KA", "KL", "KR", "RI", "TE", "JA", "BR", "RO" };

        public static string[] PassengerShips { get; } = { "TR", "KA", "KL", "KR" };

        public static string[] BusinessShips { get; } = { "RI", "TE" };

        public static string[] OtherShips { get; } = { "JA", "BR", "RO" };

        public static string InputArguments { get; } = @"^(?:(-s [a-zA-Z_0-9.]+\.csv)(?!.*\1) )?"
          + @"(-(?:(?:dm)|[pmbdc]){1} [a-zA-Z_0-9.]+\.csv)(?!.*\2)(?: (-s [a-zA-Z_0-9.]+\.csv)(?!.*\3))?"
          + @"(?: (-(?:(?:dm)|[pmbdc]){1} [a-zA-Z_0-9.]+\.csv)(?!.*\4))(?: (-s [a-zA-Z_0-9.]+\.csv)(?!.*\5))?"
          + @"(?: (-(?:(?:dm)|[pmbdc]){1} [a-zA-Z_0-9.]+\.csv)(?!.*\6))(?: (-s [a-zA-Z_0-9.]+\.csv)(?!.*\7))?"
          + @"(?: (-(?:(?:dm)|[pmbdc]){1} [a-zA-Z_0-9.]+\.csv)(?!.*\8))(?: (-s [a-zA-Z_0-9.]+\.csv)(?!.*\9))?"
          + @"(?: (-(?:(?:dm)|[pmbdc]){1} [a-zA-Z_0-9.]+\.csv)(?!.*\10))(?: (-s [a-zA-Z_0-9.]+\.csv)(?!.*\11))?"
          + @"(?: (-(?:(?:dm)|[pmbdc]){1} [a-zA-Z_0-9.]+\.csv)(?!.*\12))(?: (-s [a-zA-Z_0-9.]+\.csv)(?!.*\13))?$";

        public static string VirtualTime { get; } = "^VIRTUAL TIME ([1-9]|([012][0-9])|(3[01]))."
            + @"([0]{0,1}[1-9]|1[012]).\d\d\d\d. ([0-1]?[0-9]|2?[0-3]):([0-5]\d):([0-5]\d)$";

        public static string ReservationRequest { get; } = @"^LOAD RESERVATIONS ([a-zA-Z_0-9.]+\.csv)$";

        public static string MooringReservationRequest { get; } = @"^RESERVED MOORING \d{1,9}$";

        public static string AvailableMooringRequest { get; } = @"^NON RESERVED MOORING \d{1,9} ([0]?[1-9]|[1][0-9]|2[0-3])$$";

        public static string MooringsPrint { get; } = @"^MOORINGS ([A-Z]{2}) (A|T)( ([1-9]|([012][0-9])|(3[01]))."
            + @"([0]{0,1}[1-9]|1[012]).\d\d\d\d. ([0-1]?[0-9]|2?[0-3]):([0-5]\d):([0-5]\d)){2}$";

        public static string HeaderRow { get; } = @"^([a-zA-Z_]+)(\;[a-zA-Z_]+)*$";

        public static string Moorings { get; } = @"^([0-9]+){1}(,[0-9]+)*$";

        public static string PrintFormat { get; } = @"^PRINT SETTINGS(?: ((?:SN)|[HF]){1}(?!.*\1))?"
            + @"(?: ((?:SN)|[HF]){1}(?!.*\2))?(?: ((?:SN)|[HF]){1}(?!.*\3))?$";

        public static string PrintTakenMoorings { get; } = @"^TAKEN MOORINGS ([1-9]|([012][0-9])|(3[01]))."
            + @"([0]{0,1}[1-9]|1[012]).\d\d\d\d. ([0-1]?[0-9]|2?[0-3]):([0-5]\d)$";

        public static string ChannelConnection { get; } = @"^CHANNEL \d{1,9} \d{1,9}(?: Q)*$";

        public static string ShipStatus { get; } = @"^SHIP \d{1,9}$";

        public static string StateBackup { get; } = @"^BACKUP "".+""$";

        public static string StateRestore { get; } = @"^RESTORE "".+""$";
    }
}
