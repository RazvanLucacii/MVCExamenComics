using MVCExamenComics.Models;
using System.Data;
using System.Data.SqlClient;

namespace MVCExamenComics.Repositories
{
    public class RepositoryComicsSQL: IRepositoryComics
    {
        private DataTable tablaComics;
        private SqlConnection connection;
        private SqlCommand command;

        public RepositoryComicsSQL() 
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS01;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Password=MCSD2023";         
            this.connection = new SqlConnection(connectionString);
            this.command = new SqlCommand();
            this.command.Connection = this.connection;
            this.tablaComics = new DataTable();
            string sql = "select * from COMICS";
            SqlDataAdapter adComic = new SqlDataAdapter(sql, this.connection);
            adComic.Fill(this.tablaComics);
        }

        public List<Comic> Getcomics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() 
                           select datos;
            List<Comic> comics = new List<Comic>();
            foreach (var row in consulta)
            {
                Comic comic = new Comic
                {
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION")
                };
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
            string sql = "insert into COMICS values (@idComic, @nombre, @imagen, @descripcion)";
            this.command.Parameters.AddWithValue("@idComic", idComic);
            this.command.Parameters.AddWithValue("@nombre", nombre);
            this.command.Parameters.AddWithValue("@imagen", imagen);
            this.command.Parameters.AddWithValue("@descripcion", descripcion);
            this.command.CommandText = sql;
            this.command.CommandType = CommandType.Text;
            this.connection.Open();
            int af = this.command.ExecuteNonQuery();
            this.connection.Close();
            this.command.Parameters.Clear();
        }

        public void DeleteComic(int idComic)
        {
            string sql = "DELETE FROM COMICS WHERE IDCOMIC = @idComic";
            this.command.Parameters.AddWithValue("@idComic", idComic);
            this.command.CommandText = sql;
            this.command.CommandType = CommandType.Text;
            this.connection.Open();
            int af = this.command.ExecuteNonQuery();
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
