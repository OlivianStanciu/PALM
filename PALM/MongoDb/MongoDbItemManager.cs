using MongoDB.Driver;
using PALM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PALM.MongoDb
{
    public class MongoDbItemManager<T> where T : IMongoDbItem
    {
        private readonly IMongoCollection<T> _items;

        public MongoDbItemManager(IMongoCollection<T> collection)
        {
            _items = collection;
        }

        public virtual List<T> Get() => _items.Find(t => true).ToList();

        public virtual T Get(string id) => _items.Find<T>(t => t.Id == id).FirstOrDefault();

        public virtual T Create(T item)
        {
            _items.InsertOne(item);
            return item;
        }

        public virtual void Update(string id, T item)
        {
            _items.ReplaceOne<T>(t => t.Id == id, item);
        }

        public virtual void Remove(T item) => _items.DeleteOne(t => t.Id == item.Id);

        public virtual void Remove(string id) => _items.DeleteOne(t => t.Id == id);
    }
}
