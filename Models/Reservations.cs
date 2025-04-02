using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beauty_Salon.Models
{
    public class Reservations
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Reservation_ID { get; set; }
        public int User_ID { get; set; }
        public int Specialist_ID { get; set; }
        public int Service_ID { get; set; }
        public DateTime Appointment_Date { get; set; }
        public TimeSpan Appointment_Time { get; set; }
        public DateTime Reserv_Date { get; set; }

        public Services Service { get; set; }
        public Specialists Specialist { get; set; }
        public Users User { get; set; }
    }
}
