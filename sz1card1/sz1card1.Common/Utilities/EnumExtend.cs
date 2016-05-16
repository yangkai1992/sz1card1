using System.ComponentModel;
using System.Reflection;

namespace sz1card1.Common.Utilities
{
    public static class EnumExtend
    {
        public static string GetDescription(this System.Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

            if (objs == null || objs.Length == 0)
                return string.Empty;

            return ((DescriptionAttribute)objs[0]).Description;
        }
    }
}
