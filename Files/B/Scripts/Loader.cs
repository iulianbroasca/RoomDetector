using UnityEngine;

namespace UI
{
    public class Loader : MonoBehaviour
    {
        public float speed = 50;

        private void Update()
        {
            transform.Rotate(Vector3.back * (speed * Time.deltaTime));
        }
    }
}
