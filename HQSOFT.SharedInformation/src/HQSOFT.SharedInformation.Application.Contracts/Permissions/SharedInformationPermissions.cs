using Volo.Abp.Reflection;

namespace HQSOFT.SharedInformation.Permissions;

public class SharedInformationPermissions
{
    public const string GroupName = "SharedInformation";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(SharedInformationPermissions));
    }

    public static class Countries
    {
        public const string Default = GroupName + ".Countries";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class States
    {
        public const string Default = GroupName + ".States";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class Provinces
    {
        public const string Default = GroupName + ".Provinces";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class Districts
    {
        public const string Default = GroupName + ".Districts";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class Wards
    {
        public const string Default = GroupName + ".Wards";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class ReasonCodes
    {
        public const string Default = GroupName + ".ReasonCodes";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
    public static class Companies
    {
        public const string Default = GroupName + ".Companies";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
}