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

        public async Task<List<Disciplines>> Getasync()
        {
            var discipline = await dbContext.Disciplines.ToListAsync();

            return discipline;

        }
        public async Task<Disciplines> Putasync(Disciplines disciplines, int id)
        {

            var discipline = await dbContext.Disciplines.ToListAsync();

            if (disciplines.DisciplineId == 0)
            {
                await dbContext.Disciplines.AddAsync(disciplines);

            }
            else
            {

                Disciplines disciplineToUpdate = discipline
                  .Where(discipline => discipline.DisciplineId == id).FirstOrDefault();

                if (disciplineToUpdate != null)
                {
                    dbContext.Entry(disciplineToUpdate).CurrentValues.SetValues(disciplines);
                }
            }

            dbContext.SaveChanges();




            return disciplines;

        }
        public async Task<Disciplines> Deleteasync()
        {
            var discipline = await dbContext.Disciplines.ToListAsync();

            return null;

        }

        public Task<Disciplines> Putasync(Disciplines disciplines)
        {
            throw new NotImplementedException();
        }

        public Task<Disciplines> Deleteasync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
