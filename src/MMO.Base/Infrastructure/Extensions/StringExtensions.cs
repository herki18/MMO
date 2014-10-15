namespace MMO.Base.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string TrimDoubleQuotes(this string input)
        {
            return input.StartsWith("\"") && input.EndsWith("\"") && input.Length > 1 ? input.Substring(1, input.Length - 2) : input;
        }
    }
}
