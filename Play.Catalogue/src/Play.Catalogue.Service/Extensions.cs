using Play.Catalogue.Service.DTOs;
using Play.Catalogue.Service.Entities;

namespace Play.Catalogue.Service
{
    public static class Extensions
    {
        public static ItemDTO AsDTO(this Item item)
        {
            return new ItemDTO(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }
    }
}