﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTLib
{
    public delegate Func<TEventArgs, Task> EventHandlerAsync<TEventArgs>(object sender, TEventArgs e);
}