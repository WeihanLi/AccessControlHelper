using System;
using System.IO;
#if NET45
using System.Web.Mvc;
#else
using Microsoft.AspNetCore.Mvc.Rendering;
#endif

namespace AccessControlHelper
{
    public class SparkContainer : IDisposable
    {
        private readonly string _tagName;
        private readonly ViewContext _viewContext;
        private readonly bool _canAccess;
        private bool _disposed;

        private readonly string _content;

        public SparkContainer(ViewContext viewContext, string tagName, bool canAccess = true)
        {
            _viewContext = viewContext;
            _tagName = tagName;
            _canAccess = canAccess;
            if (!_canAccess)
            {
                _content = (_viewContext.Writer as StringWriter).GetStringBuilder().ToString();
            }
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
            if (!_canAccess)
            {
                (_viewContext.Writer as StringWriter).GetStringBuilder().Clear().Append(_content);
            }
            else
            {
                _viewContext.Writer.Write("</{0}>", _tagName);
            }
        }
    }
}