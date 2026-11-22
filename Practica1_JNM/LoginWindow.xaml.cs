using System;
using System.Windows;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace Practica1_JNM
{
    public partial class LoginWindow : Window
    {
        // Variables para controlar el bloqueo por 3 intentos fallidos (TC05 / TC07)
        private int intentosFallidos = 0;
        private bool usuarioBloqueado = false;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnNuevoUsuario_Click(object sender, RoutedEventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
            {
                lblMensaje.Foreground = Brushes.OrangeRed;
                lblMensaje.Text = "Introduzca usuario y contraseña para registrar un nuevo usuario.";
                return;
            }

            try
            {
                bool creado = DatabaseHelper.RegistrarUsuario(usuario, password);

                if (creado)
                {
                    lblMensaje.Foreground = Brushes.LightGreen;
                    lblMensaje.Text = $"Usuario '{usuario}' registrado correctamente.";
                }
                else
                {
                    lblMensaje.Foreground = Brushes.Red;
                    lblMensaje.Text = $"El usuario '{usuario}' ya existe.";
                }
            }
            catch (MySqlException ex)
            {
                lblMensaje.Foreground = Brushes.Red;
                lblMensaje.Text = $"Error de conexión con MySQL: {ex.Message}";
            }
            catch (Exception ex)
            {
                lblMensaje.Foreground = Brushes.Red;
                lblMensaje.Text = $"Error inesperado al registrar: {ex.Message}";
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Verificación previa de bloqueo
            if (usuarioBloqueado)
            {
                lblMensaje.Foreground = Brushes.Red;
                lblMensaje.Text = "El usuario se encuentra bloqueado temporalmente por exceder los 3 intentos.";
                return;
            }

            string usuarioIngresado = txtUsuario.Text.Trim();
            string passwordIngresada = txtPassword.Password;

            // TC04: No se rellenan los campos del login y pass
            if (string.IsNullOrEmpty(usuarioIngresado) && string.IsNullOrEmpty(passwordIngresada))
            {
                lblMensaje.Foreground = Brushes.OrangeRed;
                lblMensaje.Text = "Introduzca datos";
                return;
            }

            try
            {
                // Consultar contraseña del usuario en MySQL
                string? passwordCorrecta = DatabaseHelper.ObtenerPasswordUsuario(usuarioIngresado);

                // TC03: Inicio de sesión si usuario no existe
                if (passwordCorrecta == null)
                {
                    lblMensaje.Foreground = Brushes.Red;
                    lblMensaje.Text = "El usuario introducido no existe";
                    return;
                }

                // TC01: Inicio de sesión exitoso
                if (passwordIngresada == passwordCorrecta)
                {
                    intentosFallidos = 0; // Reiniciar contador de fallos
                    lblMensaje.Foreground = Brushes.LightGreen;
                    lblMensaje.Text = "Acceso concedido. Redirigiendo...";

                    MainWindow mainHome = new MainWindow(usuarioIngresado);
                    mainHome.Show();
                    this.Close();
                }
                // TC02 / TC05 / TC07: Contraseña incorrecta y control de intentos
                else
                {
                    intentosFallidos++;

                    if (intentosFallidos >= 3)
                    {
                        usuarioBloqueado = true;
                        lblMensaje.Foreground = Brushes.Red;
                        lblMensaje.Text = "Se ha bloqueado temporalmente el usuario al introducir 3 veces mal la contraseña.";
                        txtUsuario.IsEnabled = false;
                        txtPassword.IsEnabled = false;
                        btnLogin.IsEnabled = false;
                    }
                    else
                    {
                        lblMensaje.Foreground = Brushes.Red;
                        lblMensaje.Text = $"La contraseña introducida no es correcta (Intento {intentosFallidos}/3)";
                    }
                }
            }
            catch (MySqlException ex)
            {
                lblMensaje.Foreground = Brushes.Red;
                lblMensaje.Text = $"Error de conexión con MySQL: {ex.Message}";
            }
            catch (Exception ex)
            {
                lblMensaje.Foreground = Brushes.Red;
                lblMensaje.Text = $"Error inesperado: {ex.Message}";
            }
        }

        // TC06: Salir de la aplicación
        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}