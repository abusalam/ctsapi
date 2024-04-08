using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace CTS_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        public TestController()
        {

        }
        [HttpGet("GeneretXML")]
        public void GenerateXML()
        {
            string filePath = "example.xml";
            using (XmlWriter xml = XmlWriter.Create(filePath))
            {
                xml.WriteStartDocument();
                xml.WriteStartElement("Root");
                xml.WriteStartElement("RequestPayload"); //<RequestPayload>
                xml.WriteStartElement("AppHdr"); //<AppHdr>
                xml.WriteStartElement("Fr"); //<Fr>
                xml.WriteStartElement("OrgId"); //<OrgId>
                xml.WriteStartElement("Id"); //<Id>
                xml.WriteStartElement("OrgId"); //<OrgId> 
                xml.WriteStartElement("Othr"); //<OrgId> 
                xml.WriteElementString("Id", "TEST");
                xml.WriteEndElement(); //</Othr>
                xml.WriteEndElement(); //</OrgId>
                xml.WriteEndElement(); //</Id>
                xml.WriteEndElement(); //</OrgId>
                xml.WriteEndElement(); //</Fr>

                xml.WriteStartElement("To"); //<Fr>
                xml.WriteStartElement("FIId"); //<FIId>
                xml.WriteStartElement("FinInstnId"); //<FinInstnId>
                xml.WriteStartElement("ClrSysMmbId"); //<ClrSysMmbId>                          
                xml.WriteElementString("MmbId", "TEST");
                xml.WriteEndElement(); //</ClrSysMmbId>
                xml.WriteEndElement(); //</FinInstnId>
                xml.WriteEndElement(); //</FIId>
                xml.WriteEndElement(); //</To>

                xml.WriteElementString("BizMsgIdr", "TEST");
                xml.WriteElementString("MsgDefIdr", "TEST"); ;  //<MsgDefIdr>
                xml.WriteElementString("BizSvc", "TEST"); ;  //<BizSvc>
                xml.WriteElementString("CreDt", "TEST"); ;  //<CreDt>
                xml.WriteEndElement(); //</AppHdr>

                //Group Header
                xml.WriteStartElement("Document"); //<Document>
                xml.WriteStartElement("CstmrCdtTrfInitn"); //<CstmrCdtTrfInitn>
                xml.WriteStartElement("GrpHdr"); //<GrpHdr>                          
                xml.WriteElementString("MsgId", "TEST");
                xml.WriteElementString("CreDtTm", "TEST");
                xml.WriteStartElement("Authstn"); //<Authstn> 
                xml.WriteElementString("Prtry", "TEST");
                xml.WriteEndElement(); //</Authstn>
                xml.WriteElementString("NbOfTxs", "TEST");
                xml.WriteElementString("CtrlSum", "TEST");
                xml.WriteStartElement("InitgPty"); //<InitgPty>
                xml.WriteElementString("Nm", "TEST");
                xml.WriteStartElement("Id"); //<Id>
                xml.WriteStartElement("OrgId"); //<OrgId>
                xml.WriteStartElement("Othr"); //<Othr>
                xml.WriteElementString("Id", "TEST");
                xml.WriteEndElement(); //</Othr>
                xml.WriteEndElement(); //</OrgId>
                xml.WriteEndElement(); //</Id>
                xml.WriteStartElement("CtctDtls"); //<CtctDtls>
                xml.WriteElementString("EmailAdr", "TEST");
                xml.WriteEndElement(); //</CtctDtls>
                xml.WriteEndElement(); //</InitgPty>
                xml.WriteEndElement(); //</GrpHdr>

                //Payment Block

                xml.WriteStartElement("PmtInf"); //<PmtInf>  
                xml.WriteElementString("PmtInfId","TEST");
                xml.WriteElementString("PmtMtd", "TEST");
                xml.WriteElementString("BtchBookg", "TEST");
                xml.WriteElementString("NbOfTxs", "TEST");
                xml.WriteElementString("CtrlSum", "TEST");

                xml.WriteStartElement("PmtTpInf"); //<PmtTpInf>  
                xml.WriteElementString("InstrPrty", "HIGH");
                xml.WriteStartElement("SvcLvl"); //<SvcLvl> 
                xml.WriteElementString("Prtry", "TEST");
                xml.WriteEndElement(); //</SvcLvl>
                xml.WriteEndElement(); //</PmtTpInf>
                xml.WriteElementString("ReqdExctnDt", "TEST");

                xml.WriteStartElement("Dbtr"); //<Dbtr>  
                xml.WriteElementString("Nm", "TEST");
                xml.WriteStartElement("PstlAdr"); //<PstlAdr>  
                xml.WriteElementString("Dept", "TEST");
                xml.WriteElementString("SubDept", "TEST");
                xml.WriteEndElement(); //</PstlAdr>
                xml.WriteStartElement("Id"); //<Id>  
                xml.WriteStartElement("OrgId"); //<OrgId>  
                xml.WriteStartElement("Othr"); //<Othr>  
                xml.WriteElementString("Id", "TEST");
                xml.WriteEndElement(); //</Othr>
                xml.WriteEndElement(); //</OrgId>
                xml.WriteEndElement(); //</Id>                  
                xml.WriteEndElement(); //</Dbtr>

                xml.WriteStartElement("DbtrAcct"); //<DbtrAcct>  
                xml.WriteStartElement("Id"); //<Id>  
                xml.WriteStartElement("Othr"); //<Othr> 
                xml.WriteElementString("Id", "TEXT");
                //xml.WriteElementString("Issr", "")
                xml.WriteEndElement(); //</Othr>
                xml.WriteEndElement(); //</Id>
                xml.WriteEndElement(); //</DbtrAcct>

                xml.WriteStartElement("DbtrAgt"); //<DbtrAgt>  
                xml.WriteStartElement("FinInstnId"); //<FinInstnId>  
                xml.WriteStartElement("ClrSysMmbId"); //<ClrSysMmbId> 
                xml.WriteElementString("MmbId", "TEST");

                xml.WriteEndElement(); //</ClrSysMmbId>
                xml.WriteEndElement(); //</FinInstnId>
                xml.WriteEndElement(); //</DbtrAgt>

                
                //For i As Integer = 0 To dtBNFC.Rows.Count - 1

                //    xml.WriteStartElement("CdtTrfTxInf"); //<CdtTrfTxInf>  
                //    xml.WriteStartElement("PmtId"); //<PmtId>
                //    xml.WriteElementString("InstrId", dtBNFC.Rows(i)("CDTTRFTXN_INSTRID").ToString().Trim().Replace("/", ""));
                //    xml.WriteElementString("EndToEndId", dtBNFC.Rows(i)("CDTTRFTXN_ENDTOENDID").ToString().Trim());
                //    xml.WriteEndElement(); //</PmtId>

                //    Dim INSTDAMT_AMT As Decimal = 0
                //    INSTDAMT_AMT = Convert.ToDecimal(dtBNFC.Rows(i)("CDTTRFTXN_INSTDAMT_AMT").ToString());

                //    xml.WriteStartElement("Amt"); //<Amt>
                //    xml.WriteStartElement("InstdAmt"); //<InstdAmt>
                //    xml.WriteElementString("Amt", INSTDAMT_AMT.ToString("F"));
                //    xml.WriteElementString("CcyOfTrf", dtBNFC.Rows(i)("CDTTRFTXN_CCYOFTRF").ToString().Trim());
                //    xml.WriteEndElement(); //</InstdAmt>
                //    xml.WriteEndElement(); //</Amt>
                //    xml.WriteStartElement("CdtrAgt"); //<CdtrAgt>
                //    xml.WriteStartElement("FinInstnId"); //<FinInstnId>
                //    xml.WriteStartElement("ClrSysMmbId"); //<ClrSysMmbId>
                //    xml.WriteElementString("MmbId", dtBNFC.Rows(i)("CDTTRFTXN_CDTRAGT_MMBID").ToString().Trim());
                //    xml.WriteEndElement(); //</ClrSysMmbId>
                //    xml.WriteEndElement(); //</FinInstnId>
                //    xml.WriteStartElement("BrnchId"); //<BrnchId>
                //    xml.WriteElementString("Id", dtBNFC.Rows(i)("CDTTRFTXN_CDTRAGT_BRNCHID").ToString().Trim());
                //    xml.WriteEndElement(); //</BrnchId>
                //    xml.WriteEndElement(); //</CdtrAgt>

                //    xml.WriteStartElement("Cdtr"); //<Cdtr>
                //    xml.WriteElementString("Nm", Regex.Replace(dtBNFC.Rows(i)("CDTTRFTXN_CDTR_NM").ToString().Trim(), "[^0-9a-zA-Z ]+", ""));
                //    xml.WriteEndElement(); //</Cdtr>

                //    xml.WriteStartElement("CdtrAcct"); //<CdtrAcct>
                //    xml.WriteStartElement("Id"); //<Id>
                //    xml.WriteStartElement("Othr"); //<Othr>
                //    xml.WriteElementString("Id", dtBNFC.Rows(i)("CDTTRFTXN_CDTRACCT_OTHRID").ToString().Trim());
                //    xml.WriteEndElement(); //</Othr>
                //    xml.WriteEndElement(); //</Id>

                //    xml.WriteStartElement("Tp"); //<Tp>
                //    xml.WriteElementString("Cd", dtBNFC.Rows(i)("CDTTRFTXN_CDTRACCT_TPCD").ToString().Trim());
                //    xml.WriteEndElement(); //</Tp>
                //    xml.WriteEndElement(); //</CdtrAcct>

                //    //xml.WriteStartElement("RmtInf"); // < RmtInf >
                //    //xml.WriteElementString("Ustrd", dtBNFC.Rows(i)("CDTTRFTXN_USTRD").ToString().Trim());
                //    //xml.WriteEndElement(); // </ RmtInf >

                //    xml.WriteEndElement(); //</CdtTrfTxInf> 
                //Next
                xml.WriteEndElement(); //</PmtInf>
                xml.WriteEndElement(); //</CstmrCdtTrfInitn>
                xml.WriteEndElement(); //</Document>
                xml.WriteEndElement(); //</RequestPayload>
                xml.WriteEndDocument();
            }
        }
    }
}
