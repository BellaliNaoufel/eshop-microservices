var builder = WebApplication.CreateBuilder(args);
// Add Services to container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.Run();
