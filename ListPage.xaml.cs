using BejanIonelaLab7.Models;

namespace BejanIonelaLab7
{
    public partial class ListPage : ContentPage
    {
        ShopList shopList;

        public ListPage(ShopList sl)
        {
            InitializeComponent();
            shopList = sl;
            BindingContext = shopList;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Încarcă magazinele în Picker
            var items = await App.Database.GetShopsAsync();
            ShopPicker.ItemsSource = (System.Collections.IList)items;
            ShopPicker.ItemDisplayBinding = new Binding("ShopDetails");

            // Încarcă produsele din lista curentă
            listView.ItemsSource = await App.Database.GetListProductsAsync(shopList.ID);
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            // Actualizează data
            shopList.Date = DateTime.UtcNow;

            // Salvează magazinul selectat
            Shop selectedShop = ShopPicker.SelectedItem as Shop;
            if (selectedShop != null)
            {
                shopList.ShopID = selectedShop.ID;
            }

            await App.Database.SaveShoppingListAsync(shopList);
            await DisplayAlert("Info", "Lista a fost salvată.", "OK");
            await Navigation.PopAsync();
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Confirmare",
                $"Ștergi lista „{shopList.Description}”?",
                "Șterge", "Anulează");
            if (!confirm) return;

            await App.Database.DeleteShoppingListAsync(shopList);
            await Navigation.PopAsync();
        }

        async void OnChooseButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductPage(shopList)
            {
                BindingContext = new Product()
            });
        }

        async void OnDeleteItemClicked(object sender, EventArgs e)
        {
            var selectedProduct = listView.SelectedItem as Product;
            if (selectedProduct == null)
            {
                await DisplayAlert("Info", "Selectează un produs din listă.", "OK");
                return;
            }

            bool confirm = await DisplayAlert("Confirmare",
                $"Ștergi „{selectedProduct.Description}” din lista curentă?",
                "Șterge", "Anulează");
            if (!confirm) return;

            await App.Database.DeleteListProductByIdsAsync(shopList.ID, selectedProduct.ID);

            listView.ItemsSource = await App.Database.GetListProductsAsync(shopList.ID);
            listView.SelectedItem = null;
        }
    }
}
