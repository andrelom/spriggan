namespace Spriggan.Data.Identity.Contracts.Values;

public static class Roles
{
    public static class Administrator
    {
        public static readonly Guid Id = Guid.Parse("8F4B8637-548B-4752-B700-6FA4C511411C");

        public const string Name = "Administrator";
    }

    public static class User
    {
        public static readonly Guid Id = Guid.Parse("F1BFA7D1-2ABD-4F52-B64A-72232CE8BC30");

        public const string Name = "User";
    }
}
