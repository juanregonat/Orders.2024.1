using Orders.Backend.Repositories.Interfaces;
using Orders.Backend.UnitOfWork.Interfaces;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.UnitOfWork.Implementation
{
    public class StatesUnitOfWork : GenericUnitOfWork<State>, IStatesUnitOfWork
    {
        private readonly IStatesRepository _statesRepository;

        public StatesUnitOfWork(IGenericRepository<State> repository, IStatesRepository statesRepository) : base(repository)
        {
            _statesRepository = statesRepository;
        }

        public override async Task<ActionResponse<IEnumerable<State>>> GetAsync() => await _statesRepository.GetAsync();
        
        //Version extendida:
        //public override async Task<ActionResponse<IEnumerable<State>>> GetAsync()
        //{
        //    var result = await _statesRepository.GetAsync();
        //    return result;
        //}

        public override async Task<ActionResponse<State>> GetAsync(int id) => await _statesRepository.GetAsync(id);
    }
}