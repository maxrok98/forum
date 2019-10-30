using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Forum.DTOin;
using Forum.Models;

namespace Forum.Mapping
{
    public class DTOToModel : Profile
    {
        public DTOToModel()
        {
            CreateMap<ThreadDTOin, Thread>().ForMember(
                dest => dest.Image, opts => opts.MapFrom(
                    src => new ThreadImage { Image = src.Image })
                );
            CreateMap<PostDTOin, Post>().ForMember(dest => dest.Image, opts => opts.MapFrom(src => new PostImage { Image = src.Image }));
            CreateMap<ComentDTOin, Coment>();
        }
    }
}
