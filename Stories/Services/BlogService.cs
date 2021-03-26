using Abp.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Stories.Models;
using Stories.VM;
using Stories.VM.Request;
using Stories.VM.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TEK.Core.UoW;

namespace Stories.Services
{
    public interface IBlogService
    {
        Task<HomePageViewModel> GetHomePagePosts(string cat, string tag, string search, int page);
        Task<BlogSingleViewModel> GetSinglePost(string link);
    }

    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public BlogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<HomePageViewModel> GetHomePagePosts(string cat, string tag, string search, int page)
        {
            var take = 4;
            var skip = take * (page - 1);
            var p = await _unitOfWork.GetRepository<Post>().GetAll().ToListAsync();

            // get Category
            var cats = await _unitOfWork.GetRepository<Category>().GetAll().ToListAsync();

            // get Hot tags
            var mostPopularPosts = p.Where(x => x.CreatedDate.Year == DateTime.Now.Year).OrderByDescending(x => x.Views).Take(5).ToList();
            var ht = new List<string>();
            foreach (var post in mostPopularPosts)
            {
                var tags = post.Tag.Split(" "); ;
                foreach (var tg in tags)
                {
                    ht.Add(tg.ToString());
                }
            }
            ht = ht.OrderBy(r => Guid.NewGuid()).Take(6).ToList();

            // get Post for first page
            var t = 0;
            var s = "";
            var posts = new List<Post>();

            if (!string.IsNullOrEmpty(search))
            {
                posts = p.Where(x => x.Title.ToLower().Contains(search.ToLower()) || x.Content.ToLower().Contains(search.ToLower())).OrderByDescending(x => x.CreatedDate).ToList();
                t = 1;
                s = search;
            }
            else if (!string.IsNullOrEmpty(cat))
            {
                posts = p.Where(x => x.CategoryId == cat).OrderByDescending(x => x.CreatedDate).ToList();
                t = 2;
                s = cats.Where(x => x.Id == cat).FirstOrDefault().Name;
            }
            else if (!string.IsNullOrEmpty(tag))
            {
                posts = p.Where(x => x.Tag.ToLower().Contains(tag.ToLower())).OrderByDescending(x => x.CreatedDate).ToList();
                t = 3;
                s = tag;
            }
            else
            {
                posts = p.OrderByDescending(x => x.CreatedDate).ToList();
            }

            //get Last Posts
            var lp = p.OrderByDescending(x => x.CreatedDate).ToList().Take(4).ToList();

            //get Cat for Nav
            var navcat = new List<CategoryResponse>();
            foreach (var c in cats)
            {
                navcat.Add(new CategoryResponse() { 
                    Id = c.Id,
                    Name = c.Name,
                    PostCount = p.Where(x => x.CategoryId == c.Id).Count()
                });
            }

            return new HomePageViewModel
            {
                Categories = cats,
                Tags = ht,
                Posts = await ConvertToPostResponse(posts.Skip(skip).Take(take).ToList()),
                LastPosts = lp,
                CategoriesNav = navcat,
                Type = t,
                String = s,
                MaxPage = (posts.Count() / 4) + (posts.Count() % 4 >= 1 ? 1 : 0)
            };
        }

        public async Task<BlogSingleViewModel> GetSinglePost(string link)
        {
            var p = await _unitOfWork.GetRepository<Post>().GetAll().Where(x => x.Link == link).FirstOrDefaultAsync();
            var vm = _mapper.Map<BlogSingleViewModel>(p);

            return vm;
        }

        #region Helper
        private async Task<List<PostResponse>> ConvertToPostResponse(List<Post> posts)
        {
            var postR = new List<PostResponse>();
            var cats = await _unitOfWork.GetRepository<Category>().GetAll().ToListAsync();
            var com = await _unitOfWork.GetRepository<Comment>().GetAll().ToListAsync();
            foreach (var post in posts)
            {
                var pr = _mapper.Map<PostResponse>(post);
                pr.Category = cats.Find(x => x.Id == post.CategoryId).Name;
                pr.CategoryColor = cats.Find(x => x.Id == post.CategoryId).Color;
                pr.Tags = post.Tag.Split(" ").ToList();
                pr.CommentCount = com.Where(x => x.PostId == post.Id).Count();
                postR.Add(pr);
            }
            return postR;
        }

        private int CalculateReadMinutes(string content)
        {
            int length = content.Length;
            return length / 4 / 175;
        }

        private async Task<string> GenerateLinkAsync(string Title)
        {
            var unsign = convertToUnSign3(Title);
            var snakeCase = RemoveSpecialCharacters(unsign.ToLower().Replace(" ", "_"));

            if (snakeCase.Length > 250)
            {
                snakeCase = snakeCase.Substring(0, 248);
            }
            var link = await _unitOfWork.GetRepository<Post>().FindAsync(x => x.Link == snakeCase);

            if (link == null)
            {
                return snakeCase;
            }

            return snakeCase + "_2";
        }

        public static string convertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'z') || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        #endregion
    }
}
