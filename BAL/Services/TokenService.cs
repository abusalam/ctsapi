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
        public async Task<int> InsterNewToken(TokenDTO tokenDTO,long userId)
        {
            int tokenNo = await _TokenRepository.GenarateToken(tokenDTO.BillId,userId,tokenDTO.Remarks, DateOnly.ParseExact(tokenDTO.PhysicalBillDate, "dd/MM/yyyy"));
            if (tokenNo!=null)
            {
                return tokenNo;
            }
            return 0;
        }
        public async Task<IEnumerable<TokenList>> AllTokens(string treasuryCode)
        {
            var filters = new List<(string Field, string Value, string Operator)>
            {
                ("TokenNumber", "2", "Equal"),
                ("DdoCode", "BAAAEE001", "Equal"),
                ("ReferenceNo", "2024110001862", "Equal")
            };
            IEnumerable<TokenList> tokenLists = await  _TokenRepository.GetSelectedColumnByConditionAsync(entity=>entity.TreasuryCode == treasuryCode ,entity => new TokenList
            {
                TokenId = entity.Id,
                TokenNumber = entity.TokenNumber,
                DdoCode = entity.DdoCode,
                CurrentStatus = entity.TokenFlow.Status.Name,
                CurrentStatusSlug = entity.TokenFlow.Status.Slug,
                FinancialYear = entity.FinancialYear,
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
        public async Task<DynamicListResult<IEnumerable<TokenList>>> Tokens(string treasuryCode, List<int> tokenStatus, List<FilterParameter> filters = null)
        {
            IEnumerable<TokenList> tokenLists = await _TokenRepository.GetSelectedColumnByConditionAsync(entity => entity.TreasuryCode == treasuryCode && tokenStatus.Contains(entity.TokenFlow.StatusId), entity => new TokenList
            {
                TokenId = entity.Id,
                TokenNumber = entity.TokenNumber,
                DdoCode = entity.DdoCode,
                FinancialYear = entity.FinancialYear,
                ReferenceNo = entity.ReferenceNo,
                CurrentStatus = entity.TokenFlow.Status.Name,
                CurrentStatusSlug = entity.TokenFlow.Status.Slug,
                TokenDate = entity.TokenDate
            }, filters);
            DynamicListResult<IEnumerable<TokenList>> resu = new DynamicListResult<IEnumerable<TokenList>>
            {
                ListHeaders = new List<ListHeader>
                {
                    new ListHeader
                    {
                        Name="Token No",
                        DataType="numeric",
                        FieldName ="TokenNumber",
                        IsFilterable=true,
                        IsSortable=false,
                    },
                    new ListHeader
                    {
                        Name="DDO Code",
                        DataType="text",
                        FieldName ="DdoCode",
                        IsFilterable=true,
                        IsSortable=false,
                    },
                    new ListHeader
                    {
                        Name="Financial Year",
                        DataType="date",
                        FieldName ="FinancialYear",
                        IsFilterable=true,
                        IsSortable=false,
                    },
                    new ListHeader
                    {
                        Name="Reference No",
                        DataType="numeric",
                        FieldName ="ReferenceNo",
                        IsFilterable=true,
                        IsSortable=false,
                    },
                    new ListHeader
                    {
                        Name="Status",
                        DataType="text",
                        FieldName ="CurrentStatus",
                        IsFilterable=true,
                        IsSortable=false,
                    },
                    new ListHeader
                    {
                        Name="Token Date",
                        DataType="date",
                        FieldName ="TokenDate",
                        IsFilterable=true,
                        IsSortable=false,
                    },

                },
                Data = tokenLists
            };
            return resu;
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