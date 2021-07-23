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
            CreateMap<ThreadRequest, Thread>().ForMember(dest => dest.ImageLink, opts => opts.MapFrom(src => Convert.ToBase64String(src.Image)));
            CreateMap<PostRequest, Post>().ForMember(dest => dest.ImageLink, opts => opts.MapFrom(src => Convert.ToBase64String(src.Image)));
            CreateMap<PostRequest, Event>().ForMember(dest => dest.ImageLink, opts => opts.MapFrom(src => Convert.ToBase64String(src.Image)))
                                            .ForMember(dest => dest.DateOfEvent, opts => opts.MapFrom(src => src.DateOfEvent));
            CreateMap<PostRequest, Place>().ForMember(dest => dest.ImageLink, opts => opts.MapFrom(src => Convert.ToBase64String(src.Image)));
            CreateMap<ComentRequest, Coment>();
            CreateMap<PaginationQuery, PaginationFilter>();
            CreateMap<ChatRequest, Chat>().ForMember(dest => dest.Users, opts => opts.MapFrom(src => src.Users));
            CreateMap<UserRequest, UserChat>();
        }
    }
}
