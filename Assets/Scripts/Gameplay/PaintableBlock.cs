using DG.Tweening;
using System.Collections;
using UnityEngine;
using Managers;

public class PaintableBlock : Block
{
    [SerializeField] GameObject paintArea;
    [SerializeField] Animator animator;
    [SerializeField] private float paintDelay;

    private bool isPainted = false;
    private WaitForSeconds waitAmountForPaint;

    public void Start()
    {
        waitAmountForPaint = new WaitForSeconds(paintDelay);
    }

    public override Tween InitializeBlock(Vector3 currentPosition, float distanceBetweenDestination, bool isBallOnThisBlock)
    {
        if (isBallOnThisBlock) Paint();
        return base.InitializeBlock(currentPosition, distanceBetweenDestination, isBallOnThisBlock);
    }

    public IEnumerator PaintBlockCoroutine()
    {
        if (isPainted) yield break;
        Paint();
        animator.SetTrigger("PlayPaintAreaOpening");
        yield return waitAmountForPaint;
        EventManager.Instance.TriggerActionEvent(EventType.BlockPainted);
    }

    public void ResetBlock()
    {
        ChangePaintedStatus(false);
    }

    private void Paint()
    {
        ChangePaintedStatus(true);
    }

    private void ChangePaintedStatus(bool val)
    {
        isPainted = val;
        paintArea.SetActive(val);
    }

}