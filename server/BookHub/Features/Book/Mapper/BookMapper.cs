﻿namespace BookHub.Features.Book.Mapper
{
    using AutoMapper;
    using Data.Models;
    using Service.Models;
    using Web.Models;

    public class BookMapper : Profile
    {
        public BookMapper()
        {
            this.CreateMap<CreateBookWebModel, CreateBookServiceModel>();

            this.CreateMap<CreateBookServiceModel, Book>()
                .ForMember(
                    dest => dest.PublishedDate,
                    opt => opt.MapFrom(
                        src => MapperHelper.ParseDateTime(src.PublishedDate)));
        }
    }
}
