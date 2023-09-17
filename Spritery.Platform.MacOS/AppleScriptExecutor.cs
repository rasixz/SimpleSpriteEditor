using System.Diagnostics;

namespace Spritery.Platform.MacOS;

public static class AppleScriptExecutor
{
    public static string ExecuteScript(AppleScript script)
    {
        var scriptPath = CreateTempScriptFile(script);
        var output = ExecuteScriptFile(scriptPath);
        DeleteScriptFile(scriptPath);
        return output;
    }

    private static string CreateTempScriptFile(AppleScript script)
    {
        var tempDir = Path.GetTempPath();
        var tempFile = Path.Combine(tempDir, "temp_script.scpt");
        File.WriteAllText(tempFile, script.GenerateScript());
        return tempFile;
    }

    private static string ExecuteScriptFile(string scriptPath)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/usr/bin/osascript",
                Arguments = scriptPath,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        var output = process.StandardOutput.ReadToEnd().Trim();
        process.WaitForExit();

        return output;
    }
    
    private static void DeleteScriptFile(string scriptPath)
    {
        File.Delete(scriptPath);
    }
}