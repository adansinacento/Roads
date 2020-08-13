using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public class CameraMovement : MonoBehaviour
    {
        Vector3 N_est, S_est, E_est, W_est;
        [SerializeField]
        Camera Cam;
        float zInitVal;
        float DesiredSize;
        Vector3 DesiredPos;
        Vector2 MinPos = new Vector2(5.7f, -8.6f), MAxPos = new Vector2(11.23f, -5.9f);

        private void Start()
        {
            if (StaticManager.CameraMovement == null)
                StaticManager.CameraMovement = this;

            if (Cam == null)
                Cam = GetComponent<Camera>();

            zInitVal = transform.position.z;
        }

        private void Update()
        {
            Vector3 correctPos = transform.position;
            if (transform.position.y > MAxPos.y)
            {
                correctPos.y = MAxPos.y;
            }

            if (transform.position.y < MinPos.y)
            {
                correctPos.y = MinPos.y;
            }

            if (transform.position.x > MAxPos.x)
            {
                correctPos.x = MAxPos.x;
            }

            if (transform.position.x < MinPos.x)
            {
                correctPos.x = MinPos.x;
            }

            if (transform.position != correctPos)
            {
                transform.position = correctPos;
            }
        }

        private void FixedUpdate()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            transform.Translate(new Vector3(h, v) * Time.deltaTime * 4);
        }

        public void AddPoint(Vector3 point)
        {
            if (point.x > W_est.x)
                W_est = point;
            if (point.x < E_est.x)
                E_est = point;
            if (point.y > N_est.y)
                N_est = point;
            if (point.y < S_est.y)
                S_est = point;

            centerCamera();
            StartCoroutine("Adjust"); //de momento no necesitamos esto
        }

        void centerCamera()
        {
            var centerY = (S_est.y + N_est.y) / 2f;
            var centerX = (W_est.x + E_est.x) / 2f;

            DesiredPos = new Vector3(centerX, centerY, zInitVal); //centrarla

            
        }

        IEnumerator Adjust()
        {
            var VPPN = Cam.WorldToViewportPoint(N_est + Vector3.up);
            var VPPS = Cam.WorldToViewportPoint(S_est + Vector3.down);
            var VPPE = Cam.WorldToViewportPoint(E_est + Vector3.right);
            var VPPW = Cam.WorldToViewportPoint(W_est + Vector3.left);

            while (Vector3.Distance(DesiredPos, Cam.transform.position) < 0.05f || !isBet0and1(VPPN.y) || !isBet0and1(VPPS.y) || !isBet0and1(VPPE.x) || !isBet0and1(VPPW.x))
            {
                transform.position = Vector3.MoveTowards(transform.position, DesiredPos, Time.deltaTime * 3);
                Cam.orthographicSize += 0.01f;

                VPPN = Cam.WorldToViewportPoint(N_est + Vector3.up);
                VPPS = Cam.WorldToViewportPoint(S_est + Vector3.down);
                VPPE = Cam.WorldToViewportPoint(E_est + Vector3.right);
                VPPW = Cam.WorldToViewportPoint(W_est + Vector3.left);


                yield return new WaitForSeconds(0.1f);
            }
        }

        /// <summary>
        /// Is between 0 and 1
        /// </summary>
        bool isBet0and1(float n)
        {
            return n >= 0.0f && n <= 1.0f;
        }
    }

}
