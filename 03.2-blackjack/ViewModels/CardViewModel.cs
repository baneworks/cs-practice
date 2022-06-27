using System;
using System.IO;
using System.Globalization;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Data.Converters;
using ReactiveUI;

using Blackjack.Model;

namespace Blackjack.ViewModels;

public class CardViewModel : ViewModelBase
{
    Action? _callback = null;
    public Card? Card { get; set; } = null;
    public int? Width { get; set; } = null;
    public int? Height { get; set; } = null;
    public bool Enabled { get; set; } = false;
    public CardViewModel () => Card = null;
    public CardViewModel(Card card) => Card = card;
    public void OnCommand()
    {
        if (_callback != null)
            _callback();
    }
    public Action? Callback
    {
        get => _callback;
        set
        {
            Enabled = true;
            _callback = value;
        }
    }
}

public class UriConverter : IValueConverter
{
    public static readonly UriConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Uri sourceUri && targetType.IsAssignableTo(typeof(IImage)))
        {
            switch (parameter)
            {
                //! fixme: pass Avalonia.Data.Binding as parameter and dive into it is a terrible idea
                case Binding bind when bind.DefaultAnchor?.Target is Image image:

                    if (image == null)
                        break;

                    Bitmap bitmap;

                    //* if height set scale bitmap to it
                    if (image.Height > 0)
                        bitmap = Bitmap.DecodeToHeight(OpenAsset(sourceUri), (int)image.Height);
                    else
                        bitmap = new Bitmap(OpenAsset(sourceUri));

                    //* if width set crop bitmap
                    if (image.Width > 0)
                    {
                        int height = image.Height > 0 ? (int)image.Height :  (int)bitmap.Size.Height;
                        return new CroppedBitmap(bitmap, new PixelRect(0, 0, (int)image.Width, height));
                    }

                    return bitmap;
            }
            throw new InvalidOperationException("ivalid image converter param");
        }

        // converter used for the wrong type
        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    Stream? OpenAsset(Uri uri)
    {
        return AvaloniaLocator.Current?.GetService<IAssetLoader>()?.Open(uri);
    }
}