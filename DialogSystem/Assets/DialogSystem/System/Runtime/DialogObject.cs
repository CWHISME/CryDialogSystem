/**********************************************************
*Author: wangjiaying
*Date: 2016.8.11
*Func:
**********************************************************/

using UnityEngine;

namespace CryDialog.Runtime
{

    [CreateAssetMenu(fileName = "New Dialog", menuName = "Cry Dialog/Create New Dialog")]
    public class DialogObject : ScriptableObject
    {
        public byte[] _SaveData;

        public string _description = "Not Description.";

        public float _saveVersion = Dialog.SaveVersion;

        public float _zoom = 1f;

        public bool _debugMode = false;

        private Dialog _dialog;
        public Dialog Dialog
        {
            get
            {
                if (_dialog == null)
                    Load();
                return _dialog;
            }
        }


        public void Save()
        {
            _saveVersion = Dialog.SaveVersion;
            _SaveData = _dialog.Save();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);

            UnityEditor.EditorUtility.DisplayDialog("Save", "Dialog Save Successfully!", "OK");
#endif
        }

        public void Load()
        {
            _dialog = new Dialog();
            _dialog.Load(_SaveData);
            _dialog._name = name;
        }

        public Dialog GetNewDialogFromData()
        {
            Dialog dialog = new Dialog();
            dialog.Load(_SaveData);
            dialog._name = name;
            return dialog;
        }

    }


}