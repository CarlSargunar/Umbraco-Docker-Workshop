namespace UmBlazor.Services.ContentDelivery
{
    public class ContentDeliveryResponse
    {
        public int total { get; set; }
        public Item[] items { get; set; }
    }

    public class Item
    {
        public string contentType { get; set; }
        public string name { get; set; }
        public DateTime createDate { get; set; }
        public DateTime updateDate { get; set; }
        public Route route { get; set; }
        public string id { get; set; }
        public Properties properties { get; set; }
        public Cultures cultures { get; set; }
    }

    public class Route
    {
        public string path { get; set; }
        public Startitem startItem { get; set; }
    }

    public class Startitem
    {
        public string id { get; set; }
        public string path { get; set; }
    }

    public class Properties
    {
        public string productName { get; set; }
        public decimal price { get; set; }
        public string[] category { get; set; }
        public string description { get; set; }
        public string sku { get; set; }
        public Photo[] photos { get; set; }
        public Features features { get; set; }
        public Bodytext bodyText { get; set; }
    }

    public class Features
    {
        public ContentItem[] items { get; set; }
    }

    public class ContentItem
    {
        public Content content { get; set; }
        public object settings { get; set; }
    }

    public class Content
    {
        public string contentType { get; set; }
        public string id { get; set; }
        public ContentProperties properties { get; set; }
    }

    public class ContentProperties
    {
        public string featureName { get; set; }
        public string featureDetails { get; set; }
    }

    public class Bodytext
    {
        public int gridColumns { get; set; }
        public object[] items { get; set; }
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
        public PhotoProperties properties { get; set; }
        public object focalPoint { get; set; }
        public object[] crops { get; set; }
    }

    public class PhotoProperties
    {
    }

    public class Cultures
    {
    }
}
