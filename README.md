# MongoDb Management 
A simple service to seed a MongoDb for Infrastructure purposes.

# Example API Usage

## Create Basic Database
POST: https://localhost:5001/database  
Authentication: Disabled  
JSON Body:
```
{
    "Host": "sample.url.com",
    "Port": 27017,
    "User": "user-name",
    "Password": "password1",
    "DatabaseName": "Jaws"
}
```

## Verify Basic Database
GET: https://localhost:5001/database  
Authentication: Disabled  
JSON Body:
```
{
    "Host": "sample.url.com",
    "Port": 27017,
    "User": "user-name",
    "Password": "password1",
    "DatabaseName": "Jaws"
}
```

## Create a Document (and Database if Needed) With Existing Id
POST: https://localhost:5001/document  
Authentication: Disabled  
JSON Body:
```
{
    "Host": "sample.url.com",
    "Port": 27017,
    "User": "user-name",
    "Password": "password1",
    "DatabaseName": "Jaws",
    "CollectionName": "Brody",
    "Id": "myid123456",
    "DocumentData": "{\"_id\":\"myid123456\", \"Name\": \"Shiny\", \"And\": \"New\"}"
}
```

## Create a Document (and Database if Needed) Without Existing Id
POST: https://localhost:5001/document  
Authentication: Disabled  
JSON Body:
```
{
    "Host": "sample.url.com",
    "Port": 27017,
    "User": "user-name",
    "Password": "password1",
    "DatabaseName": "Jaws",
    "CollectionName": "Brody",
    "DocumentData": "{\"Name\": \"Shiny\", \"And\": \"New\"}"
}
```

## Verify a Document By Id
GET: https://localhost:5001/document/{_id} (i.e. https://localhost:5001/document/myid123456  )  
Authentication: Disabled  
JSON Body:
```
{
    "Host": "sample.url.com",
    "Port": 27017,
    "User": "user-name",
    "Password": "password1",
    "DatabaseName": "Jaws",
    "CollectionName": "Brody"
}
```

## Create Simple Index
POST: https://localhost:5001/index/create  
Authentication: Disabled  
JSON Body:
```
{
    "Host": "sample.url.com",
    "Port": 27017,
    "User": "user-name",
    "Password": "password1",
    "DatabaseName": "Jaws",
    "CollectionName": "Brody",
    "FieldName": "FirstName"
}
```

## Get Index By Field Name
GET: https://localhost:5001/index/{FieldName} (i.e. https://localhost:5001/index/FirstName )  
Authentication: Disabled  
JSON Body:
```
{
    "Host": "sample.url.com",
    "Port": 27017,
    "User": "user-name",
    "Password": "password1",
    "DatabaseName": "Jaws",
    "CollectionName": "Brody"
}
```

## Get All Indexes
GET: https://localhost:5001/index  
Authentication: Disabled  
JSON Body:
```
{
    "Host": "sample.url.com",
    "Port": 27017,
    "User": "user-name",
    "Password": "password1",
    "DatabaseName": "Jaws",
    "CollectionName": "Brody"
}
```

## Copy Database
POST: https://localhost:5001/copydatabase  
Authentication: Disabled  
JSON Body:
```
{
    "SourceDatabase":{
        "Host":"source.mongo.cosmos.azure.com",
        "Port":10255,
        "UseTls":true,
        "User":"source",
        "Password":"source_password",
        "AuthDatabaseName":"mydb",
        "ReplicaSet":"globaldb",
        "ApplicationName":"@source@",
        "MaxConnectionIdleTimeStringOfMs":"120000",
        "DatabaseName":"mydb"
    },
    "TargetDatabase":{
        "Host":"target.mongo.cosmos.azure.com",
        "Port":10255,
        "UseTls":true,
        "User":"target",
        "Password":"target_password",
        "AuthDatabaseName":"mydb",
        "ReplicaSet":"globaldb",
        "ApplicationName":"@target@",
        "MaxConnectionIdleTimeStringOfMs":"120000",
        "DatabaseName":"mydb"
    },
    "TargetIsAzure": true
}
```

# Considerations

## Creating a Basic Database
Since a Collection is needed for a Database to be browsed, a simple Collection and Document are generated when using the `/database` endpoint.  This is the seeded Document:
```
{
    "_id": "DbCreated"
}
```

## Verifying
All `POST` commands use the `GET` methods to ensure the objects have been inserted to the database before returning `201`.  The `GET` commands are there as a courtesy and are not necessary for typical use.

## TLS
By default, TLS will be enabled for the MongoConnection.  To disable, include an element in the JSON body named `UseTls` and set the value to `false`.  
```
{
    "Host": "sample.url.com",
    "Port": 27017,
    "UseTls": false,
    "User": "user-name",
    "Password": "password1",
    "DatabaseName": "Jaws",
    "CollectionName": "Brody"
}
```

## Replica Set
If needed, include an element in the JSON body named `ReplicaSet` and set the value.  
```
{
    "Host": "sample.url.com",
    "Port": 27017,
    "User": "user-name",
    "Password": "password1",
    "ReplicaSet": "globaldb"
    "DatabaseName": "Jaws"
}
```

## Application Name
If needed, include an element in the JSON body named `ApplicationName` and set the value.  
```
{
    "Host": "sample.url.com",
    "Port": 27017,
    "User": "user-name",
    "Password": "password1",
    "ApplicationName": "@my-azure-cosmos-name@"
    "DatabaseName": "Jaws"
}
```

## Retry Writes
By default, retry writes will be false.  If needed, include an element in the JSON body named `RetryWrites` and set the bool value to `true`.  
```
{
    "Host": "sample.url.com",
    "Port": 27017,
    "User": "user-name",
    "Password": "password1",
    "RetryWrites": true
    "DatabaseName": "Jaws"
}
```

## Max Connection Idle Time (String of Milliseconds)
If needed, include an element in the JSON body named `MaxConnectionIdleTimeStringOfMs` and set the value.  
```
{
    "Host": "sample.url.com",
    "Port": 27017,
    "User": "user-name",
    "Password": "password1",
    "MaxConnectionIdleTimeStringOfMs": "120000"
    "DatabaseName": "Jaws"
}
```
