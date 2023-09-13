namespace Par.CommandCenter.Application.Common.Claims
{
    public static class ParClaimTypes
    {
        public const string OBJECT_GUID = "http://schemas.microsoft.com/identity/claims/objectidentifier";

        public const string USER_ID = "par_userid";

        public const string TENANT_IDS = "par_cc_tenantIds";

        public const string UPN = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn";

        public const string NAME = "name";

        public const string ROLE = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        public const string PREFERRED_USERNAME = "preferred_username";
    }
}
