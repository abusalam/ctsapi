using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;


// using System.Security.Cryptography;
// using System.Security.Cryptography.Xml;
using System.Xml;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
namespace CTS_BE.Helper
{
    public static class SignatureHelper
    {
        public static void Sign(string xmlFi1ePath, string pfxFi1ePath, string password)
        {
            var xmlFi1e = new FileInfo(xmlFi1ePath);
            var pfxFi1e = new FileInfo(pfxFi1ePath);

            var keyStore = new Pkcs12Store(File.OpenRead(pfxFi1e.FullName), password.ToCharArray());
            var alias = (string)keyStore.Aliases.Cast<string>().First();
            var privateKey = (RsaPrivateCrtKeyParameters)keyStore.GetKey(alias).Key;
            var privateKeyParameters = DotNetUtilities.ToRSA(privateKey);
            var privateKeyXmlParams = privateKeyParameters.ExportParameters(true);
            var certificateChain = keyStore.GetCertificateChain(alias);
            var publicKeyCertificate = (Org.BouncyCastle.X509.X509Certificate)certificateChain[certificateChain.Length - 1].Certificate;
            var publicKeyCertificate2 = new System.Security.Cryptography.X509Certificates.X509Certificate2(publicKeyCertificate.GetEncoded());
            using (var stream = xmlFi1e.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var signerUtil = new SignerUtil(stream);
                signerUtil.Sign(privateKeyParameters, publicKeyCertificate2);
                stream.Close();
            }
            // var xmlDoc = new XmlDocument();
            // xmlDoc.PreserveWhitespace = true;
            // xmlDoc.LoadXml(xmlFi1ePath);
        }
        public static bool Verify(string xmlFi1ePath, string pfxFi1ePath, string password)
        {
            var xmlFi1e = new FileInfo(xmlFi1ePath);
            var pfxFi1e = new FileInfo(pfxFi1ePath);

            var keyStore = new Pkcs12Store(File.OpenRead(pfxFi1e.FullName), password.ToCharArray());
            var alias = (string)keyStore.Aliases.Cast<string>().First();
            var privateKey = (RsaPrivateCrtKeyParameters)keyStore.GetKey(alias).Key;
            var privateKeyParameters = DotNetUtilities.ToRSA(privateKey);
            var privateKeyXmlParams = privateKeyParameters.ExportParameters(true);
            var certificateChain = keyStore.GetCertificateChain(alias);
            var publicKeyCertificate = (Org.BouncyCastle.X509.X509Certificate)certificateChain[certificateChain.Length - 1].Certificate;
            var publicKeyCertificate2 = new System.Security.Cryptography.X509Certificates.X509Certificate2(publicKeyCertificate.GetEncoded());
            using (var stream = xmlFi1e.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var signerUtil = new SignerUtil(stream);
                return signerUtil.VerifySign(publicKeyCertificate2);
            }
        }
    }
    public class SignerUtil
    {
        private readonly XmlDocument _document;

        public SignerUtil(Stream stream)
        {
            _document = new XmlDocument();
            _document.Load(stream);
        }

        public void Sign(RSA privateKey, X509Certificate2 publicKeyCertificate)
        {
            var signedXml = new SignedXml(_document);
            signedXml.SigningKey = privateKey;
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;

            var reference = new Reference();
            reference.Uri = "";
            reference.AddTransform(new XmlDsigExcC14NTransform());
            signedXml.AddReference(reference);

            signedXml.KeyInfo = new KeyInfo();
            signedXml.KeyInfo.AddClause(new KeyInfoX509Data(publicKeyCertificate.GetRawCertData()));

            signedXml.ComputeSignature();

            XmlElement xmlDigitalSignature = signedXml.GetXml();
            _document.DocumentElement.AppendChild(_document.ImportNode(xmlDigitalSignature, true)); 
            _document.Save("temp.xml");
        }
        public bool VerifySign(X509Certificate2 publicKeyCertificate)
        {
            var signedXml = new SignedXml(_document);
            var nodeList = _document.GetElementsByTagName("Signature", SignedXml.XmlDsigNamespaceUrl);
            if (nodeList.Count == 0)
            {
                return false;
            }
            var node = nodeList[0] as XmlElement;
            signedXml.LoadXml(node);

            return signedXml.CheckSignature(publicKeyCertificate.GetRSAPublicKey());
        }
    }
}