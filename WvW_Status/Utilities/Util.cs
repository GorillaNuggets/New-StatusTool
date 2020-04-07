namespace WvW_Status.Utilities
{
    public class Util
    {
        public static T GetPropertyValue<T>(object obj, string propName)
        {
            return (T)obj.GetType().GetProperty(propName)?.GetValue(obj, null);
        }
    }
}