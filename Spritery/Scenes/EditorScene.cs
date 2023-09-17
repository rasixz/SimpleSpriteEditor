using System.Numerics;
using Raylib_cs;
using Spritery.Editor;
using Spritery.Revertable;
using Spritery.Utils;

namespace Spritery.Scenes;

public sealed class EditorScene : Scene
{
    private readonly EditorMeta _editorMeta = new();

    private readonly UndoList _undoList = new();

    private bool _debugOverlayVisible = true;

    private Rectangle _leftBounds;
    private Rectangle _editorBounds;
    private Rectangle _rightBounds;

    private readonly ToolboxPanel _toolboxPanel;
    private readonly ColorPalettePanel _colorPalettePanel;
    private readonly EditorPanel _editorPanel;
    private readonly ImportExportPanel _importExportPanel;

    public EditorScene()
    {
        _leftBounds = Rectangles.FromLtrb(0, 0, 140, Raylib.GetScreenHeight());
        _rightBounds = Rectangles.FromLtrb(Raylib.GetScreenWidth() - 200, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        _editorBounds = Rectangles.FromLtrb(_leftBounds.Right() - 1, 0, _rightBounds.Left() + 1, Raylib.GetScreenHeight());

        _colorPalettePanel = new ColorPalettePanel();
        _toolboxPanel = new ToolboxPanel();
        _editorPanel = new EditorPanel
        {
            Sprite = new Sprite(128)
        };
        _importExportPanel = new ImportExportPanel();
        _importExportPanel.LoadSpriteEvent += (_, args) =>
        {
            _editorPanel.Sprite = Sprite.Import(args.FilePath);
            _editorMeta.Sprite = _editorPanel.Sprite;
        };
        _importExportPanel.SaveSpriteEvent += (_, args) => { Sprite.Export(_editorPanel.Sprite, args.FilePath); };
        _importExportPanel.NewSpriteEvent += (_, args) =>
        {
            _editorPanel.Sprite = new Sprite(args.Width, args.Height);
            _editorMeta.Sprite = _editorPanel.Sprite;
        };

        _editorMeta.Sprite = _editorPanel.Sprite;
        _editorMeta.UndoList = _undoList;
    }

    public override void Draw()
    {
        _editorPanel.Draw();
        _toolboxPanel.Draw();
        _colorPalettePanel.Draw();
        _importExportPanel.Draw();
        
        if (_editorPanel.HoveredCell != EditorPanel.Invalid)
        {
            _toolboxPanel.SelectedTool.Draw();
        }

        if (_debugOverlayVisible) DrawDebugOverlay();
    }

    private void DrawDebugOverlay()
    {
        var hoveredCell = _editorPanel.HoveredCell;

        DebugOverlay.Begin(_editorPanel.ContentBounds);
        DebugOverlay.WriteLeft($"FPS: {Raylib.GetFPS()}");
        if (hoveredCell != EditorPanel.Invalid)
            DebugOverlay.WriteLeft($"Cell: {hoveredCell.X}:{hoveredCell.Y}");
        DebugOverlay.WriteLeft($"Sprite: {_editorPanel.Sprite.Width}:{_editorPanel.Sprite.Height}");
        DebugOverlay.WriteLeft($"Pixels: {_editorPanel.Sprite.Width * _editorPanel.Sprite.Height}");
        DebugOverlay.WriteLeft($"Pos: {_editorPanel.CanvasPosition.X}:{_editorPanel.CanvasPosition.Y}");
        DebugOverlay.WriteLeft($"Tool: {_toolboxPanel.SelectedTool.Name}");
    }

    public override void Update(float deltaTime)
    {
        if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SUPER) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL))
        {
            if (Raylib.IsKeyReleased(KeyboardKey.KEY_Y))
            {
                PerformUndo();
            }
            else if (Raylib.IsKeyReleased(KeyboardKey.KEY_Z))
            {
                PerformRedo();
            }
        }

        if (Raylib.IsKeyReleased(KeyboardKey.KEY_F4))
        {
            Sprite.Export(_editorPanel.Sprite, "/Users/rasix/Projects/Spritery/sprite.png");
        }

        _toolboxPanel.Bounds = _leftBounds = Rectangles.FromLtrb(0, 0, 140, Raylib.GetScreenHeight());
        _toolboxPanel.Update(deltaTime);

        _editorPanel.Bounds = _editorBounds = Rectangles.FromLtrb(_leftBounds.Right(), 0, _rightBounds.Left(), Raylib.GetScreenHeight());
        _editorPanel.Update(deltaTime);

        _rightBounds = Rectangles.FromLtrb(Raylib.GetScreenWidth() - 200, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

        _colorPalettePanel.Bounds = new Rectangle(_rightBounds.Left(), _rightBounds.Top(), _rightBounds.width, _rightBounds.height / 3 * 2);
        _colorPalettePanel.Update(deltaTime);
        _importExportPanel.Bounds = Rectangles.FromLtrb(_rightBounds.Left(), _colorPalettePanel.Bounds.Bottom(), _rightBounds.Right(), _rightBounds.Bottom());
        _importExportPanel.Update(deltaTime);

        _editorMeta.Color = _colorPalettePanel.SelectedColor;
        _editorMeta.HoveredCell = _editorPanel.HoveredCell;

        if (_editorPanel.HoveredCell != EditorPanel.Invalid)
        {
            _toolboxPanel.SelectedTool.Update(deltaTime, _editorMeta);
        }

        if (Raylib.IsKeyReleased(KeyboardKey.KEY_F3)) _debugOverlayVisible = !_debugOverlayVisible;
    }

    private void PerformRedo()
    {
        if (!_undoList.HasRedoableActions) return;

        var action = _undoList.Redo();
        var span = Diagnose.Measure(() => PerformUndoRedoAction(action, true));
        Console.WriteLine($"Redo took {span.Milliseconds}ms");
    }

    private void PerformUndoRedoAction(RevertableAction action, bool redo)
    {
        if (action is PaintAction paintAction)
        {
            for (var i = 0; i < paintAction.Pixels.Length; i++)
            {
                var pixel = paintAction.Pixels[i];
                var color = paintAction.FromColors[i];
                if (redo) color = paintAction.ToColor;

                _editorPanel.Sprite.SetPixel((int) pixel.X, (int) pixel.Y, color);
            }
        }
    }

    private void PerformUndo()
    {
        if (!_undoList.HasUndoableActions) return;

        var action = _undoList.Undo();
        var span = Diagnose.Measure(() => PerformUndoRedoAction(action, false));
        Console.WriteLine($"Undo took {span.Milliseconds}ms");
    }
}