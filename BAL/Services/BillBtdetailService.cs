using AutoMapper;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;
namespace CTS_BE.BAL
{
    public class BillBtdetailService : IBillBtdetailService
    {
        private readonly IBillBtdetailRepository _BillBtdetailRepository;
        private readonly IMapper _mapper;
        public BillBtdetailService(IBillBtdetailRepository BillBtdetailRepository, IMapper mapper)
        {
            _BillBtdetailRepository = BillBtdetailRepository;
            _mapper = mapper;
        }
    }
}