using Newtonsoft.Json;

namespace CTS_BE.Helper
{
    public static class StatusManager
    {
        private static Dictionary<string, List<int>> _billCheckingStatus;
        private static Dictionary<string, List<int>> _returnMemoStatus;
        private static Dictionary<string, Dictionary<string, List<int>>> _tokenStatusMap;
        static StatusManager()
        {
            InitBillChekingStatus();
            InitReturnMemoStatus();
            InitTokenStatusMap();
        }
        public static List<int> GetStatus(string key,int statusType)
        {
            var s= JsonConvert.SerializeObject(_billCheckingStatus);
            switch (statusType)
            {
                case (int)Enum.StatusType.BillChecking:
                    return _billCheckingStatus.TryGetValue(key, out var billChecking) ? billChecking : new List<int>();
                case (int)Enum.StatusType.ReturnMemo:
                    return _returnMemoStatus.TryGetValue(key, out var returnMemoStatus) ? returnMemoStatus : new List<int>();
                default:
                    return new List<int>();
            }
        }
        public static List<int> GetStatus(string role,string listType)
        {
            return _tokenStatusMap[role][listType];
        }
        private static void InitBillChekingStatus()
        {
            _billCheckingStatus = new Dictionary<string, List<int>>
            {
                { "dealling-assistant",
                    new List<int>
                    {
                        (int)Enum.TokenStatus.BillReceived
                    }
                },
                { "accountant",
                    new List<int>
                    {
                        (int)Enum.TokenStatus.BillReceived,
                        (int) Enum.TokenStatus.FrowardbyDealingAssistant,
                        (int)Enum.TokenStatus.ObjectedbyDealingAssistant
                    }
                },
                { "treasury-officer",
                    new List<int>
                    {
                        (int)Enum.TokenStatus.BillReceived,
                        (int)Enum.TokenStatus.FrowardbyDealingAssistant,
                        (int)Enum.TokenStatus.ObjectedbyDealingAssistant,
                        (int)Enum.TokenStatus.FrowardbyAccountant,
                        (int)Enum.TokenStatus.ObjectedbyAccountant
                    }
                },
            };
        }
        private static void InitReturnMemoStatus()
        {
            _returnMemoStatus = new Dictionary<string, List<int>>
            {
                { "dealling-assistant",
                    new List<int>
                    {
                        (int)Enum.TokenStatus.ObjectedbyTreasuryOfficer,
                        //(int)Enum.TokenStatus.RetrunMemoGenerated
                    }
                },
            };
        }
        private static void InitTokenStatusMap()
        {
            _tokenStatusMap = new Dictionary<string, Dictionary<string, List<int>>>
            {
                {
                    "dealling-assistant",
                    new Dictionary<string, List<int>>
                    {
                        {
                            "bill-cheking",
                            new List<int>
                            {
                                (int)Enum.TokenStatus.BillReceived
                            }
                        },
                        {
                            "awating-return-memo",
                            new List<int>
                            {
                                (int)Enum.TokenStatus.ObjectedbyTreasuryOfficer,
                            }
                        },
                        {
                            "generated-return-memo",
                            new List<int>
                            {
                                (int)Enum.TokenStatus.RetrunMemoGenerated,
                            }
                        }
                    }
                },

                {
                    "accountant",
                    new Dictionary<string, List<int>>
                    {
                        {
                            "bill-cheking",
                            new List<int>
                            {
                                (int)Enum.TokenStatus.BillReceived,
                                (int) Enum.TokenStatus.FrowardbyDealingAssistant,
                                (int)Enum.TokenStatus.ObjectedbyDealingAssistant
                            }
                        }
                    }
                },

                {
                    "treasury-officer",
                    new Dictionary<string, List<int>>
                    {
                        {
                            "bill-cheking",
                            new List<int>
                            {
                                (int)Enum.TokenStatus.BillReceived,
                                (int)Enum.TokenStatus.FrowardbyDealingAssistant,
                                (int)Enum.TokenStatus.ObjectedbyDealingAssistant,
                                (int)Enum.TokenStatus.FrowardbyAccountant,
                                (int)Enum.TokenStatus.ObjectedbyAccountant
                            }
                        }
                    }
                }
            };
        }
    }
}
