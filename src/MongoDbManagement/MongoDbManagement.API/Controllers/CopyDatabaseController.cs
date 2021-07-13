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
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbManagement.API.Models;

namespace MongoDbManagement.API.Controllers
{
    /// <summary>
    /// Copy database controller.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [ApiController]
    [Route("[controller]")]
    public class CopyDatabaseController : ControllerBase
    {
        /// <summary>
        /// The _logger.
        /// </summary>
        private readonly ILogger<CopyDatabaseController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyDatabaseController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public CopyDatabaseController(ILogger<CopyDatabaseController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Copies a database.
        /// </summary>
        /// <param name="mongoDatabaseCopy">The mongo database copy model.</param>
        /// <returns>
        /// The 201 or 400.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<object> CopyDatabase(MongoDatabaseCopy mongoDatabaseCopy)
        {
            try
            {
                MongoClient sourceClient = Helper.GetMongoClient(mongoDatabaseCopy.SourceDatabase);
                var sourceDatabase = sourceClient.GetDatabase(mongoDatabaseCopy.SourceDatabase.DatabaseName);

                MongoClient targetClient = Helper.GetMongoClient(mongoDatabaseCopy.TargetDatabase);
                var targetDatabase = targetClient.GetDatabase(mongoDatabaseCopy.TargetDatabase.DatabaseName);

                var sourceCollectionNames = sourceDatabase.ListCollectionNames().ToList();
                foreach (var sourceCollectionName in sourceCollectionNames)
                {
                    var sourceCollection = sourceDatabase.GetCollection<BsonDocument>(sourceCollectionName);
                    var sourceDocuments = sourceCollection.Find(_ => true).ToList();

                    targetDatabase.CreateCollection(sourceCollectionName);
                    var targetCollection = targetDatabase.GetCollection<BsonDocument>(sourceCollectionName);
                    
                    // Slow things down because Azure Cosmos default RU
                    for (int i = 0; i < sourceDocuments.Count; i = i + 100)
                    {
                        var batch = sourceDocuments.Skip(i).Take(100);
                        targetCollection.InsertMany(batch);
                        System.Threading.Thread.Sleep(500);
                    }

                    var sourceIndexes = sourceCollection.Indexes.List().ToList();
                    foreach (var sourceIndex in sourceIndexes)
                    {
                        var key = sourceIndex.GetElement("key");
                        var value = key.Value.AsBsonDocument;
                        var keyName = value.Elements.FirstOrDefault().Name;
                        var sortOrder = value.Elements.FirstOrDefault().Value.AsInt32;

                        IndexKeysDefinition<BsonDocument> keys;
                        if (sortOrder == 1)
                        {
                            keys = Builders<BsonDocument>.IndexKeys.Ascending(keyName);
                        }
                        else
                        {
                            keys = Builders<BsonDocument>.IndexKeys.Descending(keyName);
                        }

                        // TODO: FIND OPTIONS ELEMENT(?) AND DUPLICATE
                        var model = new CreateIndexModel<BsonDocument>(keys);
                        targetCollection.Indexes.CreateOne(model);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCodes.Status400BadRequest;
            }

            return StatusCodes.Status201Created;
        }
    }
}
