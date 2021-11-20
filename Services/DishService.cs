using DigitalMenuRestourant.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalMenuRestourant.Services
{
    public class DishService
    {
        private readonly IMongoCollection<Dish> _dishes;

        public DishService(IDigitalMenuDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _dishes = database.GetCollection<Dish>(settings.DishCollectionName);
        }

        public List<Dish> Get() =>
            _dishes.Find(dish => true).ToList();

        public Dish Get(string id) =>
            _dishes.Find<Dish>(dish => dish.Id == id).FirstOrDefault();

        public Dish Create(Dish dish)
        {
            _dishes.InsertOne(dish);
            return dish;
        }

        public void Update(string id, Dish dishToInsert) =>
            _dishes.ReplaceOne(dish => dish.Id == id, dishToInsert);

        public void Remove(Dish dishToInsert) =>
            _dishes.DeleteOne(dish => dish.Id == dishToInsert.Id);

        public void Remove(string id) =>
            _dishes.DeleteOne(dish => dish.Id == id);
    }
}
