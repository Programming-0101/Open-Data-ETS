using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace EdmontonTransit.Common
{
    public static class ComponentModelDescriptionExtensions
    {
        #region Extension Methods
        public static string ToDescription(this Enum self)
        {
            FieldInfo fi = self.GetType().GetField(self.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return self.ToString();
        }

        public static string ToDescription(this Enum self, string placeholderValue)
        {
            return self.ToDescription().Replace("{0}", placeholderValue);
        }
        #endregion
    }
    // TODO: Uncomment the code below and finish whatever approach is best to make an enumerable for a drop-down list
    //public abstract class ConstrainedEnumParser<TClass> where TClass : class
    //    // value type constraint S ("TEnum") depends on reference type T ("TClass") [and on struct]
    //{
    //    // internal constructor, to prevent this class from being inherited outside this code
    //    internal ConstrainedEnumParser() { }
    //    // Parse using pragmatic/adhoc hard cast:
    //    //  - struct + class = enum
    //    //  - 'guaranteed' call from derived <System.Enum>-constrained type EnumUtils
    //    public static TEnum Parse<TEnum>(string value, bool ignoreCase = false) where TEnum : struct, TClass
    //    {
    //        return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
    //    }
    //    public static bool TryParse<TEnum>(string value, out TEnum result, bool ignoreCase = false, TEnum defaultValue = default(TEnum)) where TEnum : struct, TClass // value type constraint S depending on T
    //    {
    //        var didParse = Enum.TryParse(value, ignoreCase, out result);
    //        if (didParse == false)
    //        {
    //            result = defaultValue;
    //        }
    //        return didParse;
    //    }
    //    public static TEnum ParseOrDefault<TEnum>(string value, bool ignoreCase = false, TEnum defaultValue = default(TEnum)) where TEnum : struct, TClass // value type constraint S depending on T
    //    {
    //        if (string.IsNullOrEmpty(value)) { return defaultValue; }
    //        TEnum result;
    //        if (Enum.TryParse(value, ignoreCase, out result)) { return result; }
    //        return defaultValue;
    //    }

    //    #region Utility Methods
    //    /// <summary>
    //    /// Converts enumerations to a list
    //    /// </summary>
    //    /// <typeparam name="TEnum"></typeparam>
    //    /// <returns></returns>
    //    public static IEnumerable<KeyValuePair<TEnum, string>> EnumToList<TEnum>() where TEnum : struct, TClass // value type constraint S depending on T
    //    {
    //        Type enumType = typeof(TEnum);

    //        //// Can't use generic type constraints on value types,
    //        //// so have to do check like this
    //        //if (enumType.BaseType != typeof(Enum))
    //        //    throw new ArgumentException("T must be of type System.Enum");

    //        List<KeyValuePair<TEnum, string>> enumValList = new List<KeyValuePair<TEnum, string>>();

    //        foreach (int val in Enum.GetValues(enumType))
    //        {
    //            string text = Enum.ToObject(enumType, val);
    //            enumValList.Add(new KeyValuePair<TEnum, string>(val,text));
    //        }

    //        return enumValList;
    //    }
    //    #endregion
    //}

    //public class EnumUtils : ConstrainedEnumParser<System.Enum>
    //// reference type constraint to any <System.Enum>
    //{
    //    // call to parse will then contain constraint to specific <System.Enum>-class

    //}
}

/*
 * Credits: Portions copied or adapted from
 * - http://blog.spontaneouspublicity.com/associating-strings-with-enums-in-c
 * - http://stackoverflow.com/a/16736914/2154662
 */
