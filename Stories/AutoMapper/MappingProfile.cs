using AutoMapper;
using Stories.Models;
using Stories.VM;
using Stories.VM.Request;
using Stories.VM.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreatePostRequest, Post>();
            CreateMap<Post, PostResponse>();
            CreateMap<Post, BlogSingleViewModel>();
            CreateMap<Category, HotTopic>();
            CreateMap<RegisterAccountRequest, User>();
            CreateMap<Comment, CommentResponse>();
            CreateMap<CreateCommentRequest, Comment>();
        }
    }
}
