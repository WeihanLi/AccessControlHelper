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
#if NET45
        private readonly string _content;
#else
        private readonly TextWriter _writer;
#endif
        public SparkContainer(ViewContext viewContext, string tagName, bool canAccess = true)
        {
            _viewContext = viewContext;
            _tagName = tagName;
            _canAccess = canAccess;
            if (!_canAccess)
            {
#if NET45
                _content = (_viewContext.Writer as StringWriter)?.GetStringBuilder().ToString();
#else
                _writer = viewContext.Writer;
                viewContext.Writer = TextWriter.Null;
#endif
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
#if NET45
                (_viewContext.Writer as StringWriter)?.GetStringBuilder().Clear().Append(_content);
#else
                _viewContext.Writer = _writer;
#endif
            }
            else
            {
                _viewContext.Writer.Write("</{0}>", _tagName);
            }
        }
    }
}