namespace CycleEngine.Core
{
    public static class ScriptContext
    {
        public static string CurrentFile { get; set; } = "";
        public static int CurrentLineNumber { get; set; } = 0;
        public static string CurrentLineText { get; set; } = "";
        
        public static void Update(string file, int line, string text)
        {
            CurrentFile = file;
            CurrentLineNumber = line;
            CurrentLineText = text;
        }
    }
}