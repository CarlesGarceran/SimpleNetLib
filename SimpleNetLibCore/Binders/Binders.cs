using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetLibCore.Binders
{
    public class Binder
    {
        public EventHandler handler;

        public Binder() { handler = null; }
        public Binder(Action<object, EventArgs> a) { handler += new EventHandler(a); }

        public void Bind(Action<object, EventArgs> a) { handler += new EventHandler(a); }

        public void Invoke(object sender, EventArgs e) { handler.Invoke(sender, e); }
    }

    public class Binder<T>
    {
        public EventHandler<T> handler;

        public Binder() { handler = null; }
        public Binder(Action<object, T> a) { handler += new EventHandler<T>(a); }

        public void Bind(Action<object, T> a) { handler += new EventHandler<T>(a); }

        public void Invoke(object sender, T e) { handler.Invoke(sender, e); }
    }

    public static class Binders
    {
        public static Binder onPacketRetrieved = new Binder();
        public static Binder onPacketSent = new Binder();
        public static Binder networkUpdateBinder = new Binder();
    }
}
