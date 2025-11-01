using Microsoft.Maui.Devices.Sensors;

namespace MauiQRApp.Services
{
    public class GeoService
    {
        public async Task<Location?> GetLocationAsync()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));
                return location;
            }
            catch
            {
                return null;
            }
        }
    }
}
