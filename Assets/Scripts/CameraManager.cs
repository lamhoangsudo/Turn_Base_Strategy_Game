using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCamera;
    private void Start()
    {
        BaseAction.OnAnyActionStart += BaseAction_OnAnyActionStart;
        BaseAction.OnAnyActionComplete += BaseAction_OnAnyActionComplete;
        Hide();
    }

    private void BaseAction_OnAnyActionComplete(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void BaseAction_OnAnyActionStart(object sender, System.EventArgs e)
    {
        switch (sender)
        {
            case MoveAction moveAction:
                break;
            case ShootAction shootAction:
                Transform tagetUnit = shootAction.targetUnit.transform;
                Transform shootUnit = shootAction.unit.transform;
                Vector3 shootDir = (tagetUnit.position - shootUnit.position).normalized;
                float moveToRightAmount = 0.6f;
                float moveUpAmount = 1.7f;
                float moveToBackAmount = -1.2f;
                //0.5f on the right side, up 1.7f on the up side, 1f on the back side of shoot direction
                Vector3 cameraActionPosition = shootUnit.transform.position 
                    + Quaternion.Euler(0f, 90f, 0f) * shootDir * moveToRightAmount 
                    + Vector3.up * moveUpAmount 
                    + shootDir * moveToBackAmount;
                actionCamera.transform.position = cameraActionPosition;
                actionCamera.transform.LookAt(tagetUnit.position + Vector3.up * moveUpAmount);
                Show();
                break;
            case SpinAction spinAction: 
                break;
        }
    }

    private void Show()
    {
        actionCamera.SetActive(true);
    }
    private void Hide()
    {
        actionCamera.SetActive(false);
    }
}
