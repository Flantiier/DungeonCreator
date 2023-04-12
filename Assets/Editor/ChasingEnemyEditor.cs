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
            Handles.DrawWireArc(editorPosition, Vector3.up, Vector3.forward, 360, enemy.properties.radius);

            Vector3 viewAngle01 = DirectionFromAngle(enemy.transform.eulerAngles.y, -enemy.properties.angle / 2);
            Vector3 viewAngle02 = DirectionFromAngle(enemy.transform.eulerAngles.y, enemy.properties.angle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(editorPosition, editorPosition + viewAngle01 * enemy.properties.radius);
            Handles.DrawLine(editorPosition, editorPosition + viewAngle02 * enemy.properties.radius);

            Handles.color = Color.green;
            Handles.DrawWireDisc(enemy.BasePosition, Vector3.up, enemy.properties.patrolRadius);
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

            if (!enemy.properties)
                return;

            Handles.color = Color.white;
            Vector3 editorPosition = enemy.transform.position + new Vector3(0f, 1f, 0f);
            Handles.DrawWireArc(editorPosition, Vector3.up, Vector3.forward, 360, enemy.properties.radius);

            Vector3 viewAngle01 = DirectionFromAngle(enemy.transform.eulerAngles.y, -enemy.properties.angle / 2);
            Vector3 viewAngle02 = DirectionFromAngle(enemy.transform.eulerAngles.y, enemy.properties.angle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(editorPosition, editorPosition + viewAngle01 * enemy.properties.radius);
            Handles.DrawLine(editorPosition, editorPosition + viewAngle02 * enemy.properties.radius);

            Handles.color = Color.green;
            Handles.DrawWireDisc(enemy.BasePosition, Vector3.up, enemy.properties.patrolRadius);
        }
    }

    [CustomEditor(typeof(ArcherEnemy))]
    public class SpearEnemyEditor : ChasingEnemyEditor
    {
        protected override void OnSceneGUI()
        {
            SpearEnemy enemy = (SpearEnemy)target;

            if (!enemy.properties)
                return;

            Handles.color = Color.white;
            Vector3 editorPosition = enemy.transform.position + new Vector3(0f, 1f, 0f);
            Handles.DrawWireArc(editorPosition, Vector3.up, Vector3.forward, 360, enemy.properties.radius);

            Vector3 viewAngle01 = DirectionFromAngle(enemy.transform.eulerAngles.y, -enemy.properties.angle / 2);
            Vector3 viewAngle02 = DirectionFromAngle(enemy.transform.eulerAngles.y, enemy.properties.angle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(editorPosition, editorPosition + viewAngle01 * enemy.properties.radius);
            Handles.DrawLine(editorPosition, editorPosition + viewAngle02 * enemy.properties.radius);

            Handles.color = Color.green;
            Handles.DrawWireDisc(enemy.BasePosition, Vector3.up, enemy.properties.patrolRadius);
        }
    }

    [CustomEditor(typeof(ArcherEnemy))]
    public class SnsEnemyEditor : ChasingEnemyEditor
    {
        protected override void OnSceneGUI()
        {
            SwordAndShieldEnemy enemy = (SwordAndShieldEnemy)target;

            if (!enemy.properties)
                return;

            Handles.color = Color.white;
            Vector3 editorPosition = enemy.transform.position + new Vector3(0f, 1f, 0f);
            Handles.DrawWireArc(editorPosition, Vector3.up, Vector3.forward, 360, enemy.properties.radius);

            Vector3 viewAngle01 = DirectionFromAngle(enemy.transform.eulerAngles.y, -enemy.properties.angle / 2);
            Vector3 viewAngle02 = DirectionFromAngle(enemy.transform.eulerAngles.y, enemy.properties.angle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(editorPosition, editorPosition + viewAngle01 * enemy.properties.radius);
            Handles.DrawLine(editorPosition, editorPosition + viewAngle02 * enemy.properties.radius);

            Handles.color = Color.green;
            Handles.DrawWireDisc(enemy.BasePosition, Vector3.up, enemy.properties.patrolRadius);
        }
    }
}
