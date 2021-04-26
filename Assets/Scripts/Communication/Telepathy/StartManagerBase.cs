using UnityEngine;

namespace Telepathy
{
    public class StartManagerBase : MonoBehaviour
    {
        public virtual void DeployStartManager() {}
        
        protected virtual void DestroyStartManager() {}

        public void EndCommunications()
        {
            DestroyStartManager();
        }
    }
}