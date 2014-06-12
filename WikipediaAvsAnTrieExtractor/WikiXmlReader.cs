using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace AvsAnTrie {
	public static class WikiXmlReader {

		static readonly XNamespace ns = XNamespace.Get("http://www.mediawiki.org/xml/export-0.4/");

		public static string GetArticleText(XElement page) {
			var textEl = page.Element(ns + "revision").Element(ns + "text");
			return textEl == null ? null : textEl.Value;
		}

		public static string GetArticleTitle(XElement page) {
		    return page.Element(ns + "title").Value;
		}


		public static IEnumerable<XElement> StreamElements(FileInfo wikifile) {
			using(var stream = wikifile.OpenRead())
			using(var reader = XmlReader.Create(stream))
			while (reader.Read())
				if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "page")
					yield return (XElement)XNode.ReadFrom(reader);
		}
	}
}
