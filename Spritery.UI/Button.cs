using Raylib_cs;

namespace Spritery.UI;

public sealed class Button
{
    public bool Enabled { get; set; } = true;
    public bool Focused { get; set; }

    private bool _pressed = false;

    public Rectangle Bounds { get; set; }
    public string Text { get; set; }

    public event EventHandler? ButtonClickedEvent;

    public Properties Properties { get; set; } = new();

    public Button(string text)
    {
        Text = text;
    }

    public void Draw()
    {
        var textWidth = Raylib.MeasureText(Text, Properties.TextSize);
        var halfTextWidth = (int) (textWidth / 2f);

        if (!Enabled)
        {
            Raylib.DrawRectangleRec(Bounds, Properties.BaseColorDisabled);
            Raylib.DrawRectangleLinesEx(Bounds, Properties.BorderWidth, Properties.BorderColorDisabled);
            Raylib.DrawText(Text, (int) (Bounds.x + Bounds.width / 2f - halfTextWidth), (int) (Bounds.y + Bounds.height / 2f - Properties.TextSize / 2f), Properties.TextSize, Properties.TextColorDisabled);

            return;
        }

        if (_pressed)
        {
            Raylib.DrawRectangleRec(Bounds, Properties.BaseColorPressed);
            Raylib.DrawRectangleLinesEx(Bounds, Properties.BorderWidth, Properties.BorderColorPressed);
            Raylib.DrawText(Text, (int) (Bounds.x + Bounds.width / 2f - halfTextWidth), (int) (Bounds.y + Bounds.height / 2f - Properties.TextSize / 2f), Properties.TextSize, Properties.TextColorPressed);

            return;
        }

        if (Focused)
        {
            Raylib.DrawRectangleRec(Bounds, Properties.BaseColorFocused);
            Raylib.DrawRectangleLinesEx(Bounds, Properties.BorderWidth, Properties.BorderColorFocused);
            Raylib.DrawText(Text, (int) (Bounds.x + Bounds.width / 2f - halfTextWidth), (int) (Bounds.y + Bounds.height / 2f - Properties.TextSize / 2f), Properties.TextSize, Properties.TextColorFocused);

            return;
        }

        Raylib.DrawRectangleRec(Bounds, Properties.BaseColorNormal);
        Raylib.DrawRectangleLinesEx(Bounds, Properties.BorderWidth, Properties.BorderColorNormal);
        Raylib.DrawText(Text, (int) (Bounds.x + Bounds.width / 2f - halfTextWidth), (int) (Bounds.y + Bounds.height / 2f - Properties.TextSize / 2f), Properties.TextSize, Properties.TextColorNormal);
    }

    public void Update(float deltaTime)
    {
        if (!Enabled)
        {
            if (Focused) Focused = false;
            if (_pressed) _pressed = false;
            return;
        }

        Focused = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), Bounds);
        _pressed = Focused && Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON);

        if (Focused && Raylib.IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON))
        {
            OnButtonClickedEvent();
        }
    }

    private void OnButtonClickedEvent()
    {
        ButtonClickedEvent?.Invoke(this, EventArgs.Empty);
    }
}