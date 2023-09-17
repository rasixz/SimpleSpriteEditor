using System.Text;

namespace Spritery.Platform.MacOS;

public class ChooseFileScript : AppleScript
{
    public string Prompt { get; set; }
    public List<string> FileTypes { get; set; }
    public bool ShowInvisible { get; set; }
    public bool AllowMultipleSelections { get; set; }

    public override string GenerateScript()
    {
        var sb = new StringBuilder();
        sb.Append("POSIX path of (choose file");
        if (!string.IsNullOrEmpty(Prompt))
        {
            sb.Append($" with prompt \"{Prompt}\"");
        }

        if (FileTypes is {Count: > 0})
        {
            sb.Append($" of type {{{string.Join(",", FileTypes.Select(type => $"\"{type}\""))}}}");
        }

        if (ShowInvisible)
        {
            sb.Append(" invisibles true");
        }

        if (AllowMultipleSelections)
        {
            sb.Append(" multiple selections allowed true");
        }

        sb.Append(')');

        return sb.ToString();
    }
}