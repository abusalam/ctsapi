using AutoMapper;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
namespace CTS_BE.BAL
{
    public class DdoService : IDdoService
    {
        private readonly IDdoRepository _DdoRepository;
        private readonly IMapper _mapper;
        public DdoService(IDdoRepository DdoRepository, IMapper mapper) {
            _DdoRepository = DdoRepository;
            _mapper = mapper;
        }
    }
}