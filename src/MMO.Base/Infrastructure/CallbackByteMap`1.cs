using System;

namespace MMO.Base.Infrastructure {
    public class CallbackByteMap<TCallbackType> where TCallbackType : class {
        private readonly TCallbackType[] _callbacks;
        private byte _nextCallbackId;

        public CallbackByteMap() {
            // Maybe +1
            _callbacks = new TCallbackType[byte.MaxValue];
            _nextCallbackId = 0;
        }

        public byte RegisterCallback(TCallbackType callback) {
            if (_callbacks[_nextCallbackId] != null) {
                throw new InvalidOperationException(string.Format("Callback {0} already registered", _nextCallbackId));
            }

            var callbackId = _nextCallbackId;
            unchecked {
                _nextCallbackId++;
            }

            _callbacks[callbackId] = callback;
            return callbackId;
        }

        public TCallbackType GetCallback(byte callbackId) {
            if (_callbacks[callbackId] == null) {
                throw new NotImplementedException(string.Format("Callback {0} is not registered or has already been invoked", callbackId));
            }

            var callback = _callbacks[callbackId];
            _callbacks[callbackId] = null;
            return callback;
        }
    }
}