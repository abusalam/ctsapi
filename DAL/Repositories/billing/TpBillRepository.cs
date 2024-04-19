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
                string sql = $@"SELECT tp.bill_id AS ""BillId"",
                                    tp.reference_no AS ""ReferenceNo"",
	                                tp.ddo_code AS ""DdoCode"",
	                                tp.designation AS ""DdoDesignation"",
	                                tp.bill_no AS ""BillNo"",
	                                tp.bill_date AS ""BillDate"",
	                                tp.gross_amount AS ""GrossAmount"",
	                                tp.net_amount AS ""NetAmount"",
                                    tp.demand AS ""Demand"", 
                                    tp.major_head AS ""MajorHead"", 
                                    tp.sub_major_head AS ""SubMajorHead"", 
                                    tp.minor_head AS ""MinorHead"", 
                                    tp.scheme_head AS ""SchemeHead"", 
                                    tp.voted_charged AS ""VotedCharged"", 
                                    tp.detail_head AS ""DetailHead""
                            FROM billing.""TP_Bill"" tp
                            LEFT JOIN cts.token tkn ON tkn.reference_no = tp.reference_no
                            WHERE tp.status = 3 AND tkn.reference_no IS NULL AND  tp.treasury_code = '{treasuryCode}'";

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