using UnityEngine;
using UnityEngine.EventSystems;

public class JobNote : MonoBehaviour, IPointerClickHandler
{
    public Job job; // Job is assigned at runtime by JobGenerator

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        JobDetailsModal.Instance.Show(job);
    }
}
