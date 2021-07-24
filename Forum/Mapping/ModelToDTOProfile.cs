using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Forum.DAL.Models;
using Forum.Shared.Contracts;
using Forum.Shared.Contracts.Responses;

namespace Forum.BLL.Mapping
{
    public class ModelToDTOProfile : Profile
    {
        public ModelToDTOProfile()
        {
            CreateMap<Thread, ThreadResponse>().ForMember(dest => dest.NumberOfSubscription, opt => opt.MapFrom(src => src.Subscriptions.Count()))
                                               .ForMember(dest => dest.NumberOfPost, opt => opt.MapFrom(src => src.Posts.Count()));
            CreateMap<Post, PostResponse>().ForMember(dest => dest.ThreadName, opt => opt.MapFrom(src => src.Thread.Name))
                                         .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                                         .ForMember(dest => dest.PostType, opt => opt.MapFrom(src =>
                                             src.GetType() == typeof(Event) ? PostType.Event : PostType.Place
                                         ))
                                         .ForMember(dest => dest.DateOfEvent, opt => opt.MapFrom(src => src.GetType() == typeof(Event) ? ((Event)src).DateOfEvent : DateTime.MinValue));

            CreateMap<Coment, ComentResponse>().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                                                .ForMember(dest => dest.ImageLink, opt => opt.MapFrom(src => src.User.ImageLink));
            CreateMap<Post, PostForUserResponse>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                                .ForMember(dest => dest.ComentsAmount, opt => opt.MapFrom(src => src.Coments.Count()))
                                                .ForMember(dest => dest.VotesAmount, opt => opt.MapFrom(src => src.Votes.Count()))
                                                .ForMember(dest => dest.DateOfEvent, opt => opt.MapFrom(src => src.GetType() == typeof(Event) ? ((Event)src).DateOfEvent : DateTime.MinValue));

            CreateMap<User, UserResponse>().ForMember(dest => dest.myPosts, opt => opt.MapFrom(src => src.Posts))
                                         .ForMember(dest => dest.Subscription, opt => opt.MapFrom(src => src.Subscriptions.Select(y => y.Thread)))
                                         .ForMember(dest => dest.Votes, opt => opt.MapFrom(src => src.Votes.Select(y => y.Post)))
                                         .ForMember(dest => dest.Calendar, opt => opt.MapFrom(src => src.Calendar.Select(y => (Post)y.Event)));

            CreateMap<User, UserShortResponse>().ForMember(dest => dest.myPostsAmount, opt => opt.MapFrom(src => src.Posts.Count))
                                         .ForMember(dest => dest.SubscriptionAmount, opt => opt.MapFrom(src => src.Subscriptions.Count))
                                         .ForMember(dest => dest.VotesAmount, opt => opt.MapFrom(src => src.Votes.Count));
            CreateMap<Chat, ChatShortResponse>().ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users.Select(x => x.User).ToList()))
                                                .ForMember(dest => dest.LastMessage, opt => opt.MapFrom(src => src.Messages.FirstOrDefault()));

            CreateMap<Chat, ChatResponse>().ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users.Select(x => x.User).ToList()))
                                            .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages));

            CreateMap<Message, MessageResponse>(); //.ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
        }
    }
}
