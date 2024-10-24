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
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Authorization;

[ApiController]
public class ProductosController : ControllerBase
{
    private IConfiguration _config;
    private Repository _repo;

    public ProductosController(IConfiguration config)
    {
        this._config = config;
        this._repo = new Repository();
    }
    [HttpPost("alta")]
    [Authorize]
    public async Task<BaseResponse> Alta([FromBody] Productos productos)
    {
        var user = HttpContext.User;

        if(user.IsInRole("admin"))
        {
            try
            {
                string consulta = productos.CreateProd();
                var rsp = await this._repo.InsertByQuery(consulta);
                return new BaseResponse(false, (int)HttpStatusCode.Created, "Producto nuevo agregado con Exito");
            }
            catch (Exception ex)
            {
                return new BaseResponse(false, (int)HttpStatusCode.Created, ex.Message);
            }
        }
        else
        {
            return new BaseResponse(false,(int)HttpStatusCode.BadRequest,("No es un usuario Adminitrador"));
        }
    }

}