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
    /// Database interface.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the use tls.
        /// </summary>
        /// <value>
        /// The use tls.
        /// </value>
        public bool UseTls { get; set; }

        /// <summary>
        /// Gets or sets the use scram sha256.
        /// </summary>
        /// <value>
        /// The use scram sha256.
        /// </value>
        public bool UseScramSha256 { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the auth database name.
        /// </summary>
        /// <value>
        /// The auth database name.
        /// </value>
        public string AuthDatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the replica set.
        /// </summary>
        /// <value>
        /// The replica set.
        /// </value>
        public string ReplicaSet { get; set; }

        /// <summary>
        /// Gets or sets the retry writes.
        /// </summary>
        /// <value>
        /// The retry writes.
        /// </value>
        public bool RetryWrites { get; set; }

        /// <summary>
        /// Gets or sets the max connection idle time string of ms.
        /// </summary>
        /// <value>
        /// The max connection idle time string of ms.
        /// </value>
        public string MaxConnectionIdleTimeStringOfMs { get; set; }

        /// <summary>
        /// Gets or sets the application name.
        /// </summary>
        /// <value>
        /// The application name.
        /// </value>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        /// <value>
        /// The database name.
        /// </value>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the collection name.
        /// </summary>
        /// <value>
        /// The collection name.
        /// </value>
        public string CollectionName { get; set; }
    }
}