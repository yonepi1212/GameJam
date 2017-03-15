using System;

/// <summary>
/// Enum 型の拡張メソッドを管理するクラス
/// </summary>
public static class EnumExtension
{
    /// <summary>
    /// Enumに別名を付与するカスタム属性.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class Alias : Attribute
    {
        /// <summary>
        /// 別名文字列.
        /// </summary>
        private string label;
        public string Label { get { return label; } }

        /// <summary>
        /// Enumに別名を設定します. 設定した文字列はGetAliasで取得可能.
        /// </summary>
        /// <param name="label">別名.</param>
        public Alias(string label)
        {
            this.label = label;
        }
    }
    /// <summary>
    /// EnumAlias属性で指定された別名を取得する.
    /// </summary>
    /// <returns>別名.</returns>
    public static string GetAlias(this Enum value)
    {
        Type enumType = value.GetType();
        string name = Enum.GetName(enumType, value);
        Alias[] attrs = (Alias[])enumType.GetField(name).GetCustomAttributes(typeof(Alias), false);
        return attrs[0].Label;
    }
    /// <summary>
    /// 現在のインスタンスで 1 つ以上のビット フィールドが設定されているかどうかを判断します
    /// </summary>
    public static bool HasFlag(this Enum self, Enum flag)
    {
        if (self.GetType() != flag.GetType())
        {
            throw new ArgumentException("flag の型が、現在のインスタンスの型と異なっています。");
        }

        var selfValue = Convert.ToUInt64(self);
        var flagValue = Convert.ToUInt64(flag);

        return (selfValue & flagValue) == flagValue;
    }
}
