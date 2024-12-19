using FragrantWorldApp.Data;
using FragrantWorldApp.Services;
using System.Windows;
using System.Windows.Controls;

namespace FragrantWorldApp.Pages
{
    public partial class AuthorizationPage : Page
    {
        private readonly UserService _userService;

        public AuthorizationPage()
        {
            InitializeComponent();

            _userService = new UserService(new FragrantWorldContext());

        }

        private async void LoginButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var login = LoginTextBox.Text;
            var password = PasswordBox.Password;

            if (string.IsNullOrEmpty(login))
            {
                ErrorLoginLabel.Visibility = Visibility.Visible;
                ErrorLoginLabel.Content = "Логин не может быть пустым";
            }

            if (string.IsNullOrEmpty(password))
            {
                ErrorPasswordLabel.Visibility = Visibility.Visible;
                ErrorPasswordLabel.Content = "Пароль не может быть пустым";
            }

            var user = await _userService.LoginAsync(login, password);

            if (user != null)
            {
                this.Content = new AppPageAuthorization(user);
                NavigationService.Navigate(new AppPageAuthorization(user));
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
    }
}
