// MIT License
//
// Copyright (c) 2020 RedSail Technologies
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDbManagement.API.Models;

namespace MongoDbManagement.API
{
    /// <summary>
    /// Helper.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Gets mongo client.
        /// </summary>
        /// <param name="database">The database.</param>
        public static MongoClient GetMongoClient(IDatabase database)
        {
            MongoClientSettings settings = new MongoClientSettings();
            var hosts = database.Host.Split(',');
            if (hosts.Length == 1)
            {
                settings.Server = new MongoServerAddress(database.Host, database.Port);
            }
            else
            {
                var serverList = new List<MongoServerAddress>();
                foreach (var host in hosts)
                {
                    serverList.Add(new MongoServerAddress(host, database.Port));
                }
                settings.Servers = serverList;
            }
            settings.UseTls = database.UseTls;
            settings.RetryWrites = false;

            MongoIdentity identity = new MongoInternalIdentity(database.AuthDatabaseName ?? database.DatabaseName, database.User);
            MongoIdentityEvidence evidence = new PasswordEvidence(database.Password);

            var scramShaType = database.UseScramSha256 ? "SCRAM-SHA-256" : "SCRAM-SHA-1";

            settings.Credential = new MongoCredential(scramShaType, identity, evidence);

            if (!string.IsNullOrWhiteSpace(database.ReplicaSet))
            {
                settings.ReplicaSetName = database.ReplicaSet;
            }

            if (!string.IsNullOrWhiteSpace(database.ApplicationName))
            {
                settings.ApplicationName = database.ApplicationName;
            }

            if (!string.IsNullOrWhiteSpace(database.MaxConnectionIdleTimeStringOfMs))
            {
                double ms;
                if (Double.TryParse(database.MaxConnectionIdleTimeStringOfMs, out ms))
                {
                    settings.MaxConnectionIdleTime = TimeSpan.FromMilliseconds(ms);
                }
            }

            settings.RetryWrites = database.RetryWrites;
            settings.MinConnectionPoolSize = 0;

            return new MongoClient(settings);
        }
    }
}