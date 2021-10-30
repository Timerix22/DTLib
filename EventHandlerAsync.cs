using System.Threading.Tasks;

namespace DTLib
{
    public delegate Task EventHandlerAsync<TEventArgs>(TEventArgs e);
    public delegate Task EventHandlerAsync();
}
