using AutoMapper;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
namespace CTS_BE.BAL
{
    public class TokenFlowService : ITokenFlowService
    {
        private readonly ITokenFlowRepository _TokenFlowRepository;
        private readonly IMapper _mapper;
        public TokenFlowService(ITokenFlowRepository TokenFlowRepository, IMapper mapper) {
            _TokenFlowRepository = TokenFlowRepository;
            _mapper = mapper;
        }
    }
}