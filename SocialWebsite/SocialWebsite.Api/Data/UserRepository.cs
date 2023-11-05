using SocialWebsite.Models;

namespace SocialWebsite.Api.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteUser(string userName)
        {
            User? user = await _context.Users.FindAsync();
            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUser(User user)
        {
            User? user = await _context.Users.FindAsync(user.Id);
            if (user == null)
            {
                return false;
            }
            _context.Users.Update(user);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> CreateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
