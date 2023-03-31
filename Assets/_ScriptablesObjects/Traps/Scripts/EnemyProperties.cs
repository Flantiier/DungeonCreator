using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Enemy Properties", menuName = "Traps/Enemies/Enemy Properties")]
public class EnemyProperties : ScriptableObject
{
    [TitleGroup("Character properties"), Range(50, 400), GUIColor(0, 2, 0.5f)]
    public float health = 100f;

    [TitleGroup("AI properties"), Range(0, 0.1f), GUIColor(2, 1.5f, 0.3f)]
    public float smoothRotation = 0.05f;
    [TitleGroup("AI properties"), Range(0.5f, 10f), GUIColor(2, 1.5f, 0.3f)]
    public float moveSpeed = 2f;
    [TitleGroup("AI properties"), Range(0, 5), GUIColor(2, 1.5f, 0.3f)]
    public float stoppingDistance = 2f;

    [TitleGroup("FOV properties"), Range(1, 20), GUIColor(0.5f, 1.5f, 2f)]
    public float radius = 5f;
    [TitleGroup("FOV properties"), Range(0, 360), GUIColor(0.5f, 1.5f, 2f)]
    public float angle = 170f;
    [TitleGroup("FOV properties"), GUIColor(0.5f, 1.5f, 2f)]
    public LayerMask targetMask;
    [TitleGroup("FOV properties"), GUIColor(0.5f, 1.5f, 2f)]
    public LayerMask obstructionMask;

    [TitleGroup("Patroling properties"), Range(1, 10), GUIColor(0.5f, 1.5f, 0.5f)]
    public float patrolRadius = 3f;
    [TitleGroup("Patroling properties"), Range(0, 5), GUIColor(0.5f, 1.5f, 0.5f)]
    public float patrolSpeed = 1f;
    [TitleGroup("Patroling properties"), Range(0, 10), GUIColor(0.5f, 1.5f, 0.5f)]
    public float backToBaseSpeed = 3f;
    [TitleGroup("Patroling properties"), Range(0, 10), GUIColor(0.5f, 1.5f, 0.5f)]
    public float patrolWait = 3f;
    [TitleGroup("Patroling properties"), Range(0, 2), GUIColor(0.5f, 1.5f, 0.5f)]
    public float patrolPointDistance = 0.4f;

}
