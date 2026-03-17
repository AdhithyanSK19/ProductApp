using WalkersApp.Data;
using Microsoft.EntityFrameworkCore;
using WalkersApp.Repository;
using WalkersApp.Mappings;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//for noraml swagger
//builder.Services.AddSwaggerGen();

//Swagger with Authorisation
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Walks API", Version = "v1" });
    //option.AddSecurityDefinition()
});


builder.Services.AddDbContext<WalksDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("WalksConnStr")));// this here we have injected the DbContextClass.

builder.Services.AddDbContext<WalksAuthDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("WalksAuthConnStr")));
//this option will be passed to the AppDbContext class and then 
//to the base class that is DbContext class.
builder.Services.AddScoped<IRegionRepository, RegionRespository>();
builder.Services.AddScoped<IWalkRepository, WalkRepository>();
builder.Services.AddScoped<ITokenRepository,TokenRepository>(); 
builder.Services.AddAutoMapper(typeof(AutomapperProfiles));

//add identity before JWT

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>() // to add roles
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("Walks") //specify the DB context which will act as the store
    .AddEntityFrameworkStores<WalksAuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteDev", policy =>
    {
        policy.WithOrigins("http://localhost:5173")  // Your Vite port
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();  // If using cookies/auth
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseCors("AllowViteDev");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

string a = "";