using System;
using System.Collections.Generic;
using System.Text;
using SportData.CSGO;
using SportData.CSGO.Entities;
using System.Data;
using NLESG.Common.Utils;
using NLESG.DAL.Helper;
using NLESG.Common;
using System.Linq;

namespace NLESG.BackProcess.Repo
{
    public class CSGOApiRepo : BaseRepo
    {        
        string ConnectionString = @"Data Source=192.168.0.102;Initial Catalog=ESportDB;Integrated Security=false;User ID=sa;Password=saadmin;MultipleActiveResultSets=True";        

        #region Update From Api 

        public bool UpdateDfsSlate()
        {
            Areas objAreas = GetDfsSlate();
            DataTable _dtTable = Utils.ToDataTable<Area>(objAreas);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOAreaMergeSPName, _dtTable);
            return blnIsSuccess;
        }

        public bool UpdateMembershipActive()
        {
            Memberships objMemberships = GetMembershipActive();
            DataTable _dtTable = Utils.ToDataTable<Membership>(objMemberships);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOMembershipMergeSPName, _dtTable);
            return blnIsSuccess;
        }

        public bool UdpateHistoricalMembershipsByTeam(int TeamID)
        {
            Memberships objMemberships = GetHistoricalMembershipsByTeam(TeamID);
            DataTable _dtTable = Utils.ToDataTable<Membership>(objMemberships);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOMembershipMergeSPName, _dtTable);
            return blnIsSuccess;
        }

        public bool UpdatePlayersByPlayerID(int PlayerId)
        {
            Players ObjPlayers = GetPlayersByPlayerID(PlayerId);
            DataTable _dtTable = Utils.ToDataTable<Player>(ObjPlayers);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOPlayerMergeSPName, _dtTable);
            return blnIsSuccess;
        }

        public bool UpdateGetPlayers()
        {
            Players ObjPlayers = GetPlayers();
            DataTable _dtTable = Utils.ToDataTable<Player>(ObjPlayers);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOPlayerMergeSPName, _dtTable);
            return blnIsSuccess;
        }

        public bool UpdatePlayersbyTeam(int TeamID)
        {
            Players ObjPlayers = GetPlayersbyTeam(TeamID);
            DataTable _dtTable = Utils.ToDataTable<Player>(ObjPlayers);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOPlayerMergeSPName, _dtTable);
            return blnIsSuccess;
        }
        //public bool UpdateSchedule(int RoundId)
        //{
        //    Schedules objSchedules = GetSchedule(RoundId);
        //    DataTable _dtTable = Utils.ToDataTable<Schedule>(objSchedules);
        //    bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOSche, _dtTable);
        //    return blnIsSuccess;
        //} Need to create table and SP
        public bool UpdateTeams()
        {
            Teams objTeam = GetTeams();
            DataTable _dtTable = Utils.ToDataTable<Team>(objTeam);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOTeamMergeSPName, _dtTable);
            return blnIsSuccess;
        }
        public bool UpdateCompetition()
        {
            Competitions objCompetitions = GetCompetition();

            var SeasonsList = objCompetitions.SelectMany(x => x.Seasons).ToList();
            DataTable _dtSeasonTable = Utils.ToDataTable<Season>(SeasonsList);
            _dtSeasonTable.Columns.Remove("Rounds");
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOSeasonMergeSPName, _dtSeasonTable);

            DataTable _dtTable = Utils.ToDataTable<Competition>(objCompetitions);
            _dtTable.Columns.Remove("Seasons");
            blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOCompetitionMergeSPName, _dtTable);
            return blnIsSuccess;
        }

        public bool UpdateGetCompetitionDetails(int competitionid)
        {
            CompetitionDetails objCompetitions = GetCompetitionDetails(competitionid);

            bool blnIsSuccess = false;

            //CurrentSeason
            if (objCompetitions.Select(x => x.CurrentSeason).FirstOrDefault() != null)
            {
                var CurrentSeasonList = objCompetitions.Select(x => x.CurrentSeason).ToList();
                DataTable _dtCurrentSeasonTable = Utils.ToDataTable<Season>(CurrentSeasonList);
                _dtCurrentSeasonTable.Columns.Remove("Rounds");
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOSeasonMergeSPName, _dtCurrentSeasonTable);
            }

            //TeamDetail
            if (objCompetitions.Select(x => x.Teams).FirstOrDefault() != null)
            {
                var TeamList = objCompetitions.SelectMany(x => x.Teams).ToList();

                //Player
                //foreach (TeamDetail objTeamDetail in TeamList)
                //{
                //    List<Player> lstPlayer = new List<Player>();
                //    if (objTeamDetail.Players.FirstOrDefault() != null)
                //    {
                //        var Player = objTeamDetail.Players;
                //        DataTable _dtPlayerTable = Utils.ToDataTable<Player>(Player);
                //        blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOPlayerMergeSPName, _dtPlayerTable);
                //    }
                //}

                DataTable _dtTeamTable = Utils.ToDataTable<TeamDetail>(TeamList);
                _dtTeamTable.Columns.Remove("Players");
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOTeamDetailMergeSPName, _dtTeamTable);
            }

            //Games
            if (objCompetitions.Select(x => x.Games).FirstOrDefault() != null)
            {
                var GameList = objCompetitions.SelectMany(x => x.Games).ToList();
                DataTable _dtGameTable = Utils.ToDataTable<Game>(GameList);
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOGameMergeSPName, _dtGameTable);
            }

            //Seasons
            if (objCompetitions.Select(x => x.Seasons).FirstOrDefault() != null)
            {
                var SeasonsList = objCompetitions.SelectMany(x => x.Seasons).ToList();
                DataTable _dtSeasonTable = Utils.ToDataTable<Season>(SeasonsList);
                _dtSeasonTable.Columns.Remove("Rounds");
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOSeasonMergeSPName, _dtSeasonTable);
            }


            DataTable _dtTable = Utils.ToDataTable<CompetitionDetail>(objCompetitions);
            _dtTable.Columns.Remove("CurrentSeason");
            _dtTable.Columns.Remove("Teams");
            _dtTable.Columns.Remove("Games");
            _dtTable.Columns.Remove("Seasons");
            blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOCompetitionDetailMergeSPName, _dtTable);
            return blnIsSuccess;
        }

        public bool UpdateGamebyDate(DateTime Date)
        {
            Games objGames = GetGamebyDate(Date);
            DataTable _dtTable = Utils.ToDataTable<Game>(objGames);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOGameMergeSPName, _dtTable);
            return blnIsSuccess;
        }
        public bool UpdateBoxScoresbyGameId(int GameId)
        {
            BoxScores objBoxScores = GetBoxScoresbyGameId(GameId);

            //Game
            var GameList = objBoxScores.SelectMany(x => x.Game).ToList();
            DataTable _dtGamesTable = Utils.ToDataTable<Game>(GameList);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOGameMergeSPName, _dtGamesTable);

            //Map
            if (objBoxScores.Select(x => x.Map).FirstOrDefault() != null)
            {
                var MapsList = objBoxScores.SelectMany(x => x.Map).ToList();
                DataTable _dtGamesMapsTable = Utils.ToDataTable<Map>(MapsList);
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOMapMergeSPName, _dtGamesMapsTable);
            }
            return blnIsSuccess;
        }

        public bool UpdateBoxScoresbyDate(DateTime Date)
        {
            BoxScores objBoxScores = GetBoxScoresbyDate(Date);

            //Game
            var GameList = objBoxScores.SelectMany(x => x.Game).ToList();
            DataTable _dtGamesTable = Utils.ToDataTable<Game>(GameList);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOGameMergeSPName, _dtGamesTable);

            //Map
            if (objBoxScores.Select(x => x.Map).FirstOrDefault() != null)
            {
                var MapsList = objBoxScores.SelectMany(x => x.Map).ToList();
                DataTable _dtGamesMapsTable = Utils.ToDataTable<Map>(MapsList);
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOMapMergeSPName, _dtGamesMapsTable);
            }
            return blnIsSuccess;
        }

        public bool UpdateProjectedPlayerGameStatsbyDate(DateTime Date)
        {
            PlayerGameProjections objPlayerGameProjections = GetProjectedPlayerGameStatsbyDate(Date);
            DataTable _dtTable = Utils.ToDataTable<PlayerGameProjection>(objPlayerGameProjections);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOPlayerGameProjectionMergeSPName, _dtTable);
            return blnIsSuccess;
        }
        public bool UpdateProjectedPlayerGameStatsbyDateAndPlayerId(DateTime Date, int playerid)
        {
            PlayerGameProjections objPlayerGameProjections = GetProjectedPlayerGameStatsbyDateAndPlayerId(Date, playerid);
            DataTable _dtTable = Utils.ToDataTable<PlayerGameProjection>(objPlayerGameProjections);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOPlayerGameProjectionMergeSPName, _dtTable);
            return blnIsSuccess;
        }

        public bool UpdateVenues()
        {
            bool blnIsSuccess = false;
            Venues objUpdateVenues = GetVenues();
            if (objUpdateVenues.FirstOrDefault() != null)
            {
                DataTable _dtTable = Utils.ToDataTable<Venue>(objUpdateVenues);
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOVenueMergeSPName, _dtTable);
            }
            return blnIsSuccess;
        }

        public bool UpdateSeasonTeams(int SeasonId)
        {
            bool blnIsSuccess = false;
            SeasonTeams objSeasonTeams = GetSeasonTeams(SeasonId);
            if (objSeasonTeams.FirstOrDefault() != null)
            {
                DataTable _dtTable = Utils.ToDataTable<SeasonTeam>(objSeasonTeams);
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.CSGOSeasonTeamMergeSPName, _dtTable);
            }
            return blnIsSuccess;
        }

        #endregion

        #region Get Api Data

        private Areas GetDfsSlate()
        {
            return sportDataCSGOClient.AreasServices.GetAreas();
        }
        private Memberships GetMembershipActive()
        {
            return sportDataCSGOClient.MembershipServices.GetActiveMemberships();
        }
        private Memberships GetHistoricalMembershipsByTeam(int TeamID)
        {
            return sportDataCSGOClient.MembershipServices.GetHistoricalMembershipsByTeam(TeamID);
        }
        private Players GetPlayersByPlayerID(int PlayerId)
        {
            return sportDataCSGOClient.PlayerServices.GetPlayersByPlayerID(PlayerId);
        }
        private Players GetPlayers()
        {
            return sportDataCSGOClient.PlayerServices.GetPlayers();
        }
        private Players GetPlayersbyTeam(int TeamID)
        {
            return sportDataCSGOClient.PlayerServices.GetPlayersbyTeam(TeamID);
        }
        private Schedules GetSchedule(int RoundId)
        {
            return sportDataCSGOClient.ScheduleServices.GetSchedule(RoundId);
        }
        private Teams GetTeams()
        {
            return sportDataCSGOClient.TeamServices.GetTeams();
        }
        private Competitions GetCompetition()
        {
            return sportDataCSGOClient.CompetitionServices.GetCompetition();
        }
        private CompetitionDetails GetCompetitionDetails(int competitionid)
        {
            return sportDataCSGOClient.CompetitionServices.GetCompetitionDetails(competitionid);
        }
        private Games GetGamebyDate(DateTime Date)
        {
            return sportDataCSGOClient.GameServices.GetGamebyDate(Date);
        }
        private BoxScores GetBoxScoresbyGameId(int GameId)
        {
            return sportDataCSGOClient.BoxScoreServices.GetBoxScoresbyGameId(GameId);
        }
        private BoxScores GetBoxScoresbyDate(DateTime Date)
        {
            return sportDataCSGOClient.BoxScoreServices.GetBoxScoresbyDate(Date);
        }
        private PlayerGameProjections GetProjectedPlayerGameStatsbyDate(DateTime Date)
        {
            return sportDataCSGOClient.ProjectionServices.GetProjectedPlayerGameStatsbyDate(Date);
        }
        private PlayerGameProjections GetProjectedPlayerGameStatsbyDateAndPlayerId(DateTime Date, int playerid)
        {
            return sportDataCSGOClient.ProjectionServices.GetProjectedPlayerGameStatsbyDateAndPlayerId(Date, playerid);
        }

        private Venues GetVenues()
        {
            return sportDataCSGOClient.VenueServices.GetVenues();
        }
        private SeasonTeams GetSeasonTeams(int SeasonId)
        {
            return sportDataCSGOClient.SeasonServices.GetSeasonTeams(SeasonId);
        }

        #endregion
    }
}
