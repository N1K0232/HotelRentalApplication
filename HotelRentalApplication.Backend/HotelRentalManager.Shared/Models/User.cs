﻿using HotelRentalManager.Shared.Common;

namespace HotelRentalManager.Shared.Models;

public class User : BaseObject
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public int Age { get; set; }

    public string Gender { get; set; }

    public string City { get; set; }

    public string Country { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public string UserName { get; set; }
}