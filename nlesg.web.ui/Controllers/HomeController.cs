using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLESG.BackProcess.Repo;

namespace nlesg.web.ui.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SynchCSGOData()
        {            
            CSGOApiRepo objCSGOApiRepo = new CSGOApiRepo();

            objCSGOApiRepo.UpdateDfsSlate();
            objCSGOApiRepo.UpdateMembershipActive();
            objCSGOApiRepo.UdpateHistoricalMembershipsByTeam(100000001);
            objCSGOApiRepo.UpdateGetPlayers();
            objCSGOApiRepo.UpdatePlayersbyTeam(100000078);
            objCSGOApiRepo.UpdateTeams();
            objCSGOApiRepo.UpdateCompetition();
            objCSGOApiRepo.UpdateGetCompetitionDetails(100000009); //Need to check player to update
            objCSGOApiRepo.UpdateBoxScoresbyGameId(100000091);
            objCSGOApiRepo.UpdateBoxScoresbyDate(Convert.ToDateTime("2018-01-13"));
            objCSGOApiRepo.UpdateProjectedPlayerGameStatsbyDate(Convert.ToDateTime("2018-01-13"));
            objCSGOApiRepo.UpdateProjectedPlayerGameStatsbyDateAndPlayerId(Convert.ToDateTime("2018-01-13"), 100001079);
            objCSGOApiRepo.UpdateProjectedPlayerGameStatsbyDateAndPlayerId(Convert.ToDateTime("2018-01-13"), 100001079);
            objCSGOApiRepo.UpdateVenues();
            objCSGOApiRepo.UpdateSeasonTeams(100000023);

            return View();
        }
        public IActionResult SynchLOLData()
        {
            LOLApiRepo objLOLApiRepo = new LOLApiRepo();

            objLOLApiRepo.UpdateAreas();
            objLOLApiRepo.UpdateCompetition();
            objLOLApiRepo.UpdateMembershipActive();
            objLOLApiRepo.UpdateHistoricalMemberships();
            objLOLApiRepo.UpdateMembershipsByTeam(100000009);
            objLOLApiRepo.UdpateHistoricalMembershipsByTeam(100000001);
            objLOLApiRepo.UpdatePlayersByPlayerID(100000576);
            objLOLApiRepo.UpdatePlayers();
            objLOLApiRepo.UpdatePlayersbyTeam(100000001);
            objLOLApiRepo.UpdateSeasonTeams(100000002);
            objLOLApiRepo.UpdateStandings(100000138);
            objLOLApiRepo.UpdateTeams();
            objLOLApiRepo.UpdateVenues();
            objLOLApiRepo.UpdateGetCompetitionDetails(100000002); //Need to check player to update
            objLOLApiRepo.UpdateGamebyDate(Convert.ToDateTime("2018-01-13"));
            objLOLApiRepo.UpdateBoxScoresbyGameId(100002649);
            objLOLApiRepo.UpdateBoxScoresbyDate(Convert.ToDateTime("2018-01-13"));
            objLOLApiRepo.UpdateChampions();
            objLOLApiRepo.UpdateItems();
            objLOLApiRepo.UpdateSpells();
            objLOLApiRepo.UpdateProjectedPlayerGameStatsbyDate(Convert.ToDateTime("2019-06-02"));
            objLOLApiRepo.UpdateProjectedPlayerGameStatsbyDateAndPlayerId(Convert.ToDateTime("2018-01-13"), 100001079);
            return View();
        }
    }
}