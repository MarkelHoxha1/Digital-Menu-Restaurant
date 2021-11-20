using DigitalMenuRestaurant.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace DigitalMenuRestaurant.Services
{
    public class DishService
    {
        private readonly IMongoCollection<Dish> _dishes;
        private List<Dish> seedData = new List<Dish>();



        public DishService(IDigitalMenuDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _dishes = database.GetCollection<Dish>(settings.DishCollectionName);
            seedData = new List<Dish>{
                new Dish
                {
                    Name = "Creamy Tomato Pasta",
                    Description = "This isn't the most orthodox pasta recipe, but it is a good one. Simply choose a long pasta – bucatini or spaghetti works well – buy a jar of your favorite tomato sauce (low sugar and no added tomato paste are the most healthy and flavorful), some cream cheese, and you'll have dinner ready in 20 minutes or less.",
                    availability = new List<Availability> { Availability.Lunch  , Availability.WeekDays },
                    category = DishCategory.Dessert,
                    IsAvailable = true,
                    Price = 28.59,
                    TimeToCook = 20
                },
                  new Dish
                  {
                      Name = "Baked Honey Mustard Chicken",
                      Description = "If you're not used to using the microwave when cooking, this is a great opportunity to make this appliance your new ally. Make a honey-mustard and onion sauce in the microwave, season your chicken with salt and pepper, cover with the sauce, and bake between 35 and 40 minutes at 375 F.",
                      availability = new List<Availability> { Availability.Lunch },
                      category = DishCategory.Main_course,
                      IsAvailable = true,
                      Price = 54.59,
                      TimeToCook = 40
                  },
                  new Dish
                  {
                      Name = "Red and White Tortellini",
                      Description = "This fabulous recipe uses all the tricks of a seasoned cook who knows what it's like to be pressed for time. Buy the frozen ravioli of your choice and one jar of red sauce (no added sugar) and one of Alfredo sauce. Place the ravioli on a baking pan and pour the red sauce on top plus the 1/2 cup of water that you just used to 'clean' all of the remaining red sauce from the jar. Do the same with Alfredo sauce plus the 1/4 cup of milk used to clean the jar.",
                      availability = new List<Availability> { Availability.Weekend, Availability.Lunch, Availability.Dinner },
                      category = DishCategory.Dessert,
                      IsAvailable = true,
                      Price = 8.59,
                      TimeToCook = 55
                  },

                  new Dish
                  {
                      Name = "Classic Meatloaf",
                      Description = "Meatloaf is an American staple that is too irresistible not to try and make your own. Juicy lean beef and veggies baked in a sturdy loaf are part of most people's happy childhood memories. For this recipe, use good quality beef, eggs, onions, and tomato sauce. Add any spices of your choice like cayenne, chili flakes, garlic powder, or Italian herbs. Bake for 1 hour and 10 minutes and let rest for 10 more before slicing it.",
                      availability = new List<Availability> { Availability.WeekDays, Availability.Lunch },
                      category = DishCategory.Main_course,
                      IsAvailable = true,
                      Price = 55.59,
                      TimeToCook = 80
                  }
            };

            if (_dishes.Find(dish => true).CountDocuments() == 0)
                InsertMany(seedData);
        }

        public List<Dish> Get()
        {
            return _dishes.Find(dish => true).ToList();
        }

        public Dish Get(string id) =>
            _dishes.Find<Dish>(dish => dish.Id == id).FirstOrDefault();

        public Dish Create(Dish dish)
        {
            _dishes.InsertOne(dish);
            return dish;
        }
        public void InsertMany(List<Dish> dishes)
        {
            _dishes.InsertMany(dishes);
        }

        public void Update(string id, Dish dishToInsert) =>
            _dishes.ReplaceOne(dish => dish.Id == id, dishToInsert);

        public void Remove(Dish dishToInsert) =>
            _dishes.DeleteOne(dish => dish.Id == dishToInsert.Id);

        public void Remove(string id) =>
            _dishes.DeleteOne(dish => dish.Id == id);
    }
}
