using Microsoft.AspNetCore.Mvc;
using Play.Catalogue.Service.DTOs;

namespace Play.Catalogue.Service.Controllers
{

    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        public static readonly List<ItemDTO> Items = new()
        {
            new ItemDTO(Guid.NewGuid(), "Potion", "Restores a small amount of hp", 5, DateTimeOffset.UtcNow),
            new ItemDTO(Guid.NewGuid(), "Antidote", "Cures poison", 7, DateTimeOffset.UtcNow),
            new ItemDTO(Guid.NewGuid(), "Bronze sword", "Deals a small amount of damage", 10, DateTimeOffset.UtcNow)
        };

        [HttpGet]
        public IEnumerable<ItemDTO> Get()
        {
            return Items;
        }

        // GET /items/{id}
        [HttpGet("{id}")]
        public ActionResult<ItemDTO> GetById(Guid id)
        // using ActionResult here allows us to return either the item or NotFound
        {
            var item = Items.Where(item => item.Id == id).SingleOrDefault();

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpPost]
        public ActionResult<ItemDTO> Post(CreateItemDTO createItemDTO)
        {
            var item = new ItemDTO(Guid.NewGuid(), createItemDTO.Name, createItemDTO.Description, createItemDTO.Price, DateTimeOffset.UtcNow);
            Items.Add(item);
            // CreatedAtAction returns a 201 status and location header to retrieve the created item
            // the location should match a GET request to return the same item
            return CreatedAtAction(nameof(GetById), new { IdentityServiceCollectionExtensions = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UpdateItemDTO updateItemDTO)
        {
            var existingItem = Items.Where(ItemDTO => ItemDTO.Id == id).SingleOrDefault();

            if (existingItem == null)
            {
                return NotFound();
            }

            var updatedItem = existingItem with
            {
                Name = updateItemDTO.Name,
                Description = updateItemDTO.Description,
                Price = updateItemDTO.Price
            };

            var index = Items.FindIndex(item => existingItem.Id == id);
            Items[index] = updatedItem;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var index = Items.FindIndex(item => item.Id == id);

            if (index < 0)
            {
                return NotFound();
            }

            Items.RemoveAt(index);

            return NoContent();
        }
    }
}