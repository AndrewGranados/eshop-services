using Catalog.Api.Exceptions;
using Catalog.Api.Models.CreateProduct;
using Catalog.API.Behaviors;

var builder = WebApplication.CreateBuilder(args);

TypeAdapterConfig<CreateProductRequest, CreateProductCommand>.NewConfig()
    .Map(dest => dest.Descripcion, src => src.Description)
    .Map(dest => dest.ImageFiles, src => src.ImagesFiles);

builder.Services.AddMemoryCache();

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
  });

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddCarter(); //CARTER -> url's (para q sean apis)

builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddMarten/*MARTEN -> bases de datos (conexión a bdd)*/(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

var app = builder.Build();

app.UseCors("frontend");

app.MapCarter();
app.UseExceptionHandler(options => { });
app.Run();
