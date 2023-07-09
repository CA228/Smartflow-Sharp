using Smartflow.Core.Elements;
using System.Collections.Generic;

namespace Smartflow.Core.Cache
{
    public interface ICache
    {
        void Publish(long templateId);

        Node GetNodeById(long templateId, string nodeId);

        IList<Node> GetNodesByTemplateId(long templateId);

        IDictionary<long, IList<Node>> GetAllTemplateNodes();
    }
}
