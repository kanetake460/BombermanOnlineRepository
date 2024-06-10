using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

public class CanvasBook : StrixBehaviour
{
    [SerializeField] Canvas _canvas;
    [SerializeField] GameObject[] page;
    private int _currentPage = 0;

    public void CallNextPage() {RpcToAll(nameof(NextPage)); }
    [StrixRpc]
    public void NextPage()
    {
        page[_currentPage].SetActive(false);
        _currentPage++;
        if (page[_currentPage] == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            page[_currentPage].SetActive(true);
        }
    }

    public void CallPrevPage() { RpcToAll(nameof(PrevPage)); }
    [StrixRpc]
    public void PrevPage()
    {
        page[_currentPage].SetActive(false);
        _currentPage--;
        if (_currentPage < 0)
        {
            _currentPage = 0;
        }
        page[_currentPage].SetActive(true);
    }


    /// <summary>
    /// •Â‚¶‚Ü‚·
    /// </summary>
    public void CallClose() { RpcToAll(nameof(Close)); }
    [StrixRpc]
    public void Close() { _canvas.gameObject.SetActive(false); }


    /// <summary>
    /// ŠJ‚«‚Ü‚·
    /// </summary>
    public void CallOpen() { RpcToAll(nameof(Open)); }
    [StrixRpc]
    public void Open() { _canvas.gameObject.SetActive(true); }
}
