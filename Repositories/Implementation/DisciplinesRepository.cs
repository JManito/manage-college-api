using ManageCollege.Data;
using ManageCollege.Models.Domain;
using ManageCollege.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace ManageCollege.Repositories.Implementation
{
    public class DisciplinesRepository : IDisciplinesRepository
    {
        private readonly ApplicationDBContext dbContext;

        public DisciplinesRepository(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Disciplines> Createasync(Disciplines disciplines)
        {
            await dbContext.Disciplines.AddAsync(disciplines);
            await dbContext.SaveChangesAsync();

            return disciplines;
        }
    }
}
