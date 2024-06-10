using Org.BouncyCastle.Cms;
using Org.BouncyCastle.X509.Store;
using RBI_Ekuber;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using System.Xml;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace CTS_BE.Helper
{
    public static class SignHelper
    {
        public static string signdocument(string xmlfilepath, string certpath, string sigfilename, string locsigpath)
        {
            string result = "";
            try
            {
                byte[] msg = File.ReadAllBytes(xmlfilepath);
                X509Certificate2 fileCertificate = GetFileCertificate(certpath);
                byte[] bytes = SignDocument.SignMsg(msg, fileCertificate, detached: true);
                string text = locsigpath + sigfilename + ".sig";
                File.WriteAllBytes(text, bytes);
                result = text;
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }
        public static X509Certificate2 GetFileCertificate(string path)
        {
            X509Certificate2 x509Certificate = null;
            return new X509Certificate2(path, "nic@123", X509KeyStorageFlags.MachineKeySet);
        }
        public static bool verifySignaturesRBI(FileInfo xmlfile, FileStream sigfile)
        {
            bool result = false;
            CmsProcessable val = null;
            CmsSignedData val2 = null;
            IX509Store val3 = null;
            SignerInformationStore val4 = null;
            try
            {
                val = (CmsProcessable)new CmsProcessableFile(xmlfile);
                val2 = new CmsSignedData(val, (Stream)sigfile);
                val3 = val2.GetCertificates("Collection");
                val4 = val2.GetSignerInfos();
                val4.GetSigners();
                foreach (SignerInformation signer in val4.GetSigners())
                {
                    SignerInformation val5 = signer;
                    ArrayList arrayList = new ArrayList(val3.GetMatches((IX509Selector)(object)val5.SignerID));
                    Org.BouncyCastle.X509.X509Certificate val6 = (Org.BouncyCastle.X509.X509Certificate)arrayList[0];
                    result = (val5.Verify(val6.GetPublicKey()) ? true : false);
                }

                return result;
            }
            catch (Exception ex)
            {
                //Trace.Write(ex.ToString());
                return result;
            }
        }
    }
}