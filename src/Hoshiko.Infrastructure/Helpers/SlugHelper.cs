using System.Text.RegularExpressions;

namespace Hoshiko.Infrastructure.Helpers
{
    public static class SlugHelper
    {
        public static string GenerateSlug(string text)
        {
            text = text.ToLowerInvariant();
            text = Regex.Replace(text, @"\s+", "-");
            text = Regex.Replace(text, @"[^a-z0-9\-آ-ی]", "");
            return text.Trim('-');
        }
    }
}