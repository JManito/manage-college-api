using ManageCollege.Models.Domain;

namespace ManageCollege.Repositories.Interface
{
    public interface IDisciplinesRepository
    {
        Task<Disciplines> Createasync(Disciplines disciplines);

    }
}
