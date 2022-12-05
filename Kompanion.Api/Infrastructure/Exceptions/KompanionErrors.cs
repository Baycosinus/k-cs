using System;

namespace Kompanion.Api.Infrastructure.Exceptions
{
    public class KompanionErrors 
    {
       public static readonly int VALIDATION_ERROR = 10000;
       public static readonly Tuple<int, string> INTERNAL_ERROR = Tuple.Create(10001, "An internal error occurred");

       // ACCOUNT
         public static readonly Tuple<int, string> ACCOUNT_NOT_FOUND = Tuple.Create(20002, "Account not found");
          public static readonly Tuple<int, string> ACCOUNT_ALREADY_EXISTS = Tuple.Create(20003, "Account already exists");
          public static readonly Tuple<int, string> ACCOUNT_INVALID_PASSWORD = Tuple.Create(20004, "Invalid password");
          public static readonly Tuple<int, string> ACCOUNT_INVALID_USERNAME = Tuple.Create(20005, "Invalid username");
          public static readonly Tuple<int, string> ACCOUNT_INVALID_EMAIL = Tuple.Create(20006, "Invalid email");
          public static readonly Tuple<int, string> ACCOUNT_INVALID_FIRSTNAME = Tuple.Create(20007, "Invalid first name");
          public static readonly Tuple<int, string> ACCOUNT_INVALID_LASTNAME = Tuple.Create(20008, "Invalid last name");
          public static readonly Tuple<int, string> ACCOUNT_INVALID_BIRTHDATE = Tuple.Create(20009, "Invalid birthdate");
          public static readonly Tuple<int, string> ACCOUNT_NOT_AUTHORIZED = Tuple.Create(20010, "Account not authorized");
          public static readonly Tuple<int, string> INVALID_USERNAME_OR_PASSWORD = Tuple.Create(20011, "Invalid username or password");
          public static readonly Tuple<int, string> PASSWORDS_DO_NOT_MATCH = Tuple.Create(20012, "Passwords do not match");
      // DB
          public static readonly Tuple<int, string> DB_ERROR = Tuple.Create(30001, "An error occurred while accessing the database");
    }
}