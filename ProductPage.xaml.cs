using BejanIonelaLab7.Models;

namespace BejanIonelaLab7
{
    public partial class ProductPage : ContentPage
    {
        ShopList sl;

        public ProductPage(ShopList slist)
        {
            InitializeComponent();
            sl = slist;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listView.ItemsSource = await App.Database.GetProductsAsync();
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var product = (Product)BindingContext;
            if (product == null) return;

            await App.Database.SaveProductAsync(product);
            listView.ItemsSource = await App.Database.GetProductsAsync();
            BindingContext = new Product(); 
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var product = listView.SelectedItem as Product;
            if (product == null)
            {
                await DisplayAlert("Info", "Selectează un produs pentru a-l șterge.", "OK");
                return;
            }

            bool confirm = await DisplayAlert("Confirmare",
                $"Ștergi „{product.Description}” din colecția de produse?",
                "Șterge", "Anulează");
            if (!confirm) return;

            await App.Database.DeleteProductAsync(product);
            listView.ItemsSource = await App.Database.GetProductsAsync();
            listView.SelectedItem = null;
        }

        async void OnAddButtonClicked(object sender, EventArgs e)
        {
            if (listView.SelectedItem is not Product p)
            {
                await DisplayAlert("Info", "Selectează un produs din listă.", "OK");
                return;
            }

            var lp = new ListProduct
            {
                ShopListID = sl.ID,
                ProductID = p.ID
            };

            await App.Database.SaveListProductAsync(lp);
            p.ListProducts = new List<ListProduct> { lp };

            await Navigation.PopAsync();
        }
    }
}
