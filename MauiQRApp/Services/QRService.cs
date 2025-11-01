using QRCoder;
using Microsoft.Maui.Controls;
using System.Text.Json;
using System.Drawing;

namespace MauiQRApp.Services
{
    public class QRService
    {
        public string GenerarQR(object datos)
        {
            return JsonSerializer.Serialize(datos);
        }
    }
}
