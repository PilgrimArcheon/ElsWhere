using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Spline : MonoBehaviour
{
    public List<Transform> ps;
    public List<float> lens;
    
    float splineLength;

    [Range(0 , 0.999f)] public float off = 0;

    // Start is called before the first frame update
    void Start()
    {
        ps = new List<Transform>();
        lens = new List<float>();
        ps.AddRange(transform.GetComponentsInChildren<Transform>());
        ps.RemoveAt(0);

        splineLength = 0;
        for (int i = 0; i < ps.Count - 3; i++)
        {
            lens.Add(calculateSegLength(i));
            splineLength += lens[i];
        }
    }
    void OnEnable()
    {
        ps = new List<Transform>();
        lens = new List<float>();
        ps.AddRange(transform.GetComponentsInChildren<Transform>());
        ps.RemoveAt(0);


        splineLength = 0;
        lens.Clear();
        for (int i = 0; i < ps.Count - 3; i++)
        {
            lens.Add(calculateSegLength(i));
            splineLength += lens[i];
        }
    }

    void Update()
    {
        if (ps.Count != transform.childCount)
        {
            ps = new List<Transform>();
            lens.Clear();
            ps.AddRange(transform.GetComponentsInChildren<Transform>());
            ps.RemoveAt(0);

            splineLength = 0;
            for (int i = 0; i < ps.Count - 3; i++)
            {
                lens.Add(calculateSegLength(i));
                splineLength += lens[i];
            }
        }
    }
    public float GetLength()
    {
        float len = 0;
        for (int i = 0; i < ps.Count - 3; i++)
        {
            len += calculateSegLength(i);
        }
        return len;
    }
    
    public Vector3 GetSplinePoint(float t)
    {
        int p0, p1, p2, p3;
        p1 = (int)t + 1;
        p2 = p1 + 1;
        p3 = p2 + 1;
        p0 = p1 - 1;

        t = t - (int)t;

        float tt = t * t;
        float ttt = tt * t;

        float q1 = -ttt + 2.0f * tt - t;
        float q2 = 3.0f * ttt - 5.0f * tt + 2.0f;
        float q3 = -3.0f * ttt + 4.0f * tt + t;
        float q4 = ttt - tt;

        Vector3 vec = 0.5f * (ps[p0].position * q1 + ps[p1].position * q2 +
            ps[p2].position * q3 + ps[p3].position * q4);
        return vec;
    }

    public Vector3 GetSplineSlope(float t)
    {
        int p0, p1, p2, p3;
        p1 = (int)t + 1;
        p2 = p1 + 1;
        p3 = p2 + 1;
        p0 = p1 - 1;

        t = t - (int)t;

        float tt = t * t;
        float ttt = tt * t;

        float q1 = -3.0f * tt + 4.0f * t - 1.0f;

        float q2 = 9.0f * tt - 10.0f * t;

        float q3 = -9.0f * tt + 8.0f * t + 1.0f;

        float q4 = 3.0f * tt - 2.0f *t;

        Vector3 vec = 0.5f * (ps[p0].position * q1 + ps[p1].position * q2 +
            ps[p2].position * q3 + ps[p3].position * q4);
        return vec.normalized;
    }

    float calculateSegLength(int node)
    {
        float length = 0.0f;
        float stepsize = 0.001f;

        Vector3 old_point, new_point;
        old_point = GetSplinePoint((float)node);

        for (float t = 0; t < 1.0f; t += stepsize)
        {
            new_point = GetSplinePoint((float)node + t);
            length += Vector3.Magnitude(new_point - old_point);
            old_point = new_point;
        }
        return length;
    }
    public float getNormalizedOffset(float p)
    {
        p *= splineLength;
        int i = 0;
        while(p > lens[i])
        {
            p -= lens[i];
            i++;
        }
        return (float)i + (p / lens[i]);
    }
    public float getLengthOffsetLen(float p)
    {
        int i = 0;
        while (p > lens[i])
        {
            p -= lens[i];
            i++;
        }
        return (float)i + (p / lens[i]);
    }
    private void OnDrawGizmos()
    {
        if (ps.Count < 1) return;

        Gizmos.color = new Color(0, 1, 0, 0.2f);

        for (float t = 0.005f; t < 0.999f; t += 0.005f)
        {
            float o = getNormalizedOffset(t);
            Vector3 pos = GetSplinePoint(o);
            Vector3 norm = GetSplineSlope(o);
            Vector3 ortho = new Vector3(norm.z, 0, -norm.x);

            float lo = getNormalizedOffset(t - 0.005f);
            Vector3 lpos = GetSplinePoint(lo);
            Vector3 lnorm = GetSplineSlope(lo);
            Vector3 lortho = new Vector3(lnorm.z, 0, -lnorm.x);

            Gizmos.DrawLine(lpos, pos);
            Gizmos.DrawLine(lpos + lortho * 3.5f, pos + ortho * 3.5f);
            Gizmos.DrawLine(lpos + lortho * -3.5f, pos + ortho * -3.5f);

            Gizmos.DrawLine(lpos, lpos + lortho * 3.5f);
            Gizmos.DrawLine(lpos, lpos + lortho * -3.5f);
            //Gizmos.DrawWireSphere(pos, 0.5f);
        }

        Gizmos.color = Color.red;

        float offset = getNormalizedOffset(off);
        Vector3 p1 = GetSplinePoint(offset);
        Gizmos.DrawWireSphere(p1, 0.7f);
    }
}
