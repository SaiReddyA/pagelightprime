using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ATMStore_KEKA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ATMController : ControllerBase
    {

        [HttpPost("AddCash")]
        public IActionResult AddCash([FromBody] AddCashRequest request)
        {
            int totalAdded = 0;
            foreach (var note in request.Notes)
            {
                if (ATMStore.CashNotes.ContainsKey(note.Key))
                {
                    ATMStore.CashNotes[note.Key] += note.Value;
                    totalAdded += note.Key * note.Value;
                }
            }

            ATMStore.Transactions.Add(new Transaction
            {
                Date = DateTime.Now,
                Type = "Add",
                Amount = totalAdded,
                Details = string.Join(", ", request.Notes.Select(x => $"{x.Key}x{x.Value}"))
            });

            return Ok(new { message = "Cash Added Successfully", totalAdded });
        }

        [HttpPost("WithdrawCash")]
        public IActionResult WithdrawCash([FromBody] WithdrawRequest request)
        {
            int amount = request.Amount;
            var notesUsed = new Dictionary<int, int>();
            var sortedNotes = ATMStore.CashNotes.OrderByDescending(x => x.Key);

            foreach (var note in sortedNotes)
            {
                int noteValue = note.Key;
                int noteCount = note.Value;

                int needed = Math.Min(amount / noteValue, noteCount);
                if (needed > 0)
                {
                    notesUsed[noteValue] = needed;
                    amount -= needed * noteValue;
                }
            }

            if (amount > 0)
                return BadRequest("Insufficient denominations or cash to dispense this amount.");

            foreach (var note in notesUsed)
                ATMStore.CashNotes[note.Key] -= note.Value;

            ATMStore.Transactions.Add(new Transaction
            {
                Date = DateTime.Now,
                Type = "Withdraw",
                Amount = request.Amount,
                Details = string.Join(", ", notesUsed.Select(x => $"{x.Key}x{x.Value}"))
            });

            return Ok(new { message = "Cash Withdrawn Successfully", notesUsed });
        }

        [HttpGet("GetNotesSummary")]
        public IActionResult GetNotesSummary()
        {
            return Ok(ATMStore.CashNotes.Select(x => $"{x.Key}-{x.Value}"));
        }

        [HttpGet("GetTransactions")]
        public IActionResult GetTransactions()
        {
            return Ok(ATMStore.Transactions);
        }
    }
}
