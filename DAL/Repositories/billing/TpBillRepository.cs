using AutoMapper;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.billing;
using CTS_BE.DTOs;
using Dapper;
using Npgsql;

namespace CTS_BE.DAL.Repositories.billing
{
    public class TpBillRepository : Repository<BillDetail, CTSDBContext>, ITpBillRepository
    {
        protected readonly CTSDBContext _cTSDBContext;
        protected readonly IMapper _mapper;
        protected readonly IConfiguration _config;
        public TpBillRepository(CTSDBContext context, IMapper mapper, IConfiguration config) : base(context)
        {
            _cTSDBContext = context;
            _mapper = mapper;
            _config = config;
        }
        public async Task<IEnumerable<BillsListDTO>> AllNewBills(string treasuryCode)
        {
            using (var connection = new NpgsqlConnection(_config.GetConnectionString("DBConnection")))
            {
                string sql = $@"SELECT bill.bill_id AS ""BillId"",
                                        bill.reference_no AS ""ReferenceNo"",
                                        bill.ddo_code AS ""DdoCode"",
                                        mddo.designation AS ""DdoDesignation"",
                                        bill.bill_no AS ""BillNo"",
                                        bill.bill_date AS ""BillDate""
                                FROM billing.""bill_details"" bill
                                JOIN master.ddo mddo ON bill.ddo_code = mddo.code
                                LEFT JOIN cts.token tkn ON tkn.reference_no = bill.reference_no
                                WHERE bill.status = 3 AND tkn.reference_no IS NULL AND  bill.treasury_code = '{treasuryCode}'";
                IEnumerable<BillsListDTO> results = connection.Query<BillsListDTO>(sql).ToList();
                return results;
            }
        }
        public async Task<int> NewBillCount(string treasuryCode)
        {
            using (var connection = new NpgsqlConnection(_config.GetConnectionString("DBConnection")))
            {
                string sql = $@"SELECT tp.bill_id
                            FROM billing.""bill_details"" tp
                            LEFT JOIN cts.token tkn ON tkn.reference_no = tp.reference_no
                            WHERE tp.status = 3 AND tkn.reference_no IS NULL AND  tp.treasury_code = '{treasuryCode}'";
                int results = connection.Query<BillsListDTO>(sql).Count();
                return results;
            }
        }
    }
}