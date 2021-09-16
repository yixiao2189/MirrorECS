using UnityEngine;
using UnityEngine.AI;

namespace Mirror.Examples.Tanks
{
    public class Tank : NetworkBehaviour
    {
        [Header("Components")]
        public NavMeshAgent agent;
        public Animator animator;

        [Header("Movement")]
        public float rotationSpeed = 100;

        [Header("Firing")]
        public KeyCode shootKey = KeyCode.Space;
        public GameObject projectilePrefab;
        public Transform projectileMount;

        float horizontal = 0;
        float vertical = 0;

        EasyTimer timer = null;

        private void Awake()
        {
            timer = new EasyTimer(5,()=>Time.time);
        }

        void Update()
        {
            // movement for local player
            if (!isLocalPlayer) return;

            bool isFire = false;

#if !CLIENT_TEST_MODE
            // rotate
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            isFire = Input.GetKeyDown(shootKey);
#else
            if (!timer.IsInCD(1,false))
            {
                horizontal = Mathf.Sign(UnityEngine.Random.Range(0,1f)-0.5f);
                vertical = Mathf.Sign(UnityEngine.Random.Range(0, 1f) - 0.5f);
                timer.Use(1,UnityEngine.Random.Range(1,3));
            }


            if (!timer.IsInCD(2,false))
            {
                isFire = true;
                timer.Use(2,1);
            }
            else
            {
                isFire = false;
            }
#endif


                transform.Rotate(0, horizontal * rotationSpeed * Time.deltaTime, 0);
            // move   
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            agent.velocity = forward * Mathf.Max(vertical, 0) * agent.speed;
            animator.SetBool("Moving", agent.velocity != Vector3.zero);

            // shoot
            if (isFire)
            {
                CmdFire();
            }
        }

        bool IsPressedDuraMode(string key,float cd,float duraMin,float duraMax)
        {
            var startTime = PlayerPrefs.GetFloat(key + "_Start", 0);
            var endTime = startTime + PlayerPrefs.GetFloat(key+"_Dura",0);
            if (Time.time >= startTime && Time.time < endTime)
                return true;
            if (Time.time > endTime)
            {
                PlayerPrefs.SetFloat(key + "_Start", Time.time + cd);
                PlayerPrefs.SetFloat(key + "_Dura", Time.time + Random.Range(duraMin, duraMax));
            }
            return false;
        }

        bool IsPressedCDMode(string key, float cdMin,float cdMax)
        {
            var cdTime = PlayerPrefs.GetFloat(key + "_CD", 0);

            if (Time.time < cdTime)
                return false;
            PlayerPrefs.SetFloat(key + "_CD", Time.time + Time.time + Random.Range(cdMin, cdMax));

            return true;
        }


        // this is called on the server
        [Command]
        void CmdFire()
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileMount.position, transform.rotation);
            NetworkServer.Spawn(projectile);
            RpcOnFire();
        }

        // this is called on the tank that fired for all observers
        [ClientRpc]
        void RpcOnFire()
        {
            animator.SetTrigger("Shoot");
        }
    }
}
