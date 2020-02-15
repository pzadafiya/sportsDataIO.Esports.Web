# FantasyDataESportsAPI

We have implemented below Esports API
* CS:GO
* LOL

# Prerequisites
1. Visual Studio 2019 16.4 or later with the ASP.NET
2. [.NET Core 3.1 SDK or later](https://dotnet.microsoft.com/download/dotnet-core/3.1)

### How to configure it ###

Add following code in appsettings.json file, update appsettings.json file with your SubscriptionKey which is found in [sportdata.io](https://sportsdata.io/).

**appsettings.json**
```sh
{
  "fantasyDataCSGO": {
    "baseUrl": "https://api.sportsdata.io/v3/csgo",
    "primarySubscriptionKey": "################################",
    "ProjectionPrimarySubscriptionKey": "################################"
  },
  "fantasyDataLOL": {
    "baseUrl": "https://api.sportsdata.io/v3/lol",
    "primarySubscriptionKey": "################################",
    "ProjectionPrimarySubscriptionKey": "################################"
  }
}
```

### Set up the project
* Add DLLs as per requirements to implementation of sport game, DLL for SportData and SportData.Utils are mandatory and other dll you can choose as per sport you want to implement, Regarding sport game implementation, you need to import two DLL for e.g if you want to implement CS:GO than you need to import SportData.CSGO.dll, and SportData.CSGO.Entities.dll
* all dll found in packages/ESportAPI
* below is CS:GO and LOL Integration example.

**CS:GO Integration**
This is the documentation for SportsDataIO's CS:GO API Integration. 
now check below example which show how to create request to get area(Countries)
``` sh
public class CSGOApiRepo
{
  public SportDataCSGOClient sportDataCSGOClient = null;
  private Areas GetAreas()
  {
	sportDataCSGOClient = new SportDataCSGOClient();
	return sportDataCSGOClient.AreasServices.GetAreas();
   }
}
```

**LOL Integration**
This is the documentation for SportsDataIO's LOL API Integration. 
now check below example which show how to create request to get competition
``` sh
public class CSGOApiRepo
{
  public SportDataCSGOClient sportDataLOLClient = null;
  private Competitions GetCompetition()
  {
	sportDataLOLClient = new sportDataLOLClient();
	return sportDataLOLClient.CompetitionServices.GetCompetition();
   }
}
```
