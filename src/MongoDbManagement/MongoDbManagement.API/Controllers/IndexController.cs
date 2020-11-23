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
    /// Index controller.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [ApiController]
    [Route("[controller]")]
    public class IndexController : ControllerBase
    {
        /// <summary>
        /// The _logger.
        /// </summary>
        private readonly ILogger<IndexController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public IndexController(ILogger<IndexController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets all indexes.
        /// </summary>
        /// <param name="mongoDb">The mongo db.</param>
        /// <returns>The indexes.</returns>
        [HttpGet]
        public ActionResult<object> GetAllIndexes(MongoDatabase mongoDb)
        {
            MongoClient client = Helper.GetMongoClient(mongoDb);
            var database = client.GetDatabase(mongoDb.DatabaseName);
            var collection = database.GetCollection<BsonDocument>(mongoDb.CollectionName);
            var bsonIndexes = collection.Indexes.List().ToList();
            return bsonIndexes.ToJson();
        }

        /// <summary>
        /// Finds if index exists by field name.
        /// </summary>
        /// <param name="fieldName">The field name.</param>
        /// <param name="mongoDb">The mongo db.</param>
        /// <returns>A bool.</returns>
        [HttpGet("{fieldname}")]
        public ActionResult<object> FindIfIndexExistsByFieldName(string fieldName, MongoDatabase mongoDb)
        {
            var indexes = GetAllIndexes(mongoDb).Value.ToJson();
            var indexKey = $"\\\"key\\\" : {{ \\\"{fieldName}\\\"";
            var index = indexes.Contains(indexKey);
            if (index)
            {
                return index;
            }
            else
            {
                Response.StatusCode = 404;
                return index;
            }
        }

        /// <summary>
        /// Creates index.
        /// </summary>
        /// <param name="mongoIndex">The mongo index.</param>
        /// <returns>All Indexes.</returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<object> CreateIndex(MongoIndex mongoIndex)
        {
            MongoClient client = Helper.GetMongoClient(mongoIndex);
            var database = client.GetDatabase(mongoIndex.DatabaseName);
            var collection = database.GetCollection<BsonDocument>(mongoIndex.CollectionName);
            var fieldName = mongoIndex.FieldName;

            IndexKeysDefinition<BsonDocument> keys;

            if (mongoIndex.Ascending)
            {
                keys = Builders<BsonDocument>.IndexKeys.Ascending(fieldName);
            }
            else
            {
                keys = Builders<BsonDocument>.IndexKeys.Descending(fieldName);
            }

            var indexOptions = new CreateIndexOptions
            {
                Unique = mongoIndex.Unique,
                Name = mongoIndex.IndexName,
                Sparse = mongoIndex.Sparse,
                Version = mongoIndex.Version
            };

            if (mongoIndex.ExpireAfterSeconds > 0)
            {
                indexOptions.ExpireAfter = TimeSpan.FromSeconds(mongoIndex.ExpireAfterSeconds);
            }

            if (!string.IsNullOrWhiteSpace(mongoIndex.StorageEngine))
            {
                indexOptions.StorageEngine = BsonDocument.Parse(mongoIndex.StorageEngine);
            }

            var model = new CreateIndexModel<BsonDocument>(keys, indexOptions);
            collection.Indexes.CreateOne(model);

            var indexes = GetAllIndexes(mongoIndex).Value;

            return CreatedAtAction(nameof(FindIfIndexExistsByFieldName), new { fieldName = fieldName, mongoIndex = mongoIndex }, indexes);
        }
    }
}
