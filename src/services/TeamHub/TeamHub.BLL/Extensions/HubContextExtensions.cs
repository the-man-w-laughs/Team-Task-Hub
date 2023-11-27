using TeamHub.BLL.SignalR;

namespace TeamHub.BLL.Extensions
{
    public static class HubContextExtensions
    {
        public static void SetTaskId(this IDictionary<object, object?> items, int collectionId)
        {
            items[HubContextItems.TaskId] = collectionId;
        }

        public static int GetTaskId(this IDictionary<object, object?> items)
        {
            return Convert.ToInt32(items[HubContextItems.TaskId]);
        }
    }
}
