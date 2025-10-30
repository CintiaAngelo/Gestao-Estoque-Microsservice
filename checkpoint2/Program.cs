using Infrastructure.Repositories;
using Services;

var builder = WebApplication.CreateBuilder(args);

// ?? Conexão com o banco MySQL
string connectionString = builder.Configuration.GetConnectionString("MySqlConnection")
    ?? "server=localhost;database=estoque;user=root;password=123;";

// ?? Injeção de dependências
builder.Services.AddScoped<IProdutoRepository>(sp => new ProdutoRepository(connectionString));
builder.Services.AddScoped<IMovimentacaoRepository>(sp => new MovimentacaoRepository(connectionString));

builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<MovimentacaoService>();
builder.Services.AddScoped<RelatorioService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ?? Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
