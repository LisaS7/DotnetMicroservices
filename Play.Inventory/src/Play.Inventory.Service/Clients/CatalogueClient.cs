using Play.Inventory.Service.DTOs;

namespace Play.Inventory.Service.Clients
{
    public class CatalogueClient
    {
        private readonly HttpClient httpClient;

        public CatalogueClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<CatalogueItemDTO>> GetCatalogueItemsAsync()
        {
            var items = await httpClient.GetFromJsonAsync<IReadOnlyCollection<CatalogueItemDTO>>("/items");
            return items;
        }
    }
}