using Raylib_cs;
using Spritery.UI;
using Spritery.Utils;

namespace Spritery.Editor;

public sealed class ImportExportPanel : Panel
{
    private readonly Button _loadSpriteButton;
    private readonly Button _saveSpriteButton;
    private readonly Button _newSpriteButton;

    private readonly Label _widthLabel;
    private readonly NumberTextbox _widthTextbox;
    private readonly Label _heightLabel;
    private readonly NumberTextbox _heightTextbox;

    public event EventHandler<LoadSpriteEventArgs> LoadSpriteEvent;
    public event EventHandler<SaveSpriteEventArgs> SaveSpriteEvent;
    public event EventHandler<NewSpriteEventArgs> NewSpriteEvent; 

    public ImportExportPanel()
    {
        Title = "Import/Export";
        
        _widthLabel = new Label("Width");
        _widthTextbox = new NumberTextbox();
        _heightLabel = new Label("Height");
        _heightTextbox = new NumberTextbox();

        _loadSpriteButton = new Button("Load Sprite");
        _loadSpriteButton.ButtonClickedEvent += (_, _) =>
        {
            var selectedSprite = Spritery.PlatformHandler.ChooseFile(prompt: "Select sprite: ", fileTypes: new List<string>
            {
                "public.png"
            });

            var cancelled = string.IsNullOrEmpty(selectedSprite);
            if (!cancelled)
            {
                OnLoadSpriteEvent(new LoadSpriteEventArgs(selectedSprite));   
            }
        };

        _saveSpriteButton = new Button("Save Sprite");
        _saveSpriteButton.ButtonClickedEvent += (_, _) =>
        {
            var selectedSprite = Spritery.PlatformHandler.ChooseFileName(prompt: "Select file to export to: ", fileName: "unnamed.png");

            var cancelled = string.IsNullOrEmpty(selectedSprite);
            if (!cancelled)
            {
                OnSaveSpriteEvent(new SaveSpriteEventArgs(selectedSprite));
            }
        };

        _newSpriteButton = new Button("New Sprite");
        _newSpriteButton.ButtonClickedEvent += (_, _) =>
        {
            if (_widthTextbox.Value != null && _heightTextbox.Value != null)
            {
                OnNewSpriteEvent(new NewSpriteEventArgs(_widthTextbox.Value.Value, _heightTextbox.Value.Value));
            }
        };
    }

    public override void Draw()
    {
        base.Draw();
        
        _loadSpriteButton.Draw();
        _saveSpriteButton.Draw();
        _newSpriteButton.Draw();
        
        _widthLabel.Draw();
        _widthTextbox.Draw();
        _heightLabel.Draw();
        _heightTextbox.Draw();
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        
        _loadSpriteButton.Bounds = new Rectangle(ContentBounds.Left(), ContentBounds.Top(), ContentBounds.width, 20);
        _loadSpriteButton.Update(deltaTime);
        
        _saveSpriteButton.Bounds = new Rectangle(ContentBounds.Left(), _loadSpriteButton.Bounds.Bottom() + Padding, ContentBounds.width, 20);
        _saveSpriteButton.Update(deltaTime);
        
        _newSpriteButton.Bounds = new Rectangle(ContentBounds.Left(), _saveSpriteButton.Bounds.Bottom() + Padding, ContentBounds.width, 20);
        _newSpriteButton.Update(deltaTime);

        _widthLabel.Bounds = new Rectangle(ContentBounds.Left(), _newSpriteButton.Bounds.Bottom() + Padding, ContentBounds.width / 2f - Padding / 2f, 10);
        _widthLabel.Update(deltaTime);
        _widthTextbox.Bounds = new Rectangle(ContentBounds.Left(), _widthLabel.Bounds.Bottom() + Padding, _widthLabel.Bounds.width, 20);
        _widthTextbox.Update(deltaTime);

        _heightLabel.Bounds = new Rectangle(_widthTextbox.Bounds.Right() + Padding, _newSpriteButton.Bounds.Bottom() + Padding, ContentBounds.width / 2f - Padding / 2f, 10);
        _heightLabel.Update(deltaTime);
        _heightTextbox.Bounds = new Rectangle(_widthTextbox.Bounds.Right() + Padding, _heightLabel.Bounds.Bottom() + Padding, _heightLabel.Bounds.width, 20);
        _heightTextbox.Update(deltaTime);
    }

    private void OnLoadSpriteEvent(LoadSpriteEventArgs e)
    {
        LoadSpriteEvent?.Invoke(this, e);
    }

    private void OnSaveSpriteEvent(SaveSpriteEventArgs e)
    {
        SaveSpriteEvent?.Invoke(this, e);
    }

    private void OnNewSpriteEvent(NewSpriteEventArgs e)
    {
        NewSpriteEvent?.Invoke(this, e);
    }
}

public class LoadSpriteEventArgs : EventArgs
{
    public string FilePath { get; }

    public LoadSpriteEventArgs(string filePath)
    {
        FilePath = filePath;
    }
}

public class SaveSpriteEventArgs : EventArgs
{
    public string FilePath { get; }

    public SaveSpriteEventArgs(string filePath)
    {
        FilePath = filePath;
    }
}

public class NewSpriteEventArgs : EventArgs
{
    public int Width { get; }

    public int Height { get; }

    public NewSpriteEventArgs(int width, int height)
    {
        Width = width;
        Height = height;
    }
}