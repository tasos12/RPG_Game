
namespace _NET_Course.Models
{
    /// <summary>
    /// Contains the data returned and the message of the HTTP response.
    /// </summary>
    /// <typeparam name="T">The type of data to be returned.</typeparam>
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message{ get; set; } = null;
    }
}