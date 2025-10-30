using Controllers;
using Infrastructure.Repositories;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura��o da Connection String do SEU appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Inje��o de Depend�ncia
builder.Services.AddScoped<IProdutoRepository>(provider =>
    new ProdutoRepository(connectionString));
builder.Services.AddScoped<IMovimentacaoRepository>(provider =>
    new MovimentacaoRepository(connectionString));
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<MovimentacaoService>();
builder.Services.AddScoped<RelatorioService>();

// Configura��o do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();