using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace CTS_BE.Helper
{
    public class XmlHelper
    {
        public static bool ValidateXml(string xmlFilePath, string xsdFilePath)
        {
            try
            {
                // Create XmlReaderSettings and set validation type to schema.
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;

                // Load XSD schema from file path.
                using (var xsdReader = XmlReader.Create(xsdFilePath))
                {
                    settings.Schemas.Add(null, xsdReader);
                }

                // Specify validation event handler.
                settings.ValidationEventHandler += ValidationCallback;

                // Load XML document from file path.
                using (var xmlReader = XmlReader.Create(xmlFilePath, settings))
                {
                    // Read XML content to trigger validation.
                    while (xmlReader.Read()) { }
                }

                // If validation succeeds, return true.
                return true;
            }
            catch (Exception ex)
            {
                // If validation fails, catch the exception and return false.
                Console.WriteLine($"Validation failed: {ex.Message}");
                return false;
            }
        }

        // Validation callback method to handle validation errors.
        static void ValidationCallback(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Error)
                throw new Exception($"Validation error: {e.Message}");
            else if (e.Severity == XmlSeverityType.Warning)
                Console.WriteLine($"Validation warning: {e.Message}");
        }
    }
}