using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Forum.Models;
using Forum.Contracts.Responses;

namespace Forum.Mapping
{
    public class ModelToDTOProfile : Profile
    {
        public ModelToDTOProfile()
        {
            CreateMap<Thread, ThreadResponse>().ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image.Image))
                                               .ForMember(dest => dest.NumberOfSubscription, opt => opt.MapFrom(src => src.Subscriptions.Count()))
                                               .ForMember(dest => dest.NumberOfPost, opt => opt.MapFrom(src => src.Posts.Count()));
            CreateMap<Post, PostResponse>().ForMember(dest => dest.ThreadName, opt => opt.MapFrom(src => src.Thread.Name))
                                         .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                                         .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image.Image));
                                         //.ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString()));

            CreateMap<Coment, ComentResponse>().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                                                .ForMember(dest => dest.UserImage, opt => opt.MapFrom(src => src.User.Image.Image));
            CreateMap<Post, PostForUserResponse>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                                .ForMember(dest => dest.ComentsAmount, opt => opt.MapFrom(src => src.Coments.Count()))
                                                .ForMember(dest => dest.VotesAmount, opt => opt.MapFrom(src => src.Votes.Count()));

            //CreateMap<Subscription, SubForUserResponse>();
            CreateMap<User, UserResponse>().ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image.Image))
                                         .ForMember(dest => dest.myPosts, opt => opt.MapFrom(src => src.Posts))
                                         .ForMember(dest => dest.Subscription, opt => opt.MapFrom(src => src.Subscriptions.Select(y => y.Thread)))
                                         .ForMember(dest => dest.Votes, opt => opt.MapFrom(src => src.Votes.Select(y => y.Post)));

            CreateMap<User, UserShortResponse>().ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image.Image))
                                         .ForMember(dest => dest.myPostsAmount, opt => opt.MapFrom(src => src.Posts.Count))
                                         .ForMember(dest => dest.SubscriptionAmount, opt => opt.MapFrom(src => src.Subscriptions.Count))
                                         .ForMember(dest => dest.VotesAmount, opt => opt.MapFrom(src => src.Votes.Count));
        }
    }
}
