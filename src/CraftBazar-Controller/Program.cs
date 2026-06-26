using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CraftBazar_DTO.Settings;
using CraftBazar.Commands.Authentication.Register;
using FluentValidation;
using MediatR;
using CraftBazar_Queries.Users;
using Hangfire;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add controllers.
builder.Services.AddControllers();

// Add HttpClient
builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add Hangfire
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"));
});
builder.Services.AddHangfireServer();

/*** Dependency Injection ***/
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

// Add AutoMapper Profiles
builder.Services.AddAutoMapper(cfg => { }, typeof(AuthMappingProfile).Assembly);

// Register MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(RegisterCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetUsersQuery).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

// Register all FluentValidation Validators automatically
builder.Services.AddValidatorsFromAssembly(
    typeof(RegisterCommandValidator).Assembly);


// For authentication
var JwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = JwtSettings!.Issuer,
        ValidAudience = JwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Key))
    };
});

/*** CORS Configuration ***/
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token like: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    });
    /* For Swashbuckle version 9 */
    // options.AddSecurityRequirement(new OpenApiSecurityRequirement
    // {
    //     {
    //         new OpenApiSecurityScheme
    //         {
    //             Reference = new OpenApiReference
    //             {
    //                 Type = ReferenceType.SecurityScheme,
    //                 Id = "Bearer"
    //             }
    //         },
    //         Array.Empty<string>()
    //     }
    // });
    /* For Swashbuckle version 10 */
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // 1. Keep Swashbuckle running to generate the openapi spec json file
    app.UseSwagger();
    app.UseSwaggerUI();

    // 2. Add the interactive Scalar interface instead of SwaggerUI
    app.MapScalarApiReference(options =>
{
    options.WithTitle("CraftBazar API Documentation")
           .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
           .WithTheme(ScalarTheme.DeepSpace);

    options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json");
});
}
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();

