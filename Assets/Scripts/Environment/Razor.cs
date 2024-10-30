using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Razor : MonoBehaviour
{
    //������ �߻�. ������ local transform���� z������ �޿�
    //�������� �߰������� point�� �̸����� list. point���� ����point�� �Ѿ �� raycast�ؼ� �տ� ���𰡰� ������ ���ڸ����� ����

    private LineRenderer lineRenderer;

    public float damage;

    public int pointSize;
    public List<Vector3> points = new List<Vector3>();
    public LayerMask GroundLayerMask;
    public LayerMask PlayerLayerMask;


    private Vector3 curPoint;
    public Vector3 CCTVPos;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        SetPointsList();
        DrawRazor();
    }

    private void SetPointsList()
    {
        
        lineRenderer.positionCount = pointSize;
        points.Clear();
        curPoint = transform.position;
        points.Add(curPoint);

        for(int i = 0; i < pointSize - 1; i++)
        {
            Vector3 dir = (transform.position - CCTVPos).normalized * 10f;
            Ray ray = new Ray(curPoint, dir);

            Debug.DrawRay(curPoint, dir, Color.black);

            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 10f, GroundLayerMask))
            {
                Debug.Log(hit.point);
                points.Add(hit.point);
                break;
            }
            else if(Physics.Raycast(ray, out hit, 10f, PlayerLayerMask))
            {
                if(hit.collider.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable))
                {
                    points.Add(hit.point);

                    damagable.TakePhysicalDamage((int)damage);  //FixedUpdate�� 50 ~ 60�����������ϱ� �ʴ� �׸�ŭ�� �������� ��.

                    break;
                }
            }
            else
            {
                points.Add(curPoint);
                curPoint = curPoint + dir;
            }
        }
    }

    private void DrawRazor()
    {
        lineRenderer.SetPositions(points.ToArray());
    }
}
