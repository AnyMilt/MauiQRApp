using SQLite;
using MauiQRApp.Models;

namespace MauiQRApp.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _db;

        public DatabaseService()
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "maui_qr.db3");
            _db = new SQLiteAsyncConnection(dbPath);
            _db.CreateTableAsync<Clave>().Wait();
        }

        public async Task SeedDataAsync()
        {
            var count = await _db.Table<Clave>().CountAsync();
            if (count == 0)
            {
                await _db.InsertAsync(new Clave { Codigo = "1234" });
                await _db.InsertAsync(new Clave { Codigo = "ABCD" });
            }
        }

        public async Task<bool> ValidarClaveAsync(string clave)
        {
            var item = await _db.Table<Clave>().Where(c => c.Codigo == clave).FirstOrDefaultAsync();
            return item != null;
        }

        public async Task InsertarClaveAsync(string clave)
        {
            await _db.InsertAsync(new Clave { Codigo = clave });
        }
        public async Task SaveDocenteIdAsync(string idDocente)
        {
            // Asegurar que la tabla existe
            await _db.CreateTableAsync<Docente>();

            // Buscar si ya existe un registro
            var existente = await _db.Table<Docente>().FirstOrDefaultAsync();

            if (existente == null)
            {
                // Si no existe, crear nuevo
                var nuevoDocente = new Docente
                {
                    IdDocente = idDocente                   
                };
                await _db.InsertAsync(nuevoDocente);
            }
            else
            {
                // Si existe, actualizar
                existente.IdDocente = idDocente;
                
                await _db.UpdateAsync(existente);
            }
        }

    }
}
