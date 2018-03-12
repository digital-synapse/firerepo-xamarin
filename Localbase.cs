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

namespace FireRepo.Xamarin
{
    public class Localbase
    {
        public static void Init(string apiUrl, string apiKey, string[] repos)
        {
            Firebase.Init(apiUrl, apiKey);
            _repos = repos;
        }

        private static string[] _repos;
        private static Task initTask = null; 
        private static async Task init()
        {
            var lastUpdatedRemote = DateTime.Parse( await Firebase.Get<string>("updated"));
            var lastUpdatedLocal = DateTime.Parse( await Nativebase.Get<string>("updated"));

            if (lastUpdatedRemote > lastUpdatedLocal)
            {
                foreach (var repo in _repos)
                {
                    var data = await Firebase.Get(repo);
                    var ok = await Nativebase.Put(repo, data);
                }
                await Nativebase.Put("updated", lastUpdatedRemote);
            }
        }
        private static Task lazyInit()
        {
            if (initTask== null) initTask= init();
            return initTask;
        }
        public static async Task<T> Get<T>(string name)
        {
            await lazyInit();
            return await Nativebase.Get<T>(name);
        }
    }
}