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
            CreateMap<Thread, ThreadDTOout>();
            CreateMap<Post, PostDTOout>().ForMember(dest => dest.ThreadName, opt => opt.MapFrom(src => src.Thread.Name))
                                         .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<Coment, ComentDTOout>();
            //CreateMap<ThreadImage, ThreadDTO>().ForMember(x => x.Id, opt => opt.Ignore());
            //CreateMap<Post, PostDTO>();
        }
    }
}
