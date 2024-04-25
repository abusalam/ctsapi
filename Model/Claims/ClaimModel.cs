namespace CTS_BE.Model.Claims
{
    public class ClaimModel
    {
        public class Role
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<string> Permissions { get; set; }
        }
        public class Level
        {
            public int? Id { get; set; }
            public string? Name { get; set; }
            public List<string> Scope { get; set; }
        }
        public class Application
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<Level?> Levels { get; set; }
            public List<Role> Roles { get; set; }
        }
    }
}
