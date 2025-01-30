using MauiFilters.Views.CustomViews;
using Microsoft.Maui.Controls.Shapes;
using Utilities;

namespace MauiFilters.Views;

public partial class MauiFiltersView : ContentPage
{
	public MauiFiltersView()
	{
		InitializeComponent();
        BindingContext = new MauiFilterViewModel();
    }

    private void TapGestureRecognizer_OnTapped(object? sender, TappedEventArgs e)
    {
        if (Overlay.Background is not LinearGradientBrush fromBackground)
        {
            fromBackground = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
                GradientStops =
                [
                    new GradientStop(Colors.Transparent, 0),
                    new GradientStop(Colors.Transparent, 0)
                ]
            };
        }

        var control = sender as GradientView;
        var colorRectangle = control.FindByName<RoundRectangle>("ColorRectangle");

        if (colorRectangle.Fill is LinearGradientBrush toBackground) 
            Overlay.LinearGradientBrushTo(fromBackground, toBackground, length: 1000);
    }
}