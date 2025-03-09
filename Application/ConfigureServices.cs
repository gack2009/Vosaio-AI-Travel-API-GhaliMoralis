﻿using Application.Bookings;
using Application.Hotels;
using Application.Rooms;
using FluentValidation;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        //FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        //Add Services - there are some better ways to add services but i will go with this for now
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IHotelService, HotelService>();
        services.AddScoped<IRoomService, RoomService>();

        return services;
    }
}
