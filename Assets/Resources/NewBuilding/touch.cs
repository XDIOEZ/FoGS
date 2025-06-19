using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public GameObject target; // 目标物体
    public float distanceThreshold = 5f; // 距离阈值N米
    public float skillRange = 1f; // 技能释放范围1米

    void Update()
    {
        // 计算Player与Target之间的距离
        float distance = Vector3.Distance(transform.position, target.transform.position);

        // 如果距离小于N米
        if (distance < distanceThreshold)
        {
            // 释放技能
            ReleaseSkill();
        }
    }

    void ReleaseSkill()
    {
        // 获取Player的当前位置和方向
        Vector3 playerPosition = transform.position;
        Vector3 playerForward = transform.forward;
        Vector3 playerRight = transform.right;
        Vector3 playerLeft = -transform.right;

        // 计算技能释放位置
        Vector3 skillPositionFront = playerPosition + playerForward * skillRange;
        Vector3 skillPositionRight = playerPosition + playerRight * skillRange;
        Vector3 skillPositionLeft = playerPosition + playerLeft * skillRange;

        // 在这里实现技能释放的逻辑，例如生成特效或触发事件
        Debug.Log("释放技能在正前方: " + skillPositionFront);
        Debug.Log("释放技能在右方: " + skillPositionRight);
        Debug.Log("释放技能在左方: " + skillPositionLeft);

        // 示例：在技能释放位置生成一个球体
        CreateSkillEffect(skillPositionFront);
        CreateSkillEffect(skillPositionRight);
        CreateSkillEffect(skillPositionLeft);
    }

    void CreateSkillEffect(Vector3 position)
    {
        // 创建一个球体作为技能效果
        GameObject skillEffect = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        skillEffect.transform.position = position;
        skillEffect.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f); // 缩小球体大小
        Destroy(skillEffect, 1f); // 1秒后销毁球体
    }
}