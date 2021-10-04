using System.Threading.Tasks;

namespace DTLib
{
#pragma warning disable IDE1006 // Стили именования
    public delegate Task EventHandlerAsync<TEventArgs>(object sender, TEventArgs e);
#pragma warning restore IDE1006 // Стили именования
}
