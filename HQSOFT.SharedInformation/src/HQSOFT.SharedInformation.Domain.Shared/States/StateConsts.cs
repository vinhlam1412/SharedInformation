namespace HQSOFT.SharedInformation.States
{
    public static class StateConsts
    {
        private const string DefaultSorting = "{0}CountryId asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "State." : string.Empty);
        }

    }
}