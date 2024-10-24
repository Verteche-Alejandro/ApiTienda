using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ATDapi.Responses;
using ATDapi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.BearerToken;
using ATDapi.Connection;
using System.Xml.Linq;
using static ATDapi.Models.Login;

[ApiController]
public class LoginController: ControllerBase
{
    private IConfiguration _config;
    private Repository _repo;

    public LoginController(IConfiguration config)
    {
        this._config = config;
        this._repo = new Repository();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]Login login)
    {
        string consulta = login.LoginUser(login.nombre, login.clave);
        RolName rsp = await _repo.GetByQuery<RolName>(consulta);

        if(rsp != null && rsp.nombre == login.nombre && rsp.clave == login.clave)
        {
            var token = GenerateAccessToken(login.nombre,rsp.rol);
            return Ok(new {AccessToken = new JwtSecurityTokenHandler().WriteToken(token)});

        }
        return Unauthorized("Invalid Credentials");
    }

    private JwtSecurityToken GenerateAccessToken(string name,string rol)
    {
        var claims = new List<Claim> 
        {
            new (ClaimTypes.Name, name),
            new (ClaimTypes.Role, rol)
        };

        var token = new JwtSecurityToken(
            issuer: _config["JwtSetting:Issuer"],
            audience: _config["JwtSetting:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(1),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSetting:SecretKey"])),
                SecurityAlgorithms.HmacSha256)

        );
        return token;
    }
}

