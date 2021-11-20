using DigitalMenuRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalMenuRestaurant.APIModels
{
    public class DishAvailability
    {
        public string id { get; set; }
        public List<Availability> availability { get; set; }
    }
}
