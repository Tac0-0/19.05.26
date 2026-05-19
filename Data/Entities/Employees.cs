using Doner.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doner.Data.Entities
{
    public class Employees : Users
    {
        public EmployeePosition EmployeePosition { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
    }
}
