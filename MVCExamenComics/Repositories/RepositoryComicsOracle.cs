using Microsoft.AspNetCore.Http.HttpResults;
using MVCExamenComics.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

#region PROCEDIMIENTOS ALMACENADOS

//INSERTAR COMIC
//create or replace procedure SP_INSERT_COMIC
//(p_idComic COMICS.IDCOMIC%TYPE, p_nombre COMICS.NOMBRE%TYPE, p_imagen COMICS.IMAGEN%TYPE, p_decripcion COMICS.DESCRIPCION%TYPE)
//AS
//BEGIN
//  insert into COMICS values(p_idComic, p_nombre, p_imagen, p_decripcion);
//commit;
//END;

//ELIMINAR COMIC
//create or replace procedure SP_DELETE_COMIC
//(p_idComic COMICS.IDCOMIC%TYPE)
//AS
//BEGIN
//  delete from COMICS where IDCOMIC = p_idComic;
//commit;
//END;

#endregion

namespace MVCExamenComics.Repositories
{
    public class RepositoryComicsOracle : IRepositoryComics
    {
        private DataTable tablaComics;
        private OracleCommand command;
        private OracleConnection connection;

        public RepositoryComicsOracle()
        {
            string connectionString = @"Data Source=LOCALHOST:1521/XE; Persist Security Info=True; User Id=SYSTEM; Password=oracle";
            this.connection = new OracleConnection(connectionString);
            this.command = new OracleCommand();
            this.command.Connection = this.connection;
            string sql = "select * from COMICS";
            OracleDataAdapter adComic = new OracleDataAdapter(sql, this.connection);
            this.tablaComics = new DataTable();
            adComic.Fill(this.tablaComics);
        }

        public List<Comic> Getcomics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;
            List<Comic> comics = new List<Comic>();
            foreach (var row in consulta)
            {
                Comic comic = new Comic();
                comic.IdComic = row.Field<int>("IDCOMIC");
                comic.Nombre = row.Field<string>("NOMBRE");
                comic.Imagen = row.Field<string>("IMAGEN");
                comic.Descripcion = row.Field<string>("DESCRIPCION");
                comics.Add(comic);
            }
            return comics;
        }

        public Comic FindComic(int idComic)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           where datos.Field<int>("IDCOMIC") == idComic
                           select datos;
            var row = consulta.First();
            Comic comic = new Comic();
            comic.IdComic = row.Field<int>("IDCOMIC");
            comic.Nombre = row.Field<string>("NOMBRE");
            comic.Imagen = row.Field<string>("IMAGEN");
            comic.Descripcion = row.Field<string>("DESCRIPCION");
            return comic;
        }

        public void InsertComic(int idComic, string nombre, string imagen, string descripcion)
        {
            string sql = "insert into COMICS values (p_idComic, p_nombre, p_imagen, p_descripcion)";
            OracleParameter pamIdComic = new OracleParameter(":p_idComic", idComic);
            this.command.Parameters.Add(pamIdComic);
            OracleParameter pamNombre = new OracleParameter(":p_nombre", nombre);
            this.command.Parameters.Add(pamNombre);
            OracleParameter pamImagen = new OracleParameter(":p_imagen", imagen);
            this.command.Parameters.Add(pamImagen);
            OracleParameter pamDescripcion = new OracleParameter(":p_descripcion", descripcion);
            this.command.Parameters.Add(pamDescripcion);
            this.command.CommandText = sql;
            this.command.CommandType = CommandType.Text;
            this.connection.Open();
            int af = this.command.ExecuteNonQuery();
            this.connection.Close();
            this.command.Parameters.Clear();
        }

        public void DeleteComic(int idComic)
        {
            OracleParameter pamidComic = new OracleParameter(":p_idComic", idComic);
            this.command.Parameters.Add(pamidComic);
            this.command.CommandText = "SP_DELETE_COMIC";
            this.command.CommandType = CommandType.StoredProcedure;
            this.connection.Open();
            this.command.ExecuteNonQuery();
            this.connection.Close();
            this.command.Parameters.Clear();
        }

        public List<string> GetComicsNombre()
        {
            var consulta = (from datos in this.tablaComics.AsEnumerable()
                            select datos.Field<string>("NOMBRE")).Distinct();
            List<string> nombres = new List<string>();
            foreach (string nombre in consulta)
            {
                nombres.Add(nombre);
            }
            return nombres;
        }

        public Comic FindComicDetalle(string nombre)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           where datos.Field<string>("NOMBRE") == nombre
                           select datos;
            var row = consulta.First();
            Comic comic = new Comic();
            comic.IdComic = row.Field<int>("IDCOMIC");
            comic.Nombre = row.Field<string>("NOMBRE");
            comic.Imagen = row.Field<string>("IMAGEN");
            comic.Descripcion = row.Field<string>("DESCRIPCION");
            return comic;
        }
    }
}
