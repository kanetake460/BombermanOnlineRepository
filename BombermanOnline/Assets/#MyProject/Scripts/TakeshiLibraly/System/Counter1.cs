
public class Counter
{
    // ===•Ï”====================================================
    private int count;

    private bool isStop;

    // ===ŠÖ”====================================================

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
