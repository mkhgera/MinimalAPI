using Microsoft.EntityFrameworkCore;
using MinimalAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

async Task<List<Actor>> GetAllActors(DataContext context) =>
    await context.Actors.ToListAsync();

app.MapGet("/", () => "Welcome to the Korean Actors Database");

app.MapGet("/actor", async (DataContext context) =>
    await context.Actors.ToListAsync());

app.MapGet("/actor/{id}", async (DataContext context, int id) =>
    await context.Actors.FindAsync(id) is Actor actor
        ? Results.Ok(actor)
        : Results.NotFound("Sorry actor is not found"));

app.MapPost("/actor", async (DataContext context, Actor actor) =>
{
    context.Actors.Add(actor);
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllActors(context));
});

app.MapPut("/actor/{id}", async (DataContext context, Actor actor, int id) =>
{
    var dbActor = await context.Actors.FindAsync(id);
    if (dbActor == null) return Results.NotFound("No actor found");

    dbActor.Firstname = actor.Firstname;
    dbActor.Lastname = actor.Lastname;
    dbActor.Age = actor.Age;
    await context.SaveChangesAsync();

    return Results.Ok(await GetAllActors(context));

});

app.MapDelete("/superhero/{id}", async (DataContext context, int id) =>
{
    var dbActor = await context.Actors.FindAsync(id);
    if (dbActor == null) return Results.NotFound("Not found");

    context.Actors.Remove(dbActor);
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllActors(context));
});
    

app.Run();

