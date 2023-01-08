using UnityEngine;
using _Scripts.GameplayFeatures;

public class DragAndDropTest : MonoBehaviourSingleton<DragAndDropTest>
{
    public PointerZone zone;
    public DraggableCard card;
    public GameObject draggedObject;
    public bool IsDragging = false;

    private void OnEnable()
    {
        PointerZone.OnEnterPointerZone += OnEnterPointerListener;
        PointerZone.OnExitPointerZone += OnExitPointerListener;
    }

    private void OnDisable()
    {
        PointerZone.OnEnterPointerZone -= OnEnterPointerListener;
        PointerZone.OnExitPointerZone -= OnExitPointerListener;
    }

    private void OnEnterPointerListener()
    {
        card.EnableCard(false);
    }

    private void OnExitPointerListener()
    {
        card.EnableCard(true);
    }

    public void NewObjectSelected(DraggableCard _card, GameObject obj)
    {
        if (draggedObject)
            Destroy(draggedObject);

        card = _card;
        draggedObject = Instantiate(obj);
    }
}
