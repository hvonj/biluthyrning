using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biluthyrning.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime CustomerBirthday { get; set; }
        public Car Car { get; set; }
        public int CarId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
