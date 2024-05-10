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
        public static string signdocument1(string xmlfilepath, string certpath, string sigfilename, string locsigpath)
        {
            string result = "";
            try
            {
                byte[] msg = File.ReadAllBytes(xmlfilepath);
                X509Certificate2 fileCertificate = GetFileCertificate1(certpath);
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
        public static X509Certificate2 GetFileCertificate1(string path)
        {
            X509Certificate2 x509Certificate = null;
            return new X509Certificate2(path, "nic@123", X509KeyStorageFlags.MachineKeySet);
        }
        public static bool verifySignaturesRBI(FileInfo xmlfile, FileStream sigfile)
        {
            //IL_000c: Unknown result type (might be due to invalid IL or missing references)
            //IL_0012: Expected O, but got Unknown
            //IL_0014: Unknown result type (might be due to invalid IL or missing references)
            //IL_001a: Expected O, but got Unknown
            //IL_004d: Unknown result type (might be due to invalid IL or missing references)
            //IL_0054: Expected O, but got Unknown
            //IL_0070: Unknown result type (might be due to invalid IL or missing references)
            //IL_0077: Expected O, but got Unknown
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
        public static void SignXml(string xmlFilePath, string pfxFilePath, string password)
        {
            // Load the XML document
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(xmlFilePath);

            // Load the PFX certificate
            X509Certificate2 certificate = new X509Certificate2(pfxFilePath, password);

            // Create a SignedXml object
            SignedXml signedXml = new SignedXml(xmlDoc);
            signedXml.SigningKey = certificate.PrivateKey;

            // Create a reference to the XML document
            Reference reference = new Reference("");
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            signedXml.AddReference(reference);

            // Compute the signature
            signedXml.ComputeSignature();

            // Get the XML representation of the signature
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            // Append the signature to the XML document
            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));

            // Save the signed XML document
            xmlDoc.Save(xmlFilePath);
        }
        public static bool VerifyXml(string xmlFilePath)
        {
            // Load the XML document
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(xmlFilePath);

            // Create a SignedXml object
            SignedXml signedXml = new SignedXml(xmlDoc);

            // Find the signature node
            XmlNodeList nodeList = xmlDoc.GetElementsByTagName("Signature", SignedXml.XmlDsigNamespaceUrl);
            if (nodeList.Count == 0)
            {
                throw new CryptographicException("Signature element not found.");
            }

            // Load the signature
            signedXml.LoadXml((XmlElement)nodeList[0]);

            // Verify the signature
            bool isValid = signedXml.CheckSignature();

            return isValid;
        }
    }
}