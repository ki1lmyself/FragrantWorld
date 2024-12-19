using FragrantWorldApp.Data;
using FragrantWorldApp.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FragrantWorldApp.Services
{
    public class UserService
    {
        private readonly FragrantWorldContext _context;

        public UserService(FragrantWorldContext context)
        {
            _context = context;
        }

        // Метод для получения всех пользователей из базы данных
        public async Task<List<ExamUser>> GetUsersAsync()
        {
            return await _context.ExamUsers.ToListAsync();
        }

        // Метод для авторизации пользователя
        public async Task<ExamUser> LoginAsync(string login, string password)
        {
            return await Task.Run(() => _context.ExamUsers
            .SingleOrDefault(u => u.UserLogin == login && u.UserPassword == password));
        }
    }
}
