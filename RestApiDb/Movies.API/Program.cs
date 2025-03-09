using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Movies.API;
using Movies.API.Auth;
using Movies.API.Mapping;
using Movies.Application;
using Movies.Application.Database;
using Movies.Application.Health;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["Jwt:Key"])),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = config["Jwt:Issuer"],
        ValidAudience = config["Jwt:Audience"],
        ValidateIssuer = true,
        ValidateAudience = true

    };
});

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy(AuthConstant.AdminUserPolicyName, 
        p => p.RequireClaim(AuthConstant.AdminUserClaimName, "true"));
    
    x.AddPolicy(AuthConstant.TrustedMemberPolicyName, 
        p => p.RequireAssertion(c=>
            c.User.HasClaim(m=>m is {Type: AuthConstant.AdminUserClaimName, Value:"true"}) ||
            c.User.HasClaim(m=>m is {Type: AuthConstant.TrustedMemberClaimName, Value:"true"}
        )
    ));
});

builder.Services.AddApiVersioning(x =>
{
    x.DefaultApiVersion = new ApiVersion(1, 0);
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.ReportApiVersions = true;
    // x.ApiVersionReader = new HeaderApiVersionReader("api-version");
}).AddMvc();

builder.Services.AddControllers();

builder.Services.AddOutputCache(x =>
{
    x.AddBasePolicy(c => c.Cache());
    x.AddPolicy("MovieCache", c =>
        c.Cache()
            .Expire(TimeSpan.FromMinutes(5))
            .SetVaryByQuery(new[] { "title", "year", "sortBy", "page", "pageSize" })
            .Tag("movies"));
});

builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("Database");

builder.Services.AddOpenApi();
builder.Services.AddApplicationServices();
builder.Services.AddDatabase(config["Database:ConnectionString"]);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("_health");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseOutputCache();

app.UseMiddleware<ValidationMappingMiddleware>();

app.MapControllers();

var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
await dbInitializer.InitializeAsync();

app.Run();
