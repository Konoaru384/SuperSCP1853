using Exiled.API.Interfaces;

namespace SuperSCP1853
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
    }
}
