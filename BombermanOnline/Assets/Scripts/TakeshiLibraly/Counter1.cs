
public class Counter
{
    // ===�ϐ�====================================================
    private int count;

    private bool isStop;

    // ===�֐�====================================================

    public void Count()
    {
        if(isStop == false)
        {
            count++;
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

    public bool Point(int point)
    {
        return count > point;
    }
}
