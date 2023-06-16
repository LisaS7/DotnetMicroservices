using Microsoft.AspNetCore.Mvc;
using Play.Catalogue.Service.DTOs;
using Play.Catalogue.Service.Entities;
using Play.Catalogue.Service.Repositories;

namespace Play.Catalogue.Service.Controllers
{

    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository itemsRepository;

        public ItemsController(IItemsRepository itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDTO>> GetAsync()
        {
            var items = (await itemsRepository.GetAllAsync()).Select(item => item.AsDTO());
            return items;
        }

        // GET /items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDTO>> GetByIdAsync(Guid id)
        // using ActionResult here allows us to return either the item or NotFound
        {
            var item = await itemsRepository.GetAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item.AsDTO();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDTO>> PostAsync(CreateItemDTO createItemDTO)
        {
            var item = new Item { Name = createItemDTO.Name, Description = createItemDTO.Description, Price = createItemDTO.Price, CreatedDate = DateTimeOffset.UtcNow };
            await itemsRepository.CreateAsync(item);

            // CreatedAtAction returns a 201 status and location header to retrieve the created item
            // the location should match a GET request to return the same item
            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDTO updateItemDTO)
        {
            var existingItem = await itemsRepository.GetAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = updateItemDTO.Name;
            existingItem.Description = updateItemDTO.Description;
            existingItem.Price = updateItemDTO.Price;

            await itemsRepository.UpdateAsync(existingItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var item = await itemsRepository.GetAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            await itemsRepository.DeleteAsync(item.Id);

            return NoContent();
        }
    }
}