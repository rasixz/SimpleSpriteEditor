namespace Spritery.Scenes;

public abstract class Scene
{
    public virtual void Load()
    {
    }

    public virtual void Unload()
    {
    }

    public virtual void ScreenSizeChanged(int newWidth, int newHeight)
    {
        Console.WriteLine($"W{newWidth} H{newHeight}");
    }


    public abstract void Draw();
    public abstract void Update(float deltaTime);
}