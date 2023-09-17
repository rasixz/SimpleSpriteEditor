using System.Text;

namespace Spritery.Platform.MacOS;

public class DisplayNotificationScript : AppleScript
{
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Message { get; set; }
    public string SoundName { get; set; }

    public override string GenerateScript()
    {
        var sb = new StringBuilder();
        sb.Append("display notification");
        if (!string.IsNullOrEmpty(Title))
        {
            sb.Append($" with title \"{Title}\"");
        }
        if (!string.IsNullOrEmpty(Subtitle))
        {
            sb.Append($" subtitle \"{Subtitle}\"");
        }
        if (!string.IsNullOrEmpty(Message))
        {
            sb.Append($" message \"{Message}\"");
        }
        if (!string.IsNullOrEmpty(SoundName))
        {
            sb.Append($" sound name \"{SoundName}\"");
        }

        return sb.ToString();
    }
}