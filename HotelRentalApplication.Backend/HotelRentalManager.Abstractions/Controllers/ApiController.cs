﻿using Microsoft.AspNetCore.Mvc;

namespace HotelRentalManager.Abstractions.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
	protected ApiController()
	{
	}
}