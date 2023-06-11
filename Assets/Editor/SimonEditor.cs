using _Scripts.Games;
using UnityEditor;
using UnityEngine;

namespace _Scripts.EditorExtensions
{
    /// <summary>
    /// This can be used to restructure the inspector a little bit.
    /// To group items in collapsable items and change appearences.
    /// Please use with caution! 
    /// </summary>
    [CustomEditor(typeof(Simon))]
    public class SimonEditor : Editor
    {
        bool showGameProperties, showGuessLists, showButtons, showInfos = false;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            showGameProperties = DrawFoldout(true, showGameProperties, "Base Game Properties", new[] { "currentDifficulty", "manager" });

            showGuessLists = DrawFoldout(false, showGuessLists, "Guess Lists", new[] { "displayPattern", "guessPattern", "infoPattern" });

            showButtons = DrawFoldout(false, showButtons, "Buttons", new[] { "buttonsContainer", "blue", "red", "yellow", "green", "middle" });

            showInfos = DrawFoldout(false, showInfos, "Infos", new[] { "inputOverlay", "infoOverlay", "twice", "not", "ok", "timer" });

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Draws a Foldout Group for given serialized properties.
        /// </summary>
        /// <param name="baseGame">Specifies if this is a base game foldout. Only needed once!</param>
        /// <param name="status">The current foldout status.</param>
        /// <param name="groupName">The name for this group show in the inspector.</param>
        /// <param name="serializedFields">List of properties name strings to group.</param>
        /// <returns>The new state of the foldout.</returns>
        private bool DrawFoldout(bool baseGame, bool status, string groupName, string[] serializedFields)
        {
            GUILayout.BeginVertical("Box");
            {
                status = EditorGUILayout.Foldout(status, groupName);
                if (status)
                {
                    if (baseGame)
                    {
                        DrawScriptField();
                    }
                    foreach (string field in serializedFields)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(field));
                    }
                }
            }
            GUILayout.EndVertical();

            return status;
        }

        /// <summary>
        /// Draws the usual script field, without this, it's gone.
        /// </summary>
        private void DrawScriptField()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", serializedObject.FindProperty("m_Script").objectReferenceValue, typeof(Simon), false);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();
        }
    }
}