namespace Spritery.Platform.MacOS;

public class MacPlatformHandler : PlatformHandler
{
    public override string ChooseColor(string prompt = "")
    {
        var chooseColorScript = new ChooseColorScript
        {
            Prompt = prompt
        };
        return AppleScriptExecutor.ExecuteScript(chooseColorScript);
    }

    public override string ChooseFile(string prompt = "", List<string>? fileTypes = null, bool showInvisible = false, bool allowMultipleSelections = false)
    {
        var chooseFileScript = new ChooseFileScript
        {
            Prompt = prompt,
            FileTypes = fileTypes ?? new List<string>(),
            ShowInvisible = showInvisible,
            AllowMultipleSelections = allowMultipleSelections
        };
        return AppleScriptExecutor.ExecuteScript(chooseFileScript);
    }

    public override string ChooseFileName(string prompt = "", string fileName = "")
    {
        var chooseFileNameScript = new ChooseFileNameScript
        {
            Prompt = prompt,
            FileName = fileName
        };
        return AppleScriptExecutor.ExecuteScript(chooseFileNameScript);
    }

    public override string ChooseFolder(string prompt = "")
    {
        var chooseFolderScript = new ChooseFolderScript
        {
            Prompt = prompt
        };
        return AppleScriptExecutor.ExecuteScript(chooseFolderScript);
    }

    public override string ChooseFromList(string prompt = "", List<string>? items = null, string defaultItem = "", bool allowMultipleSelections = false)
    {
        var chooseFromListScript = new ChooseFromListScript
        {
            Prompt = prompt,
            Items = items ?? new List<string>(),
            DefaultItem = defaultItem,
            AllowMultipleSelections = allowMultipleSelections
        };
        return AppleScriptExecutor.ExecuteScript(chooseFromListScript);
    }

    public override void DisplayAlert(string title = "", string message = "", List<string>? buttons = null)
    {
        var displayAlertScript = new DisplayAlertScript
        {
            Title = title,
            Message = message,
            Buttons = buttons ?? new List<string>()
        };
        AppleScriptExecutor.ExecuteScript(displayAlertScript);
    }

    public override string DisplayDialog(string title = "", string message = "", string defaultAnswer = "", bool hideAnswer = false, string button1 = "", string button2 = "", string button3 = "", string icon = "")
    {
        var displayDialogScript = new DisplayDialogScript
        {
            Title = title,
            Message = message,
            DefaultAnswer = defaultAnswer,
            HideAnswer = hideAnswer,
            Button1 = button1,
            Button2 = button2,
            Button3 = button3,
            Icon = icon
        };
        return AppleScriptExecutor.ExecuteScript(displayDialogScript);
    }

    public override void DisplayNotification(string title = "", string subtitle = "", string message = "", string soundName = "")
    {
        var displayNotificationScript = new DisplayNotificationScript
        {
            Title = title,
            Subtitle = subtitle,
            Message = message,
            SoundName = soundName
        };
        AppleScriptExecutor.ExecuteScript(displayNotificationScript);
    }
}