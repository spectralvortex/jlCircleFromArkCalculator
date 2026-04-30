using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace CircleFromArcCalculator;

/// <summary>
/// Calculates the radius of a circle from a chord and sagitta measurement.
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        CalculateAndRender();
    }

    private void InputChanged(object sender, TextChangedEventArgs e)
    {
        if (!IsLoaded)
        {
            return;
        }

        CalculateAndRender();
    }

    private void CalculateClicked(object sender, RoutedEventArgs e)
    {
        CalculateAndRender();
    }

    private void CalculateAndRender()
    {
        if (!TryReadPositiveNumber(ChordInput.Text, out double chord) ||
            !TryReadPositiveNumber(SagittaInput.Text, out double sagitta))
        {
            ShowInvalidState("Enter positive numeric values for AB and CD.");
            return;
        }

        double halfChord = chord / 2.0;
        double radius = ((halfChord * halfChord) + (sagitta * sagitta)) / (2.0 * sagitta);
        double diameter = radius * 2.0;
        double centralAngleRadians = 2.0 * Math.Asin(Math.Min(1.0, halfChord / radius));
        double centralAngleDegrees = centralAngleRadians * 180.0 / Math.PI;

        ValidationText.Text = string.Empty;
        RadiusText.Text = $"{radius:N3} mm";
        DiameterText.Text = $"{diameter:N3} mm";
        AngleText.Text = $"{centralAngleDegrees:N2} deg";

        RenderSketch(chord, sagitta, radius);
    }

    private static bool TryReadPositiveNumber(string value, out double number)
    {
        string normalized = value.Trim().Replace(',', '.');
        return double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out number) &&
               number > 0.0 &&
               double.IsFinite(number);
    }

    private void ShowInvalidState(string message)
    {
        ValidationText.Text = message;
        RadiusText.Text = "-";
        DiameterText.Text = "-";
        AngleText.Text = "-";
    }

    private void RenderSketch(double chord, double sagitta, double radius)
    {
        const double canvasWidth = 520.0;
        const double canvasHeight = 380.0;
        const double margin = 40.0;

        double halfChord = chord / 2.0;
        double centerOffsetFromChord = radius - sagitta;
        double minX = -radius;
        double maxX = radius;
        double minY = Math.Min(-centerOffsetFromChord - radius, -10.0);
        double maxY = Math.Max(-centerOffsetFromChord + radius, sagitta + 10.0);
        double scale = Math.Min((canvasWidth - (margin * 2.0)) / (maxX - minX),
                                (canvasHeight - (margin * 2.0)) / (maxY - minY));

        Point ToCanvas(double x, double y)
        {
            double canvasX = margin + ((x - minX) * scale);
            double canvasY = margin + ((y - minY) * scale);
            return new Point(canvasX, canvasY);
        }

        Point center = ToCanvas(0.0, -centerOffsetFromChord);
        Point a = ToCanvas(-halfChord, 0.0);
        Point b = ToCanvas(halfChord, 0.0);
        Point c = ToCanvas(0.0, 0.0);
        Point d = ToCanvas(0.0, sagitta);

        double scaledRadius = radius * scale;
        CircleShape.Width = scaledRadius * 2.0;
        CircleShape.Height = scaledRadius * 2.0;
        Canvas.SetLeft(CircleShape, center.X - scaledRadius);
        Canvas.SetTop(CircleShape, center.Y - scaledRadius);

        SetLine(ChordLine, a, b);
        SetLine(SagittaLine, c, d);
        SetLine(RadiusLine, center, d);

        SetLabel(LabelA, a.X - 34.0, a.Y - 19.0);
        SetLabel(LabelB, b.X + 8.0, b.Y - 19.0);
        SetLabel(LabelC, c.X - 8.0, c.Y - 38.0);
        SetLabel(LabelD, d.X - 8.0, d.Y + 4.0);
        SetLabel(LabelO, center.X + 8.0, center.Y - 28.0);
    }

    private static void SetLine(Line line, Point start, Point end)
    {
        line.X1 = start.X;
        line.Y1 = start.Y;
        line.X2 = end.X;
        line.Y2 = end.Y;
    }

    private static void SetLabel(UIElement label, double left, double top)
    {
        Canvas.SetLeft(label, left);
        Canvas.SetTop(label, top);
    }
}
