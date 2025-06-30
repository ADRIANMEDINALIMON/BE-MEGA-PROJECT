using BE_MEGA_PROJECT.Data;
using BE_MEGA_PROJECT.Repositories.Implementations;
using BE_MEGA_PROJECT.Repositories.Interfaces;
using BE_MEGA_PROJECT.Services.Implementations;
using BE_MEGA_PROJECT.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 1) servicios comunes
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2) dbcontext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 3) controllers + json options (para serializar enums como string)
builder.Services
  .AddControllers()
  .AddJsonOptions(opts => {
    // serializar enums como cadenas (ya lo tenías)
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    // IGNORAR ciclos de referencia
    opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
  });

// 4) configuracion de cors para permitir peticiones desde el FE
builder.Services.AddCors(opts =>
  opts.AddDefaultPolicy(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod()
  )
);

// 5) inyeccion de repositorios / servicios
builder.Services.AddScoped<IUserRepository,   UserRepository>();
builder.Services.AddScoped<IUserService,      UserService>();

builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
builder.Services.AddScoped<IPromotionService,    PromotionService>();

builder.Services.AddScoped<IInvoiceRepository,   InvoiceRepository>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

builder.Services.AddScoped<ISubscriberService, SubscriberService>();
builder.Services.AddScoped<ISubscriberRepository, SubscriberRepository>();


var app = builder.Build();

// 6) pipeline de middlewares

// swagger solo en dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// cors y https
app.UseCors();
app.UseHttpsRedirection();

// mapea controladores
app.MapControllers();

app.Run();
