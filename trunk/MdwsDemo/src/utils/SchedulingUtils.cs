using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MdwsDemo.domain;

namespace MdwsDemo.utils
{
    public static class SchedulingUtils
    {

        public static IList<TimeSlot> getItemsFromAvailabilityString(string clinicStartTime, int apptLengthMins, int displayIncrements, string availability)
        {
            string[] lines = availability.Split(new char[] { '\n' });
            IList<TimeSlot> results = new List<TimeSlot>();
            foreach (String line in lines)
            {
                if (String.IsNullOrEmpty(line) || line.Contains("0)=")) // doesn't seem to be any meaninful info...
                {
                    continue;
                }

                string[] pieces = line.Split(new char[] { '=' });
                if (pieces.Length != 2)
                {
                    continue;
                }
                // get current day
                int dateStartIdx = pieces[0].IndexOf('(');
                int dateEndIdx = pieces[0].IndexOf(',');
                string date = pieces[0].Substring(dateStartIdx + 1, dateEndIdx - dateStartIdx - 1);
                DateTime theDay = convertVistaDate(date);

                if (!String.IsNullOrEmpty(pieces[1]) && pieces[1].StartsWith(" ")) // holiday!
                {
                    pieces[1] = pieces[1].Trim();
                    pieces[1] = pieces[1].Substring(2); // substring past 2 day date string
                    string holidayName = pieces[1].Trim();
                    TimeSlot holiday = new TimeSlot()
                    {
                        Available = false,
                        Start = theDay,
                        End = theDay.AddDays(1).Subtract(new TimeSpan(0, 1, 0)),
                        Text = holidayName
                    };
                    results.Add(holiday);
                    continue;
                }

                //if (theDay.Date.Equals(DateTime.Today))
                //{
                //    System.Console.WriteLine("Found today!");
                //}

                if (String.IsNullOrEmpty(pieces[1]) || (!pieces[1].Contains("[") && !pieces[1].Contains("|")))
                {
                    continue; // not a valid time slot line
                }
                // get items in day
                int startIdx = 8; // this looks to be a constant width - NOT TRUE!

                int iFlag = 0;
                int slotCount = 0;
                DateTime dtClinicStartTime = new DateTime(theDay.Year, theDay.Month, theDay.Day, Convert.ToInt32(clinicStartTime), 0, 0);
                while (startIdx < pieces[1].Length)
                {
                    TimeSlot current = new TimeSlot()
                    {
                        Start = dtClinicStartTime.AddMinutes(slotCount * apptLengthMins),
                        End = dtClinicStartTime.AddMinutes((slotCount + 1) * apptLengthMins),
                    };

                    string timeDisplayPrefix = current.Start.Hour + ":" + current.Start.Minute.ToString("00") + " - ";

                    char flag = pieces[1][startIdx];
                    if (Int32.TryParse(flag.ToString(), out iFlag))
                    {
                        if (iFlag > 0)
                        {
                            current.Available = true;
                            current.Text = timeDisplayPrefix + iFlag.ToString() + " Available";
                        }
                        else
                        {
                            current.Available = false;
                            current.Text = timeDisplayPrefix + "All Slots Full!";
                        }
                    }
                    else
                    {
                        current.Available = false;
                        current.Text = timeDisplayPrefix + "Unavailable";
                    }
                    // don't forget to increment slotCount
                    slotCount++;
                    startIdx += getIndexShiftForApptLength(apptLengthMins);
                    results.Add(current);
                }
            }
            return results;
        }

        static int getIndexShiftForApptLength(int apptLength)
        {
            switch (apptLength)
            {
                case 60:
                    return 8;
                case 30:
                    return 4;
                case 20:
                    return 2; // TBD - is this valid??? need to stage test with 20 min appt length
                case 15:
                    return 2;
                case 10:
                    return 2;
                default:
                    throw new NotImplementedException(apptLength.ToString() + " is not a standard appointment length");
            }
        }

        static DateTime convertVistaDate(string vistaDate)
        {
            int iVistaDate = Convert.ToInt32(vistaDate);
            int correctedVistaDate = iVistaDate + 17000000;
            int year = correctedVistaDate / 10000;
            int month = (correctedVistaDate - (year * 10000)) / 100;
            int day = (correctedVistaDate - (year * 10000)) - month * 100;
            return new DateTime(year, month, day);
        }

    }
}