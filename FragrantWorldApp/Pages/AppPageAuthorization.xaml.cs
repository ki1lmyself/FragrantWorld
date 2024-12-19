using FragrantWorldApp.Data;
using FragrantWorldApp.Models;
using FragrantWorldApp.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace FragrantWorldApp.Pages
{
    public partial class AppPageAuthorization : Page
    {
        private readonly ProductService _productService;
        public ObservableCollection<string> Manufacturers { get; set; }

        private readonly ExamUser _currentUser;
        private readonly ExamRole _currentRole;

        public AppPageAuthorization(ExamUser user)
        {
            _productService = new ProductService(new FragrantWorldContext());
            _currentUser = user;

            InitializeComponent();
            LoadProductsAsync();
            LoadManufacturersAsync();
            DisplayInfo();
        }

        private void DisplayInfo()
        {
            UserInfoLabal.Content = $"{_currentUser.UserSurname} {_currentUser.UserName} {_currentUser.UserPatronymic}";
            
            if (_currentUser.RoleId != null &&
                (_currentUser.RoleId == 1 || _currentUser.RoleId == 3))
            {
                ChangeOrdersButton.Visibility = Visibility.Visible;
            }
            else
            {
                ChangeOrdersButton.Visibility = Visibility.Collapsed; 
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new AppPage();
            NavigationService.Navigate(new AppPage());
        }

        private async void LoadProductsAsync()
        {
            ProductsListBox.ItemsSource = await _productService.GetProductsAsync();
        }

        private async Task LoadManufacturersAsync()
        {
            var manufacturers = await _productService.GetManufacturersAsync();
            Manufacturers = new ObservableCollection<string>(manufacturers);
            Manufacturers.Insert(0, "Все производители");
            ManufacturerComboBox.ItemsSource = manufacturers;
            ManufacturerComboBox.SelectedIndex = 0;
        }

        private async void ManufacturerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ManufacturerComboBox.SelectedItem is string selectedManufacturer)
            {
                var filteredProducts = await _productService.FilterProductsByManufacturerAsync(selectedManufacturer);
                ProductsListBox.ItemsSource = filteredProducts;
            }
        }

        // Фильтрация и сортировка
        private async void SortAscendingButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем текущие параметры фильтрации и поиска
            string manufacturer = ManufacturerComboBox.SelectedItem?.ToString();
            decimal? minPrice = string.IsNullOrEmpty(MinPriceTextBox.Text)
                ? (decimal?)null : decimal.Parse(MinPriceTextBox.Text);
            decimal? maxPrice = string.IsNullOrEmpty(MaxPriceTextBox.Text)
                ? (decimal?)null : decimal.Parse(MaxPriceTextBox.Text);
            string searchString = SearchTextBox.Text;

            // Получаем отсортированные и отфильтрованные товары
            var products = await _productService.GetFilteredSortedSearchedProductsAsync
                (manufacturer, minPrice, maxPrice, searchString, ascending: true);

            // Обновляем источник данных для отображения
            ProductsListBox.ItemsSource = products;

            // Обновляем количество отображаемых товаров
            UpdateCountLabel(products.Count);
        }

        private async void SortDescendingButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем текущие параметры фильтрации и поиска
            string manufacturer = ManufacturerComboBox.SelectedItem?.ToString();
            decimal? minPrice = string.IsNullOrEmpty(MinPriceTextBox.Text)
                ? (decimal?)null : decimal.Parse(MinPriceTextBox.Text);
            decimal? maxPrice = string.IsNullOrEmpty(MaxPriceTextBox.Text)
                ? (decimal?)null : decimal.Parse(MaxPriceTextBox.Text);
            string searchString = SearchTextBox.Text;

            // Получаем отсортированные и отфильтрованные товары
            var products = await _productService.GetFilteredSortedSearchedProductsAsync
                (manufacturer, minPrice, maxPrice, searchString, ascending: false);

            // Обновляем источник данных для отображения
            ProductsListBox.ItemsSource = products;

            // Обновляем количество отображаемых товаров
            UpdateCountLabel(products.Count);
        }

        private async void UpdateCountLabel(int count)
        {
            int totalCount = await _productService.GetTotalProductCountAsync();
            CountTextBlock.Text = $"{count} из {totalCount}";
        }

        private void ChangeOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new OrderPage();
            NavigationService.Navigate(new OrderPage());
        }
    }
}
