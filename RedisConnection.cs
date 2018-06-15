using Jazea.Configuration;

namespace Jazea.Caching
{
    public class RedisConnection : IConfigItem
    {
        public string Server { get; set; }
        public string Database { get; set; } = 0.ToString();
        public string Password { get; set; }
        public bool AbortConnect { get; set; }

        public new string ToString()
        {
            return $"{Server},defaultDatabase={Database},password={Password},abortConnect={AbortConnect}";
        }
    }

}
