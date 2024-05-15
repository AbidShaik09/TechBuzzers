﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Interface;
using Techbuzzers_bank.Models;
using Techbuzzers_bank.Repository;
using TeechBuzzersBank.Models;
using static Techbuzzers_bank.Controllers.UserController;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Techbuzzers_bank.Controllers
{
    [Route("/[controller]/")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private UserRepository _user;
        private AccountRepository _account;
        public UserController( ApplicationDbContext db)
        {
            _db = db;
            _user = new UserRepository(db);
            _account = new AccountRepository(db);

        }

        [HttpGet("/[Action]")]
        public IActionResult GetUserDetails()
        {
            try
            {
                // Retrieve the JWT token from the HTTP request headers
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

                // Parse the JWT token to extract the claims
              
                var userId = _user.getIdFromToken(token);

                UserDetails user = _user.GetUserDetails(userId);
                if (user == null)
                {
                    return NotFound();
                }
                AllUserDetails allUserDetails = new AllUserDetails(user);
                allUserDetails.accounts = _account.GetAllAccounts(userId);
                allUserDetails.primaryAccountId = user.PrimaryAccountId;
                return Ok(allUserDetails);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [HttpPut("/[Action]")]
        public async Task<IActionResult> UpdateUser ([FromBody] UserDetails newUserDetails)
        {
            
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            string userId = _user.getIdFromToken(token);
            UserDetails oldDetails = _user.GetUserDetails(userId);
            
            if (newUserDetails == null)
            {
                return BadRequest("Essential User Details are missing");

            }

            

            if(!_user.CheckUser(newUserDetails.PhoneNumber))
                oldDetails.PhoneNumber = newUserDetails.PhoneNumber;
            oldDetails.Address = newUserDetails.Address;
            oldDetails.Gender= newUserDetails.Gender;
            oldDetails.FirstName= newUserDetails.FirstName;
            oldDetails.LastName = newUserDetails.LastName;
            _db.SaveChanges();
            return Ok(oldDetails);
        
        }


    }
}
