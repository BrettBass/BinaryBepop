using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet _planet;

    Editor _shapeEditor;
    Editor _colorEditor;


    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                _planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate Planet"))
            _planet.GeneratePlanet();

        DrawSettingsEditor(_planet.ShapeSettings, _planet.OnShapeSettingsUpdate, ref _planet.ShapeSettingsAccordion, ref _shapeEditor);
        DrawSettingsEditor(_planet.ColorSettings, _planet.OnColorSettingsUpdate, ref _planet.ColorSettingsAccordion, ref _colorEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdate, ref bool accordion, ref Editor e)
    {
        if (settings != null)
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                accordion = EditorGUILayout.InspectorTitlebar(accordion, settings); // title bar
                if (accordion)
                {
                    CreateCachedEditor(settings, null, ref e);
                    e.OnInspectorGUI();

                    if (check.changed)
                    {
                        if (onSettingsUpdate != null) // thanks c#, can't just check instance , have to compare to null -_-
                            onSettingsUpdate();
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        _planet = (Planet)target;
    }
}
