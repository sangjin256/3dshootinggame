using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 목표 : wasd를 누르면 캐릭터를 '카메라 방향에 맞게' 이동시키고 싶다.

    // 구현 순서 :
    // 1. 키보드 입력을 받는다.
    // 2. 입력으로부터 방향을 설정한다.
    // 3. 방향에 따라 플레이어를 이동한다.

    public float MoveSpeed = 7f;

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        Debug.Log("1 " + dir);
        // 메인카메라를 기준으로 방향을 변환한다.
        dir = transform.TransformDirection(dir);
        // TransformDirection : 지역 공간의 벡터를 월드 공간의 벡터로 바꿔주는 함수

        Debug.Log("2 " + dir);

        transform.position += dir * MoveSpeed * Time.deltaTime;
    }
}
