using System.Text;

namespace Spritery.Platform.MacOS;

public class DisplayAlertScript : AppleScript
{
    public string Message { get; set; }
    public string Title { get; set; }
    public List<string> Buttons { get; set; }

    public override string GenerateScript()
    {
        var sb = new StringBuilder();
        sb.Append("display alert");
        if (!string.IsNullOrEmpty(Title))
        {
            sb.Append($" \"{Title}\"");
        }
        if (!string.IsNullOrEmpty(Message))
        {
            sb.Append($" message \"{Message}\"");
        }
        if (Buttons != null && Buttons.Count > 0)
        {
            sb.Append($" buttons {{");
            sb.Append(string.Join(",", Buttons.Select(button => $"\"{button}\"")));
            sb.Append("}");
        }

        return sb.ToString();
    }
}