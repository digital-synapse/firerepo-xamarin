# FireRepo 

A firebase backed local repo for xamarin android. This will store data locally on disk but update from firebase whenever remote data is updated.

Getting started:
* ensure your firebase instance has anonymous authentication enabled
* in Firebase add a child key named "updated" with a valid date time value. This will be used to sync data on an as-needed basis. 

Init with a firebase API url and api key.

usage:
```
    Localbase.Init(apiUrl, apiKey, new []{ "MyRepo"});        
    var data = await Localbase.Get<List<MyRepo>>("MyRepo");
```
