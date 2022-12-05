namespace Kompanion.Api.Infrastructure.Enums
{
    public enum EntityStatus
    {
        Active = 1,
        Pending = 2,
        Inactive = 3
    }

    public enum DbProcedureType
    {
        Read,
        List,
        Create,
        Update
    }

    public enum QueryType
    {
       Read,
       List,
       Insert,
       Update
    }

    public enum UserType
    {
        Admin = 1,
        User = 2
    }
}