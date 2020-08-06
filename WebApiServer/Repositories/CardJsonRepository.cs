using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json;
using WebApiServer.Models;

namespace WebApiServer.Repositories
{
    public class CardJsonRepository : IRepository<Card>
    {
        private readonly List<Card> _cards;
        private readonly string _jsonFileName;
        private int _lastId;

        public CardJsonRepository(string jsonFileName)
        {
            _jsonFileName = jsonFileName;
            using var reader = new StreamReader(new FileStream(jsonFileName, FileMode.OpenOrCreate), Encoding.UTF8);
            var jsonString = reader.ReadToEnd();
            if (string.IsNullOrEmpty(jsonString.Trim()))
            {
                _cards = new List<Card>();
                _lastId = 0;
                return;
            }

            _cards = JsonConvert.DeserializeObject<List<Card>>(jsonString);
            _lastId = _cards.Count > 0 ? _cards.Max(t => t.Id) : 0;
        }


        public IEnumerable<Card> GetAll()
        {
            return _cards;
        }

        public Card GetById(int id)
        {
            return _cards.FirstOrDefault(t => t.Id == id);
        }

        public void Create(Card item)
        {
            _lastId++;
            item.Id = _lastId;
            _cards.Add(item);
            SaveAsync();
        }

        public void Update(Card newItem)
        {
            var oldCard = _cards.FirstOrDefault(t => t.Id == newItem.Id);

            if (oldCard != null)
            {
                oldCard.Title = newItem.Title;
                oldCard.Image = newItem.Image;
                SaveAsync();
            }
        }

        public void Delete(int id)
        {
            var toDelete = _cards.FirstOrDefault(t => t.Id == id);
            if (toDelete == null) return;
            _cards.Remove(toDelete);
            SaveAsync();
        }

        public async void SaveAsync()
        {
            var jsonString = JsonConvert.SerializeObject(_cards);
            await using var streamWriter = new StreamWriter(_jsonFileName);
            await streamWriter.WriteAsync(jsonString);
        }
    }
}