namespace CTS_BE.Helper
{
    public static class OwnTypeManager
    {
        private static Dictionary<string, int> _ownType;
        static OwnTypeManager()
        {
            _ownType = new Dictionary<string, int>
            {
                { "cleark",1},
                { "dealling-assistant",2},
                { "accountant",3},
                { "treasury-officer",4},
            };
        }
        public static int GetOwnType(string key)
        {
            if (_ownType.ContainsKey(key))
            {
                return _ownType[key];
            }
            else
            {
                return 0;
            }
        }
    }
}
