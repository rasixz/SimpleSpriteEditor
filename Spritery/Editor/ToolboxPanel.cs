using Raylib_cs;
using Spritery.Editor.Tools;
using Spritery.UI;

namespace Spritery.Editor;

public sealed class ToolboxPanel : Panel
{
    private readonly List<Tool> _tools = new();
    private readonly List<Button> _buttons = new();

    private const int ToolsPerLine = 2;
    private const int SpaceCount = ToolsPerLine - 1;
    private int _toolButtonSize = 0;

    private int _selectedButtonIndex = 0;
    public Tool SelectedTool => _tools[_selectedButtonIndex];

    public ToolboxPanel()
    {
        Title = "Tools";

        _tools.Add(new PencilTool());
        _tools.Add(new EraserTool());
        _tools.Add(new BucketTool());
        _tools.Add(new ColorPickerTool());

        for (var i = 0; i < _tools.Count; i++)
        {
            var index = i;
            var t = _tools[i];
            var title = t.Name; //[..1].ToUpper();

            /*if (t.Name.Contains(' '))
            {
                title += title.Substring(title.IndexOf(' ') + 1, 1);
            }*/


            var button = new Button(title);
            button.ButtonClickedEvent += (_, _) => { _selectedButtonIndex = index; };
            _buttons.Add(button);
        }
    }

    public override void Draw()
    {
        base.Draw();

        foreach (var button in _buttons)
        {
            button.Draw();
        }
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        _toolButtonSize = (int) ((ContentBounds.width - SpaceCount * Padding) / ToolsPerLine);

        var x = ContentBounds.x;
        var y = ContentBounds.y;

        for (var i = 0; i < _tools.Count; i++)
        {
            if (i > 0 && i % ToolsPerLine == 0)
            {
                x = ContentBounds.x;
                y += _toolButtonSize + Padding;
            }

            var bounds = new Rectangle(x, y, _toolButtonSize, _toolButtonSize);

            var button = _buttons[i];
            button.Bounds = bounds;
            button.Enabled = i != _selectedButtonIndex;
            button.Update(deltaTime);

            x += _toolButtonSize + Padding;
        }
    }
}