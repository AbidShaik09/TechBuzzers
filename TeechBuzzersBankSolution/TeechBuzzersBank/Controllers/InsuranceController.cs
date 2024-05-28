using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
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
    public class InsuranceController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IAccount _account;
        private readonly IInsurance _insurance;
        private readonly IInsurancePayables _payables;
        private readonly IUsers _user;
        public InsuranceController(ApplicationDbContext db)
        {

            _db=db;
            _account = new AccountRepository(db);
            _insurance = new InsuranceRepository(db);
            _payables = new InsurancePayablesRepository(db);
            _user = new UserRepository(db);
        }


        [HttpGet("/[Action]")]
        public IActionResult getInsurancePolicies()
        {

            try
            {

                return Ok(_insurance.getInsurancePolicies() );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        public class getUserInsuranceResponse
        {

            public string insuranceType { get; set; }
            public Insurance Insurance {  get; set; }
        }
        [HttpGet("/[Action]")]
        public IActionResult getUserInsurances()
        {
            try
            {

                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
                var userId = _user.getIdFromToken(token);
                UserDetails user = _user.GetUserDetails(userId);
                if (user == null)
                {
                    return BadRequest("Invalid User, Login Again");
                }
                List<Insurance> li = _insurance.GetUserInsurances(userId);
                List<getUserInsuranceResponse> response = new List<getUserInsuranceResponse>();
                foreach(Insurance i in li)
                {
                    getUserInsuranceResponse gr = new getUserInsuranceResponse();
                    gr.Insurance = i;
                    gr.insuranceType= _insurance.getInsurancePolicy(i.insurancePolicyId).InsuranceType;
                    response.Add(gr);
                }

                return Ok(response);

            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        public class AmountCoverageCalculationForm {

            public string insurancePolicyId { get; set; }
            public string UniqueIdentificationNumber { get; set; }
            public int yearOfPurchase { get; set; }
            public double purchaseAmount { get; set; }


        }

        [HttpPost("/[Action]")]
        public IActionResult calculateAmountCovered([FromBody] AmountCoverageCalculationForm acf )
        {

            try
            {

                Insurance insurance = new Insurance();
                insurance.insurancePolicyId=acf.insurancePolicyId;
                insurance.UniqueIdentificationNumber=acf.UniqueIdentificationNumber;
                insurance.yearOfPurchase = acf.yearOfPurchase;
                insurance.purchaseAmount = acf.purchaseAmount;  
                 
                return Ok(_insurance.calculateAmountCovered(insurance));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public class InsuranceApplicationDetails
        {
            
            public string insurancePolicyId { get; set; }
            public string UniqueIdentificationNumber { get; set; }
            public int yearOfPurchase { get; set; }
            public double purchaseAmount { get; set; }
            public string userAccountId { get; set; }
            public int pin {  get; set; }   

        }
        [HttpPost("/[Action]")]
        public IActionResult applyInsuranceAndPayOneInstallment([FromBody] InsuranceApplicationDetails iad)
        {
            try
            {

                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
                var userId = _user.getIdFromToken(token);
                UserDetails user = _user.GetUserDetails(userId);
                if (user == null)
                {
                    return BadRequest("Invalid User, Login Again");
                }
                if (iad.pin != user.Pin)
                {
                    return BadRequest("Invalid Credentials");
                }
                Insurance insurance = new Insurance();
                insurance.insurancePolicyId = iad.insurancePolicyId;
                insurance.UniqueIdentificationNumber = iad.UniqueIdentificationNumber;
                insurance.yearOfPurchase = iad.yearOfPurchase;
                insurance.purchaseAmount = iad.purchaseAmount;
               
                Account account = _account.GetAccount(iad.userAccountId);
                if (account == null)
                {
                    return BadRequest("Account Not Found");
                }
                insurance = _insurance.ApplyInsurance(insurance, account);
                if (user.insurances == null)
                {
                    user.insurances=new List<Insurance>();
                }
                user.insurances.Add(insurance);
                _db.SaveChanges();
                return Ok("Insurance Applied");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public class ClaimInsuranceDetails
        {
            public string insuranceId { get; set; }
            public string userAccountId { get; set; }
            public int pin { get; set; }
        }
        [HttpPost("/[Action]")]
        public IActionResult claimInsurance(ClaimInsuranceDetails cad)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
                var userId = _user.getIdFromToken(token);
                UserDetails user = _user.GetUserDetails(userId);
                if (user == null)
                {
                    return BadRequest("Invalid User, Login Again");
                }
                if (user.Pin != cad.pin)
                {
                    return BadRequest("Invalid credentials!");
                }
                Insurance insurance = _insurance.GetInsurance(cad.insuranceId);
                if (insurance == null)
                {
                    return BadRequest("Insurance Not Found!");
                }
                else if (insurance.claimed)
                {
                    return BadRequest("Insurance Already Claimed");
                }
                Account userAccount = _account.GetAccount(cad.userAccountId);
                if (userAccount == null)
                {
                    return BadRequest("Account Not Found!");
                }
                if (userAccount.UserId != userId)
                {
                    return BadRequest("Account Doesn't belong to the user!");
                }


                Transactions transactions = _insurance.claimInsurance(cad.insuranceId, userAccount);
                return Ok(transactions);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("/[Action]")]
        public IActionResult getUpcomingInsuranceInstallments()
        {
            try
            {

                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
                var userId = _user.getIdFromToken(token);
                UserDetails user = _user.GetUserDetails(userId);
                if (user == null)
                {
                    return BadRequest("Invalid User, Login Again");
                }
                List<InsurancePayables> lp = _payables.getUpcomingPayables(userId);
                return Ok(lp);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class PayInstallmentForm
        {
            public string installmentId { get; set; }
            public string accountId { get; set; }
            public int pin {  get; set; }    
        }

        [HttpPost("/[Action]")]
        public IActionResult payInstallment([FromBody] PayInstallmentForm pif)
        {


            try
            {

                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
                var userId = _user.getIdFromToken(token);
                UserDetails user = _user.GetUserDetails(userId);
                if (user == null)
                {
                    return BadRequest("Invalid User, Login Again");
                }

                InsurancePayables payable = _payables.getInsurancePayable(pif.installmentId);
                if (payable == null)
                {
                    return BadRequest("Installment Not Found!");



                }
                if (payable.Status.Equals("Paid") || payable.Status.Equals("Claimed"))
                {
                    return BadRequest("Installment is already Paid");
                }
                Account account = _account.GetAccount(pif.accountId);
                if (account == null)
                {
                    return BadRequest("Account Not Found!");


                }
                UserDetails ud = _user.GetUserDetails(userId);
                if (ud.Pin != pif.pin)
                {
                    return BadRequest("Invalid Credentials");
                }

                Transactions t = _payables.payInstallment(payable, account);

                return Ok(t);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
