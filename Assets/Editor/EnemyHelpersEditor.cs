using UnityEditor;
using UnityEngine;
using _Scripts.GameplayFeatures.IA;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(MovingEnemy))]
public class EnemyHelpersEditor : OdinEditor
{
    private void OnSceneGUI()
    {
        MovingEnemy enemy = (MovingEnemy)target;
        Handles.color = Color.white;
        Vector3 editorPosition = enemy.transform.position + new Vector3(0f, 1f, 0f);
        Handles.DrawWireArc(editorPosition, Vector3.up, Vector3.forward, 360, enemy.radius);

        Vector3 viewAngle01 = DirectionFromAngle(enemy.transform.eulerAngles.y, -enemy.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(enemy.transform.eulerAngles.y, enemy.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(editorPosition, editorPosition + viewAngle01 * enemy.radius);
        Handles.DrawLine(editorPosition, editorPosition + viewAngle02 * enemy.radius);

        Handles.color = Color.green;
        if (!enemy.CurrentTarget)
            Handles.DrawWireDisc(enemy.transform.position, Vector3.up, enemy.patrolRadius);
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
