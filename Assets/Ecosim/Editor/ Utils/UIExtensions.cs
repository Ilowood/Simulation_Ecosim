using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Ecosim.Editor
{
    public static class UIExtensions 
    {
        public static void TrackName(this Button btn, EntitySpecification spec) 
        {
            var so = new UnityEditor.SerializedObject(spec);
            var prop = so.FindProperty("<Name>k__BackingField");
            if (prop == null) prop = so.FindProperty("m_Name");

            btn.TrackPropertyValue(prop, p => {
                btn.text = string.IsNullOrEmpty(spec.Name) ? spec.name : spec.Name;
            });
        }
    }

}
