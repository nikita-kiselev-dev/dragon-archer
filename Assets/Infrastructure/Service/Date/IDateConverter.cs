using System;

namespace Infrastructure.Service.Date
{
    public interface IDateConverter
    {
        public long DateTimeToUnixTimeStamp(DateTime unixTimeStamp);
        public DateTime UnixTimeStampToDateTime(long unixTimeStamp);
    }
}