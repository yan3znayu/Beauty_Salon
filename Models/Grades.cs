using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beauty_Salon.Models
{
    public class Grades
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Grades_ID { get; set; }
        public int Specialists_ID { get; set; }
        public int User_ID { get; set; }
        public int Grade { get; set; }
        public Specialists Specialist { get; set; }
        public Users User { get; set; }

    }
}
