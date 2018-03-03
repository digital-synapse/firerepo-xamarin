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
        private static Task initTask = null; 
        private static async Task init()
        {
            var lastUpdatedRemote = await Firebase.Get<DateTime>("updated");
            var lastUpdatedLocal = await Nativebase.Get<DateTime>("updated");

            if (lastUpdatedRemote > lastUpdatedLocal)
            {
                await Nativebase.Put("updated", lastUpdatedRemote);
                //var planets = await Firebase.Get<List<Planet>>("planets");
                var planets = await Firebase.Get<string>("planets");
                var ok= await Nativebase.Put("planets", planets);
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