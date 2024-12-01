using CommunityToolkit.Maui.Alerts;

namespace imago.Services;

public class ToastService
{
    public async Task ShowToast(String message)
    {
        var toast = Toast.Make(message);
        await toast.Show();
    }
}