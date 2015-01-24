namespace Core
{
    public static class SizeExtensions
    {
        public static int KB(this int kb)
        {
            return kb * 1024;
        }
    }
}