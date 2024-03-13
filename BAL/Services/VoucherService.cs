using AutoMapper;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;
using CTS_BE.Helper;

namespace CTS_BE.BAL
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _VoucherRepository;
        private readonly IMapper _mapper;
        public VoucherService(IVoucherRepository VoucherRepository, IMapper mapper) {
            _VoucherRepository = VoucherRepository;
            _mapper = mapper;
        }
        public async Task<bool> InsertNewVoucher(List<CreateShrtListDTO> createShrtListDTOs,long userId)
        {
            string paymandatePayload =  JSONHelper.ObjectToJson(createShrtListDTOs);
            return await _VoucherRepository.NewVoucher(paymandatePayload,userId);
        }
    }
}