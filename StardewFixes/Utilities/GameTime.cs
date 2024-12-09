namespace StardewFixes.Utilities
{
    public static class GameTime
    {
        public static string GetDayOfWeek(int dayOfMonth, string currentSeason)
        {
            int seasonIndex = Array.IndexOf(new[] { "spring", "summer", "fall", "winter" }, currentSeason.ToLower());
            if (seasonIndex == -1)
                throw new ArgumentException("Invalid season provided.");

            int totalDays = seasonIndex * 28 + dayOfMonth;
            string[] daysOfWeek = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            return daysOfWeek[(totalDays - 1) % 7];
        }

        public static string ConvertGameTimeToString(int time)
        {
            int hours = time / 100;
            int minutes = time % 100;
            string period = hours >= 12 ? "PM" : "AM";

            if (hours > 12) hours -= 12;
            else if (hours == 0) hours = 12;

            return $"{hours}:{minutes:D2} {period}";
        }
    }
}
