using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector.Editor;
using _Scripts.GameplayFeatures.IA;

namespace Assets.Editor
{
    [CustomEditor(typeof(ChasingEnemy))]
    public class ChasingEnemyEditor : OdinEditor
    {
        protected virtual void OnSceneGUI()
        {
            ChasingEnemy enemy = (ChasingEnemy)target;
            Handles.color = Color.white;
            Vector3 editorPosition = enemy.transform.position + new Vector3(0f, 1f, 0f);
            Handles.DrawWireArc(editorPosition, Vector3.up, Vector3.forward, 360, enemy.radius);

            Vector3 viewAngle01 = DirectionFromAngle(enemy.transform.eulerAngles.y, -enemy.angle / 2);
            Vector3 viewAngle02 = DirectionFromAngle(enemy.transform.eulerAngles.y, enemy.angle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(editorPosition, editorPosition + viewAngle01 * enemy.radius);
            Handles.DrawLine(editorPosition, editorPosition + viewAngle02 * enemy.radius);

            Handles.color = Color.green;
            Handles.DrawWireDisc(enemy.BasePosition, Vector3.up, enemy.patrolRadius);
        }

        protected Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
        {
            angleInDegrees += eulerY;

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }

    [CustomEditor(typeof(ArcherEnemy))]
    public class ArcherEnemyEditor : ChasingEnemyEditor
    {
        protected override void OnSceneGUI()
        {
            ArcherEnemy enemy = (ArcherEnemy)target;
            Handles.color = Color.white;
            Vector3 editorPosition = enemy.transform.position + new Vector3(0f, 1f, 0f);
            Handles.DrawWireArc(editorPosition, Vector3.up, Vector3.forward, 360, enemy.radius);

            Vector3 viewAngle01 = DirectionFromAngle(enemy.transform.eulerAngles.y, -enemy.angle / 2);
            Vector3 viewAngle02 = DirectionFromAngle(enemy.transform.eulerAngles.y, enemy.angle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(editorPosition, editorPosition + viewAngle01 * enemy.radius);
            Handles.DrawLine(editorPosition, editorPosition + viewAngle02 * enemy.radius);

            Handles.color = Color.green;
            Handles.DrawWireDisc(enemy.BasePosition, Vector3.up, enemy.patrolRadius);
        }
    }

    [CustomEditor(typeof(ArcherEnemy))]
    public class SpearEnemyEditor : ChasingEnemyEditor
    {
        protected override void OnSceneGUI()
        {
            SpearEnemy enemy = (SpearEnemy)target;
            Handles.color = Color.white;
            Vector3 editorPosition = enemy.transform.position + new Vector3(0f, 1f, 0f);
            Handles.DrawWireArc(editorPosition, Vector3.up, Vector3.forward, 360, enemy.radius);

            Vector3 viewAngle01 = DirectionFromAngle(enemy.transform.eulerAngles.y, -enemy.angle / 2);
            Vector3 viewAngle02 = DirectionFromAngle(enemy.transform.eulerAngles.y, enemy.angle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(editorPosition, editorPosition + viewAngle01 * enemy.radius);
            Handles.DrawLine(editorPosition, editorPosition + viewAngle02 * enemy.radius);

            Handles.color = Color.green;
            Handles.DrawWireDisc(enemy.BasePosition, Vector3.up, enemy.patrolRadius);
        }
    }

    [CustomEditor(typeof(ArcherEnemy))]
    public class SnsEnemyEditor : ChasingEnemyEditor
    {
        protected override void OnSceneGUI()
        {
            SwordAndShieldEnemy enemy = (SwordAndShieldEnemy)target;
            Handles.color = Color.white;
            Vector3 editorPosition = enemy.transform.position + new Vector3(0f, 1f, 0f);
            Handles.DrawWireArc(editorPosition, Vector3.up, Vector3.forward, 360, enemy.radius);

            Vector3 viewAngle01 = DirectionFromAngle(enemy.transform.eulerAngles.y, -enemy.angle / 2);
            Vector3 viewAngle02 = DirectionFromAngle(enemy.transform.eulerAngles.y, enemy.angle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(editorPosition, editorPosition + viewAngle01 * enemy.radius);
            Handles.DrawLine(editorPosition, editorPosition + viewAngle02 * enemy.radius);

            Handles.color = Color.green;
            Handles.DrawWireDisc(enemy.BasePosition, Vector3.up, enemy.patrolRadius);
        }
    }
}
