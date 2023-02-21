using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IocIdentity.Interfaces;
using Jose;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IocIdentity;

public class JwtBuilder : IJwtBuilder
{
    
    private readonly IConfiguration _configuration;
    
    public JwtBuilder(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateToken()
    {
        //load JWTSettings section from app settings.json
        var jwtSettings = _configuration.GetSection("JWTSettings");
        
        //get the issuer from the section
        var  issuer = jwtSettings.GetSection("Issuer").Value;
        //get the audience from the section
        var audience = jwtSettings.GetSection("Audience").Value;
        
        var jwt = new JwtSecurityToken( //create secyrity token with claims
            claims: new Claim[] {//add claims here
                new Claim("sub", "test"),
                //add issuer
                new Claim(JwtRegisteredClaimNames.Iss, issuer),
                //add audience
                new Claim(JwtRegisteredClaimNames.Aud, audience),
            },
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: GetAudienceSigningKey()
        );
        
        string rsa_token = new JwtSecurityTokenHandler().WriteToken(jwt);
        
        return rsa_token;
      
    }
    
    public SigningCredentials GetAudienceSigningKey() //get private key to sign the token
    {
        var rsa = RSA.Create();
        string privateXmlKey = File.ReadAllText("./private.xml");
        rsa.FromXmlString(privateXmlKey);

        return new SigningCredentials(
            key: new RsaSecurityKey(rsa),
            algorithm: SecurityAlgorithms.RsaSha256);
    }

}