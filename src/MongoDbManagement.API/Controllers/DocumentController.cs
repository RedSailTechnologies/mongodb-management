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

using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDbManagement.API.Models;

namespace MongoDbManagement.API.Controllers
{
    /// <summary>
    /// Document controller.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        /// <summary>
        /// The _logger.
        /// </summary>
        private readonly ILogger<DocumentController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public DocumentController(ILogger<DocumentController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets by document id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="mongoDoc">The mongo doc.</param>
        /// <returns>
        /// The document.
        /// </returns>
        [HttpGet("{id}")]
        public ActionResult<object> GetByDocumentId(string id, MongoDocument mongoDoc)
        {
            MongoClient client = Helper.GetMongoClient(mongoDoc);
            var database = client.GetDatabase(mongoDoc.DatabaseName);
            var collection = database.GetCollection<BsonDocument>(mongoDoc.CollectionName);

            BsonDocument document;

            ObjectId oid;
            if (ObjectId.TryParse(id, out oid))
            {
                document = collection.Find($"{{ _id: ObjectId('{id}') }}").FirstOrDefault();
            }
            else
            {
                document = collection.Find(x => x["_id"] == id).FirstOrDefault();
            }

            if (document == null)
            {
                return null;
            }

            return document.ToString();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        /// <summary>
        /// Creates document.
        /// </summary>
        /// <param name="mongoDoc">The mongo doc.</param>
        /// <returns>
        /// The document.
        /// </returns>
        public ActionResult<object> CreateDocument(MongoDocument mongoDoc)
        {
            MongoClient client = Helper.GetMongoClient(mongoDoc);
            var database = client.GetDatabase(mongoDoc.DatabaseName);
            var collection = database.GetCollection<BsonDocument>(mongoDoc.CollectionName);

            BsonDocument document = null;

            if (!string.IsNullOrWhiteSpace(mongoDoc.Id))
            {
                ObjectId oid;
                if (ObjectId.TryParse(mongoDoc.Id, out oid))
                {
                    document = collection.Find($"{{ _id: ObjectId('{mongoDoc.Id}') }}").FirstOrDefault();
                }
                else
                {
                    document = collection.Find(x => x["_id"] == mongoDoc.Id).FirstOrDefault();
                }
            }

            if (document == null)
            {
                using (var reader = new MongoDB.Bson.IO.JsonReader(mongoDoc.DocumentData))
                {
                    var context = BsonDeserializationContext.CreateRoot(reader);
                    document = BsonDocumentSerializer.Instance.Deserialize(context);
                }
                collection.InsertOne(document);
            }

            var doc = document.ToString();

            return CreatedAtAction(nameof(GetByDocumentId), new { id = document["_id"].ToString(), mongoDoc = mongoDoc }, doc);
        }
    }
}
