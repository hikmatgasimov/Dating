﻿using DatingApi.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DatingApi.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context )
        {
            if (await context.AppUser.AnyAsync()) return;
            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            foreach (var user in users)
            {
                using var hmc = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmc.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmc.Key;

                context.AppUser.Add(user);
            }
            await context.SaveChangesAsync();
        }
    }
}
