using ShippingPortGOF.Singleton;

namespace ShippingPortGOF.Static
{
    public static class Print
    {
        public static void ErrorParseDouble(string row, string cell, string path)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetModelState($"ERROR: Invalid row '{row}' in file '{path}'."
                 + $" Cell '{cell}' could not be parsed to double. "
                   + $"ERROR count: {++ShippingPortSingleton.GetInstance().ErrorCount}");
        }

        public static void ErrorParseDaysOfWeek(string row, string cell, string path)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetModelState($"ERROR: Invalid row '{row}' in file '{path}'."
                 + $" Cell '{cell}' could not be parsed into days of week. "
                   + $"ERROR count: {++ShippingPortSingleton.GetInstance().ErrorCount}");
        }

        public static void ErrorParseHours(string row, string cell, string path)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetModelState($"ERROR: Invalid row '{row}' in file '{path}'."
                 + $" Cell '{cell}' could not be parsed to day hours. "
                   + $"ERROR count: {++ShippingPortSingleton.GetInstance().ErrorCount}");
        }

        public static void ErrorParseInt(string row, string cell, string path)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetModelState($"ERROR: Invalid row '{row}' in file '{path}'."
                + $" Cell '{cell}' could not be parsed to int. "
                + $"ERROR count: {++ShippingPortSingleton.GetInstance().ErrorCount}");
        }

        public static void ErrorParseDate(string row, string cell, string path)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetModelState($"ERROR: Invalid row '{row}' in file '{path}'."
                + $" Cell '{cell}' could not be parsed to date. "
                + $"ERROR count: {++ShippingPortSingleton.GetInstance().ErrorCount}");
        }

        public static void ErrorParseMoorings(string row, string cell, string path)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetModelState($"ERROR: Invalid row '{row}' in file '{path}'."
                + $" Cell '{cell}' could not be parsed to list of moorings. "
                + $"ERROR count: {++ShippingPortSingleton.GetInstance().ErrorCount}");
        }

        public static void ErrorInvalidHeaderRow(string row, string path)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetModelState($"ERROR: Invalid header row '{row}' in file '{path}'."
            + $" ERROR count: {++ShippingPortSingleton.GetInstance().ErrorCount}");
        }

        public static void ErrorCellsNumber(string row, string path)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetModelState($"ERROR: Invalid row '{row}' in file '{path}'."
                + $" Number of cells is invalid. "
                + $"ERROR count: {++ShippingPortSingleton.GetInstance().ErrorCount}");
        }

        public static void ErrorInvalidType(string row, string cell, string path, string type)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetModelState($"ERROR: Invalid row '{row}' in file '{path}'."
                + $" Cell '{cell}' has invalid {type} type."
                + $"ERROR count: {++ShippingPortSingleton.GetInstance().ErrorCount}");
        }

        public static void ErrorInvalidRecord(string row, string cell, string path, int id, string type)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetModelState($"ERROR: row '{row}' with invalid record in cell '{cell}'"
                + $" in file '{path}'."
                + $" {type} with passed ID {id} does not exist."
                + $"ERROR count: {++ShippingPortSingleton.GetInstance().ErrorCount}");
        }

        public static void ErrorInitialization()
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetModelState($"ERROR: Initialization unsuccessful."
                + $"ERROR count: {++ShippingPortSingleton.GetInstance().ErrorCount}");
        }

        public static void VirtualTime()
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetModelState($"Virtual time: {sps.VirtualTimeOriginator.VirtualTime.Format()}");
        }

        public static void ErrorArguments()
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetModelState("ERROR: Invalid arguments!");
        }

        public static void ErrorCommands()
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetModelState("ERROR: Invalid command!");
        }

        public static void Manual()
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetTextColor(ConsoleColor.Yellow);
            sps.Controller.SetModelState("ShippingPortGOF Commands Manual:");
            sps.Controller.SetTextColor(ConsoleColor.Green);
            sps.Controller.SetModelState("STATUS");
            sps.Controller.SetModelState("Description: Overview of the status (available or taken) " +
                "of ship mooring at the moment of application virtual time.");
            sps.Controller.SetModelState(Environment.NewLine);
            sps.Controller.SetTextColor(ConsoleColor.Green);
            sps.Controller.SetModelState("VIRTUAL TIME dd.mm.yyyy. hh:mm:ss");
            sps.Controller.SetModelState("Description: Changes virtual time with entered date and time.");
            sps.Controller.SetModelState(Environment.NewLine);
            sps.Controller.SetTextColor(ConsoleColor.Green);
            sps.Controller.SetModelState("MOORINGS [mooring_type] [status] [datetime_from] [datetime_until]");
            sps.Controller.SetModelState("Statuses: T - taken, A - available");
            sps.Controller.SetModelState("Datetime format: dd.mm.yyyy. hh:mm:ss");
            sps.Controller.SetModelState("Description: Overview of taken (T) or available (A) ship moorings " +
                "of a certain type in a given time range where the time from and to when the mooring " +
                "is taken (T) or available (A) is also printed.");
            sps.Controller.SetModelState(Environment.NewLine);
            sps.Controller.SetTextColor(ConsoleColor.Green);
            sps.Controller.SetModelState("LOAD RESERVATIONS [filename].csv");
            sps.Controller.SetModelState("Description: Loads the reservation request file.");
            sps.Controller.SetModelState(Environment.NewLine);
            sps.Controller.SetTextColor(ConsoleColor.Green);
            sps.Controller.SetModelState("RESERVED MOORING [ship ID]");
            sps.Controller.SetModelState("Description: Creates a request for enabling/permitting mooring " +
                "to a ship with a specified ID to his reserved mooring.");
            sps.Controller.SetModelState(Environment.NewLine);
            sps.Controller.SetTextColor(ConsoleColor.Green);
            sps.Controller.SetModelState("NON RESERVED MOORING [ship ID] [duration in hours]");
            sps.Controller.SetModelState("Description: Creates a request for enabling/permitting mooring " +
                "to a ship with a specified ID to a non reserved mooring, where the duration of the " +
                "mooring in hours is specified.");
            sps.Controller.SetModelState(Environment.NewLine);
            sps.Controller.SetTextColor(ConsoleColor.Green);
            sps.Controller.SetModelState("CHANNEL [ship ID] [channel frequency] [Q]");
            sps.Controller.SetModelState("Description: Connects a ship with a specified ID to a specified " +
                "communication channel where that ship then has available all information sent over " +
                "that channel. If argument \"Q\" is specified, command disconnects the ship from a specified channel.");
            sps.Controller.SetModelState(Environment.NewLine);
            sps.Controller.SetTextColor(ConsoleColor.Green);
            sps.Controller.SetModelState("PRINT SETTINGS [H | F | SN]");
            sps.Controller.SetModelState("Description: Sets table print settings. The command can receive " +
                "several different options ( H - header, F - footer, SN - sequence number). The order " +
                "of abbreviations is arbitrary.");
            sps.Controller.SetModelState(Environment.NewLine);
            sps.Controller.SetTextColor(ConsoleColor.Green);
            sps.Controller.SetModelState("TAKEN MOORINGS dd.mm.yyyy. hh:mm");
            sps.Controller.SetModelState("Description: Prints the total number of taken ship moorings by type at a given time.");
            sps.Controller.SetModelState(Environment.NewLine);
            sps.Controller.SetTextColor(ConsoleColor.Green);
            sps.Controller.SetModelState("BACKUP \"[name]\"");
            sps.Controller.SetModelState("Description: Performs a backup of the current state of occupancy " +
                "of all ship moorings and the current application virtual time.");
            sps.Controller.SetModelState(Environment.NewLine);
            sps.Controller.SetTextColor(ConsoleColor.Green);
            sps.Controller.SetModelState("RESTORE \"[name]\"");
            sps.Controller.SetModelState("Description: Returns the state of ship moorings to one of the specified " +
                "saved mooring occupancy states and associated virtual time.");
            sps.Controller.SetModelState(Environment.NewLine);
            sps.Controller.SetTextColor(ConsoleColor.Green);
            sps.Controller.SetModelState("SHIP [ship ID]");
            sps.Controller.SetModelState("Description: Checks the status of the ship with the specified ID in the " +
                "current virtual time.");
            sps.Controller.SetModelState(Environment.NewLine);
            sps.Controller.SetTextColor(ConsoleColor.Green);
            sps.Controller.SetModelState("Q");
            sps.Controller.SetModelState("Description: Terminates the program.");
            sps.Controller.SetModelState(Environment.NewLine);
        }

        public static void Mooring(int id, string label, string type, string status)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetModelState(string.Format("|{0,10}|{1,-10}|{2,-10}|{3,-10}|", id, label, type, status));
        }

        public static void Mooring(int sequenceNumber, int id, string label, string type, string status)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetModelState(string.Format("|{0,10}|{1,10}|{2,-10}|{3,-10}|{4,-10}|",
                sequenceNumber, id, label, type, status));
        }

        public static void MooringOfType(int id, string type, string status, string time)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetModelState(string.Format("|{0,10}|{1,-10}|{2,-10}|{3,-45}|", id, type, status, time));
        }

        public static void MooringOfType(int sequenceNumber, int id, string type, string status, string time)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetModelState(string.Format("|{0,10}|{1,10}|{2,-10}|{3,-10}|{4,-45}|",
                sequenceNumber, id, type, status, time));
        }

        public static void MooringHeader()
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            if (ShippingPortSingleton.GetInstance().SequenceNumberPrint)
            {
                sps.Controller.SetModelState($"--------------------Moorings status---------------------");
                sps.Controller.SetModelState(string.Format("|{0,10}|{1,10}|{2,-10}|{3,-10}|{4,-10}|",
                "No.", "ID", "Label", "Type", "Status"));
                sps.Controller.SetModelState($"--------------------------------------------------------");
            }
            else
            {
                sps.Controller.SetModelState($"---------------Moorings status---------------");
                sps.Controller.SetModelState(string.Format("|{0,10}|{1,-10}|{2,-10}|{3,-10}|", "ID", "Label", "Type", "Status"));
                sps.Controller.SetModelState($"---------------------------------------------");
            }
        }

        public static void Footer(int rowCount)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            if (sps.SequenceNumberPrint)
            {
                sps.Controller.SetModelState($"-------------------------------------------------------");
                sps.Controller.SetModelState(string.Format("|{0,52}{1, 0}|", "Number of records :", rowCount));
                sps.Controller.SetModelState($"-------------------------------------------------------");

            }
            else
            {
                sps.Controller.SetModelState($"---------------------------------------------");
                sps.Controller.SetModelState(string.Format("|{0,41}{1, 0}|", "Number of records :", rowCount));
                sps.Controller.SetModelState($"---------------------------------------------");
            }
        }

        public static void MooringOfTypeHeader()
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            if (ShippingPortSingleton.GetInstance().SequenceNumberPrint)
            {
                sps.Controller.SetModelState($"--------------------------------------Moorings status--------------------------------------");
                sps.Controller.SetModelState(string.Format("|{0,10}|{1,10}|{2,-10}|{3,-10}|{4,-45}|",
                "No.", "ID", "Type", "Status", "Time"));
                sps.Controller.SetModelState($"-------------------------------------------------------------------------------------------");
            }
            else
            {
                sps.Controller.SetModelState($"---------------------------------Moorings status--------------------------------");
                sps.Controller.SetModelState(string.Format("|{0,10}|{1,-10}|{2,-10}|{3,-45}|", "ID", "Type", "Status", "Time"));
                sps.Controller.SetModelState($"--------------------------------------------------------------------------------");
            }
        }

        public static void MooringOfTypeFooter(int rowCount)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            if (sps.SequenceNumberPrint)
            {
                sps.Controller.SetModelState($"-------------------------------------------------------------------------------------------");
                sps.Controller.SetModelState(string.Format("|{0,87}{1, 0}|", "Number of records :", rowCount));
                sps.Controller.SetModelState($"-------------------------------------------------------------------------------------------");

            }
            else
            {
                sps.Controller.SetModelState($"--------------------------------------------------------------------------------");
                sps.Controller.SetModelState(string.Format("|{0,76}{1, 0}|", "Number of records :", rowCount));
                sps.Controller.SetModelState($"--------------------------------------------------------------------------------");
            }
        }

        public static void HeaderNotAvailableMooring()
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            if (sps.SequenceNumberPrint)
            {
                sps.Controller.SetModelState($"------------Count of taken moorings by type------------");
                sps.Controller.SetModelState(string.Format("|{0,17}|{1,-17}|{2,17}|",
                "No.", "Type", "Taken count"));
                sps.Controller.SetModelState($"-------------------------------------------------------");
            }
            else
            {
                sps.Controller.SetModelState($"-------Count of taken moorings by type-------");
                sps.Controller.SetModelState(string.Format("|{0,-20}|{1,22}|",
                "Type", "Taken count"));
                sps.Controller.SetModelState($"---------------------------------------------");
            }
        }

        public static void SumTakenMoorings(string type, int takenCount)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetModelState(string.Format("|{0,-20}|{1,22}|", type, takenCount));
        }

        public static void SumTakenMoorings(int sequenceNumber, string type, int takenCount)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            sps.Controller.SetModelState(string.Format("|{0,17}|{1,-17}|{2,17}|",
                sequenceNumber, type, takenCount));
        }
    }
}
