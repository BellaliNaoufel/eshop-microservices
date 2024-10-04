using Catalog.API.Models;
using JasperFx.CodeGeneration.Frames;
using Marten;

namespace Catalog.API.Endpoints
{
    public static class CatalogEndpoints
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder app)
        {
            #region Product

            app.MapGet("/product/get", (IQuerySession session) =>
               {
                   var products = session.Query<Product>();
                   return products.ToList();
               });
            app.MapGet("/product/get/{id}", (Guid id, IQuerySession session) =>
            {
                return session.Load<Product>(id);
            });
            app.MapPost("/product/create", async (Product product, IDocumentSession session) =>
            {
                session.Store(product);
                await session.SaveChangesAsync();
                return "Product Created!";

                //Product CreateRandomProduct()
                //{
                //    return new Product
                //    {
                //        Id = Guid.NewGuid(),
                //        Name = $"Product{new Random().Next(1, 100000)}",
                //        Description = $"Description{new Random().Next(1, 1000)}",
                //        Price = (new Random().Next(1, 100000)) / 10,
                //        Categories = new List<Category>
                //        {
                //            new Category {
                //                Id= Guid.NewGuid(),
                //                Name=$"Category{new Random().Next(1, 1000)}"
                //            },
                //            new Category {
                //                Id= Guid.NewGuid(),
                //                Name=$"Category{new Random().Next(1, 1000)}"
                //            }
                //        }
                //    };
                //}
            });
            app.MapDelete("/product/delete", async (Guid id, IDocumentSession session) =>
            {
                session.Delete<Product>(id);
                await session.SaveChangesAsync();
            });
            app.MapPut("/product/update", async (Product product, IDocumentSession session) =>
            {
                var existingProduct = await session.Query<Product>().SingleAsync(x => x.Id == product.Id);
                // TODO: find how to update all item
                existingProduct.ImageFile = product.ImageFile;
                session.Update(product, existingProduct);
                await session.SaveChangesAsync();
            });

            #endregion Product
        }
    }
}