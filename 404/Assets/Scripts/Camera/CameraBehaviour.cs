using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CamState
{
    FollowingPlayer,
    Focused
}

public class CameraBehaviour : MonoBehaviour
{
    CamState state;
    private Transform focusedObject;

    [SerializeField]
    private PlayerCamera player;

    [SerializeField]
    private float offsetY;
    [SerializeField]
    private float distMaxY;

    [SerializeField]
    private float speedLerpToLeft;
    [SerializeField]
    private float speedLerpToRight;
    [SerializeField]
    private float speedLerpToCenter;
    [SerializeField]
    private float speedLerpToFocus;

    void Awake()
    {
        state = CamState.FollowingPlayer;
    }

    private void Start()
    {

    }

    void FixedUpdate()
    {
        Vector3 newPos = new Vector3();
        if (state == CamState.FollowingPlayer)
        {
            if (!player.GetComponent<SpriteRenderer>().flipX)
            {
                newPos.x = Vector3.Lerp(transform.position, player.CameraPointRight.transform.position, speedLerpToRight * Time.deltaTime).x;
            }
            else //if (player.CameraPointLeft.gameObject.activeInHierarchy)
            {
                newPos.x = Vector3.Lerp(transform.position, player.CameraPointLeft.transform.position, speedLerpToLeft * Time.deltaTime).x;
            }
            /*else
            {
                newPos.x = Vector3.Lerp(transform.position, player.transform.position, speedLerpToCenter * Time.deltaTime).x;
            }*/

            if (player.GetComponent<PlayerMovement>().IsOnGround())
            {
                newPos.y = Vector3.Lerp(transform.position, player.transform.position + new Vector3(0, offsetY, 0), speedLerpToCenter * Time.deltaTime).y;
            }
            else
            {
                float ecart = transform.position.y - Camera.main.orthographicSize + Camera.main.orthographicSize / 3f;

                if (player.GetComponent<Rigidbody2D>().velocity.y < 0f &&
                    player.transform.position.y < ecart)
                {
                    float dist = player.transform.position.y - ecart;
                    
                    if (dist > distMaxY)
                        dist = distMaxY;
                    
                    newPos.y = transform.position.y + dist;
                }
                else // on bouge vers le haut ou vers le bas mais on est pas dans la partie basse de la caméra
                {
                    newPos.y = transform.position.y;
                }
            }
        }
        else if (state == CamState.Focused)
        {
            newPos = Vector3.Lerp(transform.position, focusedObject.position, speedLerpToFocus * Time.deltaTime);
        }

        newPos.z = transform.position.z;
        transform.position = newPos;
    }

    public void EnableFocus(Transform focus)
    {
        focusedObject = focus;
        state = CamState.Focused;
    }

    public void DisableFocus()
    {
        state = CamState.FollowingPlayer;
    }
}
