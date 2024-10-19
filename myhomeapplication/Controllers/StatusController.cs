using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myhomeapplication.Data;
using myhomeapplication.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace myhomeapplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly string _connectionString;

        public StatusController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("FullConnectionstring");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Status>>> GetStatus()
        {
            return await _context.Statuses.ToListAsync();
        }
        //[HttpGet("get-bookings-with-status")]
        //public async Task<IActionResult> GetBookingsWithStatus()
        //{
        //    var bookingsTable = new DataTable();

        //    // Use the existing DbContext to execute the stored procedure
        //    await _context.Database.OpenConnectionAsync();

        //    try
        //    {
        //        using (var command = _context.Database.GetDbConnection().CreateCommand())
        //        {
        //            command.CommandText = "GetBookingsWithStatus";
        //            command.CommandType = CommandType.StoredProcedure;

        //            using (var reader = await command.ExecuteReaderAsync())
        //            {
        //                bookingsTable.Load(reader);
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        await _context.Database.CloseConnectionAsync();
        //    }

        //    return Ok(bookingsTable);
        //}
        [HttpGet("GetBookingWithStatus/{id}")]
        public async Task<IActionResult> GetBookingsWithStatus(int id)
        {
            var bookingsTable = new DataTable();
            var bookingsList = new List<Dictionary<string, object>>();

            // Open a connection to the database
            await using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(@"
        SELECT 
            b.""Id"", 
            b.""User"", 
            b.""AppointmentDate"", 
            b.""AppointmentTime"", 
            b.""Address1"", 
            b.""Address2"", 
            b.""City"", 
            b.""State"", 
            b.""Pincode"", 
            b.""Services"", 
            b.""SendPaymentLink"", 
            b.""Total"", 
            s.""Name"" AS ""StatusName""
        FROM 
            public.""Bookings"" b
        JOIN 
            public.""Statuses"" s
        ON 
            b.""StatusID"" = s.""Id""
        WHERE 
            b.""Id"" = @BookingId;", conn))
                {
                    // Add parameter to avoid SQL injection
                    cmd.Parameters.AddWithValue("@BookingId", id);

                    // Execute the query and load the results into the DataTable
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        bookingsTable.Load(reader);
                    }
                }
                await conn.CloseAsync();
            }        
            foreach (DataRow row in bookingsTable.Rows)
            {
                var rowDict = new Dictionary<string, object>();
                foreach (DataColumn col in bookingsTable.Columns)
                {
                    rowDict[col.ColumnName] = row[col];
                }
                bookingsList.Add(rowDict);
            }

            return Ok(bookingsList);
        }


    }
}
