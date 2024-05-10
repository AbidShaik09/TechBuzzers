using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Interface;
using Techbuzzers_bank.Models;
using Techbuzzers_bank.Repository;
using TeechBuzzersBank.Models;

namespace TeechBuzzersBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private UserRepository _user;
        private AccountRepository _account;

        public TransactionController(ApplicationDbContext db)
        {
            _db = db;
            _user = new UserRepository(db);
            _account = new AccountRepository(db);

        }
        [HttpGet("/[Action]")]
        public IActionResult GetPublicUserDetails(long phone)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

                var userId = _user.getIdFromToken(token);

                UserDetails user = _user.GetUserDetails(userId);
                if (user == null)
                {
                    return NotFound();
                }
                PublicUserDetails publicUserDetails = _user.getPublicDetails(user,phone);
                return Ok(publicUserDetails);

            }
            catch (Exception ex)
            {
                return StatusCode(404, ex.Message);
            }
        }
    }
}
