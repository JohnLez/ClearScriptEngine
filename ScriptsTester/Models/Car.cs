using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptsTester.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public DateTime DateManufactured { get; set; }
        public string ModelName { get; set; }

        //default ctor
        public Car() { DateManufactured = DateTime.Now; }

        public Car(string brand)
        {
            Brand = brand;
            DateManufactured = DateTime.Now;
        }

        public override string ToString() => $"ID:{Id}\tBrand:{Brand}\tModelName:{ModelName}\tDateManufactured:{DateManufactured.ToString("MM/dd/yyyy HH:mm:ss")}";
    }
}
