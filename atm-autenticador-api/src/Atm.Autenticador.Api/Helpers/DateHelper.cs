using System;

namespace Atm.Autenticador.Api.Helpers
{
    public static class DateHelper
    {
        public static DateTime GetLocalTime()
        {
            return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo"));
        }
    }
}
