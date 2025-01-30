using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static partial class ColorAnimationExtensions
    {
        public static Task<bool> LinearGradientBrushTo(this VisualElement element,
                                                       LinearGradientBrush startBrush,
                                                       LinearGradientBrush endBrush,
                                                       uint rate = 16u,
                                                       uint length = 250u,
                                                       Easing? easing = null,
                                                       CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(element);
            ArgumentNullException.ThrowIfNull(endBrush);

            var animationCompletionSource = new TaskCompletionSource<bool>();

            try
            {
                var animation = new Animation();

                // Asegurarse de que los pinceles de degradado tienen puntos de inicio y fin
                if (startBrush.StartPoint == null || startBrush.EndPoint == null ||
                    endBrush.StartPoint == null || endBrush.EndPoint == null)
                {
                    throw new ArgumentException("Los pinceles de degradado deben tener puntos de inicio y fin definidos.");
                }

                for (int i = 0; i < endBrush.GradientStops.Count; i++)
                {
                    var endStop = endBrush.GradientStops[i];
                    var startStop = startBrush?.GradientStops.FirstOrDefault(gs => Math.Abs(gs.Offset - endStop.Offset) < 0.01)
                                    ?? new GradientStop(endStop.Color, endStop.Offset);

                    animation.Add(0, 1, GetColorTransformAnimation(element, startStop, endStop, startBrush, endBrush));
                }

                animation.Commit(element, nameof(LinearGradientBrushTo), rate, length, easing, (d, b) => animationCompletionSource.SetResult(true));
            }
            catch (ArgumentException aex)
            {
                System.Diagnostics.Trace.WriteLine($"{aex.GetType().Name} thrown in {typeof(ColorAnimationExtensions).FullName}: {aex.Message}");
                animationCompletionSource.SetResult(false);
            }

            return animationCompletionSource.Task.WaitAsync(token);
        }

        static Animation GetColorTransformAnimation(VisualElement element, GradientStop startStop, GradientStop endStop,
                                                    LinearGradientBrush startBrush, LinearGradientBrush endBrush)
        {
            return new Animation(callback: v =>
            {
                var brush = (element.Background as LinearGradientBrush) ?? new LinearGradientBrush();

                // Interpolate color
                var color = Color.FromRgba(
                    startStop.Color.Red + (endStop.Color.Red - startStop.Color.Red) * v,
                    startStop.Color.Green + (endStop.Color.Green - startStop.Color.Green) * v,
                    startStop.Color.Blue + (endStop.Color.Blue - startStop.Color.Blue) * v,
                    startStop.Color.Alpha + (endStop.Color.Alpha - startStop.Color.Alpha) * v
                );

                // Interpolate start and end points
                var interpolatedStartPoint = new Point(
                    startBrush.StartPoint.X + (endBrush.StartPoint.X - startBrush.StartPoint.X) * v,
                    startBrush.StartPoint.Y + (endBrush.StartPoint.Y - startBrush.StartPoint.Y) * v
                );
                var interpolatedEndPoint = new Point(
                    startBrush.EndPoint.X + (endBrush.EndPoint.X - startBrush.EndPoint.X) * v,
                    startBrush.EndPoint.Y + (endBrush.EndPoint.Y - startBrush.EndPoint.Y) * v
                );

                brush.StartPoint = interpolatedStartPoint;
                brush.EndPoint = interpolatedEndPoint;

                var existingStop = brush.GradientStops.FirstOrDefault(gs => gs.Offset == endStop.Offset);
                if (existingStop != null)
                {
                    existingStop.Color = color;
                }
                else
                {
                    brush.GradientStops.Add(new GradientStop(color, endStop.Offset));
                }

                element.Background = brush;
            }, start: 0, end: 1);
        }
    }
}
