using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace X_RayPalette.Views
{
    public interface IView
    {
        public event EventHandler OnBack;
        public event EventHandler OnRender;

        public void Render(bool isAdmin);
        public void Back();
    }
    public abstract class View : IView
    {
        public event EventHandler OnBack;
        public event EventHandler OnRender;

        protected void OnBackEvent()
        {
            OnBack?.Invoke(this, EventArgs.Empty);
        }
        protected void OnRenderEvent()
        {
            OnRender?.Invoke(this, EventArgs.Empty);
        }

        public abstract void Render(bool isAdmin);

        public abstract void Back();
    }
}
