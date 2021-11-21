using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DigitalMenuRestaurant.Models
{
    public class Dish
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        public DishCategory category { get; set; }
        public List<Availability> availability { get; set; }
        public bool IsAvailable { get; set; }
        //Time is stored in minutes
        [Range(0, 999)]
        public int TimeToCook { get; set; }
    }
}
