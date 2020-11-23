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

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDbManagement.API.Models;

namespace MongoDbManagement.API.Controllers
{
    /// <summary>
    /// Database controller.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [ApiController]
    [Route("[controller]")]
    public class DatabaseController : ControllerBase
    {
        /// <summary>
        /// The _logger.
        /// </summary>
        private readonly ILogger<DatabaseController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public DatabaseController(ILogger<DatabaseController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets database by document id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="mongoDb">The mongo db.</param>
        /// <returns>
        /// The DbCreated document.
        /// </returns>
        [HttpGet("{id}")]
        public ActionResult<object> GetDatabaseByDocumentId(string id, MongoDatabase mongoDb)
        {
            MongoClient client = Helper.GetMongoClient(mongoDb);
            var database = client.GetDatabase(mongoDb.DatabaseName);
            var collection = database.GetCollection<BsonDocument>("DbCreated");
            var document = collection.Find(x => x["_id"] == id).FirstOrDefault();
            var myObj = BsonSerializer.Deserialize<object>(document);
            return myObj;
        }

        /// <summary>
        /// Creates database.
        /// </summary>
        /// <param name="mongoDb">The mongo db.</param>
        /// <returns>
        /// The DbCreated document.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<object> CreateDatabase(MongoDatabase mongoDb)
        {
            MongoClient client = Helper.GetMongoClient(mongoDb);
            var database = client.GetDatabase(mongoDb.DatabaseName);
            var collection = database.GetCollection<BsonDocument>("DbCreated");
            var document = collection.Find(x => x["_id"] == "DbCreated").FirstOrDefault();
            if (document == null)
            {
                var doc = new Dictionary<string, string>();
                doc.Add("_id", "DbCreated");
                var newDoc = new BsonDocument(doc);
                collection.InsertOne(newDoc);
                document = newDoc;
            }
            var myObj = BsonSerializer.Deserialize<object>(document);
            return CreatedAtAction(nameof(GetDatabaseByDocumentId), new { id = document["_id"], mongoDb = mongoDb }, myObj);
        }
    }
}
