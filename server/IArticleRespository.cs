using System.Collections.Generic;
using MongoDB.Bson;
using MongoMvc.Models;

namespace MongoMvc
{
    public interface IArticleRespository
    {
        IEnumerable<Article> AllArticles();

        Article GetById(ObjectId id);

        void Add(Article article);

        void Update(Article article);

        bool Remove(ObjectId id);
    }
}