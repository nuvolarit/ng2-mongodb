using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.OptionsModel;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoMvc.Models;

namespace MongoMvc
{
    public class ArticleRepository : IArticleRespository
    {
        private readonly IMongoDatabase _database;
        private readonly Settings _settings;

        public ArticleRepository(IOptions<Settings> settings)
        {
            _settings = settings.Value;
            _database = Connect();
        }

        public void Add(Article article)
        {
            _database.GetCollection<Article>("articles").InsertOneAsync(article);
        }

        public IEnumerable<Article> AllArticles()
        {
            var articles = _database.GetCollection<Article>("articles").Find(new BsonDocument()).ToListAsync();

            return articles.Result;
        }

        public Article GetById(ObjectId id)
        {
            var query = Builders<Article>.Filter.Eq(e => e.Id, id);
            var article = _database.GetCollection<Article>("articles").Find(query).ToListAsync();

            return article.Result.FirstOrDefault();
        }

        public bool Remove(ObjectId id)
        {
            var query = Builders<Article>.Filter.Eq(e => e.Id, id);
            var result = _database.GetCollection<Article>("articles").DeleteOneAsync(query);

            return GetById(id) == null;
        }

        public void Update(Article article)
        {
            var query = Builders<Article>.Filter.Eq(e => e.Id, article.Id);
            var update = _database.GetCollection<Article>("articles").ReplaceOneAsync(query, article);
        }

        private IMongoDatabase Connect()
        {
            var client = new MongoClient(_settings.MongoConnection);
            var database = client.GetDatabase(_settings.Database);

            return database;
        }
    }
}