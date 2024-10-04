using Catalog.API.Models;
using Marten;

namespace Catalog.API.Endpoints
{
    public static class CatalogEndpoints
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder app)
        {
            #region Product

            app.MapGet("/product/load", (IQuerySession session) =>
               {
                   var products = session.Query<Product>();
                   return products.ToList();
               });
            app.MapGet("/product/create", async (IDocumentSession session) =>
            {
                session.Store(new Product
                {
                    Id = Guid.NewGuid(),
                    Name = $"Product{new Random().Next(1, 100000)}",
                    Description = $"Description{new Random().Next(1, 1000)}",
                    Price = (new Random().Next(1, 100000)) / 10,
                    Categories = new List<Category>
        {
            new Category {
                Id= Guid.NewGuid(),
                Name=$"Category{new Random().Next(1, 1000)}"
            },
             new Category {
                Id= Guid.NewGuid(),
                Name=$"Category{new Random().Next(1, 1000)}"
            }
        }
                });
                await session.SaveChangesAsync();
                return "Product Created!";
            });

            #endregion Product
        }
    }
}