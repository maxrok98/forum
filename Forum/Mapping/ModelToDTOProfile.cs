using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Forum.DTOout;
using Forum.Models;

namespace Forum.Mapping
{
    public class ModelToDTOProfile : Profile
    {
        public ModelToDTOProfile()
        {
            //CreateMap<Thread, ThreadDTO>().ForMember(
            //    dest => dest.Image, opts => opts.MapFrom(
            //        src => src.Image.Image ?? new byte[0])
            //    );
            CreateMap<Thread, ThreadDTOout>().ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image.Image));
            CreateMap<Post, PostDTOout>().ForMember(dest => dest.ThreadName, opt => opt.MapFrom(src => src.Thread.Name))
                                         .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                                         .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image.Image));
                                                                                  
            CreateMap<Coment, ComentDTOout>().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<Post, PostForUserDTOout>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<Subscription, SubForUserDTOout>();
            CreateMap<User, UserDTOout>().ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image.Image))
                                         .ForMember(dest => dest.myPosts, opt => opt.MapFrom(src => src.Posts))
                                         .ForMember(dest => dest.myThread, opt => opt.MapFrom(src => src.Subscriptions.Select(y => y.Thread)));
            //CreateMap<ThreadImage, ThreadDTO>().ForMember(x => x.Id, opt => opt.Ignore());
            //CreateMap<Post, PostDTO>();
        }
    }
}
