﻿using System;

namespace Infrastructure.Service.Date
{
    public class DateConverter : IDateConverter
    {
        public long DateTimeToUnixTimeStamp(DateTime unixTimeStamp)
        {
            return ((DateTimeOffset)unixTimeStamp).ToUnixTimeSeconds(); 
        }
        
        public DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).UtcDateTime;
        }
    }
}