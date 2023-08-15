namespace MarvelTest.Models
{
    public class Characters
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string modified { get; set; }
        public string resourceURI { get; set; }
        public Comics[] comics { get; set; }
    }

}
