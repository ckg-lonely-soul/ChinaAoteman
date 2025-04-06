using System.Collections;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Game00_Main game00_Main;
    Animator animator;
    private void Awake()
    {

        DieEvent.Clear();
        game00_Main = GameObject.Find("GameMain").transform.GetOrAddComponent<Game00_Main>();
    }
    private void OnEnable()
    {
        DieEvent.Register((a) => { CameraMoveBoss(a); });
    }
    private void CameraMoveBoss(Animator anima)
    {
        animator = anima;
        StartCoroutine(ICameraMoveBoss());
    }
    private IEnumerator ICameraMoveBoss()
    {
        animator.speed = 0;
        Game00_Gun[] game00_Gun = GameObject.FindObjectsOfType<Game00_Gun>();
        for (int i = 0; i < game00_Gun.Length; i++)
        {
            game00_Gun[i].gameObject.SetActive(false);
        }

        float shanke = 0.3f;
        var baeBose = FindObjectOfType<BossBase>();
        //GameObject obj = baeBose.transform.GetComponentInChildren<AnimEvent>().gameObject;
        GameObject obj = FindObjectOfType<BossBase>().gameObject;

        Vector3 movePos = new Vector3(obj.transform.position.x, obj.transform.position.y + 3f, obj.transform.position.z);
        Vector3 pos = movePos + (transform.position - movePos).normalized * baeBose.GetMoveDis();
        Vector3 rot = new Vector3(obj.transform.position.x, transform.position.y, obj.transform.position.z);
        Quaternion rota = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(rot - transform.position), 1f);
        float dis = Vector3.Distance(this.transform.position, pos);
        while (dis >= 0.05f || rota != transform.rotation)
        {
            dis = Vector3.Distance(this.transform.position, pos);
            transform.position = Vector3.MoveTowards(transform.position, pos, 0.05f);
            rota = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(rot - transform.position), 1f);
            shanke = shanke < 0.3 ? shanke + 0.05f : 0.2f;
            game00_Main.ShakeStart(shanke, 1000);
            animator.speed += 0.001f;
            yield return new WaitForFixedUpdate();
            transform.rotation = Quaternion.Lerp(transform.rotation, rota, 0.5f);
        }

        float sleep = animator.speed;
        //game00_Main.ShakeStart(0.05f, 1000);
        while (sleep < 1)
        {
            sleep += 0.05f;
            animator.speed = sleep;
            shanke = shanke > 0 ? shanke - 0.08f : 0.01f;
            yield return new WaitForSeconds(0.05f);
        }
        game00_Main.ShakeStart(0, 1000);


    }


    #region 新增代码，增加出场怪物的聚焦行为

    // 保存相机的初始位置和旋转
    Vector3 _cameraOriginlocalPos;
    Quaternion _cameraOriginlocalRotation;

    public IEnumerator ICameraMoveEvent(Transform cameraMovePos, AudioClip spawnSource, float delayTime = 3f)
    {
        // 禁用玩家武器
        Game00_Gun[] game00_Gun = GameObject.FindObjectsOfType<Game00_Gun>();
        for (int i = 0; i < game00_Gun.Length; i++)
        {
            game00_Gun[i].gameObject.SetActive(false);
            game00_Gun[i].player.SetCanChangeGun(false); //不能换枪
        }

        // 保存相机的初始位置和旋转
        _cameraOriginlocalPos = transform.localPosition;
        _cameraOriginlocalRotation = transform.localRotation;

        // 说明可以进行聚焦、移动
        if (cameraMovePos != null)
        {
            // 计算目标位置和旋转
            Vector3 movePos = new Vector3(cameraMovePos.position.x, cameraMovePos.position.y + 3f, cameraMovePos.position.z);
            Vector3 pos = movePos + (transform.position - movePos).normalized * 3;
            Vector3 rot = new Vector3(cameraMovePos.position.x, transform.position.y, cameraMovePos.position.z);
            Quaternion rota = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(rot - transform.position), 1f);

            // 移动相机到目标位置
            float dis = Vector3.Distance(this.transform.position, pos);
            while (dis >= 0.05f || Quaternion.Angle(transform.rotation, rota) > 0.05f)
            {
                dis = Vector3.Distance(this.transform.position, pos);
                transform.position = Vector3.MoveTowards(transform.position, pos, 0.05f);
                rota = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(rot - transform.position), 1f);
                yield return new WaitForFixedUpdate();
                transform.rotation = Quaternion.Lerp(transform.rotation, rota, 0.5f);
                
            }

            AudioSource.PlayClipAtPoint(spawnSource,transform.position);

            yield return new WaitForSeconds(delayTime);

            yield return ReturnCameraToPlayer();


        }

        // 重新启用玩家武器
        for (int i = 0; i < game00_Gun.Length; i++)
        {
            if (game00_Gun[i] != null)
            {
                game00_Gun[i].gameObject.SetActive(true);
                game00_Gun[i].player.SetCanChangeGun(true);
            }
            else
            {
                Debug.LogError("有bug,镜头对焦时不应该可以切枪");
            }
                
        }
    }


    public IEnumerator ReturnCameraToPlayer()
    {
        //回归到初始位置和旋转
        while (Vector3.Distance(transform.localPosition, _cameraOriginlocalPos) > 0.05f ||
               Quaternion.Angle(transform.localRotation, _cameraOriginlocalRotation) > 0.1f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _cameraOriginlocalPos, 0.05f);
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, _cameraOriginlocalRotation, 1f);
            yield return new WaitForFixedUpdate();

        }

        // 确保相机完全回到初始位置和旋转
        transform.localPosition = _cameraOriginlocalPos;
        transform.localRotation = _cameraOriginlocalRotation;

    }
    #endregion
}
