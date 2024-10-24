using System.Reflection.Metadata.Ecma335;

namespace ATDapi.Models;

public class Productos
{
    public int id_prod { get; set; }
    public string nombre {  get; set; }
    public decimal precio {  get; set; }

    public string CreateProd()
    {
        return string.Format("INSERT INTO productos VALUES('{0}',{1})",this.nombre,this.precio);
    }
    public string SelectListProd()
    {
        return string.Format("SELECT * FROM productos");
    }
    public string SelectProdById(int id )
    {
        return string.Format("SELECT FROM productos WHERE id_prod = {0}",id);
    }

    public string DeleteProd(int id)
    {
        return string.Format("DELETE FROM productos WHERE id_prod = {0}", id);
    }

    public string UpdateProd(string nombre, decimal precio, int id)
    {
        return string.Format("UPDATE productos SET nombre='{0}',precio={1} WHERE id_prod ={2}", nombre,precio,id);
    }

}