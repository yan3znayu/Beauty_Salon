using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty_Salon.Models
{
    public class Favorites
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Favorite_ID { get; set; }
        public int Service_ID { get; set; }
        public int User_ID { get; set; }

        public Services Service { get; set; }
        public Users User { get; set; }
    }
}

