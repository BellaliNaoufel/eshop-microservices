using Marten;

var builder = WebApplication.CreateBuilder(args);
// Add Services to container.
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Default")!);
    options.UseSystemTextJsonForSerialization();
})
    .AssertDatabaseMatchesConfigurationOnStartup();

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
    session.Store(new Product(Guid.NewGuid(), $"Product{new Random().Next(1, 1000)}", $"Description{new Random().Next(1, 1000)}"));
    await session.SaveChangesAsync();
    return "Product Created!";
});

app.Run();

public record Product(Guid Id, string Name, string Description);
