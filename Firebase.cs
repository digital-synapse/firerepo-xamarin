using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using System.Diagnostics;

namespace FireRepo.Xamarin
{
    public class Firebase
    {
        public static void Init(string apiUrl, string apiKey)
        {
            _apiUrl = apiUrl;
            _apiKey = apiKey;
        }
        private static string _apiUrl;
        private static string _apiKey;

        private static FirebaseClient client;
        private static TaskCompletionSource<bool> initTcs = new TaskCompletionSource<bool>();
        private static Task<bool> initTask => initTcs.Task;
        private static bool initStarted = false;
        private static void init()
        {
            initStarted = true;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    client = new global::Firebase.Xamarin.Database.FirebaseClient(_apiUrl, async () =>
                    {
                        var authProvider = new global::Firebase.Xamarin.Auth.FirebaseAuthProvider(
                        new global::Firebase.Xamarin.Auth.FirebaseConfig(_apiKey));
                        var authLink = await authProvider.SignInAnonymouslyAsync();
                        return authLink.FirebaseToken;
                    });
                    initTcs.SetResult(true);
                    return;
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                }
                initTcs.SetResult(false);
            });
            return;
        }
        private static Task<bool> lazyInit()
        {
            if (!initStarted) init();
            return initTask;            
        }

        /*
        public static async Task<List<T>> GetList<T>(string name)
        {
            var ready = await lazyInit();

            var list = new List<T>();
            if (ready)
            {
                try
                {
                    var q = client.Child(name);
                    list = await q.OnceSingleAsync<List<T>>();                    
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                }
            }
            return list;
        }
        */
        public static async Task<T> Get<T>(string name)
        {
            var ready = await lazyInit();
            T obj = default(T);
            if (ready)
            {
                try
                {
                    var q = client.Child(name);
                    obj = await q.OnceSingleAsync<T>();
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                }
            }
            return obj;
        }
    }
}