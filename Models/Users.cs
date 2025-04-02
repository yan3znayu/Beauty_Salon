using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Beauty_Salon.Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int User_ID { get; set; }
        public int Role_ID { get; set; }
        public int Status_ID { get; set; }
        public string? User_Name { get; set; }
        public string? User_Fullname { get; set; }
        public string? Password_Hash { get; set; }
        public string? Password_Salt { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public DateTime Create_Date { get; set; }

        public Roles Role { get; set; }
        public Statuses Status { get; set; }
        public ICollection<Reservations> Reservations { get; set; }
        public ICollection<Favorites> Favorites{ get; set; }
        public ICollection<Grades> Grades { get; set; }
    }
}
