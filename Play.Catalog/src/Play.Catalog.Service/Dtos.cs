using System.ComponentModel.DataAnnotations;
using System;
namespace Play.Catalog.Service.Dtos
{
    //1st Dto : used to rreturn informations from get operations
    public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);
    public record CreateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);
    public record UpdateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);
    // in order to add API operations we need to introduce a web API controller 
}