using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using log4net.Repository.Hierarchy;

namespace MediaOrganizer.Model
{
    public class RssFeed
    {
        public List<RssItem> Items { get; set; }

        public RssFeed() { }

        public RssFeed(XDocument rssDocument)
        {
            //TODO: account for different types of rss formats (using torrentshack as a baseline)
            Items = new List<RssItem>();

            foreach (var rssItemElement in rssDocument.Root.Element("channel").Elements("item"))
            {
                try
                {
                    Items.Add(new RssItem(rssItemElement));
                }
                catch (Exception)
                {
                    //TODO: logging
                    throw;
                }
            }
        }
    }

    public class RssItem    
    {
        public string Title { get; set; }
        public string Link { get; set; }
        //public string Description { get; set; }

        public RssItem()
        {
        }

        public RssItem(XElement element)
        {
            Title = element.Element("title").Value;
            Link = element.Element("link").Value;
        }
    }
}
