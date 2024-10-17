using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmBlazor.Models
{
    public class BodyText
    {
        public int gridColumns { get; set; }
        public List<object> items { get; set; }
    }

    public class Content
    {
        public string id { get; set; }
        public string contentType { get; set; }
        public Properties properties { get; set; }
    }

    public class Cultures
    {
    }

    public class Features
    {
        public List<Item> items { get; set; }
    }

    public class Item
    {
        public string name { get; set; }
        public Route route { get; set; }
        public string id { get; set; }
        public string contentType { get; set; }
        public Properties properties { get; set; }
        public Cultures cultures { get; set; }
        public Content content { get; set; }
        public object settings { get; set; }
    }

    public class Photo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string mediaType { get; set; }
        public string url { get; set; }
        public string extension { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int bytes { get; set; }
        public Properties properties { get; set; }
        public object focalPoint { get; set; }
        public List<object> crops { get; set; }
    }

    public class Properties
    {
        public string productName { get; set; }
        public int price { get; set; }
        public List<string> category { get; set; }
        public string description { get; set; }
        public string sku { get; set; }
        public List<Photo> photos { get; set; }
        public Features features { get; set; }
        public BodyText bodyText { get; set; }
        public string featureName { get; set; }
        public string featureDetails { get; set; }
    }

    public class Root
    {
        public int total { get; set; }
        public List<Item> items { get; set; }
    }

    public class Route
    {
        public string path { get; set; }
        public StartItem startItem { get; set; }
    }

    public class StartItem
    {
        public string id { get; set; }
        public string path { get; set; }
    }


}
