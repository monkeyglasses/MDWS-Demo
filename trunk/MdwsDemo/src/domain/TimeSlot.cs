using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MdwsDemo.domain
{
    public class TimeSlot
    {
        public bool Available { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Text { get; set; }
        public MdwsDemo.scheduling.AppointmentTO Appointment { get; set; }
    }
}