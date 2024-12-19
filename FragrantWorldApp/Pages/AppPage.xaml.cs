using FragrantWorldApp.Data;
using FragrantWorldApp.Models;
using FragrantWorldApp.Services;
using System.Collections.ObjectModel;
using System.Runtime.Intrinsics.X86;
using System.Windows;
using System.Windows.Controls;

namespace FragrantWorldApp.Pages
{
    public partial class AppPage : Page
    {
        private readonly ProductService _productService;
        public ObservableCollection<string> Manufacturers { get; set; }

        private readonly ExamUser _currentUser;

        public AppPage()
        {
            _productService = new ProductService(new FragrantWorldContext());

            InitializeComponent();
            LoadProductsAsync();
            LoadManufacturersAsync();
        }
        private void AuthorizationButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new AuthorizationPage();
            NavigationService.Navigate(new AuthorizationPage());
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
