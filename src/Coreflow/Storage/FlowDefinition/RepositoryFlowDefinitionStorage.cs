using Coreflow.Helper;
using Coreflow.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Coreflow.Storage
{
    public class RepositoryFlowDefinitionStorage : IFlowDefinitionStorage
    {
        private static readonly HttpClient mClient = new HttpClient();

        private Coreflow mCoreflow;

        public RepositoryFlowDefinitionStorage(string pUrl)
        {
            mClient.BaseAddress = new Uri(pUrl);
            //  mClient.DefaultRequestHeaders.Accept.Clear();
            mClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void Add(FlowDefinition pFlowDefinition)
        {
            string serialized = JsonConvert.SerializeObject(FlowDefinitionSerializer.Serialize(pFlowDefinition));

            var response = mClient.PostAsync("api/FlowDefinitions", new StringContent(serialized, Encoding.UTF8, "application/json")).Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception("Flow could not be added!");
        }

        public IEnumerable<FlowDefinition> GetDefinitions()
        {
            var response = mClient.GetAsync("api/FlowDefinitions").Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var serializedFlows = JsonConvert.DeserializeObject<IEnumerable<string>>(responseContent);
                return serializedFlows.Select(f => FlowDefinitionSerializer.Deserialize(f, mCoreflow));
            }

            return Enumerable.Empty<FlowDefinition>();
        }

        public void Remove(Guid pIdentifier)
        {
            mClient.DeleteAsync("api/FlowDefinitions/" + pIdentifier).Wait();
        }

        public void SetCoreflow(Coreflow pCoreflow)
        {
            mCoreflow = pCoreflow;
        }

        public void Dispose()
        {
            mClient.Dispose();
        }
    }
}
