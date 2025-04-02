using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beauty_Salon.Models
{
    public class Services
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Service_ID { get; set; }
        public string? Service_Name { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
        public string? ImagePath { get; set; }
        public string Type { get; set; }

        public ICollection<Reservations> Reservations { get; set; }
        public ICollection<Favorites> Favorites { get; set; }
    }
}
