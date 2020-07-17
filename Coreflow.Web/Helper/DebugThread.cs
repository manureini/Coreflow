namespace Coreflow.Web.Helper
{
    public class DebugThread
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public DebugThread(string pName, int pId)
        {
            Name = pName;
            Id = pId;
        }
    }
}
