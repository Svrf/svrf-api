using UnityEngine;

namespace Svrf.Unity.Examples
{
    public class LoadingController : MonoBehaviour
    {
        public GameObject TrackedSvrfModel;

        void Update()
        {
            SvrfModel svrfModel = TrackedSvrfModel.GetComponent<SvrfModel>();

            if (!svrfModel.IsLoading)
            {
                Destroy(gameObject);
            }
        }
    }
}
