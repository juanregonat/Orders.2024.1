using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Interfaces
{
    public interface ICategoriesRepository
    {
        //Task<ActionResponse<Category>> GetAsync(int id);

        //Task<ActionResponse<IEnumerable<Category>>> GetAsync();

        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);

        Task<ActionResponse<IEnumerable<Category>>> GetAsync(PaginationDTO pagination);
    }
}
