using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using Client.Models;
using Newtonsoft.Json;

namespace Client.Services.HttpService
{
    public class HttpCardManager
    {
        private readonly HttpClient _httpClient;
        private readonly string _serverUrl; // "http://localhost:5001/api/Cards"
        private readonly Uri _apiUri;
        public HttpCardManager()
        {
            _serverUrl = ConfigurationManager.AppSettings["ApiAddress"];
            _httpClient = new HttpClient();
            _apiUri = new Uri(_serverUrl);
        }
       
        public async void SendCardToServer(Card card)
        {
            var networkCard = new NetworkCard(card);
            var serialized = JsonConvert.SerializeObject(networkCard);
            try
            {
                await _httpClient.PostAsync(_apiUri,
                    new StringContent(serialized, Encoding.UTF8, "application/json"));
            }
            catch(Exception ex)
            {
                MessageBox.Show("Send error\r\nMessage: " + ex.Message);
                Environment.Exit(1);
            }
        }

        public async Task<List<NetworkCard>> GetAllCards()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiUri);
                var responseJson = await response.Content.ReadAsStringAsync();
                var networkCards = JsonConvert.DeserializeObject<List<NetworkCard>>(responseJson);
                return networkCards;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Http get data error, check your App.config ApiAddress key or server is offline\r\n Error messsage:" +
                                ex.Message);
                Environment.Exit(2);
                return null;
            }

        }

        public async void DeleteRequest(Card card)
        {
            try
            {
                await _httpClient.DeleteAsync(new Uri(_serverUrl + card.Id.ToString()));
            }
            catch(Exception ex)
            {
                MessageBox.Show("Delete error. \r\nMessage:" + ex.Message);
                Environment.Exit(3);
            }
        }

        public async void Replace(Card newCard)
        {
            try
            {
                var networkCard = new NetworkCard(newCard);
                var serialized = JsonConvert.SerializeObject(networkCard);
                await _httpClient.PutAsync(_apiUri,
                    new StringContent(serialized, Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Replace error. \r\nMessage:" + ex.Message);
                Environment.Exit(4);
            }
        }
    }
}