using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.BAL.Interfaces;
using CTS_BE.BAL.Interfaces.paymandate;
using CTS_BE.Helper;
using CTS_BE.Model;
using CTS_BE.Model.e_Kuber;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CTS_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LocalTest : Controller
    {
        private readonly IPaymandateService _paymandateService;
        private readonly ITransactionLotService _transactionLotService;

        public LocalTest(IPaymandateService paymandateService, ITransactionLotService transactionLotService)
        {
            _paymandateService = paymandateService;
            _transactionLotService = transactionLotService;
        }
        //[HttpGet("GeneretXML")]
        //public void GenerateXML()
        //{
        //    string filePath = "example.xml";
        //    XmlWriterSettings settings = new XmlWriterSettings
        //    {
        //        Indent = true,
        //        IndentChars = "\t",
        //        Encoding = System.Text.Encoding.UTF8
        //    };

        //    using (XmlWriter writer = XmlWriter.Create(filePath, settings))
        //    {
        //        writer.WriteStartDocument();

        //        // Start RequestPayload element
        //        writer.WriteStartElement("RequestPayload");

        //        // Write AppHdr element
        //        writer.WriteStartElement("AppHdr");

        //        // Write Fr element
        //        writer.WriteStartElement("Fr");
        //        writer.WriteStartElement("OrgId");
        //        writer.WriteStartElement("Id");
        //        writer.WriteStartElement("OrgId");
        //        writer.WriteStartElement("Othr");
        //        writer.WriteElementString("Id", "125");
        //        writer.WriteEndElement(); // Othr
        //        writer.WriteEndElement(); // OrgId
        //        writer.WriteEndElement(); // Id
        //        writer.WriteEndElement(); // OrgId
        //        writer.WriteEndElement(); // Fr

        //        // Write To element
        //        writer.WriteStartElement("To");
        //        writer.WriteStartElement("FIId");
        //        writer.WriteStartElement("FinInstnId");
        //        writer.WriteStartElement("ClrSysMmbId");
        //        writer.WriteElementString("MmbId", "RBI");
        //        writer.WriteEndElement(); // ClrSysMmbId
        //        writer.WriteEndElement(); // FinInstnId
        //        writer.WriteEndElement(); // FIId
        //        writer.WriteEndElement(); // To

        //        writer.WriteElementString("BizMsgIdr", "EPV80125000000000210202402100338");
        //        writer.WriteElementString("MsgDefIdr", "pain.001.001.08");
        //        writer.WriteElementString("BizSvc", "CustomerCreditTransferInitiationV08");
        //        writer.WriteElementString("CreDt", "2024-02-10T14:02:46");

        //        writer.WriteEndElement(); // AppHdr

        //        // Write Document element
        //        writer.WriteStartElement("Document");

        //        // Write CstmrCdtTrfInitn element
        //        writer.WriteStartElement("CstmrCdtTrfInitn");

        //        // Write GrpHdr element
        //        writer.WriteStartElement("GrpHdr");
        //        writer.WriteElementString("MsgId", "EPV80125000000000210202402100338");
        //        writer.WriteElementString("CreDtTm", "2024-02-10T14:02:46");
        //        writer.WriteStartElement("Authstn");
        //        writer.WriteElementString("Prtry", "ALL");
        //        writer.WriteEndElement(); // Authstn
        //        writer.WriteElementString("NbOfTxs", "2");
        //        writer.WriteElementString("CtrlSum", "41701.00");

        //        // Write InitgPty element
        //        writer.WriteStartElement("InitgPty");
        //        writer.WriteElementString("Nm", "JHARKHAND");
        //        writer.WriteStartElement("Id");
        //        writer.WriteStartElement("OrgId");
        //        writer.WriteStartElement("Othr");
        //        writer.WriteElementString("Id", "125");
        //        writer.WriteEndElement(); // Othr
        //        writer.WriteEndElement(); // OrgId
        //        writer.WriteEndElement(); // Id
        //        writer.WriteStartElement("CtctDtls");
        //        writer.WriteElementString("EmailAdr", "test@gmail.com");
        //        writer.WriteEndElement(); // CtctDtls
        //        writer.WriteEndElement(); // InitgPty

        //        writer.WriteEndElement(); // GrpHdr

        //        // Write PmtInf element
        //        writer.WriteStartElement("PmtInf");
        //        writer.WriteElementString("PmtInfId", "0125CKP0002100020240210202402");
        //        writer.WriteElementString("PmtMtd", "TRF");
        //        writer.WriteElementString("BtchBookg", "true");
        //        writer.WriteElementString("NbOfTxs", "2");
        //        writer.WriteElementString("CtrlSum", "41701.00");
        //        writer.WriteStartElement("PmtTpInf");
        //        writer.WriteElementString("InstrPrty", "HIGH");
        //        writer.WriteStartElement("SvcLvl");
        //        writer.WriteElementString("Prtry", "NEFT");
        //        writer.WriteEndElement(); // SvcLvl
        //        writer.WriteEndElement(); // PmtTpInf
        //        writer.WriteElementString("ReqdExctnDt", "2024-02-10");

        //        // Write Dbtr element
        //        writer.WriteStartElement("Dbtr");
        //        writer.WriteElementString("Nm", "FINANCE DEPT GoJH");
        //        writer.WriteStartElement("PstlAdr");
        //        writer.WriteElementString("Dept", "CKP");
        //        writer.WriteElementString("SubDept", "CKPRWD001");
        //        writer.WriteEndElement(); // PstlAdr
        //        writer.WriteStartElement("Id");
        //        writer.WriteStartElement("OrgId");
        //        writer.WriteStartElement("Othr");
        //        writer.WriteElementString("Id", "125");
        //        writer.WriteEndElement(); // Othr
        //        writer.WriteEndElement(); // OrgId
        //        writer.WriteEndElement(); // Id
        //        writer.WriteEndElement(); // Dbtr

        //        // Write DbtrAcct element
        //        writer.WriteStartElement("DbtrAcct");
        //        writer.WriteStartElement("Id");
        //        writer.WriteStartElement("Othr");
        //        writer.WriteElementString("Id", "01602501045");
        //        writer.WriteEndElement(); // Othr
        //        writer.WriteEndElement(); // Id
        //        writer.WriteEndElement(); // DbtrAcct

        //        // Write DbtrAgt element
        //        writer.WriteStartElement("DbtrAgt");
        //        writer.WriteStartElement("FinInstnId");
        //        writer.WriteStartElement("ClrSysMmbId");
        //        writer.WriteElementString("MmbId", "RBIS0GOJHEP");
        //        writer.WriteEndElement(); // ClrSysMmbId
        //        writer.WriteEndElement(); // FinInstnId
        //        writer.WriteEndElement(); // DbtrAgt

        //        // Write CdtTrfTxInf elements
        //        for (int i = 0; i < 2; i++)
        //        {
        //            writer.WriteStartElement("CdtTrfTxInf");
        //            writer.WriteStartElement("PmtId");
        //            writer.WriteElementString("InstrId", (i == 0) ? "DRNPAYEE03891A21010022024CKP" : "20RCHR00508E1DNA21010022024CKP");
        //            writer.WriteElementString("EndToEndId", (i == 0) ? "EP012500000000039824041001576" : "EP012500000000039824041001577");
        //            writer.WriteEndElement(); // PmtId
        //            writer.WriteStartElement("Amt");
        //            writer.WriteStartElement("InstdAmt");
        //            writer.WriteElementString("Amt", (i == 0) ? "40979.00" : "722.00");
        //            writer.WriteElementString("CcyOfTrf", "INR");
        //            writer.WriteEndElement(); // InstdAmt
        //            writer.WriteEndElement(); // Amt
        //            writer.WriteStartElement("CdtrAgt");
        //            writer.WriteStartElement("FinInstnId");
        //            writer.WriteStartElement("ClrSysMmbId");
        //            writer.WriteElementString("MmbId", (i == 0) ? "SBIN0001672" : "RBIS0GSTPMT");
        //            writer.WriteEndElement(); // ClrSysMmbId
        //            writer.WriteEndElement(); // FinInstnId
        //            writer.WriteStartElement("BrnchId");
        //            writer.WriteElementString("Id", (i == 0) ? "SBIN0001672" : "RBIS0GSTPMT");
        //            writer.WriteEndElement(); // BrnchId
        //            // writer.WriteEndElement(); // FinInstnId
        //            writer.WriteEndElement(); // CdtrAgt
        //            writer.WriteStartElement("Cdtr");
        //            writer.WriteElementString("Nm", (i == 0) ? "RAIDER SECURITY SERVICES PVT LTD" : "GST");
        //            writer.WriteEndElement(); // Cdtr
        //            writer.WriteStartElement("CdtrAcct");
        //            writer.WriteStartElement("Id");
        //            writer.WriteStartElement("Othr");
        //            writer.WriteElementString("Id", (i == 0) ? "33544440518" : "24022000007067");
        //            writer.WriteEndElement(); // Othr
        //            writer.WriteEndElement(); // Id
        //            writer.WriteStartElement("Tp");
        //            writer.WriteElementString("Cd", (i == 0) ? "10" : "10");
        //            writer.WriteEndElement(); // Tp
        //            writer.WriteEndElement(); // CdtrAcct
        //            writer.WriteEndElement(); // CdtTrfTxInf
        //        }

        //        writer.WriteEndElement(); // PmtInf

        //        writer.WriteEndElement(); // CstmrCdtTrfInitn

        //        writer.WriteEndElement(); // Document

        //        writer.WriteEndElement(); // RequestPayload

        //        writer.WriteEndDocument();
        //    }
        //}

        [HttpGet("GeneretXML")]
        public async Task<APIResponse<string>> GenerateXML([FromQuery] bool pushFile = true)
        {
            APIResponse<string> aPIResponse = new();
            string fileName = "";
            string filePath = "";
            try
            {
                List<TransactionLotModel> pendingLots = await _transactionLotService.pendingLots();
                foreach (TransactionLotModel pendingLot in pendingLots)
                {
                    EKuber eKuberData = await _transactionLotService.GetXMLData(pendingLot.Id, "", "");
                    // fileName = eKuberData.requestPayload.AppHdr.BizMsgIdr;
                    fileName = "EPV80116001516701174202404150055";
                    _paymandateService.GenerateXML(eKuberData, fileName, fileName + ".xml");
                    bool isValid = XmlHelper.ValidateXml(fileName + ".xml", "pain.001.001.08v2.4.xsd");
                    if (isValid)
                    {
                        // SignHelper.signdocument1(fileName + ".xml", "ifms.gowb.pfx", fileName, "");
                        string ZipFileName = fileName + ".zip";
                        using (var zip = ZipFile.Open(ZipFileName, ZipArchiveMode.Create))
                        {
                            zip.CreateEntryFromFile(fileName + ".xml", fileName + ".xml");
                            // zip.CreateEntryFromFile(fileName + ".sig", fileName + ".sig");
                        }
                        //var tstFile = File.OpenRead(filePath);
                        if (pushFile)
                        {
                            filePath = "./" + fileName + ".zip";
                            SFTPHelper.UploadFile("10.176.100.62", 22, "admin", "admin", filePath, "EPAY/IN//" + fileName + ".temp");
                            SFTPHelper.RenameFile("10.176.100.62", 22, "admin", "admin", fileName + ".temp", fileName + ".zip", "EPAY/IN//");
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

        [HttpGet("PushtoRBI/{fileName}")]
        public async Task<APIResponse<bool>> PushtoRBI(string fileName)
        {
            APIResponse<bool> aPIResponse = new();
            try
            {

                SFTPHelper.UploadFile("1.6.198.15", 22, "GOWB", "Gowb1234$", fileName + ".zip", "/EPAY/IN/" + fileName + ".zip");

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

        [HttpGet("VerifyXML")]
        public APIResponse<bool> VerifyXML()
        {
            APIResponse<bool> aPIResponse = new();
            try
            {
                System.IO.FileInfo xmlfile = new System.IO.FileInfo("./ACK/EPV80116001516701174202404150001" + ".xml");
                System.IO.FileStream sigfile = new System.IO.FileStream("./ACK/temp.sig", System.IO.FileMode.Open);
                bool verify = SignHelper.verifySignaturesRBI(xmlfile, sigfile);
                sigfile.Close();
                aPIResponse.result = verify;
                aPIResponse.Message = (verify) ? "Valid XML" : "Invalid XML";
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
        [HttpGet("INACK/{fileName}")]
        public async Task<APIResponse<string>> DownloadandVerifyACK(string fileName)
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
                SFTPHelper.DownloadFile("10.176.100.62", 22, "admin", "admin", "/CTS/" + fileName + ".zip", localFilePath);
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
                    SFTPHelper.MoveFile("10.176.100.62", 22, "admin", "admin", "/CTS/", "/CTS/Done/", fileName + ".zip");
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
        [HttpGet("DownloadandVerify")]
        public async Task<APIResponse<string>> DownloadandVerify([FromQuery] string fileName, [FromQuery] string localPath, [FromQuery] string remotePath)
        {
            APIResponse<string> aPIResponse = new();
            try
            {
                string localFilePath = localPath + fileName + ".zip";
                string remoteFilePath = remotePath + fileName + ".zip";
                // string fileNameTrim = fileName.Substring(3);
                string xmlFileName = fileName + ".xml";
                string xmlFilePath = localPath + xmlFileName;
                string xmlSigFileName = fileName + ".sig";
                string xmlSigFilePath = localPath + xmlSigFileName;
                // SFTPHelper.MoveFile("10.176.100.62", 22, "admin", "admin", "/CTS/", "/CTS/Done/", fileName + ".zip");
                // SFTPHelper.DownloadFile("1.6.198.15", 22, "GOWB", "Gowb1234$", "/EPAY/INACK" + fileName + ".zip", localFilePath);
                SFTPHelper.DownloadFile("10.176.100.62", 22, "admin", "admin", remoteFilePath, localFilePath);
                using (ZipArchive archive = ZipFile.OpenRead(localFilePath))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        entry.ExtractToFile(localPath + entry.FullName);
                    }
                }
                if (SignHelper.VerifyXMLSignatures(xmlFilePath, xmlSigFilePath))
                {
                    // SFTPHelper.MoveFile("10.176.100.62", 22, "admin", "admin","/CTS/"+xmlFileName,"/CTS/Done/"+xmlFileName,fileName+".zip");
                    SFTPHelper.MoveFile("10.176.100.62", 22, "admin", "admin", remotePath, remotePath + "/Done/", fileName + ".zip");
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
        [HttpGet("PushACK")]
        public APIResponse<string> PushACK([FromQuery] string fileName,[FromQuery] string xsdfileName,  [FromQuery] string remotePath,[FromQuery] string acknowledgementType,[FromQuery] string eventCode)
        {
            APIResponse<string> aPIResponse = new();
            try
            {
                EKuber ackData = _paymandateService.GetDNAcknowledgementXMLData(fileName,acknowledgementType,eventCode);
                string ackFileName = ackData.requestPayload.AppHdr.BizMsgIdr;
                _paymandateService.GenerateDNACKXML(ackData, ackFileName, ackFileName + ".xml");
                bool isValid = XmlHelper.ValidateXml(ackFileName + ".xml", xsdfileName+".xsd");
                if (isValid)
                {
                    // SignHelper.signdocument1(ackFileName + ".xml", "ifms.gowb.pfx", ackFileName, "");
                    string ZipFileName = ackFileName + ".zip";
                    using (var zip = ZipFile.Open(ZipFileName, ZipArchiveMode.Create))
                    {
                        zip.CreateEntryFromFile(ackFileName + ".xml", ackFileName + ".xml");
                        // zip.CreateEntryFromFile(ackFileName + ".sig", ackFileName + ".sig");
                    }
                    //var tstFile = File.OpenRead(filePath);

                    string filePath = "./" + ackFileName + ".zip";
                    SFTPHelper.UploadFile("10.176.100.62", 22, "admin", "admin", filePath, remotePath + ackFileName + ".temp");
                    SFTPHelper.RenameFile("10.176.100.62", 22, "admin", "admin", ackFileName + ".temp", ackFileName + ".zip", remotePath);
                }
                aPIResponse.Message = " ACK PUSH Successfully";
                aPIResponse.result = fileName;
                aPIResponse.apiResponseStatus = Enum.APIResponseStatus.Success;
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
        [HttpGet("Recall")]
        public APIResponse<string> Recall()
        {
            APIResponse<string> aPIResponse = new();
            try
            {
                EKuber data = _paymandateService.GetRecallXMLData();
                _paymandateService.GenerateRecallXMLData(data);
                // string ackFileName = ackData.requestPayload.AppHdr.BizMsgIdr;
                // _paymandateService.GenerateDNACKXML(ackData, ackFileName, ackFileName + ".xml");
                // bool isValid = XmlHelper.ValidateXml(ackFileName + ".xml", "admi.004.001.02.xsd");
                // if (isValid)
                // {
                //     // SignHelper.signdocument1(ackFileName + ".xml", "ifms.gowb.pfx", ackFileName, "");
                //     string ZipFileName = ackFileName + ".zip";
                //     using (var zip = ZipFile.Open(ZipFileName, ZipArchiveMode.Create))
                //     {
                //         zip.CreateEntryFromFile(ackFileName + ".xml", ackFileName + ".xml");
                //         // zip.CreateEntryFromFile(ackFileName + ".sig", ackFileName + ".sig");
                //     }
                //     //var tstFile = File.OpenRead(filePath);

                //     string filePath = "./" + ackFileName + ".zip";
                //     SFTPHelper.UploadFile("10.176.100.62", 22, "admin", "admin", filePath, remotePath + ackFileName + ".temp");
                //     SFTPHelper.RenameFile("10.176.100.62", 22, "admin", "admin", ackFileName + ".temp", ackFileName + ".zip", remotePath);
                // }
                aPIResponse.Message = " Recall Successfully";
                aPIResponse.result = "";
                aPIResponse.apiResponseStatus = Enum.APIResponseStatus.Success;
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