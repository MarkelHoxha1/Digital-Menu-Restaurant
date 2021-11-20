using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalMenuRestaurant.APIModels;
using DigitalMenuRestaurant.Models;
using DigitalMenuRestaurant.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMenuRestaurant.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DishController : ControllerBase
    {
        private readonly DishService _dishService;

        public DishController(DishService dishService)
        {
            _dishService = dishService;
        }

        [HttpGet]
        public ActionResult<List<Dish>> Get() =>
            _dishService.Get();

        [HttpGet("{id:length(24)}", Name = "GetDish")]
        public ActionResult<Dish> Get(string id)
        {
            var dish = _dishService.Get(id);

            if (dish == null)
            {
                return NotFound();
            }

            return dish;
        }

        [HttpPost]
        public ActionResult<Dish> Create(Dish dish)
        {
            _dishService.Create(dish);

            return CreatedAtRoute("GetDish", new { id = dish.Id.ToString() }, dish);
        }

        [HttpPost("insertmany")]
        public ActionResult<Dish> InsertMany(List<Dish> dishes)
        {
            _dishService.InsertMany(dishes);

            return CreatedAtRoute("InsertMany", new { count = dishes.Count.ToString() });
        }

        [HttpPost("addAvailability")]
        public ActionResult<Dish> AddAvailability([FromBody] DishAvailability dishAvailability)
        {
            var dish = _dishService.Get(dishAvailability.id);

            if (dish == null)
            {
                return NotFound();
            }

            dish.availability.AddRange(dishAvailability.availability);

            _dishService.Update(dishAvailability.id, dish);
            return NoContent();
        }

        [HttpPost("activatedish/{id}")]
        public ActionResult<Dish> ActivateDish(string id)
        {
            var dish = _dishService.Get(id);

            if (dish == null)
            {
                return NotFound();
            }

            dish.IsAvailable = true;

            _dishService.Update(id, dish);
            return NoContent();
        }

        [HttpPost]
        [Route("deactivatedish/{id}")]
        public ActionResult<Dish> DeactivateDish(string id)
        {
            var dish = _dishService.Get(id);

            if (dish == null)
            {
                return NotFound();
            }

            dish.IsAvailable = false;

            _dishService.Update(id, dish);
            return NoContent();
        }


        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Dish dishToBeInserted)
        {
            var dish = _dishService.Get(id);

            if (dish == null)
            {
                return NotFound();
            }

            _dishService.Update(id, dishToBeInserted);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var dish = _dishService.Get(id);

            if (dish == null)
            {
                return NotFound();
            }

            _dishService.Remove(dish.Id);

            return NoContent();
        }
    }
}