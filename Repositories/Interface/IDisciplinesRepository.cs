using ManageCollege.Models.Domain;

namespace ManageCollege.Repositories.Interface
{
    public interface IDisciplinesRepository
    {
        Task<Disciplines> Createasync(Disciplines disciplines);
        Task<List<Disciplines>> Getasync();
        Task<Disciplines> GetDisciplineAsync(int id);
        Task<Disciplines> Putasync(Disciplines disciplines);
        Task<Disciplines> Deleteasync(int id);

    }
}
