using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using MongoDB.Bson;
using MongoMvc.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MongoMvc.Controllers
{
    [Route("api/[controller]")]
    public class ArticleController : Controller
    {
        private readonly IArticleRespository _articleRepository;

        public ArticleController(IArticleRespository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        [HttpGet]
        public IEnumerable<Article> GetAll()
        {
            var articles = _articleRepository.AllArticles();
            return articles;
        }

        [HttpGet("{id:length(24)}", Name = "GetByIdRoute")]
        public IActionResult GetById(string id)
        {
            var item = _articleRepository.GetById(new ObjectId(id));
            if (item == null)
            {
                return HttpNotFound();
            }

            return new ObjectResult(item);
        }

        [HttpPost]
        public void CreateArticle([FromBody] Article article)
        {
            if (!ModelState.IsValid)
            {
                HttpContext.Response.StatusCode = 400;
            }
            else
            {
                _articleRepository.Add(article);

                var url = Url.RouteUrl("GetByIdRoute", new {id = article.Id.ToString()}, Request.Scheme,
                    Request.Host.ToUriComponent());
                HttpContext.Response.StatusCode = 201;
                HttpContext.Response.Headers["Location"] = url;
            }
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult DeleteArticle(string id)
        {
            if (_articleRepository.Remove(new ObjectId(id)))
            {
                return new HttpStatusCodeResult(204); // 204 No Content
            }
            return HttpNotFound();
        }
    }
}