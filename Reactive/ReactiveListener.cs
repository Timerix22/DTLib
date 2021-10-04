using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTLib.Reactive
{
    public class ReactiveListener<T> : ReactiveWorker<T>
    {
        public ReactiveListener()
        {
            StreamCollectionAccess.Execute(() =>
            {
                foreach(var stream in Streams)
                {
                    stream.ElementAdded +=  async (sender, value) => { await Task.Run(() =>{ }); };
                }
            });
        }
    }
}
