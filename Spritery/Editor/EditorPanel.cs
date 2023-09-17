using System.Drawing;
using System.Numerics;
using Raylib_cs;
using Spritery.UI;
using Spritery.Utils;
using Color = Raylib_cs.Color;
using Rectangle = Raylib_cs.Rectangle;

namespace Spritery.Editor;

public class EditorPanel : Panel
{
    private static readonly List<CellHoverType> HoverTypes = new()
    {
        new FullOutlineHoverType(),
        new FillHoverHoverType(),
    };

    private readonly Button _centerCanvasButton;
    private readonly Button _toggleGridButton;
    private readonly Button _increaseZoomButton, _decreaseZoomButton;

    private readonly Panel _buttonPanel, _canvasPanel;

    public bool GridVisible { get; set; } = true;

    private const int SelectedCellHoverType = 1;
    public static CellHoverType CellHoverType => HoverTypes[SelectedCellHoverType];

    public float CanvasZoom { get; private set; } = 20;
    public float CanvasZoomFactor { get; set; } = 0.75f;
    public Sprite Sprite { get; set; }

    private Vector2 _canvasPosition;
    private Vector2 _editorCenter;
    private Vector2 _canvasSize;

    private float _minCanvasZoom = 3, _maxCanvasZoom = 90;

    public static readonly Point Invalid = new(-1);

    private Vector2 _cellSize;
    private Point _hoveredCell = Invalid;

    private bool _initialized;

    public Point HoveredCell => _hoveredCell;

    public Vector2 CanvasPosition => _canvasPosition - _editorCenter;

    public EditorPanel()
    {
        Title = "Editor";
        Padding = 0;

        _buttonPanel = new Panel();
        _canvasPanel = new Panel();
        _centerCanvasButton = new Button("Center");
        _centerCanvasButton.ButtonClickedEvent += (_, _) => CenterCanvas();
        _toggleGridButton = new Button("Grid");
        _toggleGridButton.ButtonClickedEvent += (_, _) => GridVisible = !GridVisible;
        _increaseZoomButton = new Button("Zoom in");
        _increaseZoomButton.ButtonClickedEvent += (_, _) => ZoomIn(+CanvasZoomFactor);
        _decreaseZoomButton = new Button("Zoom out");
        _decreaseZoomButton.ButtonClickedEvent += (_, _) => ZoomOut(CanvasZoomFactor);
    }

    private void DrawCanvas()
    {
        var left = _canvasPosition.X - _canvasSize.Y / 2;
        var top = _canvasPosition.Y - _canvasSize.Y / 2;

        var spriteWidth = Sprite.Width;
        var spriteHeight = Sprite.Height;
        var cellSizeX = _cellSize.X;
        var cellSizeY = _cellSize.Y;

        var startX = Math.Max(0, (int) Math.Floor((_canvasPanel.ContentBounds.x - left) / cellSizeX));
        var endX = Math.Min(spriteWidth - 1, (int) Math.Ceiling((_canvasPanel.ContentBounds.Right() - left) / cellSizeX));

        var startY = Math.Max(0, (int) Math.Floor((Bounds.y - top) / cellSizeY));
        var endY = Math.Min(spriteHeight - 1, (int) Math.Ceiling((_canvasPanel.ContentBounds.Bottom() - top) / cellSizeY));

        var bounds = new Rectangle(0, 0, cellSizeX, cellSizeY);

        for (var x = startX; x <= endX; x++)
        {
            for (var y = startY; y <= endY; y++)
            {
                var color = Sprite.GetPixel(x, y);
                // Skip not visible colors for drawing

                var cellX = left + x * cellSizeX;
                var cellY = top + y * cellSizeY;

                bounds.x = cellX;
                bounds.y = cellY;
                if (color.Equals(Colors.Invisible))
                {
                    Raylib.DrawRectangleRec(bounds, (x + y) % 2 == 0 ? Colors.White : Colors.LightGray);
                    continue;
                }

                Raylib.DrawRectangleRec(bounds, color);
            }
        }
    }

    private void DrawOverlay()
    {
        var left = _canvasPosition.X - _canvasSize.Y / 2;
        var top = _canvasPosition.Y - _canvasSize.Y / 2;
        var right = _canvasPosition.X + _canvasSize.Y / 2;
        var bottom = _canvasPosition.Y + _canvasSize.Y / 2;

        DrawGrid();

        Raylib.DrawRectangleLinesEx(Rectangles.FromLtrb(left, top, right, bottom), 2f, Colors.Black);

        DrawHoveredCell();
    }

    private void DrawGrid()
    {
        if (!GridVisible) return;

        var left = _canvasPosition.X - _canvasSize.Y / 2;
        var top = _canvasPosition.Y - _canvasSize.Y / 2;
        var right = _canvasPosition.X + _canvasSize.Y / 2;
        var bottom = _canvasPosition.Y + _canvasSize.Y / 2;

        for (var x = left; x < right; x += _cellSize.X)
        {
            if (!(x > _canvasPanel.ContentBounds.Left()) || !(x < _canvasPanel.ContentBounds.Right())) continue;

            var start = new Vector2(x, MathF.Max(top, _canvasPanel.ContentBounds.Top()));
            var end = new Vector2(x, MathF.Min(bottom, _canvasPanel.ContentBounds.Bottom()));

            Raylib.DrawLineEx(start, end, 1f, Colors.Black);
        }

        for (var y = top; y < bottom; y += _cellSize.Y)
        {
            if (!(y > _canvasPanel.ContentBounds.Top()) || !(y < _canvasPanel.ContentBounds.Bottom())) continue;

            var start = new Vector2(MathF.Max(left, _canvasPanel.ContentBounds.Left()), y);
            var end = new Vector2(MathF.Min(right, _canvasPanel.ContentBounds.Right()), y);

            Raylib.DrawLineEx(start, end, 1f, Colors.Black);
        }
    }

    public override void Draw()
    {
        base.Draw();

        _buttonPanel.Draw();
        _canvasPanel.Draw();

        DrawButtons();

        Raylib.BeginScissorMode((int) _canvasPanel.ContentBounds.x, (int) _canvasPanel.ContentBounds.y, (int) _canvasPanel.ContentBounds.width, (int) _canvasPanel.ContentBounds.height);
        DrawCanvas();
        DrawOverlay();
        Raylib.EndScissorMode();
    }

    private void DrawHoveredCell()
    {
        if (_hoveredCell == Invalid) return;
        if (_hoveredCell.X < 0f || _hoveredCell.Y < 0f) return;

        var cellX = _canvasPosition.X - _canvasSize.X / 2f + _hoveredCell.X * _cellSize.X;
        var cellY = _canvasPosition.Y - _canvasSize.Y / 2f + _hoveredCell.Y * _cellSize.Y;
        var bounds = new Rectangle(cellX, cellY, _cellSize.X, _cellSize.Y);

        CellHoverType.Draw(bounds);
    }

    private void DrawButtons()
    {
        _centerCanvasButton.Draw();
        _toggleGridButton.Draw();
        _increaseZoomButton.Draw();
        _decreaseZoomButton.Draw();
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        UpdateBounds();
        UpdateButtons(deltaTime);

        _buttonPanel.Update(deltaTime);
        _canvasPanel.Update(deltaTime);

        _editorCenter.X = _canvasPanel.ContentBounds.x + _canvasPanel.ContentBounds.width / 2;
        _editorCenter.Y = _canvasPanel.ContentBounds.y + _canvasPanel.ContentBounds.height / 2;

        _canvasSize.X = Sprite.Width * CanvasZoom;
        _canvasSize.Y = Sprite.Height * CanvasZoom;

        _cellSize.X = _canvasSize.X / Sprite.Width;
        _cellSize.Y = _canvasSize.Y / Sprite.Height;

        if (!_initialized)
        {
            CenterCanvas();
            _initialized = true;
        }

        HandleCanvasZoom();
        HandleCanvasMovement();
        HandleCellHovering();
    }

    private void UpdateButtons(float deltaTime)
    {
        _centerCanvasButton.Update(deltaTime);
        _toggleGridButton.Update(deltaTime);
        _increaseZoomButton.Update(deltaTime);
        _decreaseZoomButton.Update(deltaTime);

        _toggleGridButton.Text = $"{(GridVisible ? "Hide" : "Show")} Grid";
        _decreaseZoomButton.Enabled = CanvasZoom > _minCanvasZoom;
        _increaseZoomButton.Enabled = CanvasZoom < _maxCanvasZoom;
    }

    private void UpdateBounds()
    {
        _buttonPanel.Bounds = Rectangles.FromLtrb(ContentBounds.Left(), ContentBounds.Bottom() - 30, ContentBounds.Right(), ContentBounds.Bottom());
        _canvasPanel.Bounds = Rectangles.FromLtrb(ContentBounds.Left(), ContentBounds.Top(), ContentBounds.Right(), _buttonPanel.Bounds.Top());

        _toggleGridButton.Bounds = new Rectangle(_buttonPanel.ContentBounds.Left(), _buttonPanel.ContentBounds.Top(), _buttonPanel.ContentBounds.height * 3f, _buttonPanel.ContentBounds.height);
        _centerCanvasButton.Bounds = new Rectangle(_toggleGridButton.Bounds.Right() + _buttonPanel.Padding, _buttonPanel.ContentBounds.Top(), _buttonPanel.ContentBounds.height * 3f, _buttonPanel.ContentBounds.height);
        _increaseZoomButton.Bounds = new Rectangle(_centerCanvasButton.Bounds.Right() + _buttonPanel.Padding, _buttonPanel.ContentBounds.Top(), _buttonPanel.ContentBounds.height * 3f, _buttonPanel.ContentBounds.height);
        _decreaseZoomButton.Bounds = new Rectangle(_increaseZoomButton.Bounds.Right() + _buttonPanel.Padding, _buttonPanel.ContentBounds.Top(), _buttonPanel.ContentBounds.height * 3f, _buttonPanel.ContentBounds.height);
    }

    private void HandleCellHovering()
    {
        var mousePosition = Raylib.GetMousePosition();
        if (!Raylib.CheckCollisionPointRec(mousePosition, _canvasPanel.ContentBounds))
        {
            _hoveredCell = Invalid;
            return;
        }

        var left = _canvasPosition.X - _canvasSize.Y / 2;
        var top = _canvasPosition.Y - _canvasSize.Y / 2;
        var right = _canvasPosition.X + _canvasSize.Y / 2;
        var bottom = _canvasPosition.Y + _canvasSize.Y / 2;
        var bounds = Rectangles.FromLtrb(left, top, right, bottom);

        if (Raylib.CheckCollisionPointRec(mousePosition, bounds))
        {
            var newCell = GetCellAt(mousePosition);

            if (_hoveredCell == Invalid)
            {
                _hoveredCell = newCell;
            }
            else if (newCell.X != _hoveredCell.X || newCell.Y != _hoveredCell.Y)
            {
                _hoveredCell = newCell;
            }
        }
        else
        {
            _hoveredCell = Invalid;
        }
    }

    private void HandleCanvasMovement()
    {
        var mousePosition = Raylib.GetMousePosition();
        if (!Raylib.CheckCollisionPointRec(mousePosition, _canvasPanel.ContentBounds)) return;
        
        if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
        {
            _canvasPosition.Y += 1;
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
        {
            _canvasPosition.Y -= 1;
        }

        if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
        {
            _canvasPosition.X += 1;
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
        {
            _canvasPosition.X -= 1;
        }
    }

    private void HandleCanvasZoom()
    {
        var mousePosition = Raylib.GetMousePosition();
        if (!Raylib.CheckCollisionPointRec(mousePosition, _canvasPanel.ContentBounds)) return;
            
        if (Raylib.IsKeyDown(KeyboardKey.KEY_E))
        {
            ZoomIn(CanvasZoomFactor);
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_Q))
        {
            ZoomOut(CanvasZoomFactor);
        }
    }

    private void ZoomIn(float factor)
    {
        if (CanvasZoom < _maxCanvasZoom) CanvasZoom += factor;
        else CanvasZoom = _maxCanvasZoom;
    }

    private void ZoomOut(float factor)
    {
        if (CanvasZoom > _minCanvasZoom) CanvasZoom -= factor;
        else CanvasZoom = _minCanvasZoom;
    }

    private Point GetCellAt(Vector2 position)
    {
        var left = _canvasPosition.X - _canvasSize.Y / 2;
        var top = _canvasPosition.Y - _canvasSize.Y / 2;
        var right = _canvasPosition.X + _canvasSize.Y / 2;
        var bottom = _canvasPosition.Y + _canvasSize.Y / 2;

        var cellX = Maths.Map(position.X, left, right, 0, Sprite.Width);
        var cellY = Maths.Map(position.Y, top, bottom, 0, Sprite.Height);

        return new Point((int) cellX, (int) cellY);
    }

    public void CenterCanvas()
    {
        _canvasPosition.X = _editorCenter.X;
        _canvasPosition.Y = _editorCenter.Y;
    }
}

public abstract class CellHoverType
{
    public abstract void Draw(Rectangle cellBounds);

    public virtual void Update(float deltaTime)
    {
    }
}

public sealed class FullOutlineHoverType : CellHoverType
{
    public override void Draw(Rectangle cellBounds)
    {
        Raylib.DrawRectangleLinesEx(cellBounds, 1f, Colors.Black);
    }
}

public sealed class FillHoverHoverType : CellHoverType
{
    private readonly Color _color = Colors.White.MakeTransparent(100);

    public override void Draw(Rectangle cellBounds)
    {
        Raylib.DrawRectangleRec(cellBounds, _color);
    }
}