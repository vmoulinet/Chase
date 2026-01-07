using UnityEngine;

public class MirrorFragment : MonoBehaviour
{
    public MirrorFragment linkA;
    public MirrorFragment linkB;

    [HideInInspector] public Transform root;

    void Awake()
    {
        if (root == null)
            root = transform;
    }

    void OnTriggerEnter(Collider other)
    {
        var otherFrag = other.GetComponent<MirrorFragment>();
        if (otherFrag == null) return;
        if (otherFrag != linkA && otherFrag != linkB) return;
        if (otherFrag.root == root) return;

        SnapWith(otherFrag);
    }

    void SnapWith(MirrorFragment other)
    {
        Transform newRoot = new GameObject("MirrorRoot").transform;
        newRoot.position = transform.position;
        newRoot.rotation = transform.rotation;

        root.SetParent(newRoot, true);
        other.root.SetParent(newRoot, true);

        root = newRoot;
        other.root = newRoot;

        var turb = newRoot.gameObject.AddComponent<TurbulenceRoot>();
        turb.Init();
    }
}
