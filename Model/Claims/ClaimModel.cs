namespace CTS_BE.Model.Claims
{
    public class ClaimModel
    {
        public class Role
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public List<string> Permissions { get; set; } = null!;
        }

        public class Level
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public List<string> Scope { get; set; } = null!;
        }

        public class Application
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public List<Level> Levels { get; set; } = null!;
            public List<Role> Roles { get; set; } = null!;
        }
    }

    public class SsoAccess
    {
        public string? ROLE { get; set; }
        public string? LEVEL { get; set; }
        public string? AC { get; set; }
        public string? USERNAME { get; set; }
    }
}
