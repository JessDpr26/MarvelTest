namespace MarvelTest.Models
{

    public class Comics
    {
        public string id { get; set; }
        public string digitalId { get; set; }
        public string title { get; set; }
        public string issueNumber { get; set; }
        public string variantDescription { get; set; }
        public string description { get; set; }
        public Characters[] characters { get; set; }
    }

}
