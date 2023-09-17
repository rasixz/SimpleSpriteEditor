namespace Spritery.Editor.Tools;

public abstract class Tool
{
    public abstract string Name { get; }

    public virtual void Draw()
    {
    }

    public virtual void Update(float deltaTime, EditorMeta editorMeta)
    {
    }
}