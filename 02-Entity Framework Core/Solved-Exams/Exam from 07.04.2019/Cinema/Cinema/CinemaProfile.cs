﻿using AutoMapper;
using Cinema.Data.Models;
using Cinema.Data.Models.Enums;
using Cinema.DataProcessor.ImportDto;
using System;
using System.Globalization;

namespace Cinema
{
    public class CinemaProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public CinemaProfile()
        {
            this.CreateMap<ImportMoviesDto, Movie>()
                .ForMember(x => x.Genre, y => y.MapFrom(x => Enum.Parse(typeof(Genre), x.Genre)));

            this.CreateMap<ImportHallsDto, Hall>();

            this.CreateMap<ImportProjectionDto, Projection>()
                .ForMember(x => x.DateTime,
                           y => y.MapFrom(
                                    x => DateTime.ParseExact(x.DateTime,
                                                 @"yyyy-MM-dd HH:mm:ss",
                                                 CultureInfo.InvariantCulture)));

            this.CreateMap<ImportTicketDto, Ticket>();

            this.CreateMap<ImportCustomerDto, Customer>();
        }
    }
}
