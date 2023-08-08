namespace HQSOFT.SharedInformation.ReasonCodes
{
    public static class ReasonCodeConsts
    {
        private const string DefaultSorting = "{0}Code asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "ReasonCode." : string.Empty);
        }

    }
}