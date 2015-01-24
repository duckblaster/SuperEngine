using System;

namespace SuperEngineLib.Misc {
    public static partial class Extensions {
        public static UnixTime ToUnixTime(this DateTime dateTime) {
            return UnixTime.FromDateTime(dateTime);
        }
    }

    public struct UnixTime {
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public long UnixTimeStamp;

        public UnixTime(string unixTime) {
            UnixTimeStamp = Convert.ToInt64(unixTime);
        }

        public UnixTime(long unixTime) {
            UnixTimeStamp = unixTime;
        }

        public UnixTime(uint unixTime) {
            UnixTimeStamp = unixTime;
        }

        public UnixTime(double unixTime) {
            UnixTimeStamp = (long)unixTime;
        }

        public UnixTime(int unixTime) {
            UnixTimeStamp = unixTime;
        }

        public UnixTime(ulong unixTime) {
            UnixTimeStamp = (long)unixTime;
        }

        public UnixTime(DateTime dateTime) {
            TimeSpan timeSpan = dateTime - UnixEpoch;
            //return timeSpan.Ticks / TimeSpan.TicksPerSecond;
            UnixTimeStamp = Convert.ToInt64(timeSpan.TotalSeconds);
        }

        public static UnixTime FromDateTime(DateTime dateTime) {
            return new UnixTime(dateTime);
        }

        public static DateTime ToDateTime(string unixTime) {
            return ToDateTime(Convert.ToInt64(unixTime));
        }

        public static DateTime ToDateTime(UnixTime unixTime) {
            return ToDateTime(unixTime.UnixTimeStamp);
        }

        public static DateTime ToDateTime(long unixTime) {
            //return UnixEpoch.AddTicks(unixTime * TimeSpan.TicksPerSecond);
            return UnixEpoch.AddSeconds(Convert.ToDouble(unixTime));
        }

        public static DateTime ToDateTime(int unixTime) {
            return ToDateTime((long)unixTime);
        }

        public static DateTime ToDateTime(double unixTime) {
            return ToDateTime((long)unixTime);
        }

        public static DateTime ToDateTime(uint unixTime) {
            return ToDateTime((long)unixTime);
        }

        public static DateTime ToDateTime(ulong unixTime) {
            return ToDateTime((long)unixTime);
        }

        public static implicit operator long(UnixTime unixTime) {
            return unixTime.UnixTimeStamp;
        }

        public static implicit operator UnixTime(long dateTime) {
            return new UnixTime(dateTime);
        }

        public static implicit operator DateTime(UnixTime unixTime) {
            return ToDateTime(unixTime);
        }

        public static implicit operator UnixTime(DateTime dateTime) {
            return FromDateTime(dateTime);
        }
    }
}

