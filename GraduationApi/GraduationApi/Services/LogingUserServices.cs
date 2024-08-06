using GraduationApi.Interfaces;
using GraduationApi.Models;
using GraduationApi.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationApi.Services
{
    public class LogingUserServices : ILogingUserServices 
    {

        private readonly ApplicationDbContext _context;

        public LogingUserServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddUser(LogingUser User)
        {
            await _context.LogingUsers.AddAsync(User);
            _context.SaveChanges();
            return "a new user added successfully";
        }

        public string DeleteUser(LogingUser User)
        {
            _context.LogingUsers.Remove(User);
            _context.SaveChanges();
            return "An existing user deleted successfully";
        }

        public async Task<IEnumerable<LogingUser>> GetAllUsers()
        {
            return await _context.LogingUsers.OrderBy(b => b.UserEmail).ToListAsync();
        }

        public async Task<LogingUser> GetUserByEmail(string userEmail)
        {
            return await _context.LogingUsers.FirstOrDefaultAsync(b => b.UserEmail==userEmail);
        }

        public async Task<LogingUser> GetUserById(int id)
        {
            return await _context.LogingUsers.FirstOrDefaultAsync(b => b.LogingUserId == id);
        }

        public string UpdateUser(LogingUser User)
        {
            _context.LogingUsers.Update(User);
            _context.SaveChanges();
            return "An existing user updated successfully";
        }

    }
}
