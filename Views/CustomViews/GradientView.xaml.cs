//using static Android.Icu.Text.CaseMap;

using Microsoft.Maui.Controls.Shapes;

namespace MauiFilters.Views.CustomViews;

public partial class GradientView : ContentView
{
    public static readonly BindableProperty GradientNameProperty =
        BindableProperty.Create(nameof(GradientName), typeof(string), typeof(GradientView), default(string));

    public static readonly BindableProperty ColorsProperty =
        BindableProperty.Create(nameof(Colors), typeof(string), typeof(GradientView), default(string));

    public static readonly BindableProperty DirectionProperty =
        BindableProperty.Create(nameof(Direction), typeof(string), typeof(GradientView), default(string));

    public static readonly BindableProperty FillColorsProperty =
        BindableProperty.Create(nameof(FillColors), typeof(LinearGradientBrush), typeof(GradientView), new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(0, 1),
            GradientStops = new GradientStopCollection
            {
                new GradientStop { Color = Microsoft.Maui.Graphics.Colors.White, Offset = 0.0f },
                new GradientStop { Color = Color.FromArgb("#D3D3D3"), Offset = 1.0f }
            }
        });

    public string GradientName
    {
        get => (string)GetValue(GradientNameProperty);
        set => SetValue(GradientNameProperty, value);
    }

    public string Colors
    {
        get => (string)GetValue(ColorsProperty);
        set => SetValue(ColorsProperty, value);
    }

    public string Direction
    {
        get => (string)GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    public LinearGradientBrush FillColors
    {
        get => (LinearGradientBrush)GetValue(FillColorsProperty);
        set => SetValue(FillColorsProperty, value);
    }

    public GradientView()
	{
		InitializeComponent();
	}
}