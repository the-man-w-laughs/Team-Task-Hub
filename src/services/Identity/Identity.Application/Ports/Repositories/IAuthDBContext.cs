namespace Identity.Application.Ports.Repositories
{
    public interface IAuthDBContext
    {
        public Task<int> SaveChangesAsync();
    }
}