﻿using System.Security.Claims;
using System.Security.Principal;

namespace HotelRentalManager.Authentication.Extensions;

public static class ClaimsExtensions
{
    public static Guid GetId(this IPrincipal user)
    {
        string value = GetClaimValueInternal(user, ClaimTypes.NameIdentifier);
        if (Guid.TryParse(value, out Guid id))
        {
            return id;
        }

        return Guid.Empty;
    }

    public static string GetFirstName(this IPrincipal user)
    {
        return GetClaimValueInternal(user, ClaimTypes.GivenName);
    }

    public static string GetLastName(this IPrincipal user)
    {
        return GetClaimValueInternal(user, ClaimTypes.Surname);
    }

    public static DateTime GetDateOfBirth(this IPrincipal user)
    {
        string value = GetClaimValueInternal(user, ClaimTypes.DateOfBirth);
        if (DateTime.TryParse(value, out DateTime dateOfBirth))
        {
            return dateOfBirth;
        }

        return DateTime.MinValue;
    }

    public static int GetAge(this IPrincipal user)
    {
        string value = GetClaimValueInternal(user, CustomClaimTypes.Age);
        if (int.TryParse(value, out int age))
        {
            return age;
        }

        return 0;
    }

    public static string GetGender(this IPrincipal user)
    {
        return GetClaimValueInternal(user, ClaimTypes.Gender);
    }

    public static string GetCity(this IPrincipal user)
    {
        return GetClaimValueInternal(user, CustomClaimTypes.City);
    }

    public static string GetCountry(this IPrincipal user)
    {
        return GetClaimValueInternal(user, ClaimTypes.Country);
    }

    public static string GetEmail(this IPrincipal user)
    {
        return GetClaimValueInternal(user, ClaimTypes.Email);
    }

    public static string GetUserName(this IPrincipal user)
    {
        return GetClaimValueInternal(user, ClaimTypes.Name);
    }

    public static string GetPhoneNumber(this IPrincipal user)
    {
        return GetClaimValueInternal(user, ClaimTypes.MobilePhone);
    }

    public static string GetClaimValue(this IPrincipal user, string claimType)
    {
        return GetClaimValueInternal(user, claimType);
    }

    private static string GetClaimValueInternal(IPrincipal user, string claimType)
    {
        if (string.IsNullOrEmpty(claimType) || string.IsNullOrWhiteSpace(claimType))
        {
            throw new ArgumentNullException(nameof(claimType), "");
        }

        ClaimsPrincipal principal = (ClaimsPrincipal)user;
        Claim claim = principal.FindFirst(claimType);
        return claim?.Value;
    }
}