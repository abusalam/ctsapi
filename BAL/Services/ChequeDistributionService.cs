using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL;
using CTS_BE.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services
{
    public class ChequeDistributionService : IChequeDistributionService
    {

        private readonly CTSDBContext _context;

        public ChequeDistributionService(CTSDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<UserListDTO>> UserList()
        {
            var userList = await (from user in _context.UserLists
                                  join chequeEntry in _context.ChequeEntries on user.TreasurieCode equals chequeEntry.TreasurieCode
                                  join chequeInvoiceDetails in _context.ChequeInvoiceDetails on chequeEntry.Id equals chequeInvoiceDetails.ChequeEntryId
                                  join chequeReceived in _context.ChequeReceiveds on chequeInvoiceDetails.ChequeInvoiceId equals chequeReceived.ChequeInvoiceDetailsId
                                  select new UserListDTO
                                  {
                                      UserId = user.UserId,
                                      TreasurieCode = user.TreasurieCode,
                                      UserName = user.UserName,
                                      // add more fields as needed
                                  }).ToListAsync();

            return userList;
        }

    }

    internal class ApplicationDbContext
    {
    }
}