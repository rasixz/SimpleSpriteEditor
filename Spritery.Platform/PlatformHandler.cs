namespace Spritery.Platform;

public abstract class PlatformHandler
{
    public abstract string ChooseColor(string prompt = "");
    public abstract string ChooseFile(string prompt = "", List<string>? fileTypes = null, bool showInvisible = false, bool allowMultipleSelections = false);
    public abstract string ChooseFileName(string prompt = "", string fileName = "");
    public abstract string ChooseFolder(string prompt = "");
    public abstract string ChooseFromList(string prompt = "", List<string>? items = null, string defaultItem = "", bool allowMultipleSelections = false);
    public abstract void DisplayAlert(string title = "", string message = "", List<string>? buttons = null);
    public abstract string DisplayDialog(string title = "", string message = "", string defaultAnswer = "", bool hideAnswer = false, string button1 = "", string button2 = "", string button3 = "", string icon = "");
    public abstract void DisplayNotification(string title = "", string subtitle = "", string message = "", string soundName = "");
}