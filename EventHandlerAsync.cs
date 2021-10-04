using System.Threading.Tasks;

namespace DTLib
{
    public delegate Task EventHandlerAsync<TEventArgs>(object sender, TEventArgs e);
}
