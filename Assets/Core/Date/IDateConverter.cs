using System;

namespace Core.Date
{
    public interface IDateConverter
    {
        public long DateTimeToUnixTimeStamp(DateTime unixTimeStamp);
        public DateTime UnixTimeStampToDateTime(long unixTimeStamp);
    }
}