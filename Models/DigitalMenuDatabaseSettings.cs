using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalMenuRestourant.Models
{
    public class DigitalMenuDatabaseSettings : IDigitalMenuDatabaseSettings
    {
        public string DishCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
