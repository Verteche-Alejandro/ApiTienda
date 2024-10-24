using System.ComponentModel.DataAnnotations;

namespace ATDapi.Models;

public class Login 
{
    public int id_user { get; set; }
    [Required(ErrorMessage ="Pone el nombre papa.")]
    public string nombre { get; set; }

    [Required(ErrorMessage = "Y la contraseña?")]
    [DataType(DataType.Password)]
    public string clave { get; set; }

    public int id_rol { get; set; }



    public string SelectUserById(int id)
    {
        return string.Format("SELECT FROM usuarios WHERE id_user = {0}", id);
    }

    public string LoginUser(string name, string pass) {
        return string.Format("SELECT u.*, r.nombre as rol " +
                            "FROM usuarios as u " +
                            "LEFT JOIN roles as r " +
                            "ON r.id_rol = u.id_rol "+
                            "WHERE u.nombre='{0}' and u.clave ='{1}'", name, pass);
    }

    public string SelectListUser() 
    {
        return string.Format("SELECT * FROM usuarios");
    }

    public string GetRol(int id_user)
    {
        return string.Format("SELECT r.nombre FROM roles AS r " +
                            "INNER JOIN usuarios as u " +
                            "ON u.id_rol = r.id_rol " +
                            "WHERE u.id_user={0}",id_user);
    }

    public string GetPermisosbyIdRol(int id_rol)
    {
        return string.Format("SELECT p.permiso FROM permisos as p " +
                            "INNER JOIN ppr " +
                            "ON ppr.id_prm = p.id_prm " +
                            "WHERE ppr.id_rol = {0}",id_rol);
    }
   public class RolName : Login
    {
        public string rol = "";
    }

}