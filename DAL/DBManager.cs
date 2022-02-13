using MessagingAPI.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingAPI.DAL
{
    public class DBManager
    {
        public async Task<User> ValidateUser(string username, string password)
        {
            User res = null;
            try
            {
                List<MySqlParameter> sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter(){ ParameterName = "username", Value = username, DbType = DbType.String },
                    new MySqlParameter(){ ParameterName = "userPassword", Value = password, DbType = DbType.String },
                };

                var resSets = await DBRepo.DataSource.Procedure(DBRepo.Procedures.ValidateUser, sqlParams)
                                    .ToTableSet(DBRepo.Procedures.ValidateUser)
                                    .ExecuteAsync();

                res = resSets[DBRepo.Procedures.ValidateUser].ToObjects<User>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Write to log...
            }

            return res;
        }

        public async Task<Message> PostMessage(int userId, string message)
        {
            Message res = null;
            try
            {
                List<MySqlParameter> sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter(){ ParameterName = "textMessage", Value = message, DbType = DbType.String },
                    new MySqlParameter(){ ParameterName = "userId", Value = userId, DbType = DbType.Int32 },
                };

                var resSets = await DBRepo.DataSource.Procedure(DBRepo.Procedures.PostMessage, sqlParams)
                                    .ToTableSet(DBRepo.Procedures.PostMessage)
                                    .ExecuteAsync();

                res = resSets[DBRepo.Procedures.PostMessage].ToObjects<Message>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Write to log...
            }

            return res;
        }

        public async Task<User> GetUser(string username)
        {
            User res = null;
            try
            {
                List<MySqlParameter> sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter(){ ParameterName = "username", Value = username, DbType = DbType.String },
                };

                var resSets = await DBRepo.DataSource.Procedure(DBRepo.Procedures.GetUser, sqlParams)
                                    .ToTableSet(DBRepo.Procedures.GetUser)
                                    .ExecuteAsync();

                res = resSets[DBRepo.Procedures.GetUser].ToObjects<User>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Write to log...
            }

            return res;
        }

        public async Task<Follower> AddFollower(int followerUserId, int followingUserId)
        {
            Follower res = null;
            try
            {
                List<MySqlParameter> sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter(){ ParameterName = "followerUserId", Value = followerUserId, DbType = DbType.Int32 },
                    new MySqlParameter(){ ParameterName = "followingUserId", Value = followingUserId, DbType = DbType.Int32 },
                };

                var resSets = await DBRepo.DataSource.Procedure(DBRepo.Procedures.AddFollower, sqlParams)
                                    .ToTableSet(DBRepo.Procedures.AddFollower)
                                    .ExecuteAsync();

                res = resSets[DBRepo.Procedures.AddFollower].ToObjects<Follower>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Write to log...
            }

            return res;
        }

        public async Task<List<Message>> GetFollowFeed(int userId)
        {
            List<Message> res = null;
            try
            {
                List<MySqlParameter> sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter(){ ParameterName = "userId", Value = userId, DbType = DbType.Int32 },
                };

                var resSets = await DBRepo.DataSource.Procedure(DBRepo.Procedures.GetFollowFeed, sqlParams)
                                    .ToTableSet(DBRepo.Procedures.GetFollowFeed)
                                    .ExecuteAsync();

                res = resSets[DBRepo.Procedures.GetFollowFeed].ToObjects<Message>().ToList();

            }
            catch (Exception ex)
            {
                // Write to log...
            }

            return res;
        }

        public async Task<List<Message>> GetGlobalFeed()
        {
            List<Message> res = null;
            try
            {
                var resSets = await DBRepo.DataSource.Procedure(DBRepo.Procedures.GetGlobalFeed, null)
                                    .ToTableSet(DBRepo.Procedures.GetGlobalFeed)
                                    .ExecuteAsync();

                res = resSets[DBRepo.Procedures.GetGlobalFeed].ToObjects<Message>().ToList();

            }
            catch (Exception ex)
            {
                // Write to log...
            }

            return res;
        }

        public async Task<bool> SignOut(int userId)
        {
            bool signedOut = false;
            try
            {
                List<MySqlParameter> sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter(){ ParameterName = "userId", Value = userId, DbType = DbType.Int32 },
                    new MySqlParameter(){ ParameterName = "signedInTime", Value = null, DbType = DbType.DateTime },
                };

                await DBRepo.DataSource.Procedure(DBRepo.Procedures.SetSignIn, sqlParams)
                                    .ExecuteAsync();

                signedOut = true;

            }
            catch (Exception ex)
            {
                // Write to log...
            }

            return signedOut;
        }

        public async Task<Dictionary<int, DateTime>> GetSignedInUsers()
        {
            Dictionary<int, DateTime> res = new Dictionary<int, DateTime>();
            try
            {
                List<MySqlParameter> sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter(){ ParameterName = "hoursBack", Value = 5, DbType = DbType.Int32 },
                };

                var resSets = await DBRepo.DataSource.Procedure(DBRepo.Procedures.GetSignedInUsers, sqlParams)
                                    .ToTableSet(DBRepo.Procedures.GetSignedInUsers)
                                    .ExecuteAsync();

                res = resSets[DBRepo.Procedures.GetSignedInUsers].ToObjects<User>().ToDictionary(u => u.Id, u => u.LastSignIn);

            }
            catch (Exception ex)
            {
                // Write to log...
            }

            return res;
        }

    }
}
