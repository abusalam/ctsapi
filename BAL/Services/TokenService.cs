using AutoMapper;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;
using System.Collections.Generic;

namespace CTS_BE.BAL.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _TokenRepository;
        private readonly IMapper _mapper;
        public TokenService(ITokenRepository TokenRepository, IMapper mapper)
        {
            _TokenRepository = TokenRepository;
            _mapper = mapper;
        }
        public async Task<TokenDetailsDto> TokenDeatisById(long tokenId)
        {
            TokenDetailsDto tokenDetailsDto = (TokenDetailsDto) await _TokenRepository.GetSingleSelectedColumnByConditionAsync(entity=>entity.Id==tokenId,
                entity=> new TokenDetailsDto
                {
                    TokenId = entity.Id,
                    TokenNumber = entity.TokenNumber,
                    ReferenceNo = entity.ReferenceNo,
                    BillId = entity.BillId,
                    TokenDate = entity.TokenDate,
                    Status =  entity.TokenFlow.Status.Name,
                    StatusId = entity.TokenFlow.Status.Id
                });
            return tokenDetailsDto;
        }
        public async Task<GeneratedTokenDTO> InsterNewToken(TokenDTO tokenDTO,long userId)
        {
            return await _TokenRepository.GenarateToken(tokenDTO.BillId,userId,tokenDTO.Remarks, DateOnly.ParseExact(tokenDTO.PhysicalBillDate, "dd/MM/yyyy"));
            
        }
        public async Task<IEnumerable<TokenList>> AllTokens(string treasuryCode)
        {
            IEnumerable<TokenList> tokenLists = await  _TokenRepository.GetSelectedColumnByConditionAsync(entity=>entity.TreasuryCode == treasuryCode ,entity => new TokenList
            {
                TokenId = entity.Id,
                TokenNumberr = entity.TokenNumber,
                DdoCode = entity.DdoCode,
                CurrentStatus = entity.TokenFlow.Status.Name,
                CurrentStatusSlug = entity.TokenFlow.Status.Slug,
                FinancialYear = entity.FinancialYearId.ToString(),
                ReferenceNo =  entity.ReferenceNo,
                TokenDate = entity.TokenDate
            });
            return tokenLists;
        }
        //public async Task<IEnumerable<TokenList>> Tokens(string treasuryCode, List<int> tokenStatus, List<FilterParameter> filters=null)
        //{
        //    IEnumerable<TokenList> tokenLists = await _TokenRepository.GetSelectedColumnByConditionAsync(entity => entity.TreasuryCode == treasuryCode, entity => new TokenList
        //    {
        //        TokenId = entity.Id,
        //        TokenNumber = entity.TokenNumber,
        //        DdoCode = entity.DdoCode,
        //        FinancialYear = entity.FinancialYear,
        //        ReferenceNo = entity.ReferenceNo,
        //        CurrentStatus = entity.TokenFlow.Status.Name,
        //        CurrentStatusSlug = entity.TokenFlow.Status.Slug,
        //        TokenDate = entity.TokenDate
        //    },filters);
        //    return tokenLists;
        //}
        public async Task<DynamicListResult<IEnumerable<TokenList>>> Tokens(string treasuryCode, List<int> tokenStatus, List<FilterParameter> filters = null,int pageIndex=0,int pageSize=10,SortParameter sortParameters=null)
        {
            IEnumerable<TokenList> tokenLists = await _TokenRepository.GetSelectedColumnByConditionAsync(entity => entity.TreasuryCode == treasuryCode && tokenStatus.Contains(entity.TokenFlow.StatusId), entity => new TokenList
            {
                TokenId = entity.Id,
                TokenNumberr = entity.TokenNumber,
                DdoCode = entity.DdoCode,
                FinancialYear = entity.FinancialYearId.ToString(),
                ReferenceNo = entity.ReferenceNo,
                CurrentStatus = entity.TokenFlow.Status.Name,
                CurrentStatusId = entity.TokenFlow.Status.Id,
                TokenDate = entity.TokenDate
            },pageIndex,pageSize,filters,(sortParameters!=null)?sortParameters.Field:null,(sortParameters!=null)?sortParameters.Order:null);
            DynamicListResult<IEnumerable<TokenList>> resu = new DynamicListResult<IEnumerable<TokenList>>
            {
                Headers = new List<ListHeader>
                {
                    new ListHeader
                    {
                        Name="Token No",
                        DataType="text",
                        FieldName ="tokenNumberr",
                        FilterField ="TokenNumber",
                        ObjectTypeValueField="currentStatusId",
                        IsFilterable=true,
                        IsSortable=false,
                    },
                    new ListHeader
                    {
                        Name="Token Date",
                        DataType="date",
                        FieldName ="tokenDate",
                        FilterField ="TokenDate",
                        IsFilterable=true,
                        IsSortable=false,
                    },
                    new ListHeader
                    {
                        Name="DDO Code",
                        DataType="text",
                        FieldName ="ddoCode",
                        FilterField ="DdoCode",
                        IsFilterable=true,
                        IsSortable=false,
                    },
                    new ListHeader
                    {
                        Name="Financial Year",
                        DataType="date",
                        FieldName ="financialYear",
                        FilterField ="FinancialYear",
                        IsFilterable=true,
                        IsSortable=false,
                    },
                    new ListHeader
                    {
                        Name="Reference No",
                        DataType="numeric",
                        FieldName ="referenceNo",
                        FilterField ="ReferenceNo",
                        IsFilterable=true,
                        IsSortable=false,
                    },
                    new ListHeader
                    {
                        Name="Status",
                        DataType="object",
                        ObjectTypeValueField="currentStatusId",
                        FieldName ="currentStatus",
                        FilterField ="TokenFlow.Status.Id",
                        FilterEnums = new List<FilterEnum>
                        {
                            new FilterEnum
                            {
                                Value = (int) Enum.TokenStatus.BillReceived,
                                Label = "Bill Received",
                                StyleClass = "primary"
                            },
                            new FilterEnum
                            {
                                Value = (int) Enum.TokenStatus.ObjectedbyDealingAssistant,
                                Label = "Objectedby Dealing Assistant",
                                StyleClass = "warning"
                            },
                            new FilterEnum
                            {
                                Value = (int) Enum.TokenStatus.FrowardbyDealingAssistant,
                                Label = "Frowardby Dealing Assistant",
                                StyleClass = "success"
                            },
                            new FilterEnum
                            {
                                Value = (int) Enum.TokenStatus.ObjectedbyAccountant,
                                Label = "Objected by Accountant",
                                StyleClass = "warning"
                            },
                            new FilterEnum
                            {
                                Value = (int) Enum.TokenStatus.FrowardbyAccountant,
                                Label = "Froward by Accountant",
                                StyleClass = "success"
                            },
                            new FilterEnum
                            {
                                Value = (int) Enum.TokenStatus.ObjectedbyTreasuryOfficer,
                                Label = "Objected by TreasuryOfficer",
                                StyleClass = "warning"
                            },
                            new FilterEnum
                            {
                                Value = (int) Enum.TokenStatus.FrowardbyTreasuryOfficer,
                                Label = "Froward by TreasuryOfficer",
                                StyleClass = "success"
                            },
                            new FilterEnum
                            {
                                Value = (int) Enum.TokenStatus.RetrunMemoGenerated,
                                Label = "Retrun Memo Generated",
                                StyleClass = "danger"
                            },

                        },
                        IsFilterable=true,
                        IsSortable=false,
                    }

                },
                Data = tokenLists
            };
            return resu;
        }
        public async Task<TokenPrintDTO> PrintByTokenId(long tokenId)
        {
            TokenPrintDTO tokenPrintDTO = await _TokenRepository.GetSingleSelectedColumnByConditionAsync(entity => entity.Id == tokenId, entity => new TokenPrintDTO
            {
                TokenNumber = entity.TokenNumber,
                TokenDate = entity.TokenDate,
                BillNo = entity.Bill.BillNo,
                BillDate = entity.Bill.BillDate,
                DdoCode = entity.DdoCode,
                GrossAmount = entity.Bill.GrossAmount,
                NetAmount = entity.Bill.NetAmount,
                PayeeDept = entity.Bill.DemandNavigation.Name,
                HOAChain = new HOAChain
                {
                    Demand = entity.Bill.Demand,
                    MajorHead = entity.Bill.MajorHead,
                    SubMajorHead = entity.Bill.SubMajorHead,
                    MinorHead = entity.Bill.MinorHead,
                    SchemeHead = entity.Bill.SchemeHead,
                    VotedCharged = entity.Bill.VotedCharged,
                    DetailHead = entity.Bill.DetailHead,
                }
            });
            return tokenPrintDTO;
        }
        public async Task<int> AllTokensCount()
        {
            return _TokenRepository.Count();
        }
        public async Task<int> TokenCountByStatus(string treasuryCode, List<int> tokenStatus)
        {
            return _TokenRepository.CountWithCondition(entity => entity.TreasuryCode == treasuryCode && tokenStatus.Contains(entity.TokenFlow.StatusId));
        }
        public async Task<int> TokenCountByStatus(string treasuryCode, List<int> tokenStatus, List<FilterParameter> dynamicFilters = null)
        {
            return _TokenRepository.CountWithCondition(entity => entity.TreasuryCode == treasuryCode && tokenStatus.Contains(entity.TokenFlow.StatusId),dynamicFilters);
        }
        public async Task<ReturnMemoBillDetailsDTO> ReturnMemoBillDetails(long tokenId)
        {
            ReturnMemoBillDetailsDTO returnMemoBillDetailsDTO =(ReturnMemoBillDetailsDTO) await _TokenRepository.GetSingleSelectedColumnByConditionAsync(entity => entity.Id == tokenId, entity => new ReturnMemoBillDetailsDTO
            {
                TokenId = entity.Id,
                TokenNumber = entity.TokenNumber,
                TokenDate = entity.TokenDate,
                BillNo = entity.Bill.BillNo,
                BillDate = entity.Bill.BillDate,
                DdoCode = entity.Bill.DdoCode,
                GrossAmount = entity.Bill.GrossAmount,
                HOAChain = new HOAChain
                {
                    Demand = entity.Bill.Demand,
                    MajorHead = entity.Bill.MajorHead,
                    SubMajorHead = entity.Bill.SubMajorHead,
                    MinorHead = entity.Bill.MinorHead,
                    SchemeHead = entity.Bill.SchemeHead,
                    VotedCharged = entity.Bill.VotedCharged,
                    DetailHead = entity.Bill.DetailHead,
                },
                NetAmount = entity.Bill.NetAmount
            });
            return returnMemoBillDetailsDTO;
        }
        public async Task<bool> GenerateReturnMemo(long tokenId, string referenceNo, long userId, int ownType)
        {
            return await _TokenRepository.GenerateReturnMemo(tokenId,referenceNo,userId,ownType);
        }
    }
}