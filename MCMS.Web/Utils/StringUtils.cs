using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace MCMS.Web.Utils
{
    public static class StringUtils
    {
        /// <summary>
        /// Converts a string to a URL-friendly slug.
        /// </summary>
        /// <param name="text">The text to convert to a slug</param>
        /// <returns>A URL-friendly slug</returns>
        public static string GenerateSlug(string text)
        {
            // Return empty string if null
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            // Convert to lowercase
            string slug = text.ToLowerInvariant();

            // Remove accents and special characters
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            // Replace spaces with hyphens
            slug = Regex.Replace(slug, @"\s+", "-");

            // Remove multiple hyphens
            slug = Regex.Replace(slug, @"-+", "-");

            // Trim hyphens from start and end
            slug = slug.Trim('-');

            return slug;
        }
    }
}
