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
    private Repository basedatos;

    public ProductosController(IConfiguration config)
    {
        this._config = config;
        this.basedatos = new Repository();
    }
    
    [HttpGet("lista")]
    [Authorize]
    public async Task<BaseResponse> Lista()
    {
        try
        {
            string consulta = Productos.SelectListProd();
            var rsp = await this.basedatos.GetListBy<Productos>(consulta);
            return new DataResponse<dynamic>(true, (int)HttpStatusCode.Created,(""), rsp);
        }
        catch (Exception ex)
        {
            return new BaseResponse(false, (int)HttpStatusCode.Created, ex.Message);
        }
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
                var rsp = await this.basedatos.InsertByQuery(consulta);
                return new BaseResponse(false, (int)HttpStatusCode.Created, "Producto nuevo agregado con Exito");
            }
            catch (Exception ex)
            {
                return new BaseResponse(false, (int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
        else
        {
            return new BaseResponse(false,(int)HttpStatusCode.BadRequest,("No es un usuario Adminitrador"));
        }
    }

    

    [HttpDelete("baja/{id}")]
    [Authorize]
    public async Task<BaseResponse> Baja([FromQuery] int id)
    {
        var user = HttpContext.User;
        if (user.IsInRole("admin"))
        {
            try
            {
                string consulta = Productos.DeleteProd(id);
                var rsp = await this.basedatos.DeleteAsync(consulta);
                return new BaseResponse(true, (int)HttpStatusCode.Created, ("Producto eliminado con exito"));
            }catch (Exception ex)
            {

                return new BaseResponse(false, (int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
        else
        {
            return new BaseResponse(false, (int)HttpStatusCode.BadRequest, ("No es un usuario Adminitrador"));
        }
    }

    [HttpPut("modificar")]
    [Authorize]
    public async Task<BaseResponse> Modi([FromBody] Productos productos)
    {
        var user = HttpContext.User;
        if (user.IsInRole("admin"))
        {

            if(productos.id_prod != 0)
            {
                try
                {
                    string query = productos.UpdateProd(productos.nombre,productos.precio,productos.id_prod);
                    var rsp = await this.basedatos.UpdateByQuery(query);
                    return new BaseResponse(true, (int)HttpStatusCode.Created, ("Producto modificado correctamente"));
                }
                catch (Exception ex)
                {
                    return new BaseResponse(false, (int)HttpStatusCode.BadRequest, ex.Message);
                }
            }
            else
            {
                return new BaseResponse(false, (int)HttpStatusCode.BadRequest, ("Se necesita un ID del producto"));
            }
        }
        else
        {
            return new BaseResponse(false, (int)HttpStatusCode.BadRequest, ("No es un usuario Administrador"));
        }
    }

}