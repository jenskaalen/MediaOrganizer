using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MediaOrganizer.Scanner;
using MediaOrganizer.Scanner.Handlers.Xml;
using MediaOrganizer.Scanner.Matching;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaOrganizer.Tests
{
    [TestClass]
    public class Parsing
    {
        [TestMethod]
        public void Parse_ShowHandler()
        {
            XElement element = fileAsXElement("XmlSamples\\ShowHandler.xml");
            var matcher = HandlerXmlParser.ParseContentMatches(element).FirstOrDefault();

            Assert.IsTrue(matcher is ShowMatcher);
            Assert.IsTrue(matcher.Match("somethingDexterfaa"));
        }


        private static XElement fileAsXElement(string file)
        {
            using (var reader = new StreamReader(file))
            {
                XDocument xDoc = XDocument.Load(file);
                return xDoc.Root;
            }
        }
    }
}
