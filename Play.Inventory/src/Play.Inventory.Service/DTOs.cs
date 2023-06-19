namespace Play.Inventory.Service.DTOs
{
    public record GrantItemsDTO(Guid UserId, Guid CatalogueItemId, int Quantity);
    public record InventoryItemDTO(Guid CatalogueItemId, string Name, string Description, int Quantity, DateTimeOffset AcquiredDate);

    public record CatalogueItemDTO(Guid Id, string Name, string Description);

}