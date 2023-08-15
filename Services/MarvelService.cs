using System.Text;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MarvelTest.Models;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace MarvelTest.Services
{
    public class MarvelService
    {
        private readonly HttpClient _httpClient;
        private const string MarvelApiBaseUrl = "https://gateway.marvel.com:443/v1/public";
        private const string PublicKey = "3f35ee1da11fd61f814d526bca0d710d";
        private const string PrivateKey = "092e8698e903b1a635155049e0c338f7b9f785d9";

        public MarvelService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Characters>> GetCharacters()
        {
            var timeStamp = DateTime.Now.Ticks.ToString();
            var hash = CalculateHash();

            var url = $"{MarvelApiBaseUrl}/characters?ts={timeStamp}&apikey={PublicKey}&hash={hash}";

            var response = await _httpClient.GetFromJsonAsync<List<Characters>>(url);

            return response;
        }

        public async Task<List<Comics>> GetComics()
        {
            var timeStamp = DateTime.UtcNow.Ticks.ToString();
            var hash = CalculateHash();

            var url = $"{MarvelApiBaseUrl}/comics?ts={timeStamp}&apikey={PublicKey}&hash={hash}";

            var response = await _httpClient.GetFromJsonAsync<List<Comics>>(url);

            return response;
        }

        private static string CalculateHash()
        {
            string timeZone = DateTime.Now.Ticks.ToString();

            byte[] bytes = Encoding.UTF8.GetBytes(timeZone + PrivateKey + PublicKey);
            var gerador = MD5.Create();
            byte[] bytesHash = gerador.ComputeHash(bytes);
            return BitConverter.ToString(bytesHash).ToLower().Replace("-", String.Empty);
        }
    }
}
