using Play.Inventory.Service.DTOs;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service
{
    public static class Extensions
    {
        public static InventoryItemDTO AsDTO(this InventoryItem item, string name, string description)
        {
            return new InventoryItemDTO(item.CatalogueItemId, name, description, item.Quantity, item.AcquiredDate);
        }
    }
}