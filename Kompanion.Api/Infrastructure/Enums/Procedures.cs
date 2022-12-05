namespace Kompanion.Api.Infrastructure.Enums{
    public static class UserProcedures
    {
        public const string Read = "ReadUser";
        public const string List = "ListUsers";
        public const string Create = "CreateUser";
        public const string Update = "UpdateUser";
        public const string FindByUsername = "FindUserByUsername";
    }

    public static class MoveProcedures
    {
        public const string Read = "ReadMove";
        public const string List = "ListMoves";
        public const string Create = "CreateMove";
        public const string Update = "UpdateMove";
    }

    public static class MoveSetProcedures
    {
        public const string Read = "ReadMoveSet";
        public const string List = "ListMoves";
        public const string Create = "CreateMoveSet";
        public const string Update = "UpdateMoveSet";
    }
}