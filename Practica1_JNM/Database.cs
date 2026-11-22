using MySql.Data.MySqlClient;

namespace Practica1_JNM
{
    public static class DatabaseHelper
    {
        // Cambia TU_CONTRASEÑA_ROOT por la contraseña que pusiste para root en MySQL
        private const string ConnectionString =
            "Server=localhost;Port=3306;Database=catalogo_videojuegos;Uid=root;Pwd=root;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public static bool ValidarUsuario(string usuario, string password)
        {
            using var conn = GetConnection();
            conn.Open();

            const string sql = "SELECT COUNT(*) FROM Usuarios WHERE nombre_usuario = @usuario AND password = @password;";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Parameters.AddWithValue("@password", password);

            long count = (long)(cmd.ExecuteScalar() ?? 0L);
            return count > 0;
        }

        public static string? ObtenerPasswordUsuario(string usuario)
        {
            using var conn = GetConnection();
            conn.Open();

            const string sql = "SELECT password FROM Usuarios WHERE nombre_usuario = @usuario LIMIT 1;";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@usuario", usuario);

            object? result = cmd.ExecuteScalar();
            return result == null || result == DBNull.Value ? null : result.ToString();
        }

        public static bool RegistrarUsuario(string usuario, string password)
        {
            using var conn = GetConnection();
            conn.Open();

            const string sql = "INSERT INTO Usuarios (nombre_usuario, password) VALUES (@usuario, @password);";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Parameters.AddWithValue("@password", password);

            try
            {
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                // Error 1062: Entrada duplicada en columna UNIQUE (nombre_usuario)
                return false;
            }
        }
    }
}