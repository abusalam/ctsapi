using AutoMapper;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;
using CTS_BE.Helper;

namespace CTS_BE.BAL
{
    public class TokenHasObjectionService : ITokenHasObjectionService
    {
        private readonly ITokenHasObjectionsRepository _TokenHasObjectionRepository;
        private readonly IMapper _mapper;
        public TokenHasObjectionService(ITokenHasObjectionsRepository TokenHasObjectionRepository, IMapper mapper) {
            _TokenHasObjectionRepository = TokenHasObjectionRepository;
            _mapper = mapper;
        }
        public async Task<bool> Insert(BillCheckingDto billCheckingDto,long userId,int ownType)
        {
            string objections = JSONHelper.ObjectToJson(billCheckingDto.BillObjections);
            string overruledObjections = JSONHelper.ObjectToJson(billCheckingDto.OverruledObjections);
            return await _TokenHasObjectionRepository.BillCheck(billCheckingDto.TokenId, billCheckingDto.ReferenceNo, objections,overruledObjections, userId, ownType);
        }
        public async Task<List<TokenWithObjectionDto>> ObjectionByTokenId(long tokenId)
        {
            List<TokenWithObjectionDto> tokenObjectiopns= (List<TokenWithObjectionDto>) await _TokenHasObjectionRepository.GetSelectedColumnByConditionAsync(entity => entity.TokenId == tokenId, entity => new TokenWithObjectionDto
            {
                Id = entity.Id,
                ToeknId = entity.TokenId,
                ObjectionDescription = (entity.GobalObjectionId != null) ? entity.GobalObjection.Description : entity.LocalObjection,
                ObjectionId = entity.GobalObjectionId,
                ObjectionType = (entity.GobalObjectionId != null) ? "Global" : "Local",
                ObjectionBy = entity.ObjectedBy,
                ObjectionRemark = entity.ObjectionRemark,
                IsOverruled = (entity.OverruledBy !=null)? true:false,
                OverruledBy = entity.OverruledBy.ToString(),
            });
            return tokenObjectiopns;
        }
    }
}