namespace HQSOFT.SharedInformation.Districts
{
    public static class DistrictConsts
    {
        private const string DefaultSorting = "{0}ProvinceId asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "District." : string.Empty);
        }

    }
}