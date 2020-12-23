using Coreflow.Helper;
using Coreflow.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Coreflow.Storage
{
    public class RepositoryFlowDefinitionStorage : IFlowDefinitionStorage
    {
        private static readonly HttpClient mClient = new HttpClient();

        private CoreflowRuntime mCoreflow;

        public RepositoryFlowDefinitionStorage(string pUrl)
        {
            mClient.BaseAddress = new Uri(pUrl);
            //  mClient.DefaultRequestHeaders.Accept.Clear();
            mClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void Add(IFlowDefinition pFlowDefinition)
        {
            string serialized = JsonSerializer.Serialize(FlowDefinitionSerializer.Serialize(pFlowDefinition));

            var response = mClient.PostAsync("api/FlowDefinitions/" + pFlowDefinition.Identifier, new StringContent(serialized, Encoding.UTF8, "application/json")).Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception("Flow could not be added!");
        }

        public IEnumerable<IFlowDefinition> GetDefinitions()
        {
            var response = mClient.GetAsync("api/FlowDefinitions").Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var serializedFlows = JsonSerializer.Deserialize<IEnumerable<string>>(responseContent);
                return serializedFlows.Select(f => FlowDefinitionSerializer.Deserialize(f, mCoreflow));
            }

            return Enumerable.Empty<IFlowDefinition>();
        }

        public IFlowDefinition Get(Guid pIdentifier)
        {
            return GetDefinitions().FirstOrDefault(d => d.Identifier == pIdentifier);
        }

        public void Remove(Guid pIdentifier)
        {
            mClient.DeleteAsync("api/FlowDefinitions/" + pIdentifier).Wait();
        }

        public void Update(IFlowDefinition pFlowDefinition)
        {
            Remove(pFlowDefinition.Identifier);
            Add(pFlowDefinition);
        }

        public void SetCoreflow(CoreflowRuntime pCoreflow)
        {
            mCoreflow = pCoreflow;
        }

        public void Dispose()
        {
            mClient.Dispose();
        }       
    }
}
