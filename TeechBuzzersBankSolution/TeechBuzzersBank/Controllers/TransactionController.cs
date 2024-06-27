using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Interface;
using Techbuzzers_bank.Models;
using Techbuzzers_bank.Repository;
using TeechBuzzersBank.Models;
using TeechBuzzersBank.Repository;

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
        private TransactionRepository _transaction;

        public TransactionController(ApplicationDbContext db)
        {
            _db = db;
            _user = new UserRepository(db);
            _account = new AccountRepository(db);
            _transaction = new TransactionRepository(db);

        }
        [HttpGet("/[Action]")]
        public IActionResult SearchUser(long phone)
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
                PublicUserDetails publicUserDetails = _user.getPublicDetails(user, phone);
                return Ok(publicUserDetails);

            }
            catch (Exception ex)
            {
                return StatusCode(404, ex.Message);
            }
        }


        [HttpPost("/[Action]")]
        public IActionResult Transfer([FromBody] TransferDetaiils transferDetaiils)
        {

            try
            {

                if (transferDetaiils.amount <= 0)
                {
                    return BadRequest("Amount must always be greater than 0!");
                }
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

                var userId = _user.getIdFromToken(token);

                UserDetails user = _user.GetUserDetails(userId);

                if (user.Pin != transferDetaiils.pin)
                {
                    return BadRequest("Invalid Credentials!");
                }

                PublicUserDetails otherUser = _user.getPublicDetails(user, transferDetaiils.receiverPhoneNumber);


                string receiverPrimaryAccountId = otherUser.primaryAccountId;
                if (!_account.CheckAccount(transferDetaiils.senderAccountId) || !_account.CheckAccount(receiverPrimaryAccountId))
                {
                    return BadRequest("Invalid Sender/Receiver Account IDs");
                }
                if(transferDetaiils.senderAccountId.Equals(receiverPrimaryAccountId))
                {
                    return BadRequest("Cannot transfer money to the same account");

                }
                Account sender = _account.GetAccount(transferDetaiils.senderAccountId);
                Account receiver = _account.GetAccount(receiverPrimaryAccountId);

                if (sender.UserId != userId)
                {
                    return BadRequest("Sender Account doesnot belong to user! ");
                }
                Transactions t = _transaction.transfer(sender, receiver, transferDetaiils.amount);

                return Ok(t);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }



        }

        [HttpGet("/[Action]")]
        public IActionResult GetCommonTransactions(long oppositeUserPhoneNumber)
        {
            try
            {


                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

                var userId = _user.getIdFromToken(token);

                UserDetails currentUser = _user.GetUserDetails(userId);
                if (currentUser == null)
                {
                    return NotFound();
                }

                string oppositeUserId = _user.getPublicDetails(currentUser, oppositeUserPhoneNumber).userId;


                UserDetails user = _user.GetUserDetails(userId);
                if (user == null)
                {
                    return NotFound();
                }
                List<Transactions> transactions = _transaction.getTransactions(userId, oppositeUserId);

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("/[Action]")]
        public IActionResult GetTransactionStatus(string transactionId) {
            try
            {

                var res = _transaction.getTransactionStatus(transactionId);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            }
        [HttpGet("/[Action]")]
        public IActionResult GetTransactions()
        {

            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

                var userId = _user.getIdFromToken(token);

                UserDetails currentUser = _user.GetUserDetails(userId);
                if (currentUser == null)
                {
                    return NotFound();
                }




                List<Transactions> t = _transaction.getTransactions(currentUser.Id);
                SortTransactions s = new SortTransactions();
                t.Sort(s);
                return Ok(t);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/[Action]")]
        public IActionResult SelfTransfer([FromBody] SelfTransferDetaiils selftransferDetaiils)
        {

            try
            {

                if (selftransferDetaiils == null)
                {
                    return BadRequest("body is null!");
                }

                if (selftransferDetaiils.senderAccountId == selftransferDetaiils.receiverAccountId)
                {
                    return BadRequest("Cannot transfer money to the same account!");
                }

                if (!_account.CheckAccount(selftransferDetaiils.senderAccountId) || !_account.CheckAccount(selftransferDetaiils.receiverAccountId))
                {
                    return BadRequest("Invalid Account Id!");
                }

                Account sender = _account.GetAccount(selftransferDetaiils.senderAccountId);
                Account receiver = _account.GetAccount(selftransferDetaiils.receiverAccountId);
                Transactions t = _transaction.transfer(sender, receiver, selftransferDetaiils.amount,"Self Transfer");
                return Ok(t);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        public class TransferDetaiils
        {
            public float amount { get; set; }
            public string senderAccountId { get; set; }
            public long receiverPhoneNumber { get; set; }

            public int pin {  get; set; }   

        }

        public class SelfTransferDetaiils
        {
            public float amount { get; set; }
            public string senderAccountId { get; set; }
            public string receiverAccountId { get; set; }

        }


        private class SortTransactions : IComparer<Transactions>
        {
            int IComparer<Transactions>.Compare(Transactions a , Transactions b)
            {
                if(a.Timestamp< b.Timestamp) return 1;
                if(a.Timestamp>b.Timestamp) return -1;  
                return 0;
            }
        }

    }
}
