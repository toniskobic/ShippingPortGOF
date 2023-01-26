namespace ShippingPortGOF.Models
{
    public class Terminal
    {
        public int RowsNumber { get; set; }

        public string? WindowsRatio { get; set; }

        public bool OperatingWindowFirst { get; set; }

        public int OperatingWindowStartPosition { get; set; }

        public int OperatingWindowEndPosition { get; set; }

        public int ErrorsWindowStartPosition { get; set; }

        public int ErrorsWindowEndingPosition { get; set; }

        public int MiddlePosition { get; set; }

        public bool LastError { get; set; } = false;
    }
}
