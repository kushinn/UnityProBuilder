namespace UnityProBuilder
{
    public static class Utilities
    {
        public static string ToFistLower(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            return string.Concat(char.ToLower(str[0]).ToString(), str.Substring(1));
        }
    }
}
