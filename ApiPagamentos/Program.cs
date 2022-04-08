using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("Pagamentos"));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();

app.MapPost("/Pagamentos", async (Pagamento pagamento, AppDbContext dbContext) =>
{
    dbContext.Pagamentos.Add(pagamento);
    await dbContext.SaveChangesAsync();

    if (dbContext.valor.Count < 100)
    {
        Console.WriteLine("Compra aprovada!");
    }
    else
    {
        dbContext.SaveChanges();
        Console.WriteLine("Compra reprovada!");
    }

    return pagamento;
});

app.UseSwaggerUI();

app.Run();

public class Pagamento
{

    public int Id { get; set; }
    public int valor { get; set; }
    public string? Cartão { get; set; }
    public string? titular { get; set; }
    public int numero { get; set; }
    public int data_expiracao { get; set; }
    public string? bandeira { get; set; }
    public int ccv { get; set; }

}

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<Pagamento> Pagamentos { get; set; }
}
