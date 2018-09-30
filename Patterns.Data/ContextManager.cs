using System;
using System.Collections;
using System.Collections.Generic;

namespace Patterns.Data
{
    public static class ContextManager
    {
        private static Func<IDictionary> contextStorageGetter;

        static ContextManager()
        {
            var dictionary = new Dictionary<String, Object>();
            contextStorageGetter = () => dictionary;
        }

        public static void Init(Func<IDictionary> contextStorageGetter)
        {
            ContextManager.contextStorageGetter = contextStorageGetter;
        }

        public static T GetCurrentContext<T>() where T: new()
        {
            IDictionary contexts = GetContextStorage();
            if (!contexts.Contains(typeof(T).ToString()))
            {
                CreateContext<T>();
            }
            var currentSession = (T)contexts[typeof(T).ToString()];
            return currentSession;
        }

        private static IDictionary GetContextStorage()
        {
            return contextStorageGetter();
        }

        private static void CreateContext<T>() where T: new()
        {
            var context = new T();
            GetContextStorage()[typeof(T).ToString()] = context;
        }
    }
}
