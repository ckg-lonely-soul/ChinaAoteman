using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class AnimatorStateNameEditor : EditorWindow
{
    private Animator animator;

    [MenuItem("GunGameTool/Animator State Name Formatter")]
    public static void ShowWindow()
    {
        GetWindow<AnimatorStateNameEditor>("Animator State Name Formatter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Format Animator State Names", EditorStyles.boldLabel);

        animator = (Animator)EditorGUILayout.ObjectField("Animator", animator, typeof(Animator), true);

        if (animator == null)
        {
            EditorGUILayout.HelpBox("Please assign an Animator component.", MessageType.Warning);
            return;
        }

        if (GUILayout.Button("Format State Names"))
        {
            FormatStateNames();
        }
    }

    private void FormatStateNames()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator is not assigned.");
            return;
        }

        AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;
        if (controller == null)
        {
            Debug.LogWarning("The Animator does not have an Animator Controller assigned.");
            return;
        }

        foreach (AnimatorControllerLayer layer in controller.layers)
        {
            AnimatorStateMachine stateMachine = layer.stateMachine;
            FormatStateMachine(stateMachine);
        }

        Debug.Log("Animator state names formatted successfully.");
    }

    private void FormatStateMachine(AnimatorStateMachine stateMachine)
    {
        foreach (ChildAnimatorState state in stateMachine.states)
        {
            string originalName = state.state.name;
            string formattedName = char.ToUpper(originalName[0]) + originalName.Substring(1);
            state.state.name = formattedName;
            Debug.Log($"Formatted state name: {originalName} -> {formattedName}");
        }

        foreach (ChildAnimatorStateMachine subStateMachine in stateMachine.stateMachines)
        {
            FormatStateMachine(subStateMachine.stateMachine);
        }
    }
}
