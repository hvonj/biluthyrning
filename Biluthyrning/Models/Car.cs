using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biluthyrning.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string RegNumber { get; set; }
        public int Km { get; set; }
        public Cartype CarType { get; set; }
        public bool FreeOrNot { get; set; }

    }

    public enum CarType
    {
        Small,
        Van,
        Minibus
    }
}
