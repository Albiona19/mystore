﻿using Microsoft.EntityFrameworkCore;

namespace mystore.Services
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{

		}

	}
}