namespace PlaywrightCSharp
{
    public class PlaywrightBrowserSettings
    {
        public string BaseUrl { get; set; }
        public string Browser { get; set; }
        public bool Headless { get; set; }
        public string LogFolderPath { get; set; }

        public override string ToString()
        {
            return $"Browser Settings: Browser: {Browser}, HeadlessMode: {Headless}, DefaultUrl: {BaseUrl}";
        }
    }
}
