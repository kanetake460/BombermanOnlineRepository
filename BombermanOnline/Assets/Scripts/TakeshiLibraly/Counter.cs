using UnityEngine;

public class Timer
{
    // ===�ϐ�====================================================
    private float count;

    private bool isStop;

    // ===�֐�====================================================

    public void Count()
    {
        if(isStop == false)
        {
            count += Time.deltaTime;
        }
    }

    public void Finish()
    {
        isStop = true;
        count = 0;
    }

    public void ReStart()
    {
        isStop= false;
        count = 0;
    }

    public void Reset()
    {
        count = 0;
    }

    public bool Point(float point)
    {
        return count > point;
    }
}
