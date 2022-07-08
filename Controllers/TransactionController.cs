using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TransactionMicroservice.Models;
using TransactionMicroservice.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TransactionMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class TransactionController : ControllerBase
    {
       
        ITransactionRep db;
        public TransactionController(ITransactionRep _db)
        {
            db = _db;
        }
        [HttpPost]
        [Route("deposit")]
        public IActionResult deposit([FromBody]WithdrawDeposit value)
        {
            TokenInfo.StringToken = Request.Headers["Authorization"];
            try
            {
                var ob = db.deposit(value);
                if(ob==null)
                {
                    return NotFound();
                }
                return Ok(ob);
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("withdraw")]
        public IActionResult withdraw([FromBody] WithdrawDeposit value)
        {
            TokenInfo.StringToken = Request.Headers["Authorization"];
            try
            {
                var ob = db.withdraw(value);
                if (ob == null)
                {
                    return NotFound();
                }
                return Ok(ob);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("transfer")]
        public IActionResult transfer([FromBody] Transfers value)
        {
            TokenInfo.StringToken = Request.Headers["Authorization"];
            try
            {
                var ob = db.transfer(value);
                if (ob == null)
                {
                    return NotFound();
                }
                return Ok(ob);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
