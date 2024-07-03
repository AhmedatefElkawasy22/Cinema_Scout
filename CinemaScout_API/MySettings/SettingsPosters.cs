namespace CinemaScout_API.MySettings
{
    public static class SettingsPosters
    {
        public static readonly List<string> AllowExtensions = new List<string>
        {
            ".jpg",
            ".png"
        };
        public static readonly long MaxAllowPosterSize = 1048576; // 1 MB
    }
}
