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

namespace MongoDbManagement.API.Models
{
    /// <summary>
    /// Mongo index.
    /// </summary>
    /// <seealso cref="MongoDatabase" />
    public class MongoIndex : MongoDatabase
    {
        /// <summary>
        /// Gets or sets the field name.
        /// </summary>
        /// <value>
        /// The field name.
        /// </value>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the ascending.
        /// </summary>
        /// <value>
        /// The ascending.
        /// </value>
        public bool Ascending { get; set; } = true;

        /// <summary>
        /// Gets or sets the unique.
        /// </summary>
        /// <value>
        /// The unique.
        /// </value>
        public bool Unique { get; set; }

        /// <summary>
        /// Gets or sets the index name.
        /// </summary>
        /// <value>
        /// The index name.
        /// </value>
        public string IndexName { get; set; }

        /// <summary>
        /// Gets or sets the sparse.
        /// </summary>
        /// <value>
        /// The sparse.
        /// </value>
        public bool Sparse { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int? Version { get; set; }

        /// <summary>
        /// Gets or sets the expire after seconds.
        /// </summary>
        /// <value>
        /// The expire after seconds.
        /// </value>
        public int ExpireAfterSeconds { get; set; }

        /// <summary>
        /// Gets or sets the storage engine.
        /// </summary>
        /// <value>
        /// The storage engine.
        /// </value>
        public string StorageEngine { get; set; }
    }
}