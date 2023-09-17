using System.Text;

namespace Spritery.Platform.MacOS;

public class ChooseFileNameScript : AppleScript
{
    public string Prompt { get; set; }
    public string FileName { get; set; }

    public override string GenerateScript()
    {
        var sb = new StringBuilder();
        sb.Append("POSIX path of (choose file name");
        if (!string.IsNullOrEmpty(Prompt))
        {
            sb.Append($" with prompt \"{Prompt}\"");
        }

        if (!string.IsNullOrEmpty(FileName))
        {
            sb.Append($" default name \"{FileName}\"");
        }
        sb.Append(")");

        return sb.ToString();
    }
}