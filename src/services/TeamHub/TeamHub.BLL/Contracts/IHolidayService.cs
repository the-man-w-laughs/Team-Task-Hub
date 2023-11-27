namespace TeamHub.BLL.Contracts
{
    public interface IHolidayService
    {
        public Task<bool> IsDayOffAsync(DateTime date);
    }
}
