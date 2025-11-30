using BejanIonelaLab7.Models;

namespace BejanIonelaLab7
{
    public partial class ListEntryPage : ContentPage
    {
        public ListEntryPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listsView.ItemsSource = await App.Database.GetShopListsAsync();
        }

        async void OnShopListAddedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListPage(new ShopList
            {
                Description = string.Empty
            }));
        }

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is ShopList selected)
            {
                await Navigation.PushAsync(new ListPage(selected));
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}
