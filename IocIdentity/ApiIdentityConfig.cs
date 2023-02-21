using System.Security.Cryptography;
using System.Text;
using IocIdentity.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace IocIdentity;

public static class ApiIdentityConfig
{

    public static void AddApiConfig(this IServiceCollection services,IConfiguration configuration)
    {
        
        RsaSecurityKey issuerSigningKey = GetIssuerSigningKey();
        var tokenValidationParameters = new TokenValidationParameters
        {
            // Token signature will be verified using a private key.
            ValidateIssuerSigningKey = true,
            RequireSignedTokens = true,
           //Assymetic key
            IssuerSigningKey = issuerSigningKey, //AssymeticEncyption is used to encrypt the token
            
            // Token will only be valid for "iss" claim.
            ValidateIssuer = true,
            ValidIssuer = configuration["JWTSettings:Issuer"],

            // Token will only be valid for "aud" claim.
            ValidateAudience = true,
            ValidAudience = configuration["JWTSettings:Audience"],

            // Token will only be valid if not expired yet, with 5 minutes clock skew.
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ClockSkew = new TimeSpan(0, 5, 0),

            ValidateActor = false
        };

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            
        }).AddJwtBearer(options => { options.TokenValidationParameters = tokenValidationParameters; });


        services.AddScoped<IJwtBuilder, JwtBuilder>();
       
    }
    
    //Load public key to validate signature
    public static RsaSecurityKey GetIssuerSigningKey()
    {
        var rsa = RSA.Create();
        string publicXmlKey = File.ReadAllText("./public.xml");
        rsa.FromXmlString(publicXmlKey);

        return new RsaSecurityKey(rsa);
    }

}