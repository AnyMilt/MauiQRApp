namespace MauiQRApp.Views;
using ZXing.Net.Maui;

public partial class QRResultPage : ContentPage
{
    public QRResultPage(string qrValue)
    {
        InitializeComponent();
        qrView.Format = BarcodeFormat.QrCode;
        qrView.Value = qrValue;
    }
}
