using System;
using System.Web.Mvc;

namespace AccessControlHelper
{
    public class ShopContainer : IDisposable
    {
        private readonly string _tagName;
        private readonly ViewContext _viewContext;
        private readonly bool _canAccess;
        private bool _disposed;

        public ShopContainer(ViewContext viewContext, string tagName, bool canAccess = true)
        {
            _viewContext = viewContext;
            _tagName = tagName;
            _canAccess = canAccess;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                EndShopContainer();
            }
        }

        public void EndShopContainer()
        {
            _viewContext.Writer.Write("</{0}>", _tagName);
        }
    }
}