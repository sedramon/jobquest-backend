using jobquest_backend.Configuration;
using jobquest.Application;
using jobquest.Infrastructure;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CUSTOM DEPENDENCY INJECTION
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var corsConfiguration = new CorsConfiguration();
builder.Configuration.GetSection("Cors").Bind(corsConfiguration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(ConstantsConfiguration.AllowedOrigins,
        policyBuilder =>
        {
            if (corsConfiguration.AllowedOrigins != null)
                policyBuilder
                    .WithMethods("GET", "POST", "PATCH", "DELETE", "OPTIONS", "PUT")
                    .WithHeaders(HeaderNames.Accept, HeaderNames.ContentType, HeaderNames.Authorization)
                    .AllowCredentials()
                    .WithOrigins(corsConfiguration.AllowedOrigins);
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(ConstantsConfiguration.AllowedOrigins);

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
