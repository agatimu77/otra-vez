using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path : MonoBehaviour {

    [SerializeField, HideInInspector]
    public List<Vector2> points;

    public Path(Vector2 centro)
    {
        points = new List<Vector2>
        {
        centro + Vector2.left,
        centro + (Vector2.left + Vector2.up)*.5f,
        centro + (Vector2.right + Vector2.down)*.5f,
        centro + Vector2.right
        };


    }

    public Vector2 this [int i] {
        get
        {
            return points[i];
        }
    }

    public int NumPoints  {

        get
        {
            return points.Count;
        }
    } 


    public int NumSegments
    {
        get
        {
            return (NumPoints - 4) / 3 + 1;
        }
    }

    public void AddSegment(Vector2 anchorPoint)
    {
        points.Add(points[points.Count - 1] + points[points.Count - 1] - points[points.Count - 2]);
        points.Add((points[points.Count - 1] + anchorPoint) / 2);
        points.Add(anchorPoint);

    }

    public Vector2[] GetPointsInSegment(int i)
    {
        return new Vector2[]
        {
            points[i*3],points[i*3+1],points[i*3+2], points[i*3+3]

        };
    }

    public void MovePoint(int i, Vector2 pos)
    {
        Vector2 deltaMove = pos - points[i];
        points[i] = pos;

        if (i%3==0)
        {
            if (i+1<points.Count&&i-1>=0)
            {
                points[i + 1] += deltaMove;
                points[i - 1] += deltaMove;
            }
        }
        else
        {
            bool nextPointIAnchorPoint = (i + 1) % 3 == 0;
            int correspondingControlIndex = nextPointIAnchorPoint ? i+ 2 : i - 2;
            int anchorIndex = nextPointIAnchorPoint ? i + 1 : i - 1;

            if (correspondingControlIndex>=0&&correspondingControlIndex<points.Count)
            {
                float dist = (points[anchorIndex] - points[correspondingControlIndex]).magnitude;
                Vector2 dir = (points[anchorIndex] - pos).normalized;
                points[correspondingControlIndex] = points[anchorIndex] + dist * dir;
            }
        }
    }
}
