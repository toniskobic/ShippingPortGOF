using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tskobic_zadaca_1.Static
{
    public static class Konstante
    {
        public static string[] VrsteVezova { get; } = { "PU", "PO", "OS" };

        public static string[] VrsteBrodova { get; } = { "TR", "KA", "KL", "KR", "RI", "TE", "JA", "BR", "RO" };

        public static string[] PutnickiBrodovi { get; } = { "TR", "KA", "KL", "KR" };

        public static string[] PoslovniBrodovi { get; } = { "RI", "TE", };

        public static string[] OstaliBrodovi { get; } = { "JA", "BR", "RO" };
    }
}
