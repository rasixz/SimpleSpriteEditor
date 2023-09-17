using System.Numerics;
using System.Runtime.InteropServices;
using Raylib_cs;
using Spritery.Utils;

namespace Spritery.UI;

// TODO: Add cursor, navigation with arrow-keys
// TODO: Add delete key
// TODO: Add selection
//  - All
//  - Parts

public sealed class NumberTextbox
{
    public bool Enabled { get; set; } = true;
    public bool Focused { get; set; }
    public bool EditModeActive { get; set; }

    public Rectangle Bounds { get; set; }

    public int? Value { get; set; }

    private string Text
    {
        get => Value.ToString() ?? "";
        set
        {
            if (string.IsNullOrEmpty(value)) Value = null;
            else Value = int.Parse(value);
        }
    }

    public event EventHandler? NumberConfirmedEvent;
    public event EventHandler? NumberAbortedEvent;

    public NumberTextBoxProperties Properties { get; set; } = new();


    private bool _cursorVisible = true;
    private int _cursor = 0;

    public const int AutoCursorCooldown = 40;
    public const int CursorBlinkRate = 20;

    private int _autoCursorCooldownCounter = 0;
    private int _cursorBlinkCounter = 0;

    public void Draw()
    {
        if (!Enabled)
        {
            Raylib.DrawRectangleRec(Bounds, Properties.BaseColorDisabled);
            Raylib.DrawRectangleLinesEx(Bounds, Properties.BorderWidth, Properties.BorderColorDisabled);
            Raylib.DrawText(Text, (int) (Bounds.x + Properties.TextPadding), (int) (Bounds.y + Bounds.height / 2f - Properties.TextSize / 2f), Properties.TextSize, Properties.TextColorDisabled);

            return;
        }

        Raylib.DrawRectangleRec(Bounds, Properties.BackgroundColor);

        if (EditModeActive)
        {
            Raylib.DrawRectangleLinesEx(Bounds, Properties.BorderWidth, Properties.BorderColorPressed);

            // Draw visible Text
            Raylib.DrawText(Text, (int) (Bounds.x + Properties.TextPadding), (int) (Bounds.y + Bounds.height / 2f - Properties.TextSize / 2f), Properties.TextSize, Properties.TextColorPressed);
            if (_cursorVisible)
            {
                const float width = 2f;
                var drawX = (int) (Raylib.MeasureText(Text[..(_cursor)], Properties.TextSize) + Bounds.x + Properties.TextPadding) + width;
                var height = Properties.TextSize * 1.5f;
                var drawY = Bounds.y + Bounds.height / 2f - height / 2f;
                Raylib.DrawLineEx(new Vector2(drawX, drawY), new Vector2(drawX, drawY + height), width, Properties.TextColorPressed);
            }

            return;
        }

        if (Focused)
        {
            Raylib.DrawRectangleLinesEx(Bounds, Properties.BorderWidth, Properties.BorderColorFocused);
            Raylib.DrawText(Text, (int) (Bounds.x + Properties.TextPadding), (int) (Bounds.y + Bounds.height / 2f - Properties.TextSize / 2f), Properties.TextSize, Properties.TextColorFocused);

            return;
        }

        Raylib.DrawRectangleLinesEx(Bounds, Properties.BorderWidth, Properties.BorderColorNormal);
        Raylib.DrawText(Text, (int) (Bounds.x + Properties.TextPadding), (int) (Bounds.y + Bounds.height / 2f - Properties.TextSize / 2f), Properties.TextSize, Properties.TextColorNormal);
    }

    private void DrawCursor()
    {
    }

    public void Update(float deltaTime)
    {
        if (!Enabled)
        {
            if (Focused) Focused = false;
            if (EditModeActive) EditModeActive = false;
            return;
        }

        Focused = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), Bounds);
        if (Focused && Raylib.IsMouseButtonReleased(MouseButton.MOUSE_BUTTON_LEFT))
        {
            EditModeActive = !EditModeActive;
            if (!EditModeActive)
            {
                OnTextConfirmedEvent();
                return;
            }
        }

        if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_BUTTON_LEFT) && !Focused && EditModeActive)
        {
            OnTextConfirmedEvent();
            return;
        }

        if (EditModeActive)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        _cursorBlinkCounter++;
        if (_cursorBlinkCounter > CursorBlinkRate)
        {
            _cursorVisible = !_cursorVisible;
            _cursorBlinkCounter = 0;
        }

        if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT) || Raylib.IsKeyDown(KeyboardKey.KEY_UP) || Raylib.IsKeyDown(KeyboardKey.KEY_DOWN) || Raylib.IsKeyDown(KeyboardKey.KEY_DELETE) || Raylib.IsKeyDown(KeyboardKey.KEY_BACKSPACE)) _autoCursorCooldownCounter++;
        else _autoCursorCooldownCounter = 0;

        if (Raylib.IsKeyPressed(KeyboardKey.KEY_HOME))
        {
            if (_cursor > 0) _cursor = 0;
        }
        else if (Raylib.IsKeyPressed(KeyboardKey.KEY_END))
        {
            if (_cursor < Text.Length) _cursor = Text.Length;
        }
        else if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT) || (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT) && _autoCursorCooldownCounter > AutoCursorCooldown))
        {
            if (_cursor > 0) _cursor--;
            _cursorVisible = true;
        }
        else if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT) || (Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT) && _autoCursorCooldownCounter > AutoCursorCooldown))
        {
            if (_cursor < Text.Length) _cursor++;
            _cursorVisible = true;
        }
        else if (Raylib.IsKeyPressed(KeyboardKey.KEY_BACKSPACE) || (Raylib.IsKeyDown(KeyboardKey.KEY_BACKSPACE) && _autoCursorCooldownCounter > AutoCursorCooldown))
        {
            if (_cursor > 0)
            {
                Text = Text.Remove(_cursor - 1, 1);
                _cursor--;
            }

            _cursorVisible = true;
        }
        else if (Raylib.IsKeyPressed(KeyboardKey.KEY_DELETE) || (Raylib.IsKeyDown(KeyboardKey.KEY_DELETE) && _autoCursorCooldownCounter > AutoCursorCooldown))
        {
            if (_cursor < Text.Length)
            {
                Text = Text.Remove(_cursor, 1);
            }

            _cursorVisible = true;
        }
        else
        {
            var charPressed = (char) Raylib.GetCharPressed();
            if (charPressed != 0)
            {
                if (charPressed is < '0' or > '9') return;

                _cursorVisible = true;
                Text = Text.Insert(_cursor, charPressed.ToString());
                _cursor++;
            }
        }
    }

    private void OnTextConfirmedEvent()
    {
        EditModeActive = false;
        Console.WriteLine("Confirm");

        NumberConfirmedEvent?.Invoke(this, EventArgs.Empty);
    }

    private void OnTextAbortedEvent()
    {
        EditModeActive = false;
        Console.WriteLine("Abort");

        NumberAbortedEvent?.Invoke(this, EventArgs.Empty);
    }
}

public sealed class NumberTextBoxProperties : Properties
{
    public int TextPadding = 5;
    public Color ColorSelectedForeground = Colors.FromRgba(0xf0fffeff);
    public Color ColorSelectedBackground = Colors.FromRgba(0x839affe0);
}