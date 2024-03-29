﻿using System.Text.RegularExpressions;
using ShippingPortGOF.ChainOfResponsibility;
using ShippingPortGOF.Composite;
using ShippingPortGOF.FactoryMethod;
using ShippingPortGOF.Models;
using ShippingPortGOF.MVC;
using ShippingPortGOF.Observer;
using ShippingPortGOF.Singleton;
using ShippingPortGOF.Static;
using ShippingPortGOF.Visitor;

namespace ShippingPortGOF
{
    public class Program
    {
        private static bool Terminated { get; set; } = false;

        #region Methods
        private static bool Initialization(List<Group> groups)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            LoadData(new ShippingPortCreator(), groups, "-p");
            if (sps.ShippingPort == null)
            {
                return false;
            }

            LoadData(new PiersCreator(), groups, "-d");
            List<IComponent> piers = sps.ShippingPort!.Find(c => c is Pier).ToList();
            if (piers.Count == 0)
            {
                return false;
            }

            LoadData(new MooringsCreator(), groups, "-m");
            List<IComponent> moorings = sps.ShippingPort!.Find(c => c is Mooring);
            if (moorings.Count == 0)
            {
                return false;
            }

            LoadData(new MooringsPiersCreator(), groups, "-dm");
            moorings = sps.ShippingPort!.Find(c => c is Mooring);
            if (!moorings.Exists(v => ((Mooring)v).IdPier != null))
            {
                return false;
            }
            sps.ShippingPort.RemoveAll(c => c is Mooring mooring && mooring.IdPier == null);
            moorings = sps.ShippingPort!.Find(c => c is Mooring);

            LoadData(new ChannelsCreator(), groups, "-c");
            if (sps.ShippingPort.Channels.Count == 0)
            {
                return false;
            }

            LoadData(new ShipsCreator(), groups, "-b");
            if (sps.ShippingPort.Ships.Count == 0)
            {
                return false;
            }

            LoadData(new SchedulesCreator(), groups, "-s");
            return true;
        }

        private static void LoadData(Creator creator, List<Group> groups, string option)
        {
            Group? group = groups.Find(x => x.Value.StartsWith(option));
            if (group != null)
            {
                string[] data = group!.Value.Split(" ");
                creator.ReadData(data[1]);
            }
        }

        private static void MoorReservedShip(int idShip)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            Ship? ship = sps.ShippingPort!.Ships.Find(x => x.ID == idShip);
            if (ship == null)
            {
                sps.Controller.SetModelState($"Ship with the entered ID {idShip} does not exist.");
                return;
            }
            List<Channel> channels = sps.ShippingPort.Channels;
            Channel? channel = channels.Find(x => x.Observers.Contains(ship));
            if (channel == null)
            {
                sps.Controller.SetModelState($"Ship with the entered ID {idShip} mora biti u komunikaciji "
                    + $"the Harbour Master's Office to request the mooring of the ship.");
                return;
            }
            DateTime date = sps.VirtualTimeOriginator.VirtualTime;
            TimeOnly time = TimeOnly.FromTimeSpan(date.TimeOfDay);
            DayOfWeek day = date.DayOfWeek;
            MooredShip? existingMooredShip = sps.ShippingPort.MooredShips.Find(x => x.IdShip == idShip
                && x.DateFrom <= date && date <= x.DateTo);
            string message = "";
            if (existingMooredShip == null)
            {
                Schedule? schedule = sps.ShippingPort.Schedules.Find(x => x.IdShip == idShip
                    && x.DaysOfWeek.Contains(day) && x.TimeFrom <= time && time <= x.TimeTo);
                if (schedule != null)
                {
                    DateTime timeFrom = date.Date.Add(schedule.TimeTo.ToTimeSpan());
                    MooredShip mooredShip = new MooredShip(schedule.IdMooring, idShip, date, timeFrom);
                    sps.ShippingPort.MooredShips.Add(mooredShip);
                    sps.ShippingPort.RecordLog.Add(new Record(RequestType.ZD, idShip, false, date, timeFrom));
                    WriteMessage(channel, $"Ship {ship.ID} is approved for tieing down on mooring {schedule.IdMooring}.");
                    sps.Controller.SetModelState("Command successful.");
                    return;
                }
                else
                {
                    Reservation? reservation = sps.ShippingPort.Reservations.Find(x => x.IdShip == idShip
                        && x.DateFrom.Date.Equals(date.Date) && TimeOnly.FromDateTime(x.DateFrom) <= time
                        && time <= TimeOnly.FromDateTime(x.DateFrom).AddHours(x.HoursDuration));
                    if (reservation != null)
                    {
                        DateTime dateFrom = reservation.DateFrom.AddHours(reservation.HoursDuration);
                        MooredShip mooredShip = new MooredShip(reservation.IdMooring, idShip,
                            date, dateFrom);
                        sps.ShippingPort.MooredShips.Add(mooredShip);
                        sps.ShippingPort.RecordLog.Add(new Record(RequestType.ZD, idShip, false, date, dateFrom));
                        WriteMessage(channel, $"Ship {ship.ID} is approved for tieing down on mooring {reservation.IdMooring}.");
                        sps.Controller.SetModelState("Command successful.");
                        return;
                    }
                    else
                    {
                        message = $"Ship {ship.ID} is denied for mooring, reservation does not exist.";
                    }
                }
            }
            else
            {
                message = $"Ship {ship.ID} is denied for mooring, it is already moored.";
            }
            sps.ShippingPort.RecordLog.Add(new Record(RequestType.ZD, idShip, true));
            WriteMessage(channel, $"{message}");
            sps.Controller.SetModelState(message);
        }

        private static void MoorNotReservedShip(int idShip, int duration)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            Ship? ship = sps.ShippingPort!.Ships.Find(x => x.ID == idShip);
            if (ship == null)
            {
                sps.Controller.SetModelState($"Ship with the ID {idShip} does not exist.");
                return;
            }
            List<Channel> channels = sps.ShippingPort.Channels;
            Channel? channel = channels.Find(x => x.Observers.Contains(ship));
            if (channel == null)
            {
                sps.Controller.SetModelState($"Ship with the ID {idShip} must be in a communication with "
                    + $"the Harbour Master's Office to request the mooring of the ship.");
                return;
            }
            DateTime date = sps.VirtualTimeOriginator.VirtualTime;
            TimeOnly time = TimeOnly.FromTimeSpan(date.TimeOfDay);
            DayOfWeek day = date.DayOfWeek;
            MooredShip? existingMooredShip = sps.ShippingPort.MooredShips.Find(ms => ms.IdShip == idShip
                && ms.DateFrom <= date && date <= ms.DateTo);
            string message = "";
            if (existingMooredShip == null)
            {
                List<Mooring> moorings = Utils.FindMoorings(ship);
                if (moorings.Count > 0)
                {
                    List<Mooring> fMooringsMooredShips = moorings.FindAll(m => sps.ShippingPort.MooredShips.Any(x => x.IdMooring == m.ID
                        && x.DateFrom <= date.AddHours(duration) && date <= x.DateTo));

                    List<Mooring> fMooringsSchedules = moorings.FindAll(m => sps.ShippingPort.Schedules
                        .Any(x => x.IdMooring == m.ID
                        && x.DaysOfWeek.Contains(day)
                        && x.TimeFrom <= time.AddHours(duration) && time <= x.TimeTo));

                    List<Mooring> fMooringsReservations = moorings.FindAll(m => sps.ShippingPort.Reservations
                        .Any(x => x.IdMooring == m.ID
                        && x.DateFrom.Date == date.Date
                        && TimeOnly.FromTimeSpan(x.DateFrom.TimeOfDay) <= time.AddHours(duration)
                        && time <= TimeOnly.FromTimeSpan(x.DateFrom.TimeOfDay).AddHours(x.HoursDuration)));

                    List<Mooring> availableMoorings = moorings.Except(fMooringsMooredShips)
                        .Except(fMooringsSchedules).Except(fMooringsReservations).ToList();

                    if (availableMoorings.Count > 0)
                    {
                        Mooring mooring = Utils.FindOptimalMooring(availableMoorings);
                        MooredShip mooredShip = new MooredShip(mooring.ID, idShip, date, date.AddHours(duration));
                        sps.ShippingPort.MooredShips.Add(mooredShip);
                        sps.ShippingPort.RecordLog.Add(new Record(RequestType.ZP, idShip,
                            false, date, date.AddHours(duration)));
                        WriteMessage(channel, $"Ship with the ID {ship.ID} is approved for tieing down on mooring {mooring.ID}.");
                        sps.Controller.SetModelState("Command successful.");
                        return;
                    }
                    else
                    {
                        message = $"Ship {ship.ID} is denied for mooring, there are no available moorings";
                    }
                }
                else
                {
                    message = $"Ship {ship.ID} is denied for mooring, suitable mooring does not exist.";
                }
            }
            else
            {
                message = $"Ship {ship.ID} is denied for mooring, ship is already moored.";
            }
            sps.ShippingPort.RecordLog.Add(new Record(RequestType.ZP, idShip, true));
            WriteMessage(channel, $"{message}");
            sps.Controller.SetModelState(message);
        }

        private static void PrintMooringsStatus()
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            List<Mooring> moorings = sps.ShippingPort!.Find(c => c is Mooring).Cast<Mooring>().ToList();

            DateTime date = sps.VirtualTimeOriginator.VirtualTime;
            TimeOnly time = TimeOnly.FromTimeSpan(date.TimeOfDay);
            DayOfWeek day = date.DayOfWeek;

            List<Mooring> fMooringsMooredShips = moorings.FindAll(m => sps.ShippingPort.MooredShips.Any(ms => ms.IdMooring == m.ID
                && ms.DateFrom <= date && date <= ms.DateTo));

            List<Mooring> fMooringsSchedules = moorings.FindAll(m => sps.ShippingPort.Schedules.Any(s => s.IdMooring == m.ID
                 && s.DaysOfWeek.Contains(day) && s.TimeFrom <= time && time <= s.TimeTo));

            List<Mooring> fMooringsReservations = moorings.FindAll(m => sps.ShippingPort.Reservations.Any(r => r.IdMooring == m.ID
                && r.DateFrom.Date == date.Date
                && TimeOnly.FromTimeSpan(r.DateFrom.TimeOfDay) <= time
                && time <= TimeOnly.FromTimeSpan(r.DateFrom.TimeOfDay).AddHours(r.HoursDuration)));

            List<Mooring> takenMoorings = fMooringsMooredShips.Union(fMooringsSchedules)
                .Union(fMooringsReservations).Distinct().ToList();
            List<Mooring> availableMoorings = moorings.Except(takenMoorings).ToList();

            if (sps.HeaderPrint)
            {
                Print.MooringHeader();
            }
            PrintMoorings(availableMoorings, "Available");
            PrintMoorings(takenMoorings, "Taken");
            if (sps.FooterPrint)
            {
                int rowsCount = availableMoorings.Count + takenMoorings.Count;
                Print.Footer(rowsCount);
            }
        }

        private static void PrintMoorings(List<Mooring> moorings, string status)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();

            for (int i = 0; i < moorings.Count; i++)
            {
                if (sps.SequenceNumberPrint)
                {
                    Print.Mooring(i + 1, moorings[i].ID, moorings[i].Label, moorings[i].Type, status);
                }
                else
                {
                    Print.Mooring(moorings[i].ID, moorings[i].Label, moorings[i].Type, status);

                }
            }
        }

        private static void PrintMooringsOfType(List<MooringTaken> moorings, string type, string status)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();

            for (int i = 0; i < moorings.Count; i++)
            {
                if (sps.SequenceNumberPrint)
                {
                    Print.MooringOfType(i + 1, moorings[i].ID, type, status, $"{moorings[i].TakenFrom.Format()} - {moorings[i].TakenUntil.Format()}");
                }
                else
                {
                    Print.MooringOfType(moorings[i].ID, type, status, $"{moorings[i].TakenFrom.Format()} - {moorings[i].TakenUntil.Format()}");
                }
            }
        }

        private static void PrintFormat(string input)
        {
            List<string> options = input.Split(" ").ToList();
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            if (options.Count == 2)
            {
                sps.HeaderPrint = false;
                sps.FooterPrint = false;
                sps.SequenceNumberPrint = false;
            }
            else
            {
                sps.HeaderPrint = options.Contains("H");
                sps.FooterPrint = options.Contains("F");
                sps.SequenceNumberPrint = options.Contains("SN");
            }
        }

        private static List<MooringTaken> CalculateTakenMoorings(string type, DateTime dateFrom, DateTime dateTo)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();

            List<Mooring> moorings = sps.ShippingPort!.Find(c => c is Mooring mooring && mooring.Type == type)
                .Cast<Mooring>().ToList();

            List<MooringTaken> takenMoorings = new List<MooringTaken>();

            foreach (var m in moorings)
            {
                List<MooredShip> mooredShips = sps.ShippingPort.MooredShips.Where(ms => ms.IdMooring == m.ID).ToList();
                foreach (var ms in mooredShips)
                {
                    if (ms.DateFrom <= dateTo && dateFrom <= ms.DateTo)
                    {
                        takenMoorings.Add(new MooringTaken(m.ID, ms.DateFrom, ms.DateTo));
                    }
                }

                List<Schedule> schedules = sps.ShippingPort.Schedules.Where(s => s.IdMooring == m.ID).ToList();
                foreach (var schedule in schedules)
                {
                    for (var day = dateFrom.Date; day.Date <= dateTo.Date; day = day.AddDays(1))
                    {
                        var timeFrom = TimeOnly.MinValue;
                        var timeTo = TimeOnly.MaxValue;
                        if (day.Date == dateFrom.Date)
                        {
                            timeFrom = TimeOnly.FromDateTime(dateFrom);
                        }
                        if (day.Date == dateTo.Date)
                        {
                            timeTo = TimeOnly.FromDateTime(dateTo);
                        }
                        if (schedule!.DaysOfWeek.Contains(day.DayOfWeek)
                            && schedule.TimeFrom <= timeTo && timeFrom <= schedule.TimeTo)
                        {
                            var date = DateOnly.FromDateTime(day);
                            takenMoorings.Add(new MooringTaken(m.ID, date.ToDateTime(schedule.TimeFrom), date.ToDateTime(schedule.TimeTo)));
                        }
                    }
                }

                List<Reservation> reservations = sps.ShippingPort.Reservations.Where(r => r.IdMooring == m.ID).ToList();
                foreach (var r in reservations)
                {
                    if (r.DateFrom <= dateTo && dateFrom <= r.DateFrom.AddHours(r.HoursDuration))
                    {
                        takenMoorings.Add(new MooringTaken(m.ID, r.DateFrom, r.DateFrom.AddHours(r.HoursDuration)));
                    }
                }
            }

            return takenMoorings;
        }

        private static List<MooringTaken> CalculateAvailableMoorings(string type, DateTime dateFrom, DateTime dateTo)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();

            List<Mooring> moorings = sps.ShippingPort!.Find(c => c is Mooring mooring && mooring.Type == type)
                .Cast<Mooring>().ToList();
            List<MooringTaken> takenMoorings = CalculateTakenMoorings(type, dateFrom, dateTo);
            List<MooringTaken> availableMoorings = new List<MooringTaken>();
            takenMoorings.Sort((x, y) => x.TakenFrom.CompareTo(y.TakenFrom));

            foreach (var mooring in moorings)
            {
                DateTime startDate = dateFrom;
                DateTime endDate = dateTo;
                var currentTakenMoorings = takenMoorings.Where(tm => tm.ID == mooring.ID).ToList();
                if (currentTakenMoorings.Count == 0)
                {
                    availableMoorings.Add(new MooringTaken(mooring.ID, dateFrom, dateTo));
                }
                foreach (var m in currentTakenMoorings)
                {
                    if (startDate >= m.TakenFrom) startDate = m.TakenUntil;
                    else
                    {
                        endDate = m.TakenFrom;
                        availableMoorings.Add(new MooringTaken(m.ID, startDate, endDate));
                        startDate = m.TakenUntil;
                        if (currentTakenMoorings.Last() == m)
                        {
                            availableMoorings.Add(new MooringTaken(m.ID, startDate, dateTo));
                        }
                    }
                }
            }

            return availableMoorings;
        }

        private static void PrintMooringsStatusOfType(string type, string status, string dateFromInput, string dateToInput)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            Utils.CheckDateParse(dateFromInput, out DateTime dateFrom);
            Utils.CheckDateParse(dateToInput, out DateTime dateTo);

            List<MooringTaken> moorings = new List<MooringTaken>();
            string mooringsStatus = string.Empty;
            if (status == "T")
            {
                mooringsStatus = "Taken";
                moorings = CalculateTakenMoorings(type, dateFrom, dateTo);
            }
            else if (status == "A")
            {
                mooringsStatus = "Available";
                moorings = CalculateAvailableMoorings(type, dateFrom, dateTo);
            }

            moorings.Sort((x, y) =>
            {
                var ret = x.ID.CompareTo(y.ID);
                if (ret == 0) ret = x.TakenFrom.CompareTo(y.TakenFrom);
                return ret;
            });

            if (sps.HeaderPrint)
            {
                Print.MooringOfTypeHeader();
            }
            PrintMooringsOfType(moorings, type, mooringsStatus);
            if (sps.FooterPrint)
            {
                int rowsCount = moorings.Count;
                Print.MooringOfTypeFooter(rowsCount);
            }
        }

        private static void PrintTakenMoorings(string input)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            List<IComponent> moorings = sps.ShippingPort!.Find(c => c is Mooring);

            Utils.CheckDateParse(input, out DateTime date);
            TimeOnly time = TimeOnly.FromTimeSpan(date.TimeOfDay);
            DayOfWeek day = date.DayOfWeek;

            List<MooredShip> mooredShips = sps.ShippingPort.MooredShips;
            List<Schedule> schedules = sps.ShippingPort.Schedules;
            List<Reservation> reservations = sps.ShippingPort.Reservations;

            List<IElement> elements = mooredShips.ToList<IElement>().Concat(schedules).Concat(reservations).ToList();

            List<Mooring> takenMoorings = new List<Mooring>();
            IElementVisitor elementVisitor = new ElementVisitor(date);

            foreach (IElement element in elements)
            {
                int? id = element.Accept(elementVisitor);
                if (id != null)
                {
                    Mooring? mooring = (Mooring?)moorings.Find(m => m.GetId() == id);
                    if (mooring != null && !takenMoorings.Contains(mooring))
                    {
                        takenMoorings.Add(mooring);
                    }
                }
            }
            if (takenMoorings.Count > 0)
            {
                SumTakenMooringsByType(takenMoorings);
                return;
            }
            sps.Controller.SetModelState("All moorings are available.");
        }

        public static void SumTakenMooringsByType(List<Mooring> moorings)
        {
            int passengerMoorings = 0;
            int businessMoorings = 0;
            int otherMoorings = 0;
            IMooringVisitor mooringVisitor = new MooringVisitor();

            foreach (IMooring mooring in moorings)
            {
                string type = mooring.Accept(mooringVisitor);
                switch (type)
                {
                    case "PA":
                        {
                            passengerMoorings++;
                            break;
                        }
                    case "BU":
                        {
                            businessMoorings++;
                            break;
                        }
                    case "OT":
                        {
                            otherMoorings++;
                            break;
                        }
                    default:
                        break;
                }
            }
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();

            if (sps.HeaderPrint)
            {
                Print.HeaderNotAvailableMooring();
            }
            PrintSumTakenMoorings(passengerMoorings, businessMoorings, otherMoorings);
            if (sps.FooterPrint)
            {
                int rowsCount = 3;
                Print.Footer(rowsCount);
            }
        }

        private static void PrintSumTakenMoorings(int passengerMoorings, int businessMoorings, int otherMoorings)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            if (sps.SequenceNumberPrint)
            {
                Print.SumTakenMoorings(1, "PA", passengerMoorings);
                Print.SumTakenMoorings(2, "BU", businessMoorings);
                Print.SumTakenMoorings(3, "OT", otherMoorings);
            }
            else
            {
                Print.SumTakenMoorings("PA", passengerMoorings);
                Print.SumTakenMoorings("BU", businessMoorings);
                Print.SumTakenMoorings("OT", otherMoorings);
            }
        }

        private static void ChannelConnect(int idShip, int frequency, bool unsubscription)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            List<Channel> channels = sps.ShippingPort!.Channels;

            Channel? channel = channels.Find(x => x.Frequency == frequency);
            if (channel == null)
            {
                sps.Controller.SetModelState("Command unsuccessful, entered channel does not exist.");
                return;
            }
            Ship? ship = sps.ShippingPort.Ships.Find(x => x.ID == idShip);
            if (ship == null)
            {
                sps.Controller.SetModelState("Command unsuccessful, ship with the entered ID does not exist.");
                return;
            }
            if (!unsubscription)
            {
                if (channel.IsBusy())
                {
                    sps.Controller.SetModelState("Command unsuccessful, maximum number of channel connections.");
                    return;
                }
                if (channels.Exists(x => x.Observers.Contains(ship)))
                {
                    sps.Controller.SetModelState("Command unsuccessful, ship already is has a connection.");
                    return;
                }
                channel.Attach(ship);
                sps.Controller.SetModelState("Command successful.");
            }
            else if (unsubscription)
            {
                if (channel.Observers.Contains(ship))
                {
                    DateTime date = sps.VirtualTimeOriginator.VirtualTime;
                    MooredShip? mooredShip = sps.ShippingPort.MooredShips.Find(x => x.IdShip == ship.ID
                        && x.DateFrom <= date && date <= x.DateTo);
                    if (mooredShip != null)
                    {
                        mooredShip.DateTo = date;
                    }
                    channel.Detach(ship);
                    sps.Controller.SetModelState($"Command successful, ship {idShip} is " +
                        $"unsubscribed from a channel with frequency {frequency}.");
                }
                else
                {
                    sps.Controller.SetModelState($"Command unsuccessful, active connection does not exist "
                        + $"on channel {frequency} for ship {idShip}.");
                }
            }
        }

        public static void WriteMessage(int idShip, string message)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            Ship? ship = sps.ShippingPort!.Ships.Find(x => x.ID == idShip);
            if (ship == null)
            {
                return;
            }
            List<Channel> channels = sps.ShippingPort.Channels;
            Channel? channel = channels.Find(x => x.Observers.Contains(ship));
            if (channel != null)
            {
                channel.SetState(message);
            }
        }

        public static void WriteMessage(Channel channel, string message)
        {
            channel.SetState(message);
        }

        public static void ShipStatus(int idShip)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            Ship? ship = sps.ShippingPort!.Ships.Find(x => x.ID == idShip);
            if (ship == null)
            {
                sps.Controller.SetModelState($"Ship with the entered ID {idShip} does not exist.");
                return;
            }

            IHandler handler = GetHandlersChain();
            handler.Handle(idShip);
        }

        public static IHandler GetHandlersChain()
        {
            IHandler mooredShipHandler = new MooredShipHandler();
            IHandler reservationHandler = new ReservationHandler();
            IHandler scheduleHandler = new ScheduleHandler();

            mooredShipHandler.SetNext(reservationHandler);
            reservationHandler.SetNext(scheduleHandler);

            return mooredShipHandler;
        }
        #endregion

        #region Main method
        static void Main(string[] args)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            Model model = new Model();
            View view = new View();
            model.Attach(view);
            sps.Controller.AddModel(model);
            sps.Controller.AddView(view);

            Regex rg = new Regex(Constants.InputArguments);
            Match match = rg.Match(string.Join(" ", args));
            List<Group> groups = new List<Group>();

            if (match.Success)
            {
                for (int i = 1; i < match.Groups.Count; i++)
                {
                    if (match.Groups[i].Success)
                    {
                        groups.Add(match.Groups[i]);
                    }
                }
                if (!Initialization(groups))
                {
                    Print.ErrorInitialization();
                    return;
                }
                sps.VirtualTimeOriginator.RealTime = DateTime.Now;

                while (!Terminated)
                {
                    sps.Controller.SetModelState("Insert command (STATUS, VIRTUAL TIME, " +
                        "LOAD RESERVATIONS, RESERVED MOORING, NON RESERVED MOORING, TAKEN MOORINGS, " +
                        "CHANNEL, PRINT SETTINGS, SHIP, BACKUP, RESTORE, MANUAL):");
                    switch (Console.ReadLine())
                    {
                        case "STATUS":
                            {
                                sps.VirtualTimeOriginator.ShiftVirtualTime();
                                Print.VirtualTime();
                                PrintMooringsStatus();
                                break;
                            }
                        case string input when new Regex(Constants.MooringsPrint).IsMatch(input):
                            {
                                sps.VirtualTimeOriginator.ShiftVirtualTime();
                                Print.VirtualTime();
                                string[] data = input.Split(" ");
                                if (Constants.MooringTypes.Contains(data[1]))
                                {
                                    PrintMooringsStatusOfType(data[1], data[2], $"{data[3]} {data[4]}", $"{data[5]} {data[6]}");
                                }
                                else
                                {
                                    sps.Controller.SetModelState("Command unsuccessful, " +
                                        "entered mooring type does not exist!");
                                }
                                break;
                            }
                        case string input when new Regex(Constants.VirtualTime).IsMatch(input):
                            {
                                sps.VirtualTimeOriginator.RealTime = DateTime.Now;
                                sps.VirtualTimeOriginator.VirtualTime = DateTime.Parse(input.Substring(13));
                                Print.VirtualTime();
                                break;
                            }
                        case string input when new Regex(Constants.ReservationRequest).IsMatch(input):
                            {
                                Creator creator = new ReservationsCreator();
                                sps.VirtualTimeOriginator.ShiftVirtualTime();
                                Print.VirtualTime();
                                creator.ReadData(input.Substring(18));
                                break;
                            }
                        case string input when new Regex(Constants.MooringReservationRequest).IsMatch(input):
                            {
                                sps.VirtualTimeOriginator.ShiftVirtualTime();
                                Print.VirtualTime();
                                int idShip = int.Parse(input.Substring(17));
                                WriteMessage(idShip, input);
                                MoorReservedShip(idShip);
                                break;
                            }
                        case string input when new Regex(Constants.AvailableMooringRequest).IsMatch(input):
                            {
                                sps.VirtualTimeOriginator.ShiftVirtualTime();
                                Print.VirtualTime();
                                string[] data = input.Split(" ");
                                int idShip = int.Parse(data[3]);
                                int duration = int.Parse(data[4]);
                                WriteMessage(idShip, input);
                                MoorNotReservedShip(idShip, duration);
                                break;
                            }
                        case string input when new Regex(Constants.ChannelConnection).IsMatch(input):
                            {
                                sps.VirtualTimeOriginator.ShiftVirtualTime();
                                Print.VirtualTime();
                                string[] options = input.Split(" ");
                                int idShip = int.Parse(options[1]);
                                int frequency = int.Parse(options[2]);
                                if (options.Length == 3)
                                {
                                    ChannelConnect(idShip, frequency, false);
                                }
                                else if (options.Length == 4)
                                {
                                    WriteMessage(idShip, input);
                                    ChannelConnect(idShip, frequency, true);
                                }
                                break;
                            }
                        case string input when new Regex(Constants.PrintFormat).IsMatch(input):
                            {
                                sps.VirtualTimeOriginator.ShiftVirtualTime();
                                Print.VirtualTime();
                                PrintFormat(input);
                                break;
                            }
                        case string input when new Regex(Constants.PrintTakenMoorings).IsMatch(input):
                            {
                                sps.VirtualTimeOriginator.ShiftVirtualTime();
                                Print.VirtualTime();
                                PrintTakenMoorings(input.Substring(15));
                                break;
                            }
                        case string input when new Regex(Constants.ShipStatus).IsMatch(input):
                            {
                                sps.VirtualTimeOriginator.ShiftVirtualTime();
                                Print.VirtualTime();
                                string[] data = input.Split(" ");
                                int idShip = int.Parse(data[1]);
                                ShipStatus(idShip);
                                break;
                            }
                        case string input when new Regex(Constants.StateBackup).IsMatch(input):
                            {
                                sps.VirtualTimeOriginator.ShiftVirtualTime();
                                Print.VirtualTime();
                                string name = input.Substring(4).Trim('"');
                                if (sps.CareTaker.Get(name) != null)
                                {
                                    sps.Controller.SetModelState("Command unsuccessful, " +
                                        "there is already a previously saved state with that name!");
                                }
                                else
                                {
                                    sps.CareTaker.Add(sps.VirtualTimeOriginator.SaveStateToMemento(name));
                                    sps.Controller.SetModelState("Command successful");
                                }
                                break;
                            }
                        case string input when new Regex(Constants.StateRestore).IsMatch(input):
                            {
                                sps.VirtualTimeOriginator.ShiftVirtualTime();
                                Print.VirtualTime();
                                string name = input.Substring(4).Trim('"');
                                Memento.Memento? memento = sps.CareTaker.Get(name);
                                if (memento == null)
                                {
                                    sps.Controller.SetModelState("Command unsuccessful, " +
                                        "there is no previously saved state with that name!");
                                }
                                else
                                {
                                    sps.VirtualTimeOriginator.RealTime = DateTime.Now;
                                    sps.VirtualTimeOriginator.VirtualTime = memento.VirtualTime;
                                    sps.Controller.SetModelState("Command succesful");
                                    Print.VirtualTime();
                                }
                                break;
                            }
                        case "MANUAL":
                            {
                                sps.VirtualTimeOriginator.ShiftVirtualTime();
                                Print.VirtualTime();
                                Print.Manual();
                                break;
                            }
                        case "Q":
                            {
                                sps.VirtualTimeOriginator.ShiftVirtualTime();
                                Print.VirtualTime();
                                Terminated = true;
                                break;
                            }
                        default:
                            {
                                sps.VirtualTimeOriginator.ShiftVirtualTime();
                                Print.VirtualTime();
                                Print.ErrorCommands();
                                break;
                            }
                    }
                }
            }
            else
            {
                Print.ErrorArguments();
            }
        }
        #endregion
    }
}