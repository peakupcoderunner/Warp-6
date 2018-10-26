using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace CodeRunner.Warp6.PeopleFunctions
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string FunctionHandler(string input, ILambdaContext context)
        {
            return input?.ToUpper();
        }

        public async Task<Person> InsertPerson(Person input, ILambdaContext context)
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            Table people = Table.LoadTable(client, "People");

            Document pDocument = new Document();
            pDocument["Id"] = input?.Id;
            pDocument["Name"] = input?.Name;
            pDocument["Surname"] = input?.Surname;

            await people.PutItemAsync(pDocument);
            return input;
        }

        public async Task<Person> FetchPerson(PersonFetchModel input, ILambdaContext context)
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            Table people = Table.LoadTable(client, "People");
            var pDocument = await people.GetItemAsync(input?.Id);
            return JsonConvert.DeserializeObject<Person>(pDocument.ToJson());
        }
    }

    public class Person
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public class PersonFetchModel
    {
        public string Id { get; set; }
    }
}
