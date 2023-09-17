using System.Text;

namespace Spritery.Platform.MacOS;

public class DisplayDialogScript : AppleScript
{
    public string Message { get; set; }
    public string Title { get; set; }
    public string DefaultAnswer { get; set; }
    public bool HideAnswer { get; set; }
    public string Button1 { get; set; }
    public string Button2 { get; set; }
    public string Button3 { get; set; }
    public string Icon { get; set; }

    public override string GenerateScript()
    {
        var sb = new StringBuilder();
        sb.Append("display dialog");
        if (!string.IsNullOrEmpty(Message))
        {
            sb.Append($" \"{Message}\"");
        }
        if (!string.IsNullOrEmpty(Title))
        {
            sb.Append($" with title \"{Title}\"");
        }
        if (!string.IsNullOrEmpty(DefaultAnswer))
        {
            sb.Append($" default answer \"{DefaultAnswer}\"");
        }
        if (HideAnswer)
        {
            sb.Append(" hidden answer true");
        }
        if (!string.IsNullOrEmpty(Button1) || !string.IsNullOrEmpty(Button2) || !string.IsNullOrEmpty(Button3))
        {
            sb.Append(" buttons {");
            if (!string.IsNullOrEmpty(Button1))
            {
                sb.Append($"\"{Button1}\"");
            }
            if (!string.IsNullOrEmpty(Button2))
            {
                sb.Append($",\"{Button2}\"");
            }
            if (!string.IsNullOrEmpty(Button3))
            {
                sb.Append($",\"{Button3}\"");
            }
            sb.Append("}");
        }
        if (!string.IsNullOrEmpty(Icon))
        {
            sb.Append($" with icon {Icon}");
        }

        return sb.ToString();
    }
}