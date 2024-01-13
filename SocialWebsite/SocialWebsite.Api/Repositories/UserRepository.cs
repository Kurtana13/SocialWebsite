using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialWebsite.Api.Data;
using SocialWebsite.Api.Repositories.IRepositories;
using SocialWebsite.Models;
using SocialWebsite.Models.Roles;
using SocialWebsite.Models.ViewModels;
using System.Runtime.CompilerServices;

namespace SocialWebsite.Api.Repositories
{
    public class UserRepository :GenericRepository<User>, IUserRepository
    {
        private UserManager<User> _userManager;

        //Without Unit of Work
        public UserRepository(ApplicationDbContext context)
            :base(context) 
        {

        }

        public UserRepository(ApplicationDbContext context, UserManager<User> userManager)
           : base(context)
        {
            _userManager = userManager;
        }

        //With Unit of Work
        public UserRepository(IUnitOfWork<ApplicationDbContext> unitOfWork)
           : base(unitOfWork)
        {

        }

        //With Unit of Work
        public UserRepository(IUnitOfWork<ApplicationDbContext> unitOfWork,UserManager<User> userManager)
           : base(unitOfWork)
        {
            _userManager = userManager;
        }

        private async Task<User> Create(User user,string password)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (await _dbSet.Where(x => x.UserName == user.UserName).FirstOrDefaultAsync() != null)
                    {
                        return null;
                    }
                    var createdUser = await _userManager.CreateAsync(user, password);
                    //await _userManager.AddToRoleAsync(user, UserRoles.User);
                    if (createdUser.Succeeded)
                    {
                        var currentUser = await _userManager.FindByNameAsync(user.UserName);
                        var roleResult = await _userManager.AddToRoleAsync(currentUser, UserRoles.User);
                        if(roleResult.Succeeded)
                        {
                            await transaction.CommitAsync();
                            return user;
                        }
                    }
                    await transaction.RollbackAsync();
                }
                catch (Exception ex)
                {
                    // Handle exceptions as needed
                    // You may want to log the exception or perform other error handling
                    await transaction.RollbackAsync();
                }
                return null;
            }
        }

        public async Task<User> Create(UserViewModel userViewModel)
        {
            return await Create(new User(userViewModel),userViewModel.Password!);
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _dbSet.FirstAsync(x => x.UserName == username);
        }
    }
}
