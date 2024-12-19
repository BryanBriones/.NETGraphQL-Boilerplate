using System.Text;
using AppAny.HotChocolate.FluentValidation;
using AuthCore.Helpers;
using Data;
using HotChocolate.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Schema;
using Schema.Mutations;
using Schema.Queries;


var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddScoped<AuthCore.Services.AuthService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>{
    options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                            ValidateAudience = true,
                            ValidateIssuer = true,
                            ValidateIssuerSigningKey = true,
                            ValidAudience = "audience",
                            ValidIssuer = "issuer",
                            RequireSignedTokens = false,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthSettings.PrivateKey))
                    };
});

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("Admin", builder =>
        builder
            .RequireAuthenticatedUser()
            .RequireRole("Admin")
    );

});

builder.Services.AddSchemaLayer(builder.Configuration);


builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
        .ModifyOptions(o =>
     {
       o.EnableDefer = true;
       o.EnableStream = true;
     })
    .InitializeOnStartup()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    // .AddType<UploadType>()
    .AddProjections()
    .AddFluentValidation(o =>
    {
        o.UseDefaultErrorMapper();
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapGraphQL();
app.UseCors();
app.Run();