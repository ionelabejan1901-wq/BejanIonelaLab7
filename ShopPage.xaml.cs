using BejanIonelaLab7.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.ApplicationModel;
using Plugin.LocalNotification;
using System;
using System.Linq;

namespace BejanIonelaLab7
{
    public partial class ShopPage : ContentPage
    {
        public ShopPage()
        {
            InitializeComponent();
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var shop = (Shop)BindingContext;
            await App.Database.SaveShopAsync(shop);
            await Navigation.PopAsync();
        }

        async void OnShowMapButtonClicked(object sender, EventArgs e)
        {
            var shop = (Shop)BindingContext;
            var address = shop.Adress;

            if (string.IsNullOrWhiteSpace(address))
            {
                await DisplayAlert("Adresă lipsă", "Te rog introdu o adresă validă.", "OK");
                return;
            }

            var shoplocation = new Location(46.7492379, 23.5745597); 
            var myLocation = new Location(46.7731796289, 23.6213886738); 

            var distance = myLocation.CalculateDistance(shoplocation, DistanceUnits.Kilometers);

            if (distance < 5)
            {
                var request = new NotificationRequest
                {
                    Title = "Ai de făcut cumpărături în apropiere!",
                    Description = address,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(1)
                    }
                };
                LocalNotificationCenter.Current.Show(request);
            }

            var options = new MapLaunchOptions { Name = "Magazinul meu preferat" };
            await Map.OpenAsync(shoplocation, options);
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var shop = (Shop)BindingContext;
            await App.Database.DeleteShopAsync(shop);
            await Navigation.PopAsync();
        }
    }
}
