using System.Collections.Generic;

namespace WHUTRunner.Models;

public class ScriptItem
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Type { get; set; } = "Python"; // Python, JavaScript, etc.
    public string Path { get; set; } = "";
    public Dictionary<string, string> Parameters { get; set; } = new();
    public bool IsEnabled { get; set; } = true;
} 