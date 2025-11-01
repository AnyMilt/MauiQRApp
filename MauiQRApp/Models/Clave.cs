using SQLite;

namespace MauiQRApp.Models
{
    public class Clave
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Codigo { get; set; }
    }
}
