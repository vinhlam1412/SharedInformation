namespace HQSOFT.SharedInformation.Wards
{
    public static class WardConsts
    {
        private const string DefaultSorting = "{0}DistrictId asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Ward." : string.Empty);
        }

    }
}