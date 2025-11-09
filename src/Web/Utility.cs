#if SQLiteVersion

namespace Web
{
    public static class Utility
    {
        public static string GetLastChars(Guid token)
        {
            return token.ToString().Substring(
                                    token.ToString().Length - 3);
        }
    }
}
#else
namespace CleanContosoUniversity.Web
{
    public static class Utility
    {
        public static string GetLastChars(byte[] token)
        {
            return token[7].ToString();
        }
    }
}
#endif