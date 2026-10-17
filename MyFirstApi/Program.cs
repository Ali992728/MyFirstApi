using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyFirstApi.Data;
using MyFirstApi.IService;
using MyFirstApi.Services;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);          //webapplication http request er jonno ekta pipeline create kore.

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));    //Services hocche container jekhane shb services add kora hoy jate pore dependecy injection er maddhome sheta use kra jay.
                                                                                                                                            //ekhane services er moddhe AppDbContext add krsi jate pore use krte pari.
                                                                                                                                            //option => hocche lambda expression
                                                                                                                                            //ekhane ami connection string ar appdbcontext er binding o krtesi abr pore DI er jonno services eo rakhtesi appdbcontext ke. Jate pore controller er moddhe use krte pari.
                                                                                                                                            //services contains the service registrations(like appdbcontext) and DI container uses those service registrations to create and provide objects so that these objects can be used in controllers





builder.Services.AddAuthentication(options =>                                        //AddAuthentication -> registers authentication services , AddJwtBearer -> Configures JWT Bearer authentication , TokenValidationParameters -> Define token validation rules  
{                                                                                    
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;      //DefaultAuthenticateScheme -> ASP.NET Core uses JWT Bearer authentication to check the incoming token and identify the user
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;         //DefaultChallengeScheme -> If the client is unauthenticated, ASP.NET Core uses JWT Bearer to handle the challenge.
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;                                            //RequireHttpsMetadata = false , means doesn't require HTTPS when retrieving metadata. 
    options.SaveToken = true;                                                        //saves the token.

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters      //defines the rules ASP.NET Core uses to validate an incoming JWT token
    {
        ValidateIssuer = true,                                                          //checks who issued the token
        ValidateAudience =true,                                                         //checks whether the token is intended for your API.
        ValidateLifetime =true,                                                         //hecks whether the token has expired or is not yet valid
        ValidateIssuerSigningKey =true,                                                 //checks the token's signature using the signing key
        ValidIssuer =builder.Configuration.GetValue<string>("JWT:Issuer"),              //retrieves the expected issuer from the appsettings.json JWT section
        ValidAudience = builder.Configuration.GetValue<string>("JWT:Audience"),         //retrieves the expected audience from the appsettings.json JWT section
        IssuerSigningKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWT:Key")))       //provides the key used to verify the JWT's signature
    };
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
