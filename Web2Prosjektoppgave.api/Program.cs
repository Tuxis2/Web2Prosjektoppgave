using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Web2Prosjektoppgave.api.Data;
using Web2Prosjektoppgave.api.Hubs;
using Web2Prosjektoppgave.api.Models.Interfaces;
using Web2Prosjektoppgave.api.Repositories;
using Web2Prosjektoppgave.api.Security;
using Web2Prosjektoppgave.shared.Security;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BlogDbConnectionFinal");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddDbContextPool<BlogDbContext>(options =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
    {

    });
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IBlogPostTagRepository, BlogPostTagRepository>();
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = JwtTokenHelper.Issuer,
            ValidAudience = JwtTokenHelper.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenHelper.Key))
        };
    });

builder.Services.AddAuthorization(options =>
{

});

builder.Services.AddSingleton<IAuthorizationHandler, UserHandler>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<SignalHub>("/SignalHub");

app.Run();
