using Microsoft.CSharp.RuntimeBinder;
using Raylib_cs;
using Spritery.Platform;
using Spritery.Platform.MacOS;
using Spritery.Scenes;
using Spritery.Utils;

namespace Spritery;

public sealed class Spritery
{
    private int _screenWidth, _screenHeight;

    private readonly Dictionary<string, Scene> _scenes = new();

    private Scene _activeScene;

    private static PlatformHandler _platformHandler;

    public static PlatformHandler PlatformHandler
    {
        get
        {
            if (_platformHandler != null) return _platformHandler;

            if (OperatingSystem.IsMacOS())
            {
                _platformHandler = new MacPlatformHandler();
            }
            else
            {
                throw new RuntimeBinderException($"OS not supported.");
            }

            return _platformHandler;
        }
    }

    public Spritery()
    {
        _screenWidth = 1280;
        _screenHeight = 720;
    }

    private void CreateWindow()
    {
        Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_RESIZABLE);
        Raylib.InitWindow(_screenWidth, _screenHeight, $"Spritery - v0.1");

        Raylib.SetExitKey(0);
    }

    private void Initialize()
    {
        _scenes["editor"] = new EditorScene();

        LoadScene("editor");
    }

    private void LoadScene(string scene)
    {
        var nextScene = _scenes[scene];
        _activeScene?.Unload();
        _activeScene = nextScene;
        _activeScene!.Load();
    }

    public void Start()
    {
        CreateWindow();
        Initialize();

        Raylib.SetTargetFPS(60);

        // Main game loop
        while (!Raylib.WindowShouldClose()) // Detect window close button
        {
            // Update
            var oldW = _screenWidth;
            var oldH = _screenHeight;
            _screenWidth = Raylib.GetScreenWidth();
            _screenHeight = Raylib.GetScreenHeight();

            if (oldW != _screenWidth || oldH != _screenHeight)
                _activeScene?.ScreenSizeChanged(_screenWidth, _screenHeight);

            var deltaTime = Raylib.GetFrameTime();
            _activeScene?.Update(deltaTime);

            // Draw
            Raylib.BeginDrawing();
            {
                Raylib.ClearBackground(Colors.WhiteSmoke);
                _activeScene?.Draw();
            }
            Raylib.EndDrawing();
        }

        // De-Initialization
        Raylib.CloseWindow(); // Close window and OpenGL context
    }
}