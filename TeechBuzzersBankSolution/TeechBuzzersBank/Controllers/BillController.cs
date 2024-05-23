using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Interface;
using Techbuzzers_bank.Models;
using Techbuzzers_bank.Repository;
using TeechBuzzersBank.Interface;
using TeechBuzzersBank.Models;
using TeechBuzzersBank.Repository;

namespace TeechBuzzersBank.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        /*
         
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            var userId = _user.getIdFromToken(token);
            UserDetails user = _user.GetUserDetails(userId);
            if(user == null)
            {
                return Unauthorized("Session Expired, Try Signing in again!");
            }
         */
        private readonly ApplicationDbContext _db;
        private readonly IBills _bill;
        private readonly IAccount _account;
        private readonly IUsers _user;

       

        public BillController( ApplicationDbContext db)
        {
            _db = db;
            _bill = new BillRepository(db);
            _account = new AccountRepository(db);
            _user = new UserRepository(db);
            
        }

        [HttpGet("/[Action]")]
        public IActionResult getBillDetails()
        {
            try
            {

                return Ok(_bill.getBillDetails());

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public class BillPaymnet
        {
            public string billDetailsId { get; set; }
            public string billType { get; set; }
            public string billNumber { get; set; }
            public float amount { get; set; }
            public int pin { get; set; }
            public string UseraccountId { get; set; }

        }

        [HttpPost("/[Action]")]
        public IActionResult payBill([FromBody]  BillPaymnet bill)
        {


            try
            {

                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
                var userId = _user.getIdFromToken(token);
                UserDetails user = _user.GetUserDetails(userId);
                if (user == null)
                {
                    return Unauthorized("Session Expired, Try Signing in again!");
                }
                if (user.Pin != bill.pin)
                {
                    return Unauthorized("Invalid Pin");
                }

                Bill b = new Bill();
                b.Id = "BLN" + _bill.GenerateUniqueBillId();
                b.billDetailsId = bill.billDetailsId;
                b.billType = bill.billType;
                b.billNumber = bill.billNumber;
                b.amount = bill.amount;

                Account userAccount = _account.GetAccount(bill.UseraccountId);
                if (userAccount == null)
                {
                    return BadRequest("User Account Not Found!");

                }
                b = _bill.payBill(b, userAccount);
                if (user.bills == null)
                {
                    user.bills = new List<Bill>();
                }
                user.bills.Add(b);
                userAccount.Transactions.Add(b.transaction.Id);
                _db.SaveChanges();
                return Ok(b);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/[Action]")]
        public IActionResult getRecentBillPayments()
        {

            try
            {
                
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            var userId = _user.getIdFromToken(token);
            UserDetails user = _user.GetUserDetails(userId);
            if (user == null)
            {
                return Unauthorized("Session Expired, Try Signing in again!");
            }


            List<Bill> bills = _bill.getRecentBillPayments(userId);
            return Ok( bills);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
