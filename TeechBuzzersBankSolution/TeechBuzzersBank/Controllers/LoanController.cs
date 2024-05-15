using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Repository;
using TeechBuzzersBank.Repository;

namespace TeechBuzzersBank.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {

        private readonly ApplicationDbContext _db;
        private UserRepository _user;
        private AccountRepository _account;
        private LoanRepository _loan;
        public LoanController(ApplicationDbContext db)
        {
            _db = db;
            _user = new UserRepository(db);
            _account = new AccountRepository(db);

        }

        [HttpGet]
        public IActionResult GetLoanDetails()
        {
            try
            {

                return Ok(_loan.getLoanDetails());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
    }
}
