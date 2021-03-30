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
        Task<HomePageViewModel> GetHomePagePosts(string cat, string tag, string search, string username, int page);
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

        public async Task<HomePageViewModel> GetHomePagePosts(string cat, string tag, string search, string username, int page)
        {
            var take = 4;
            var skip = take * (page - 1);
            var p = await _unitOfWork.GetRepository<Post>().GetAll().OrderByDescending(x => x.CreatedDate).ToListAsync();

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
            var user = new User();

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
            else if (!string.IsNullOrEmpty(username))
            {
                user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(x => x.Username == username);
                posts = p.Where(x => x.AuthorId == user.Id).ToList();
                t = 4;
                s = username;
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
                User = user,
                Type = t,
                String = s,
                MaxPage = (posts.Count() / 4) + (posts.Count() % 4 >= 1 ? 1 : 0)
            };
        }

        public async Task<BlogSingleViewModel> GetSinglePost(string link)
        {
            var p = await _unitOfWork.GetRepository<Post>().GetAll().OrderBy(x => x.CreatedDate).ToListAsync();
            var p2 = p.OrderByDescending(x => x.CreatedDate).ToList();
            var users = await _unitOfWork.GetRepository<User>().GetAll().ToListAsync();

            // get Single Post
            var sp = p.First(x => x.Link == link);
            var vm = _mapper.Map<BlogSingleViewModel>(sp);

            // get author
            var user = users.Find(x => x.Id == sp.AuthorId);

            // get Comments
            var comments = await _unitOfWork.GetRepository<Comment>().GetAll().Where(x => x.PostId == sp.Id).OrderByDescending(x => x.CreatedDate).ToListAsync();
            var commentR = new List<CommentResponse>();
            foreach (var comment in comments)
            {
                var cmt = _mapper.Map<CommentResponse>(comment);
                if (comment.UserId != null && comment.UserId != Guid.Empty)
                {
                    var us = users.Find(x => x.Id == comment.UserId);
                    cmt.Username = us.Username;
                    cmt.Avatar = us.Avatar;
                    cmt.Name = us.Name;
                }
                else
                {
                    cmt.Avatar = "/images/team/8.jpg";
                }
                commentR.Add(cmt);
            }

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

            // get Last Posts
            var lp = p.OrderByDescending(x => x.CreatedDate).ToList().Take(4).ToList();

            // get Cat for Nav
            var navcat = new List<CategoryResponse>();
            foreach (var c in cats)
            {
                navcat.Add(new CategoryResponse()
                {
                    Id = c.Id,
                    Name = c.Name,
                    PostCount = p.Where(x => x.CategoryId == c.Id).Count()
                });
            }

            // get next and prev post
            var prev = p.Find(x => x.CreatedDate > vm.CreatedDate);
            var next = p2.Find(x => x.CreatedDate < vm.CreatedDate);

            var list = new List<Post>();
            list.Add(prev);
            list.Add(next);

            vm.PrevNextPosts = list;

            vm.CategoryName = cats.Find(x => x.Id == vm.CategoryId).Name;
            vm.AuthorName = user.Name;
            vm.AuthorUsername = user.Username;
            vm.AuthorAvatar = user.Avatar;
            vm.AuthorDescription = user.Description;
            vm.Comments = commentR;
            vm.Tags = ht;
            vm.LastPosts = lp;
            vm.Categories = cats;
            vm.CategoriesNav = navcat;

            sp.Views += 1;
            await _unitOfWork.CommitAsync();

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
