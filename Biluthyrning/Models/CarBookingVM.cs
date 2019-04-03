using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biluthyrning.Models
{
    public class CarBookingVM
    {
        public Booking Booking { get; set; }
        public IEnumerable<SelectListItem> AllCars { get; set; }
        public IEnumerable<SelectListItem> AllCustomers { get; set; }
    }
}
