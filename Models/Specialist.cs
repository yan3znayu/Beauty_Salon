using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beauty_Salon.Models
{
    public class Specialists
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Specialists_ID { get; set; }
        public string Specialist_Name { get; set; }
        public string Bio { get; set; }
        public string Photo { get; set; }
        public float Average_Grade { get; set; }

        public ICollection<Grades> Grades { get; set; }
        public ICollection<Reservations> Reservations { get; set; }
    }
}
