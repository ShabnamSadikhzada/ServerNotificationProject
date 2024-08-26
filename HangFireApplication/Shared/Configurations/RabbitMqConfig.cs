namespace Shared.Configurations;

public class RabbitMqConfig
{
    public string Host { get; set; }
    public int Port { get; set; } = 5672;  // varsayılan port
    public string Username { get; set; } // default username -> guest
    public string Password { get; set; } // default password -> guest
    public string VirtualHost { get; set; } // -> "/"
}