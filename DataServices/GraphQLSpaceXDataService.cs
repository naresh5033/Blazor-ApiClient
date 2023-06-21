using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorAPIClient.Dtos;

namespace BlazorAPIClient.DataServices
{
    public class GraphQLSpaceXDataService : ISpaceXDataService
    {
        private readonly HttpClient _httpclient;

        public GraphQLSpaceXDataService(HttpClient httpclient)
        {
            _httpclient = httpclient;
        }

        public async Task<LaunchDto[]> GetAllLaunches() //the qraph ql way of fetching, the following qurying and the post method
        {
            //1.create the query object
           var queryObject = new{
            query  = @"{launches {id is_tentative mission_name launch_date_local}}",
            variables = new {}
           };

            // 2. launch the query object
           var launchQuery = new StringContent( //the string content provides the http content based on the str
               JsonSerializer.Serialize(queryObject),
               Encoding.UTF8,
               "application/json"); //media type

            // 3. send(post) the query object
            var response = await _httpclient.PostAsync("graphql", launchQuery);

            if(response.IsSuccessStatusCode)
            {
                var gqlData = await JsonSerializer.DeserializeAsync<GqlData>  
                    (await response.Content.ReadAsStreamAsync());
                // finally return the launches from the gql data dto
                return gqlData.Data.Launches;
            }
            return null;

        }
    }
}