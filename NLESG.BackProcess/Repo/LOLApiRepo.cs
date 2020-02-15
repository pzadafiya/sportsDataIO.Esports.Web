using System;
using System.Collections.Generic;
using System.Text;
using SportData.LOL;
using SportData.LOL.Entities;
using System.Data;
using NLESG.Common.Utils;
using NLESG.DAL.Helper;
using NLESG.Common;
using System.Linq;

namespace NLESG.BackProcess.Repo
{
    public class LOLApiRepo : BaseRepo
    {
        string ConnectionString = @"Data Source=192.168.0.102;Initial Catalog=ESportDB;Integrated Security=false;User ID=sa;Password=saadmin;MultipleActiveResultSets=True";

        #region Update From Api 

        public bool UpdateAreas()
        {
            Areas objAreas = GetAreas();
            DataTable _dtTable = Utils.ToDataTable<Area>(objAreas);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLAreaMergeSPName, _dtTable);
            return blnIsSuccess;
        }
        public bool UpdateCompetition()
        {
            Competitions objCompetitions = GetCompetition();
            bool blnIsSuccess = false;

            var SeasonsList = objCompetitions.SelectMany(x => x.Seasons).ToList();

            if (SeasonsList != null)
            {
                // Update Round
                var RoundList = SeasonsList.SelectMany(x => x.Rounds).ToList();
                DataTable _dtRoundTable = Utils.ToDataTable<Round>(RoundList);
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLRoundMergeSPName, _dtRoundTable);

                //Update Season
                DataTable _dtSeasonTable = Utils.ToDataTable<Season>(SeasonsList);
                _dtSeasonTable.Columns.Remove("Rounds");
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLSeasonMergeSPName, _dtSeasonTable);
            }

            //Update Competition
            DataTable _dtTable = Utils.ToDataTable<Competition>(objCompetitions);
            _dtTable.Columns.Remove("Seasons");
            blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLCompetitionMergeSPName, _dtTable);
            return blnIsSuccess;
        }
        public bool UpdateMembershipActive()
        {
            Memberships objMemberships = GetMembershipActive();
            DataTable _dtTable = Utils.ToDataTable<Membership>(objMemberships);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLMembershipMergeSPName, _dtTable);
            return blnIsSuccess;
        }
        public bool UpdateHistoricalMemberships()
        {
            Memberships objMemberships = GetHistoricalMemberships();
            DataTable _dtTable = Utils.ToDataTable<Membership>(objMemberships);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLMembershipMergeSPName, _dtTable);
            return blnIsSuccess;
        }
        public bool UpdateMembershipsByTeam(int TeamID)
        {
            Memberships objMemberships = GetMembershipsByTeam(TeamID);
            DataTable _dtTable = Utils.ToDataTable<Membership>(objMemberships);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLMembershipMergeSPName, _dtTable);
            return blnIsSuccess;
        }
        public bool UdpateHistoricalMembershipsByTeam(int TeamID)
        {
            Memberships objMemberships = GetHistoricalMembershipsByTeam(TeamID);
            DataTable _dtTable = Utils.ToDataTable<Membership>(objMemberships);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLMembershipMergeSPName, _dtTable);
            return blnIsSuccess;
        }
        public bool UpdatePlayersByPlayerID(int PlayerId)
        {
            Players ObjPlayers = GetPlayersByPlayerID(PlayerId);
            DataTable _dtTable = Utils.ToDataTable<Player>(ObjPlayers);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLPlayerMergeSPName, _dtTable);
            return blnIsSuccess;
        }
        public bool UpdatePlayers()
        {
            Players ObjPlayers = GetPlayers();
            DataTable _dtTable = Utils.ToDataTable<Player>(ObjPlayers);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLPlayerMergeSPName, _dtTable);
            return blnIsSuccess;
        }
        public bool UpdatePlayersbyTeam(int TeamID)
        {
            Players ObjPlayers = GetPlayersbyTeam(TeamID);
            DataTable _dtTable = Utils.ToDataTable<Player>(ObjPlayers);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLPlayerMergeSPName, _dtTable);
            return blnIsSuccess;
        }
        public bool UpdateSeasonTeams(int SeasonId)
        {
            bool blnIsSuccess = false;
            SeasonTeams objSeasonTeams = GetSeasonTeams(SeasonId);

            if (objSeasonTeams.FirstOrDefault() != null)
            {
                //Update Team
                var TeamList = objSeasonTeams.Select(x => x.Team).ToList();
                DataTable _dtRoundTable = Utils.ToDataTable<Team>(TeamList);
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLTeamMergeSPName, _dtRoundTable);

                //Update SeasonTeam
                DataTable _dtTable = Utils.ToDataTable<SeasonTeam>(objSeasonTeams);
                _dtTable.Columns.Remove("Team");
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLSeasonTeamMergeSPName, _dtTable);
            }
            return blnIsSuccess;
        }
        public bool UpdateStandings(int RoundId)
        {
            Standings objStandings = GetStandings(RoundId);
            DataTable _dtTable = Utils.ToDataTable<Standing>(objStandings);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLStandingMergeSPName, _dtTable);
            return blnIsSuccess;
        }
        public bool UpdateTeams()
        {
            Teams objTeam = GetTeams();
            DataTable _dtTable = Utils.ToDataTable<Team>(objTeam);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLTeamMergeSPName, _dtTable);
            return blnIsSuccess;
        }
        public bool UpdateVenues()
        {
            bool blnIsSuccess = false;
            Venues objUpdateVenues = GetVenues();
            if (objUpdateVenues.FirstOrDefault() != null)
            {
                DataTable _dtTable = Utils.ToDataTable<Venue>(objUpdateVenues);
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLVenueMergeSPName, _dtTable);
            }
            return blnIsSuccess;
        }
        public bool UpdateGetCompetitionDetails(int competitionid)
        {
            CompetitionDetails objCompetitions = GetCompetitionDetails(competitionid);

            bool blnIsSuccess = false;


            if (objCompetitions != null)
            {
                //Season
                if (objCompetitions.Select(x => x.CurrentSeason).FirstOrDefault() != null)
                {
                    var CurrentSeasonList = objCompetitions.Select(x => x.CurrentSeason).ToList();

                    //Update Round
                    var RoundsList = CurrentSeasonList.SelectMany(x => x.Rounds).ToList();
                    DataTable _dtRoundsTable = Utils.ToDataTable<Round>(RoundsList);
                    blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLRoundMergeSPName, _dtRoundsTable);

                    //Update Season
                    DataTable _dtCurrentSeasonTable = Utils.ToDataTable<Season>(CurrentSeasonList);
                    _dtCurrentSeasonTable.Columns.Remove("Rounds");
                    blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLSeasonMergeSPName, _dtCurrentSeasonTable);
                }

                //TeamDetail                
                if (objCompetitions.Select(x => x.Teams).FirstOrDefault() != null)
                {
                    var TeamList = objCompetitions.SelectMany(x => x.Teams).ToList();

                    //Update Player
                    // Seprate method is available to update Players
                    //var PlayersList = TeamList.SelectMany(x => x.Players).ToList();
                    //DataTable _dtPlayersTable = Utils.ToDataTable<Player>(PlayersList);
                    //blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLPlayerMergeSPName, _dtPlayersTable);

                    //Update TeamDetail
                    //DataTable _dtTeamTable = Utils.ToDataTable<TeamDetail>(TeamList);
                    //_dtTeamTable.Columns.Remove("Players");
                    //blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLTeamDetailMergeSPName, _dtTeamTable);
                    //Implement below section instead of above section due to duplicate record in TeamDetail
                    List<TeamDetail> objTeamDetail = objCompetitions.SelectMany(x => x.Teams).ToList();
                    var TeamDetailList = objTeamDetail.GroupBy(d => new {
                                d.TeamId, d.AreaId, d.AreaName, d.Key, d.Name, d.ShortName, d.Active, d.Gender, d.Type, d.Website, d.Email, d.Founded, d.PrimaryColor,
                                d.SecondaryColor, d.TertiaryColor, d.QuaternaryColor, d.Facebook, d.Twitter, d.YouTube, d.Instagram
                    }).Select(m => new {                        
                                m.Key.TeamId, m.Key.AreaId, m.Key.AreaName, m.Key.Key, m.Key.Name, m.Key.ShortName, m.Key.Active, m.Key.Gender, m.Key.Type, m.Key.Website,
                                m.Key.Email, m.Key.Founded, m.Key.PrimaryColor, m.Key.SecondaryColor, m.Key.TertiaryColor,  m.Key.QuaternaryColor, m.Key.Facebook,
                                m.Key.Twitter, m.Key.YouTube, m.Key.Instagram
                    }).ToList();

                    List<TeamDetail> UniqueTeamDetail = new List<TeamDetail>();
                    TeamDetail TeamDetail = null;
                    foreach (var item in TeamDetailList)
                    {
                        TeamDetail = new TeamDetail();
                        TeamDetail.TeamId = item.TeamId;
                        TeamDetail.AreaId = item.AreaId;
                        TeamDetail.AreaName = item.AreaName;
                        TeamDetail.Key = item.Key;
                        TeamDetail.Name = item.Name;
                        TeamDetail.ShortName = item.ShortName;
                        TeamDetail.Active = item.Active;
                        TeamDetail.Gender = item.Gender;
                        TeamDetail.Type = item.Type;
                        TeamDetail.Website = item.Website;
                        TeamDetail.Email = item.Email;
                        TeamDetail.Founded = item.Founded;
                        TeamDetail.PrimaryColor = item.PrimaryColor;
                        TeamDetail.SecondaryColor = item.SecondaryColor;
                        TeamDetail.TertiaryColor = item.TertiaryColor;
                        TeamDetail.QuaternaryColor = item.QuaternaryColor;
                        TeamDetail.Facebook = item.Facebook;
                        TeamDetail.Twitter = item.Twitter;
                        TeamDetail.YouTube = item.YouTube;
                        TeamDetail.Instagram = item.Instagram;
                        UniqueTeamDetail.Add(TeamDetail);
                    }
                    DataTable _dtTeamTable = Utils.ToDataTable<TeamDetail>(UniqueTeamDetail);
                    _dtTeamTable.Columns.Remove("Players");
                    blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLTeamDetailMergeSPName, _dtTeamTable);
                }

                //Games                
                if (objCompetitions.Select(x => x.Games).FirstOrDefault() != null)
                {
                    var GamesList = objCompetitions.SelectMany(x => x.Games).ToList();
                    DataTable _dtGameTable = Utils.ToDataTable<Game>(GamesList);
                    blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLGameMergeSPName, _dtGameTable);
                }

                //Seasons
                if (objCompetitions.Select(x => x.Seasons).FirstOrDefault() != null)
                {
                    var SeasonsList = objCompetitions.SelectMany(x => x.Seasons).ToList();

                    //Update Round
                    if (SeasonsList.Select(x => x.Rounds).FirstOrDefault() != null)
                    {
                        var RoundList = SeasonsList.SelectMany(x => x.Rounds).ToList();
                        DataTable _dtRoundTable = Utils.ToDataTable<Round>(RoundList);
                        blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLRoundMergeSPName, _dtRoundTable);
                    }
                    //Update Seasons
                    DataTable _dtSeasonTable = Utils.ToDataTable<Season>(SeasonsList);
                    _dtSeasonTable.Columns.Remove("Rounds");
                    blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLSeasonMergeSPName, _dtSeasonTable);
                }

                //CompetitionDetail
                DataTable _dtCompetitionsTable = Utils.ToDataTable<CompetitionDetail>(objCompetitions);
                _dtCompetitionsTable.Columns.Remove("CurrentSeason");
                _dtCompetitionsTable.Columns.Remove("Teams");
                _dtCompetitionsTable.Columns.Remove("Games");
                _dtCompetitionsTable.Columns.Remove("Seasons");
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLCompetitionDetailMergeSPName, _dtCompetitionsTable);
            }

            return blnIsSuccess;
        }
        public bool UpdateGamebyDate(DateTime Date)
        {
            Games objGames = GetGamebyDate(Date);
            DataTable _dtTable = Utils.ToDataTable<Game>(objGames);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLGameMergeSPName, _dtTable);
            return blnIsSuccess;
        }
        //public bool UpdateSchedule(int RoundId)
        //{
        //    Schedules objSchedules = GetSchedule(RoundId);
        //    DataTable _dtTable = Utils.ToDataTable<Schedule>(objSchedules);
        //    bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLSche, _dtTable);
        //    return blnIsSuccess;
        //} Need to create table and SP
        public bool UpdateBoxScoresbyGameId(int GameId)
        {
            BoxScores objBoxScores = GetBoxScoresbyGameId(GameId);

            bool blnIsSuccess = false;
            //Game
            if (objBoxScores.Select(x => x.Game).FirstOrDefault() != null)
            {
                var GameList = objBoxScores.Select(x => x.Game).ToList();
                DataTable _dtGamesTable = Utils.ToDataTable<Game>(GameList);
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLGameMergeSPName, _dtGamesTable);
            }
            //PlayerGames
            if (objBoxScores.Select(x => x.PlayerGames).FirstOrDefault() != null)
            {
                var PlayerGamesList = objBoxScores.SelectMany(x => x.PlayerGames).ToList();
                DataTable _dtGamesMapsTable = Utils.ToDataTable<PlayerGame>(PlayerGamesList);
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLPlayerGameMergeSPName, _dtGamesMapsTable);
            }
            //TeamGames
            if (objBoxScores.Select(x => x.TeamGames).FirstOrDefault() != null)
            {
                var TeamList = objBoxScores.SelectMany(x => x.TeamGames).ToList();
                DataTable _dtGamesMapsTable = Utils.ToDataTable<Team>(TeamList);
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLTeamMergeSPName, _dtGamesMapsTable);
            }
            //Matches
            if (objBoxScores.Select(x => x.Matches).FirstOrDefault() != null)
            {
                var MatchesList = objBoxScores.SelectMany(x => x.Matches).ToList();
                //MatchBans
                if (MatchesList.Select(x => x.MatchBans).FirstOrDefault() != null)
                {
                    var MatchBansList = MatchesList.SelectMany(x => x.MatchBans).ToList();
                    //ChampionInfo
                    if (MatchBansList.Select(x => x.Champion).FirstOrDefault() != null)
                    {
                        ////var ChampionList = MatchBansList.Select(x => x.Champion).ToList();
                        ////DataTable _dtChampionTable = Utils.ToDataTable<ChampionInfo>(ChampionList);
                        ////blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLChampionInfoMergeSPName, _dtChampionTable);

                        ////Implement below section instead of above section due to duplicate record in ChampionsInfoList
                        //// Seprate method are availabel to udpate ChampionInfo
                        //List<ChampionInfo> ObjChampion = MatchBansList.Select(x => x.Champion).ToList();
                        //var ChampionList = ObjChampion.GroupBy(d => new { d.ChampionId, d.Name, d.Title })
                        //               .Select(m => new { m.Key.ChampionId, m.Key.Name, m.Key.Title }).ToList();

                        //List<ChampionInfo> UniqueChampionInfo = new List<ChampionInfo>();
                        //ChampionInfo objChampionInfo = null;
                        //foreach (var item in ChampionList)
                        //{
                        //    objChampionInfo = new ChampionInfo();
                        //    objChampionInfo.ChampionId = item.ChampionId;
                        //    objChampionInfo.Name = item.Name;
                        //    objChampionInfo.Title = item.Title;
                        //    UniqueChampionInfo.Add(objChampionInfo);
                        //}
                        //DataTable _dtChampionTable = Utils.ToDataTable<ChampionInfo>(UniqueChampionInfo);
                        //blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLChampionInfoMergeSPName, _dtChampionTable);
                    }

                    //MatchBans
                    //DataTable _dtMatchBansTable = Utils.ToDataTable<MatchBan>(MatchBansList);
                    //_dtMatchBansTable.Columns.Remove("Champion");
                    //blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLMatchBanMergeSPName, _dtMatchBansTable);

                    //Implement below section instead of above section due to duplicate record in MatchBanList
                    List<MatchBan> ObjMatchBan = MatchesList.SelectMany(x => x.MatchBans).ToList();
                    var MatchBanList = ObjMatchBan.GroupBy(d => new { d.MatchId, d.TeamId, d.ChampionId })
                                   .Select(m => new { m.Key.MatchId, m.Key.TeamId, m.Key.ChampionId }).ToList();

                    List<MatchBan> UniqueMatchBan = new List<MatchBan>();
                    MatchBan objMatchBan = null;
                    foreach (var item in MatchBanList)
                    {
                        objMatchBan = new MatchBan();
                        objMatchBan.MatchId = item.MatchId;
                        objMatchBan.TeamId = item.TeamId;
                        objMatchBan.ChampionId = item.ChampionId;
                        UniqueMatchBan.Add(objMatchBan);
                    }
                    DataTable _dtMatchBansTable = Utils.ToDataTable<MatchBan>(UniqueMatchBan);
                    _dtMatchBansTable.Columns.Remove("Champion");
                    blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLMatchBanMergeSPName, _dtMatchBansTable);
                }

                //PlayerMatches
                if (MatchesList.Select(x => x.PlayerMatches).FirstOrDefault() != null)
                {
                    var PlayerMatchesList = MatchesList.SelectMany(x => x.PlayerMatches).ToList();

                    //Seprate method are availeble to update ChampionInfo Item and Spells
                    //ChampionInfo
                    //if (PlayerMatchesList.Select(x => x.Champion).FirstOrDefault() != null)
                    //{
                    //    var ChampionList = PlayerMatchesList.Select(x => x.Champion).ToList();
                    //    DataTable _dtChampionTable = Utils.ToDataTable<ChampionInfo>(ChampionList);
                    //    blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLChampionInfoMergeSPName, _dtChampionTable);
                    //}
                    ////Item
                    //if (PlayerMatchesList.Select(x => x.Items).FirstOrDefault() != null)
                    //{
                    //    var ItemsList = PlayerMatchesList.SelectMany(x => x.Items).ToList();
                    //    DataTable _dtItemTable = Utils.ToDataTable<Item>(ItemsList);
                    //    blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLItemMergeSPName, _dtItemTable);
                    //}
                    ////Spells
                    //if (PlayerMatchesList.Select(x => x.Spells).FirstOrDefault() != null)
                    //{
                    //    var SpellsList = PlayerMatchesList.SelectMany(x => x.Spells).ToList();
                    //    DataTable _dtSpellsTable = Utils.ToDataTable<Spell>(SpellsList);
                    //    blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLSpellMergeSPName, _dtSpellsTable);
                    //}

                    //PlayerMatches
                    DataTable _dtPlayerMatchesTable = Utils.ToDataTable<PlayerMatch>(PlayerMatchesList);
                    _dtPlayerMatchesTable.Columns.Remove("Champion");
                    _dtPlayerMatchesTable.Columns.Remove("Items");
                    _dtPlayerMatchesTable.Columns.Remove("Spells");
                    blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLPlayerMatchMergeSPName, _dtPlayerMatchesTable);
                }

                //DataTable _dtGamesMapsTable = Utils.ToDataTable<Match>(MatchesList);
                //_dtGamesMapsTable.Columns.Remove("MatchBans");
                //_dtGamesMapsTable.Columns.Remove("PlayerMatches");
                //blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLMatchMergeSPName, _dtGamesMapsTable);

                //Implement below section instead of above section due to duplicate record in MatchBanList
                List<Match> objMatchesList = objBoxScores.SelectMany(x => x.Matches).ToList();
                var MatcheList = objMatchesList.GroupBy(d => new { d.GameId, d.Number, d.MapName, d.WinningTeamId, d.GameVersion })
                               .Select(m => new { m.Key.GameId, m.Key.Number, m.Key.MapName, m.Key.WinningTeamId, m.Key.GameVersion }).ToList();

                List<Match> UniqueMatch = new List<Match>();
                Match objMatch = null;
                foreach (var item in MatcheList)
                {
                    objMatch = new Match();
                    objMatch.GameId = item.GameId;
                    objMatch.Number = item.Number;
                    objMatch.MapName = item.MapName;
                    objMatch.WinningTeamId = item.WinningTeamId;
                    objMatch.GameVersion = item.GameVersion;
                    UniqueMatch.Add(objMatch);
                }
                DataTable _dtGamesMapsTable = Utils.ToDataTable<Match>(UniqueMatch);
                _dtGamesMapsTable.Columns.Remove("MatchBans");
                _dtGamesMapsTable.Columns.Remove("PlayerMatches");
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLMatchMergeSPName, _dtGamesMapsTable);
            }            
            return blnIsSuccess;
        }
        public bool UpdateBoxScoresbyDate(DateTime Date)
        {
            BoxScores objBoxScores = GetBoxScoresbyDate(Date);
            //Game
            var GameList = objBoxScores.Select(x => x.Game).ToList();
            DataTable _dtGamesTable = Utils.ToDataTable<Game>(GameList);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLGameMergeSPName, _dtGamesTable);

            //Matches
            if (objBoxScores.Select(x => x.Matches).FirstOrDefault() != null)
            {
                var MapsList = objBoxScores.SelectMany(x => x.Matches).ToList();
                DataTable _dtGamesMapsTable = Utils.ToDataTable<Match>(MapsList);
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLMatchMergeSPName, _dtGamesMapsTable);
            }
            //PlayerGames
            if (objBoxScores.Select(x => x.PlayerGames).FirstOrDefault() != null)
            {
                var PlayerGamesList = objBoxScores.SelectMany(x => x.PlayerGames).ToList();
                DataTable _dtGamesMapsTable = Utils.ToDataTable<PlayerGame>(PlayerGamesList);
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLPlayerGameMergeSPName, _dtGamesMapsTable);
            }
            //TeamGames
            if (objBoxScores.Select(x => x.TeamGames).FirstOrDefault() != null)
            {
                var TeamList = objBoxScores.SelectMany(x => x.TeamGames).ToList();
                DataTable _dtGamesMapsTable = Utils.ToDataTable<Team>(TeamList);
                blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLTeamMergeSPName, _dtGamesMapsTable);
            }
            return blnIsSuccess;
        }
        public bool UpdateChampions()
        {
            Champions objChampions = GetChampions();
            DataTable _dtTable = Utils.ToDataTable<Champion>(objChampions);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLChampionMergeSPName, _dtTable);
            return blnIsSuccess;
        }
        public bool UpdateItems()
        {
            Items objItems = GetItems();
            DataTable _dtTable = Utils.ToDataTable<Item>(objItems);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLItemMergeSPName, _dtTable);
            return blnIsSuccess;
        }
        public bool UpdateSpells()
        {
            Spells objSpells = GetSpells();
            DataTable _dtTable = Utils.ToDataTable<Spell>(objSpells);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLSpellMergeSPName, _dtTable);
            return blnIsSuccess;
        }
        public bool UpdateProjectedPlayerGameStatsbyDate(DateTime Date)
        {
            PlayerGameProjections objPlayerGameProjections = GetProjectedPlayerGameStatsbyDate(Date);
            DataTable _dtTable = Utils.ToDataTable<PlayerGameProjection>(objPlayerGameProjections);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLPlayerGameProjectionMergeSPName, _dtTable);
            return blnIsSuccess;
        }
        public bool UpdateProjectedPlayerGameStatsbyDateAndPlayerId(DateTime Date, int playerid)
        {
            PlayerGameProjections objPlayerGameProjections = GetProjectedPlayerGameStatsbyDateAndPlayerId(Date, playerid);
            DataTable _dtTable = Utils.ToDataTable<PlayerGameProjection>(objPlayerGameProjections);
            bool blnIsSuccess = DBHelper.ExecuteSPForBulkUpdate(ConnectionString, CONTANTS.LOLPlayerGameProjectionMergeSPName, _dtTable);
            return blnIsSuccess;
        }

        #endregion Update From Api

        #region Get Api Data

        private Areas GetAreas()
        {
            return sportDataLOLClient.AreasServices.GetAreas();
        }
        private Competitions GetCompetition()
        {
            return sportDataLOLClient.CompetitionServices.GetCompetition();
        }
        private Memberships GetMembershipActive()
        {
            return sportDataLOLClient.MembershipServices.GetActiveMemberships();
        }
        private Memberships GetHistoricalMemberships()
        {
            return sportDataLOLClient.MembershipServices.GetHistoricalMemberships();
        }
        private Memberships GetMembershipsByTeam(int TeamID)
        {
            return sportDataLOLClient.MembershipServices.GetMembershipsByTeam(TeamID);
        }
        private Memberships GetHistoricalMembershipsByTeam(int TeamID)
        {
            return sportDataLOLClient.MembershipServices.GetHistoricalMembershipsByTeam(TeamID);
        }
        private Players GetPlayersByPlayerID(int PlayerId)
        {
            return sportDataLOLClient.PlayerServices.GetPlayersByPlayerID(PlayerId);
        }
        private Players GetPlayers()
        {
            return sportDataLOLClient.PlayerServices.GetPlayers();
        }
        private Players GetPlayersbyTeam(int TeamID)
        {
            return sportDataLOLClient.PlayerServices.GetPlayersbyTeam(TeamID);
        }
        private SeasonTeams GetSeasonTeams(int SeasonId)
        {
            return sportDataLOLClient.SeasonServices.GetSeasonTeams(SeasonId);
        }
        private Standings GetStandings(int RoundId)
        {
            return sportDataLOLClient.StandingsServices.GetStandings(RoundId);
        }
        private Teams GetTeams()
        {
            return sportDataLOLClient.TeamServices.GetTeams();
        }
        private Venues GetVenues()
        {
            return sportDataLOLClient.VenueServices.GetVenues();
        }
        private CompetitionDetails GetCompetitionDetails(int competitionid)
        {
            return sportDataLOLClient.CompetitionServices.GetCompetitionDetails(competitionid);
        }
        private Games GetGamebyDate(DateTime Date)
        {
            return sportDataLOLClient.GameServices.GetGamebyDate(Date);
        }
        private Schedules GetSchedule(int RoundId)
        {
            return sportDataLOLClient.ScheduleServices.GetSchedule(RoundId);
        }
        private BoxScores GetBoxScoresbyGameId(int GameId)
        {
            return sportDataLOLClient.BoxScoreServices.GetBoxScoresbyGameId(GameId);
        }
        private BoxScores GetBoxScoresbyDate(DateTime Date)
        {
            return sportDataLOLClient.BoxScoreServices.GetBoxScoresbyDate(Date);
        }
        private Champions GetChampions()
        {
            return sportDataLOLClient.ChampionServices.GetChampions();
        }
        private Items GetItems()
        {
            return sportDataLOLClient.ItemServices.GetItems();
        }
        private Spells GetSpells()
        {
            return sportDataLOLClient.SpellServices.GetSpells();
        }
        private PlayerGameProjections GetProjectedPlayerGameStatsbyDate(DateTime Date)
        {
            return sportDataLOLClient.ProjectionServices.GetProjectedPlayerGameStatsbyDate(Date);
        }
        private PlayerGameProjections GetProjectedPlayerGameStatsbyDateAndPlayerId(DateTime Date, int playerid)
        {
            return sportDataLOLClient.ProjectionServices.GetProjectedPlayerGameStatsbyDateAndPlayerId(Date, playerid);
        }

        #endregion Get Api Data

    }
}
