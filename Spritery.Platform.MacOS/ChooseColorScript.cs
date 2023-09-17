using System.Text;

namespace Spritery.Platform.MacOS;

public class ChooseColorScript : AppleScript
{
    public string Prompt { get; set; }

    public override string GenerateScript()
    {
        var sb = new StringBuilder();
        sb.Append("choose color");
        if (!string.IsNullOrEmpty(Prompt))
        {
            sb.Append($" with prompt \"{Prompt}\"");
        }

        return sb.ToString();
    }
}