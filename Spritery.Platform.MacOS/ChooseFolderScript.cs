using System.Text;

namespace Spritery.Platform.MacOS;

public class ChooseFolderScript : AppleScript
{
    public string Prompt { get; set; }

    public override string GenerateScript()
    {
        var sb = new StringBuilder();
        sb.Append("POSIX path of (choose folder");
        if (!string.IsNullOrEmpty(Prompt))
        {
            sb.Append($" with prompt \"{Prompt}\"");
        }
        sb.Append(")");

        return sb.ToString();
    }
}