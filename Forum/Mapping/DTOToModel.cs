using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Forum.Models;
using Forum.Contracts;
using Forum.Contracts.Requests.Queries;
using Forum.Contracts.Requests;

namespace Forum.Mapping
{
    public class DTOToModel : Profile
    {
        public DTOToModel()
        {
            CreateMap<ThreadRequest, Thread>().ForMember(
                dest => dest.Image, opts => opts.MapFrom(
                    src => new ThreadImage { Image = src.Image })
                );
            CreateMap<PostRequest, Post>().ForMember(dest => dest.Image, opts => opts.MapFrom(src => new PostImage { Image = src.Image }));
            CreateMap<ComentRequest, Coment>();
            CreateMap<PaginationQuery, PaginationFilter>();
        }
    }
}
