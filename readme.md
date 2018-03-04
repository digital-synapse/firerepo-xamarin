# FireRepo 

A firebase backed local repo for xamarin android

Make sure your firebase instance has anonymous authentication enabled
Init with a firebase API url and api key.

usage:
		Localbase.Init(apiUrl, apiKey, new []{ "MyRepo"});
            
        var data = await Localbase.Get<List<MyRepo>>("MyRepo");