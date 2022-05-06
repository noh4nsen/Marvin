using System;

namespace Atm.Clientes.Api.Helpers
{
    public static class DateHelper
    {
        public static DateTime GetLocalTime()
        {
            return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo"));
        }
    }
}
