using System.Windows;

namespace Practica1_JNM
{
    public partial class MainWindow : Window
    {
        // Constructor por defecto
        public MainWindow()
        {
            InitializeComponent();
        }

        // Constructor sobrecargado que recibe el nombre del usuario desde el Login
        public MainWindow(string usuario) : this()
        {
            lblUsuarioBienvenida.Text = $"Hola, {usuario}";
        }

        // Evento para el botón "Cerrar Sesión"
        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close(); // Cierra MainWindow y regresa a la pantalla de Login
        }
    }
}