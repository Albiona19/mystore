﻿
using Microsoft.AspNetCore.Identity;
namespace mystore.Models
	{
		public class ApplicationUser : IdentityUser
		{


    public string FirstName { get; set; } = "";
			public string LastName { get; set; } = "";
			public string Address { get; set; } = "";
			public DateTime CreatedAt { get; set; }

		}
	}
