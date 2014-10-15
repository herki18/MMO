using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MMO.Base.Infrastructure.Extensions;

namespace MMO.Web.Infrastructure
{
    public class MultipartDataProvider : MultipartStreamProvider
    {
        private readonly Func<string, Stream> _fileStreamFactory;
        private readonly Dictionary<string, string> _formData;
        private readonly List<HttpContent> _files;
        private readonly List<string> _operationError;

        public IEnumerable<HttpContent> Files { get { return _files; } }
        public IEnumerable<string> OperationErrors { get { return _operationError; } }
        public bool IsValid { get { return _operationError.Count == 0; } }

        public MultipartDataProvider(Func<string, Stream> fileStreamFactory) {
            _fileStreamFactory = fileStreamFactory;
            _formData = new Dictionary<string, string>();
            _files = new List<HttpContent>();
            _operationError = new List<string>();
        }

        public bool TryGetFormdata<T>(string name, out T value) {
            string stringValue;
            if (!_formData.TryGetValue(name, out stringValue)) {
                value = default(T);
                return false;
            }

            try {
                value = (T) Convert.ChangeType(stringValue, typeof (T));
                return true;
            }
            catch (Exception) {
                value = default(T);
                return false;
            }
        }

        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers) {
            if (string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName)) {
                return new MemoryStream();
            }

            var fileStream = _fileStreamFactory(headers.ContentDisposition.Name.TrimDoubleQuotes());
            if (fileStream != null) {
                return fileStream;
            }

            _operationError.Add(string.Format("File {0} is not supported", headers.ContentDisposition.Name));
            return new MemoryStream();
        }

        public override async Task ExecutePostProcessingAsync() {
            foreach (var content in Contents) {
                if (string.IsNullOrWhiteSpace(content.Headers.ContentDisposition.FileName)) {
                    var key = (content.Headers.ContentDisposition.Name ?? "").TrimDoubleQuotes();
                    if (_formData.ContainsKey(key)) {
                        _operationError.Add(string.Format("Multiple values with the same key of {0} are not supported", key));
                        continue;
                    }

                    _formData.Add(key, await content.ReadAsStringAsync());
                }
                else {
                    _files.Add(content);
                }


            }
        }

    }
}