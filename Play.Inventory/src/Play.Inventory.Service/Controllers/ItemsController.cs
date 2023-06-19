using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Play.Common;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.DTOs;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> itemsRepository;
        private readonly CatalogueClient catalogueClient;

        public ItemsController(IRepository<InventoryItem> itemsRepository, CatalogueClient catalogueClient)
        {
            this.itemsRepository = itemsRepository;
            this.catalogueClient = catalogueClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDTO>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty) { return BadRequest(); }

            var catalogueItems = await catalogueClient.GetCatalogueItemsAsync();
            var inventoryItemEntities = await itemsRepository.GetAllAsync(item => item.UserId == userId);

            var inventoryItemDTOs = inventoryItemEntities.Select(inventoryItem =>
            {
                var catalogueItem = catalogueItems.Single(catalogueItem => catalogueItem.Id == inventoryItem.CatalogueItemId);
                return inventoryItem.AsDTO(catalogueItem.Name, catalogueItem.Description);
            });

            return Ok(inventoryItemDTOs);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemsDTO grantItemsDTO)
        {
            var inventoryItem = await itemsRepository.GetAsync(
                item => item.UserId == grantItemsDTO.UserId && item.CatalogueItemId == grantItemsDTO.CatalogueItemId
            );

            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem
                {
                    CatalogueItemId = grantItemsDTO.CatalogueItemId,
                    UserId = grantItemsDTO.UserId,
                    Quantity = grantItemsDTO.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };

                await itemsRepository.CreateAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += grantItemsDTO.Quantity;
                await itemsRepository.UpdateAsync(inventoryItem);
            }

            return Ok();
        }
    }
}