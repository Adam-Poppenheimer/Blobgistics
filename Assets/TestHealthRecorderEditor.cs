using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEditor;

using UnityCustomUtilities.Combat;

namespace Assets {

    [CustomEditor(typeof(TestHealthRecorder))]
    [CanEditMultipleObjects]
    public class TestHealthRecorderEditor : HealthRecorderEditor {
    }

}
