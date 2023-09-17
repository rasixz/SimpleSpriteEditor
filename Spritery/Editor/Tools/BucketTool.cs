using System.Diagnostics;
using System.Drawing;
using Raylib_cs;
using Spritery.Revertable;
using Spritery.Utils;
using Color = Raylib_cs.Color;

namespace Spritery.Editor.Tools;

/// <summary>
/// This tool uses the flood-fill algorithm to fill the contents. This removes recursion and makes it fast as fuck
/// </summary>
public class BucketTool : Tool
{
    public override string Name => "Bucket";

    public override void Update(float deltaTime, EditorMeta editorMeta)
    {
        if (!Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT)) return;

        var color = editorMeta.Color;
        var hoveredColor = editorMeta.Sprite.GetPixel(editorMeta.HoveredCell);

        if (color.Equals(hoveredColor)) return;
        
        // calculate cells to paint
        var (elapsed, result) = Diagnose.Measure(() => FloodFill(editorMeta, editorMeta.HoveredCell, hoveredColor, color));

        Console.WriteLine($"Filling took {elapsed.Milliseconds}ms");

        var points = result.Keys.ToArray();
        var colors = result.Values.ToArray();

        editorMeta.UndoList.Do(new PaintAction(points, colors, color));
    }

    private Dictionary<Point, Color> FloodFill(EditorMeta editorMeta, Point startPoint, Color targetColor, Color replacementColor)
    {
        var width = editorMeta.Sprite.Width;
        var height = editorMeta.Sprite.Height;

        // Create a queue to store the points to be processed
        var queue = new Queue<Point>();

        // Add the starting point to the queue
        queue.Enqueue(startPoint);

        // Create a dictionary to store the original point with its original color
        var originalColors = new Dictionary<Point, Color>();

        // Get the target color of the starting point
        var startColor = editorMeta.Sprite.GetPixel(startPoint.X, startPoint.Y);

        // Process the points in the queue
        while (queue.Count > 0)
        {
            // Dequeue a point from the queue
            var currentPoint = queue.Dequeue();

            // Check if the current point is within the canvas boundaries
            if (currentPoint.X < 0 || currentPoint.X >= width || currentPoint.Y < 0 || currentPoint.Y >= height) continue;
            // Check if the current point has not been visited and has the target color
            if (originalColors.ContainsKey(currentPoint) || !editorMeta.Sprite.GetPixel(currentPoint.X, currentPoint.Y).Equals(startColor)) continue;
            
            // Store the original color of the current point
            originalColors[currentPoint] = startColor;

            // Set the color of the current point to the replacement color
            editorMeta.Sprite.SetPixel(currentPoint.X, currentPoint.Y, replacementColor);

            // Enqueue the adjacent points
            queue.Enqueue(currentPoint with {X = currentPoint.X - 1}); // Left
            queue.Enqueue(currentPoint with {X = currentPoint.X + 1}); // Right
            queue.Enqueue(currentPoint with {Y = currentPoint.Y - 1}); // Up
            queue.Enqueue(currentPoint with {Y = currentPoint.Y + 1}); // Down
        }

        return originalColors;
    }
}