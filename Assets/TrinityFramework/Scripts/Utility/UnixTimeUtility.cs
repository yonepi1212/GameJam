using System;

/*  UnixTime
    ミリ秒でUnixTimeを扱うクラス
*/
public static class UnixTimeUtility
{
    #region Declaration
    private static readonly DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    #endregion

    #region Public method
    // ミリ秒用
    public static class MilliSecond
    {
        // 現在時刻からUnixTimeを計算する.
        public static long Now()
        {
            return (FromDateTime(DateTime.UtcNow));
        }

        // UnixTimeからDateTimeに変換.
        public static DateTime FromUnixTime(long unixTime_)
        {
            return unixEpoch.AddSeconds(unixTime_ / 1000).ToLocalTime();
        }

        // 指定時間をUnixTimeに変換する.
        public static long FromDateTime(DateTime dateTime_)
        {
            double nowTicks = (dateTime_.ToUniversalTime() - unixEpoch).TotalSeconds;
            var unixTimeMilliSecond = (long)nowTicks * 1000 + dateTime_.Millisecond;
            return unixTimeMilliSecond;
        }
    }
    // 秒用
    public static class Second
    {
        // 現在時刻からUnixTimeを計算する.
        public static long Now()
        {
            return (FromDateTime(DateTime.UtcNow));
        }

        // UnixTimeからDateTimeに変換.
        public static DateTime FromUnixTime(long unixTime_)
        {
            return unixEpoch.AddSeconds(unixTime_).ToLocalTime();
        }

        // 指定時間をUnixTimeに変換する.
        public static long FromDateTime(DateTime dateTime_)
        {
            double nowTicks = (dateTime_.ToUniversalTime() - unixEpoch).TotalSeconds;
            return (long)nowTicks;
        }
    }
    #endregion
}
