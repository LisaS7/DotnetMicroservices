namespace Play.Inventory.Service.DTOs
{
    public record GrantItemsDTO(Guid UserId, Guid CatalogueItemId, int Quantity);
    public record InventoryItemDTO(Guid CatalogueItemId, int Quantity, DateTimeOffset AcquiredDate);
}