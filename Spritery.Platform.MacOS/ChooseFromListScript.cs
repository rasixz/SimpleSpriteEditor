using System.Text;

namespace Spritery.Platform.MacOS;

public class ChooseFromListScript : AppleScript
{
    public string Prompt { get; set; }
    public List<string> Items { get; set; }
    public string DefaultItem { get; set; }
    public bool AllowMultipleSelections { get; set; }

    public override string GenerateScript()
    {
        var sb = new StringBuilder();
        sb.AppendLine("choose from list {");
        if (Items != null)
        {
            foreach (string item in Items)
            {
                sb.AppendLine($"\"{item}\",");
            }
        }
        sb.AppendLine("}");

        if (!string.IsNullOrEmpty(Prompt))
        {
            sb.AppendLine($"with prompt \"{Prompt}\"");
        }

        if (!string.IsNullOrEmpty(DefaultItem))
        {
            sb.AppendLine($"default items {{\"{DefaultItem}\"}}");
        }

        if (AllowMultipleSelections)
        {
            sb.AppendLine("multiple selections allowed true");
        }

        return sb.ToString();
    }
}