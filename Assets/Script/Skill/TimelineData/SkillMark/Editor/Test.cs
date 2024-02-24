using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

[InitializeOnLoad]
public class CompilationEventListener
{
    static CompilationEventListener()
    {
        CompilationPipeline.compilationFinished += OnCompilationFinished;
        CompilationPipeline.compilationStarted += OnCompilationStarted;
    }

    private static void OnCompilationStarted(object obj)
    {
        var a = 1;
    }

    private static void OnCompilationFinished(object obj)
    {
        var b = 1;
    }
} 