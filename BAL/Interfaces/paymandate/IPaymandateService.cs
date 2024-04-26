using CTS_BE.DTOs;
using CTS_BE.Model.e_Kuber;

namespace CTS_BE.BAL.Interfaces.paymandate
{
    public interface IPaymandateService
    {
        public Task<IEnumerable<PayMandateShortListDTO>> TokenForShortList();
        public Task<bool> NewShortList(long loggendInUserId, List<NewShortlistDTO> newShortlistDTO);
        public EKuber GetXMLData();
        public void GenerateXML(EKuber kuber, string fileName, string filePath);
    }
}
