namespace HQSOFT.SharedInformation.Provinces
{
    public static class ProvinceConsts
    {
        private const string DefaultSorting = "{0}Idx asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Province." : string.Empty);
        }

    }
}