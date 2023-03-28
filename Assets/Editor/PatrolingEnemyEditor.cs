using UnityEngine; 
using UnityEditor;
using _Scripts.GameplayFeatures.IA;

namespace Assets.Editor
{
    [CustomEditor(typeof(PatrolingEnemy))]
    public class PatrolingEnemyEditor : ChasingEnemyEditor
    {
        protected override void OnSceneGUI()
        {
            base.OnSceneGUI();

            PatrolingEnemy enemy = (PatrolingEnemy)target;

            Handles.color = Color.green;
            Handles.DrawWireDisc(enemy.transform.position, Vector3.up, enemy.patrolRadius);
        }
    }
}