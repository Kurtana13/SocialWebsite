using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialWebsite.Api.Data;
using SocialWebsite.Api.Filters;
using SocialWebsite.Api.Identity;
using SocialWebsite.Api.Repositories;
using SocialWebsite.Api.Repositories.IRepositories;
using SocialWebsite.Api.Services;
using SocialWebsite.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<User,IdentityRole<int>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = config["Jwt:ValidIssuer"],
        ValidAudience = config["Jwt:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(config["Jwt:Secret"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization(options=>
{
    options.AddPolicy(IdentityData.AdminUserPolicyName, p =>
    p.RequireClaim(IdentityData.AdminUserClaimName, IdentityData.AdminUserClaimName));
    options.AddPolicy(IdentityData.UserPolicyName, p =>
    p.RequireClaim(IdentityData.UserClaimName, IdentityData.UserClaimName));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
           new string[] { }
        }
    });
});

builder.Services.AddSingleton<IConfiguration>(config);

builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<RoleManager<IdentityRole<int>>>();
builder.Services.AddScoped<IUnitOfWork<ApplicationDbContext>, UnitOfWork<ApplicationDbContext>>(provider=>
    new UnitOfWork<ApplicationDbContext>(provider.GetRequiredService<ApplicationDbContext>()));
builder.Services.AddScoped<IUserRepository, UserRepository>(provider =>
    new UserRepository(provider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>(),provider.GetRequiredService<UserManager<User>>()));
builder.Services.AddScoped<IPostRepository, PostRepository>(provider =>
    new PostRepository(provider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>()));
builder.Services.AddScoped<ICommentRepository, CommentRepository>(provider =>
    new CommentRepository(provider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>()));
builder.Services.AddScoped<IGroupRepository, GroupRepository>(provider =>
    new GroupRepository(provider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>()));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<JwtTokenGenerator>(provider =>
    new JwtTokenGenerator(provider.GetRequiredService<UserManager<User>>(), config));

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
