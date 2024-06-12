using CTS_BE.BAL.Interfaces;
using CTS_BE.BAL.Interfaces.paymandate;
using CTS_BE.Helper;
using CTS_BE.Model;
using CTS_BE.Model.e_Kuber;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using System.Text;
using System;
using System.ComponentModel.DataAnnotations;
//using System.IO.File;

namespace CTS_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        private readonly IPaymandateService _paymandateService;
        private readonly ITransactionLotService _transactionLotService;

        public TestController(IPaymandateService paymandateService, ITransactionLotService transactionLotService)
        {
            _paymandateService = paymandateService;
            _transactionLotService = transactionLotService;
        }
        [HttpGet("PushtoRBI/{fileName}")]
        public async Task<APIResponse<bool>> PushtoRBI(string fileName)
        {
            APIResponse<bool> aPIResponse = new();
            try
            {
                string filePath = "./" + fileName + ".zip";
                SFTPHelper.UploadFile("1.6.198.15", 22, "GOWB", "Gowb1234$", filePath, "EPAY/IN//" + fileName + ".temp");
                SFTPHelper.RenameFile("1.6.198.15", 22, "GOWB", "Gowb1234$", fileName + ".temp", fileName + ".zip", "EPAY/IN//");
                aPIResponse.Message = "Pushed to RBI Successfully";
                aPIResponse.result = true;
                aPIResponse.apiResponseStatus = Enum.APIResponseStatus.Success;
                return aPIResponse;
            }
            catch (Exception ex)
            {
                aPIResponse.Message = ex.Message;
                aPIResponse.result = false;
                aPIResponse.apiResponseStatus = Enum.APIResponseStatus.Error;
                return aPIResponse;
            }
        }

        [HttpGet("GeneretXML")]
        public async Task<APIResponse<string>> GenerateXMLAsync([FromQuery] bool pushFile = true)
        {
            APIResponse<string> aPIResponse = new();
            string fileName = "";
            string filePath = "";
            try
            {
                List<TransactionLotModel> pendingLots = await _transactionLotService.pendingLots();
                foreach (TransactionLotModel pendingLot in pendingLots)
                {
                    EKuber eKuberData = await _transactionLotService.GetXMLData(pendingLot.Id);
                    // fileName = eKuberData.requestPayload.AppHdr.BizMsgIdr;
                    fileName = "EPV80116001516701174202404150055";
                    _paymandateService.GenerateXML(eKuberData, fileName, fileName + ".xml");
                    bool isValid = XmlHelper.ValidateXml(fileName + ".xml", "pain.001.001.08v2.4.xsd");
                    if (isValid)
                    {
                        SignHelper.signdocument1(fileName + ".xml", "ifms.gowb.pfx", fileName, "");
                        string ZipFileName = fileName + ".zip";
                        using (var zip = ZipFile.Open(ZipFileName, ZipArchiveMode.Create))
                        {
                            zip.CreateEntryFromFile(fileName + ".xml", fileName + ".xml");
                            zip.CreateEntryFromFile(fileName + ".sig", fileName + ".sig");
                        }
                        //var tstFile = File.OpenRead(filePath);
                        if (pushFile)
                        {
                            filePath = "./" + fileName + ".zip";
                            SFTPHelper.UploadFile("1.6.198.15", 22, "GOWB", "Gowb1234$", filePath, "EPAY/IN//" + fileName + ".temp");
                            SFTPHelper.RenameFile("1.6.198.15", 22, "GOWB", "Gowb1234$", fileName + ".temp", fileName + ".zip", "EPAY/IN//");
                        }
                    }
                    break;
                }
                aPIResponse.Message = "XML Generated & Signed Successfully";
                aPIResponse.result = fileName;
                aPIResponse.apiResponseStatus = Enum.APIResponseStatus.Success;
                return aPIResponse;
            }
            catch (Exception ex)
            {
                aPIResponse.Message = ex.Message;
                aPIResponse.result = filePath;
                aPIResponse.apiResponseStatus = Enum.APIResponseStatus.Error;
                return aPIResponse;
            }
        }
        [HttpGet("INACK")]
        public async Task<APIResponse<string>> DownloadandVerifyACK([FromQuery] [Required]string fileName)
        {
            APIResponse<string> aPIResponse = new();
            try
            {
                string localFilePath = "./ACK/" + fileName + ".zip";
                string fileNameTrim = fileName.Substring(3);
                string xmlFileName = fileNameTrim + ".xml";
                string xmlFilePath = "./ACK/" + xmlFileName;
                string xmlSigFileName = "temp.sig";
                string xmlSigFilePath = "./ACK/" + xmlSigFileName;
                // SFTPHelper.MoveFile("10.176.100.62", 22, "admin", "admin", "/CTS/", "/CTS/Done/", fileName + ".zip");
                // SFTPHelper.DownloadFile("1.6.198.15", 22, "GOWB", "Gowb1234$", "/EPAY/INACK" + fileName + ".zip", localFilePath);
                SFTPHelper.DownloadFile("1.6.198.15", 22, "GOWB", "Gowb1234$", "/CTS/" + fileName + ".zip", localFilePath);
                using (ZipArchive archive = ZipFile.OpenRead(localFilePath))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        entry.ExtractToFile("./ACK/" + entry.FullName);
                    }
                }
                if (SignHelper.VerifyXMLSignatures(xmlFilePath, xmlSigFilePath))
                {
                    // SFTPHelper.MoveFile("10.176.100.62", 22, "admin", "admin","/CTS/"+xmlFileName,"/CTS/Done/"+xmlFileName,fileName+".zip");
                    SFTPHelper.MoveFile("1.6.198.15", 22, "GOWB", "Gowb1234$", "/CTS/", "/CTS/Done/", fileName + ".zip");
                    aPIResponse.Message = "INACK Downloaded and Verified Successfully";
                    aPIResponse.result = "true";
                    aPIResponse.apiResponseStatus = Enum.APIResponseStatus.Success;
                    return aPIResponse;
                }
                aPIResponse.Message = "INACK Downloaded and Verified Faild";
                aPIResponse.result = "False";
                aPIResponse.apiResponseStatus = Enum.APIResponseStatus.Error;
                return aPIResponse;
            }
            catch (Exception ex)
            {
                aPIResponse.Message = ex.Message;
                aPIResponse.result = "false";
                aPIResponse.apiResponseStatus = Enum.APIResponseStatus.Error;
                return aPIResponse;
            }
        }

        [HttpGet("CreateLot")]
        public async Task<APIResponse<bool>> CreateLot()
        {
            APIResponse<bool> aPIResponse = new();
            try
            {

                aPIResponse.Message = "Lot Created Successfully";
                aPIResponse.result = true;
                aPIResponse.apiResponseStatus = Enum.APIResponseStatus.Success;
                return aPIResponse;
            }
            catch (Exception ex)
            {
                aPIResponse.Message = ex.Message;
                aPIResponse.result = false;
                aPIResponse.apiResponseStatus = Enum.APIResponseStatus.Error;
                return aPIResponse;
            }
        }
                [HttpGet("DownloadandVerify")]
        public async Task<APIResponse<string>> DownloadandVerify([FromQuery]string fileName,[FromQuery]string localPath,[FromQuery]string remotePath)
        {
            APIResponse<string> aPIResponse = new();
            try
            {
                string localFilePath = localPath + fileName + ".zip";
                string remoteFilePath = remotePath + fileName + ".zip";
                string xmlFileName = fileName + ".xml";
                string xmlFilePath = localPath + xmlFileName;
                string xmlSigFileName = fileName+".sig";
                string xmlSigFilePath = localPath + xmlSigFileName;
                SFTPHelper.DownloadFile("1.6.198.15", 22, "GOWB", "Gowb1234$", remoteFilePath, localFilePath);
                using (ZipArchive archive = ZipFile.OpenRead(localFilePath))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        entry.ExtractToFile(localPath + entry.FullName);
                    }
                }
                if (SignHelper.VerifyXMLSignatures(xmlFilePath, xmlSigFilePath))
                {
                    SFTPHelper.MoveFile("1.6.198.15", 22, "GOWB", "Gowb1234$", remotePath, remotePath+"Done/", fileName + ".zip");
                    aPIResponse.Message = "File Downloaded and Verified Successfully";
                    aPIResponse.result = "true";
                    aPIResponse.apiResponseStatus = Enum.APIResponseStatus.Success;
                    return aPIResponse;
                }
                aPIResponse.Message = "Verified Faild";
                aPIResponse.result = "False";
                aPIResponse.apiResponseStatus = Enum.APIResponseStatus.Error;
                return aPIResponse;
            }
            catch (Exception ex)
            {
                aPIResponse.Message = ex.Message;
                aPIResponse.result = "false";
                aPIResponse.apiResponseStatus = Enum.APIResponseStatus.Error;
                return aPIResponse;
            }
        }
    }
}
