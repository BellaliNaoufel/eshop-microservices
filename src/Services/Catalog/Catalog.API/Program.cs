using Catalog.API.Models;
using Marten;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);
// Add Services to container.
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Default")!);
    options.UseSystemTextJsonForSerialization(enumStorage: EnumStorage.AsString, casing: Casing.Default);
    options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate; // TODO: only on dev
})
    .AssertDatabaseMatchesConfigurationOnStartup()
    .UseLightweightSessions();

var app = builder.Build();
// Configure the HTTP request pipeline.
app.MapGet("/", () => "hello world");
app.MapGet("/load", (IQuerySession session) =>
{
    var products = session.Query<Product>();
    return products.ToList();
});
app.MapGet("/create", async (IDocumentSession session) =>
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

app.Run();