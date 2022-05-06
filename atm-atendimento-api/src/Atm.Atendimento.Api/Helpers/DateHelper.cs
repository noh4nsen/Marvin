using System;

namespace Atm.Atendimento.Api.Helpers
{
    public static class DateHelper
    {
        public static DateTime GetLocalTime()
        {
            return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo"));
        }
    }
}
