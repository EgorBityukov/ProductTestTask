using ProductApi.Data;
using Microsoft.EntityFrameworkCore;
using ProductApi.Middleware;
using ProductApi.Data.Repository;
using ProductApi.Services;
using ProductApi.MappingProfiles;
using FluentValidation.AspNetCore;
using ProductApi.Validators;
using FluentValidation;
using ProductApi.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models.Response;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ProductionDbContext>(options =>
        options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(typeof(ProductProfile));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage)
                                               .ToList();
        return new BadRequestObjectResult(ApiResponse<string>.Fail(string.Join("; ", errors)));
    };
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<ProductCreateRequest>, ProductCreateRequestValidator>();
builder.Services.AddScoped<IValidator<ProductUpdateRequest>, ProductUpdateRequestValidator>();
builder.Services.AddScoped<IValidator<ProductVersionCreateRequest>, ProductVersionCreateRequestValidator>();
builder.Services.AddScoped<IValidator<ProductVersionUpdateRequest>, ProductVersionUpdateRequestValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();
