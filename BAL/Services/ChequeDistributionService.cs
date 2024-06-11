using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DAL.Repositories;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services
{
    public class ChequeDistributionService : IChequeDistributionService
    {

        private readonly CTSDBContext _context;
        private readonly IChequeDistributionRepository _IChequeDistributionRepository;

        public ChequeDistributionService(CTSDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<UserListDTO>> UserList()
        {
            var userList = await (from user in _context.UserLists
                                  join chequeEntry in _context.ChequeEntries on user.TreasurieCode equals chequeEntry.TreasurieCode
                                  join chequeInvoiceDetails in _context.ChequeInvoiceDetails on chequeEntry.Id equals chequeInvoiceDetails.ChequeEntryId
                                  join chequeReceived in _context.ChequeReceiveds on chequeInvoiceDetails.ChequeInvoiceId equals chequeReceived.InvoiceId
                                  select new UserListDTO
                                  {
                                      UserId = user.UserId,
                                      TreasurieCode = user.TreasurieCode,
                                      UserName = user.UserName,
                                      // add more fields as needed
                                  }).ToListAsync();

            return userList;
        }

        public async Task<bool> SaveChequeDistribution(ChequeDistributionDTO chequeDistributionDTO)
        {
            string chequeDistributionDetails = JSONHelper.ObjectToJson(chequeDistributionDTO);
            return await _IChequeDistributionRepository.Insert(chequeDistributionDetails);
        }

        public Task<IEnumerable<ChequeDistributionDTO>> saveChequeDistribution()
        {
            throw new NotImplementedException();
        }

        internal class ApplicationDbContext
        {
        }
    }
}