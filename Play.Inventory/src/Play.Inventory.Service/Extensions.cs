using Play.Inventory.Service.DTOs;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service
{
    public static class Extensions
    {
        public static InventoryItemDTO AsDTO(this InventoryItem item)
        {
            return new InventoryItemDTO(item.CatalogueItemId, item.Quantity, item.AcquiredDate);
        }
    }
}