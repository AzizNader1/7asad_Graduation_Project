using GraduationApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationApi.Interfaces
{
    public interface ILogingUserServices
    {
        Task<IEnumerable<LogingUser>> GetAllUsers();

        Task<LogingUser> GetUserById(int id);

        Task<LogingUser> GetUserByEmail(string userEmail);

        Task<string> AddUser(LogingUser User);

        string UpdateUser(LogingUser User);

        string DeleteUser(LogingUser User);

    }
}
